using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Windows.Input;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Solitaire.Models;
using Solitaire.Utils;
using Solitaire.ViewModels.Pages;

namespace Solitaire.ViewModels;

/// <summary>
/// Base class for a ViewModel for a card game.
/// 卡片遊戲視圖模型的基類。所有接龍遊戲（克朗代克、蜘蛛、空當）都繼承此類別。
/// 提供通用的遊戲邏輯，如計時器、分數、移動次數、撤銷功能等。
/// </summary>
public abstract partial class CardGameViewModel : ViewModelBase
{
    /// <summary>
    /// 牌組。包含遊戲中所有 52 張牌的不可變陣列。
    /// The deck. Contains an immutable array of all 52 cards in the game.
    /// </summary>
    public ImmutableArray<PlayingCardViewModel>? Deck;

    /// <summary>
    /// 自動移動命令。某些遊戲可以自動將牌移動到目標位置。
    /// Auto-move command. Some games can automatically move cards to their target positions.
    /// </summary>
    public ICommand? AutoMoveCommand { get; protected set; }

    /// <summary>
    /// 移動堆疊。用於實作撤銷功能，記錄所有移動操作。
    /// Move stack. Used to implement undo functionality, recording all move operations.
    /// </summary>
    private readonly Stack<CardOperation[]> _moveStack = new();

    /// <summary>
    /// 遊戲名稱（由子類別實作）。
    /// Game name (implemented by subclasses).
    /// </summary>
    public abstract string? GameName { get; }

    /// <summary>
    /// 清除撤銷堆疊。在開始新遊戲時調用。
    /// Clears the undo stack. Called when starting a new game.
    /// </summary>
    private void ClearUndoStack()
    {
        _moveStack.Clear();
    }
    
    
    
    /// <summary>
    /// 記錄移動操作以供撤銷。
    /// Records move operations for undo.
    /// </summary>
    /// <param name="operations">要記錄的操作 (Operations to record)</param>
    protected void RecordMoves(params CardOperation[] operations)
    {
        _moveStack.Push(operations);
    }

    /// <summary>
    /// 撤銷上一步移動。
    /// Undoes the last move.
    /// </summary>
    private void UndoMove()
    {
        if (_moveStack.Count > 0)
        {
            var operations = _moveStack.Pop();

            // 反向執行所有操作以恢復狀態
            // Revert all operations to restore state
            foreach (var operation in operations)
            {
                operation.Revert(this);
            }

           
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CardGameViewModel"/> class.
    /// 初始化卡片遊戲視圖模型的新實例。
    /// </summary>
    /// <param name="casinoViewModel">賭場視圖模型引用，用於導航和保存 (Casino view model reference for navigation and saving)</param>
    protected CardGameViewModel(CasinoViewModel casinoViewModel)
    {
        // 設定返回賭場（主選單）的命令
        // Set up the command to return to casino (main menu)
        NavigateToCasinoCommand =
            new RelayCommand(() =>
            {
                // 如果有移動過，更新統計並保存
                // If moves were made, update statistics and save
                if (Moves > 0)
                {
                    _gameStats.UpdateStatistics();
                    casinoViewModel.Save();
                }

                casinoViewModel.CurrentView = casinoViewModel.TitleInstance;
            });

        // 設定撤銷命令
        // Set up the undo command
        UndoCommand = new RelayCommand(UndoMove);

        DoInitialize();
    }

    /// <summary>
    /// 執行初始化。設定計時器並初始化牌組。
    /// Performs initialization. Sets up the timer and initializes the deck.
    /// </summary>
    private void DoInitialize()
    {
        //  Set up the timer.
        //  設定計時器，每 500 毫秒更新一次經過時間。
        _timer.Interval = TimeSpan.FromMilliseconds(500);
        _timer.Tick += timer_Tick;
        InitializeDeck();
    }

    /// <summary>
    /// 初始化牌組。創建所有 52 張撲克牌的視圖模型。
    /// Initializes the deck. Creates view models for all 52 playing cards.
    /// </summary>
    protected virtual void InitializeDeck()
    {
        if (Deck is { }) return;

        // 從 CardType 列舉創建所有 52 張牌
        // Create all 52 cards from the CardType enum
        var playingCards = Enum
            .GetValuesAsUnderlyingType(typeof(CardType))
            .Cast<CardType>()
            .Select(cardType => new PlayingCardViewModel(this)
                { CardType = cardType, IsFaceDown = true })
            .ToImmutableArray();

        Deck = playingCards;
    }

    /// <summary>
    /// 取得新洗好的牌組。重置所有牌並隨機排序。
    /// Gets a new shuffled deck. Resets all cards and randomizes their order.
    /// </summary>
    /// <returns>洗好的牌組 (The shuffled deck)</returns>
    protected IList<PlayingCardViewModel> GetNewShuffledDeck()
    {
        // 重置所有牌的狀態
        // Reset all card states
        foreach (var card in Deck!)
        {
            card.Reset();
        }

        // 使用隨機數排序來洗牌
        // Shuffle using random number ordering
        var playingCards = Deck.Value.OrderBy(_ => PlatformProviders.NextRandomDouble()).ToList();

        return playingCards.Count == 0
            ? throw new InvalidOperationException("Starting deck cannot be empty.")
            : playingCards;
    }


    /// <summary>
    /// 取得指定牌所屬的集合（由子類別實作）。
    /// Gets the collection that the specified card belongs to (implemented by subclasses).
    /// </summary>
    /// <param name="card">要查詢的牌 (The card to query)</param>
    /// <returns>包含該牌的集合 (The collection containing the card)</returns>
    public abstract IList<PlayingCardViewModel>? GetCardCollection(PlayingCardViewModel card);


    /// <summary>
    /// 檢查並移動牌（由子類別實作）。實作各種接龍遊戲的移動規則。
    /// Checks and moves a card (implemented by subclasses). Implements the move rules for various solitaire games.
    /// </summary>
    /// <param name="from">來源集合 (Source collection)</param>
    /// <param name="to">目標集合 (Target collection)</param>
    /// <param name="card">要移動的牌 (Card to move)</param>
    /// <param name="checkOnly">是否只檢查不執行移動 (Whether to only check without moving)</param>
    /// <returns>移動是否成功 (Whether the move was successful)</returns>
    public abstract bool CheckAndMoveCard(IList<PlayingCardViewModel> from,
        IList<PlayingCardViewModel> to,
        PlayingCardViewModel card,
        bool checkOnly = false);

    /// <summary>
    /// Deals a new game.
    /// 發新牌開始新遊戲。重置所有內部狀態。
    /// </summary>
    protected void ResetInternalState()
    {
        ClearUndoStack();
        
        //  Stop the timer and reset the game data.
        //  停止計時器並重置遊戲資料。
        StopTimer();
        ElapsedTime = TimeSpan.FromSeconds(0);
        Moves = 0;
        Score = 0;
        IsGameWon = false;
        OnPropertyChanged(nameof(IsGameWon));
    }

    /// <summary>
    /// Starts the timer.
    /// 啟動計時器。當玩家開始移動牌時調用。
    /// </summary>
    protected void StartTimer()
    {
        _lastTick = DateTime.Now;
        _timer.Start();
    }

    /// <summary>
    /// Stops the timer.
    /// 停止計時器。當遊戲結束時調用。
    /// </summary>
    protected void StopTimer()
    {
        _timer.Stop();
    }

    /// <summary>
    /// Handles the Tick event of the timer control.
    /// 處理計時器的 Tick 事件。每 500 毫秒更新一次經過時間。
    /// </summary>
    /// <param name="sender">The source of the event. 事件來源。</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data. 事件資料。</param>
    private void timer_Tick(object? sender, EventArgs e)
    {
        //  Get the time, update the elapsed time, record the last tick.
        //  取得當前時間，更新經過時間，記錄最後一次 tick。
        var timeNow = DateTime.Now;
        ElapsedTime += timeNow - _lastTick;
        _lastTick = timeNow;
    }

    /// <summary>
    /// Fires the game won event.
    /// 觸發遊戲勝利事件。當玩家完成遊戲時調用。
    /// </summary>
    protected void FireGameWonEvent()
    {
        _gameStats.UpdateStatistics();

        var wonEvent = GameWon;
        if (wonEvent is not { })
            wonEvent?.Invoke();
    }

    /// <summary>
    /// The timer for recording the time spent in a game.
    /// 用於記錄遊戲時間的計時器。
    /// </summary>
    private readonly DispatcherTimer _timer = new();

    /// <summary>
    /// The time of the last tick.
    /// 最後一次 tick 的時間。
    /// </summary>
    private DateTime _lastTick;

    /// <summary>
    /// 分數。根據不同遊戲規則計算。
    /// Score. Calculated based on different game rules.
    /// </summary>
    [ObservableProperty] private int _score;

    /// <summary>
    /// 經過時間。顯示玩家花費的時間。
    /// Elapsed time. Shows the time spent by the player.
    /// </summary>
    [ObservableProperty] private TimeSpan _elapsedTime;

    /// <summary>
    /// 移動次數。記錄玩家移動牌的次數。
    /// Number of moves. Records the number of times the player moved cards.
    /// </summary>
    [ObservableProperty] private int _moves;

    /// <summary>
    /// 遊戲是否已獲勝。
    /// Whether the game is won.
    /// </summary>
    [ObservableProperty] private bool _isGameWon;
    
    /// <summary>
    /// 遊戲統計視圖模型。用於追蹤遊戲統計資料。
    /// Game statistics view model. Used to track game statistics.
    /// </summary>
    private GameStatisticsViewModel _gameStats = null!;

    /// <summary>
    /// Gets the go to casino command.
    /// 取得返回賭場（主選單）的命令。
    /// </summary>
    /// <value>The go to casino command. 返回賭場命令。</value>
    public ICommand? NavigateToCasinoCommand { get; }

    /// <summary>
    /// Gets the deal new game command.
    /// 取得發新牌（開始新遊戲）的命令。
    /// </summary>
    /// <value>The deal new game command. 新遊戲命令。</value>
    public ICommand? NewGameCommand { get; protected set; }

    /// <summary>
    /// 撤銷命令。允許玩家撤銷上一步操作。
    /// Undo command. Allows the player to undo the last operation.
    /// </summary>
    public ICommand? UndoCommand { get; protected set; }

    /// <summary>
    /// Occurs when the game is won.
    /// 當遊戲獲勝時觸發的事件。
    /// </summary>
    public event Action GameWon = null!;

    /// <summary>
    /// 重置遊戲（由子類別實作）。
    /// Resets the game (implemented by subclasses).
    /// </summary>
    public abstract void ResetGame();

    /// <summary>
    /// 註冊統計實例。連結遊戲與統計資料。
    /// Registers the statistics instance. Links the game with statistics data.
    /// </summary>
    /// <param name="gameStatsInstance">遊戲統計實例 (Game statistics instance)</param>
    public void RegisterStatsInstance(GameStatisticsViewModel gameStatsInstance)
    {
        _gameStats = gameStatsInstance;
    }

    /// <summary>
    /// 卡片操作基類。用於實作撤銷功能。
    /// Base class for card operations. Used to implement undo functionality.
    /// </summary>
    public abstract class CardOperation
    {
        /// <summary>
        /// 還原操作。
        /// Reverts the operation.
        /// </summary>
        /// <param name="game">遊戲實例 (Game instance)</param>
        public abstract void Revert(CardGameViewModel game);
    }

    /// <summary>
    /// 通用操作。用於包裝任意操作以支援撤銷。
    /// Generic operation. Used to wrap arbitrary operations to support undo.
    /// </summary>
    public class GenericOperation : CardOperation
    {
        private readonly Action _action;
        
        public GenericOperation(Action action)
        {
            _action = action;
        }

        public override void Revert(CardGameViewModel game)
        {
            _action();
        }
    }

    /// <summary>
    /// 移動操作。記錄牌的移動以供撤銷。
    /// Move operation. Records card moves for undo.
    /// </summary>
    public class MoveOperation : CardOperation
    {
        public MoveOperation(IList<PlayingCardViewModel> from, IList<PlayingCardViewModel> to, IList<PlayingCardViewModel> run,
            int score)
        {
            From = from;
            To = to;
            Run = run;
            Score = score;
        }

        /// <summary>
        /// 來源集合。
        /// Source collection.
        /// </summary>
        public IList<PlayingCardViewModel> From { get; }

        /// <summary>
        /// 目標集合。
        /// Target collection.
        /// </summary>
        public IList<PlayingCardViewModel> To { get; }

        /// <summary>
        /// 移動的牌序列。
        /// The sequence of cards moved.
        /// </summary>
        public IList<PlayingCardViewModel> Run { get; }

        /// <summary>
        /// 此移動獲得的分數。
        /// Score gained from this move.
        /// </summary>
        public int Score { get; }
        
        /// <summary>
        /// 還原移動。將牌移回原處並調整分數。
        /// Reverts the move. Moves cards back to original position and adjusts score.
        /// </summary>
        /// <param name="game">遊戲實例 (Game instance)</param>
        public override void Revert(CardGameViewModel game)
        {

            game.Score -= Score;

            // 將牌移回來源集合
            // Move cards back to source collection
            foreach (var runCard in Run)
                From.Add(runCard);
            // 從目標集合移除
            // Remove from target collection
            foreach (var runCard in Run)
                To.Remove(runCard);

            game.Moves--;
        }
    }
}