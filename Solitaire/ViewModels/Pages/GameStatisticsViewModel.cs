using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Solitaire.Models;

namespace Solitaire.ViewModels.Pages;

/// <summary>
/// A set of general statistics for a game.
/// 遊戲的通用統計資料集。追蹤單一遊戲的各種統計數據，如勝率、連勝、分數等。
/// </summary>
public partial class GameStatisticsViewModel : ViewModelBase
{
#if DEBUG
    /// <summary>
    /// 設計時建構函式（僅用於 Visual Studio 設計器）。
    /// Design-time constructor (only for Visual Studio designer).
    /// </summary>
    public GameStatisticsViewModel()
    {
    }
#endif

    /// <summary>
    /// 初始化遊戲統計視圖模型。
    /// Initializes the game statistics view model.
    /// </summary>
    /// <param name="cardGameInstance">卡片遊戲實例 (Card game instance)</param>
    public GameStatisticsViewModel(CardGameViewModel cardGameInstance)
    {
        _cardGameInstance = cardGameInstance;
        GameName = cardGameInstance.GameName;
        cardGameInstance.RegisterStatsInstance(this);
        //  Create the reset command.
        //  創建重置命令。
        ResetCommand = new RelayCommand(DoReset);
    }

    /// <summary>
    /// 重置統計資料的命令。
    /// Command to reset statistics.
    /// </summary>
    public ICommand? ResetCommand { get; }

    /// <summary>
    /// Resets the statistics.
    /// 重置所有統計資料為初始值。
    /// </summary>
    private void DoReset()
    {
        GamesPlayed = 0;
        GamesWon = 0;
        GamesLost = 0;
        HighestWinningStreak = 0;
        HighestLosingStreak = 0;
        CurrentStreak = 0;
        CumulativeScore = 0;
        HighestScore = 0;
        AverageScore = 0;
        CumulativeGameTime = TimeSpan.FromSeconds(0);
        AverageGameTime = TimeSpan.FromSeconds(0);
    }

    /// <summary>
    /// 更新統計資料。在每場遊戲結束時調用。
    /// Updates statistics. Called at the end of each game.
    /// </summary>
    public void UpdateStatistics()
    {
        //  Update the games won or lost.
        //  更新獲勝或失敗的場數。
        GamesPlayed++;
        if (_cardGameInstance?.IsGameWon ?? false)
            GamesWon++;
        else
            GamesLost++;

        //  Update the current streak.
        //  更新目前連勝/連敗紀錄（正數為連勝，負數為連敗）。
        if (_cardGameInstance?.IsGameWon ?? false)
            CurrentStreak = CurrentStreak < 0 ? 1 : CurrentStreak + 1;
        else
            CurrentStreak = CurrentStreak > 0 ? -1 : CurrentStreak - 1;

        //  Update the highest streaks.
        //  更新最高連勝/連敗紀錄。
        if (CurrentStreak > HighestWinningStreak)
            HighestWinningStreak = CurrentStreak;
        else if (Math.Abs(CurrentStreak) > HighestLosingStreak)
            HighestLosingStreak = Math.Abs(CurrentStreak);

        //  Update the highest score.
        //  更新最高分數。
        if (_cardGameInstance?.Score > HighestScore)
            HighestScore = _cardGameInstance.Score;

        //  Update the average score. Only won games
        //  contribute to the running average.
        //  更新平均分數。只有獲勝的遊戲才計入平均分數。
        if (_cardGameInstance?.IsGameWon ?? false)
        {
            CumulativeScore += _cardGameInstance.Score;
            AverageScore = CumulativeScore / (double) GamesWon;
        }

        //  Update the average game time.
        //  更新平均遊戲時間。
        CumulativeGameTime += _cardGameInstance?.ElapsedTime ?? TimeSpan.Zero;
        AverageGameTime = TimeSpan.FromTicks(CumulativeGameTime.Ticks / (GamesWon + GamesLost));
    }

    /// <summary>
    /// 遊戲名稱。
    /// Game name.
    /// </summary>
    public string? GameName { get; }

    /// <summary>
    /// 已遊玩的場數。
    /// Number of games played.
    /// </summary>
    [ObservableProperty] private int _gamesPlayed;
    
    /// <summary>
    /// 獲勝的場數。
    /// Number of games won.
    /// </summary>
    [ObservableProperty] private int _gamesWon;
    
    /// <summary>
    /// 失敗的場數。
    /// Number of games lost.
    /// </summary>
    [ObservableProperty] private int _gamesLost;
    
    /// <summary>
    /// 最高連勝紀錄。
    /// Highest winning streak.
    /// </summary>
    [ObservableProperty] private int _highestWinningStreak;
    
    /// <summary>
    /// 最高連敗紀錄。
    /// Highest losing streak.
    /// </summary>
    [ObservableProperty] private int _highestLosingStreak;
    
    /// <summary>
    /// 目前連勝/連敗（正數為連勝，負數為連敗）。
    /// Current streak (positive for wins, negative for losses).
    /// </summary>
    [ObservableProperty] private int _currentStreak;
    
    /// <summary>
    /// 累計分數（所有獲勝遊戲的總分）。
    /// Cumulative score (total of all winning games).
    /// </summary>
    [ObservableProperty] private int _cumulativeScore;
    
    /// <summary>
    /// 最高分數。
    /// Highest score.
    /// </summary>
    [ObservableProperty] private int _highestScore;
    
    /// <summary>
    /// 平均分數（僅計算獲勝的遊戲）。
    /// Average score (only winning games counted).
    /// </summary>
    [ObservableProperty] private double _averageScore;
    
    /// <summary>
    /// 累計遊戲時間。
    /// Cumulative game time.
    /// </summary>
    [ObservableProperty] private TimeSpan _cumulativeGameTime;
    
    /// <summary>
    /// 平均遊戲時間。
    /// Average game time.
    /// </summary>
    [ObservableProperty] private TimeSpan _averageGameTime;
    
    /// <summary>
    /// 卡片遊戲實例的引用。
    /// Reference to the card game instance.
    /// </summary>
    private readonly CardGameViewModel? _cardGameInstance;

    /// <summary>
    /// 應用保存的統計狀態。
    /// Applies saved statistics state.
    /// </summary>
    /// <param name="state">要應用的狀態 (State to apply)</param>
    public void ApplyState(GameStatisticsState state)
    {
        GamesPlayed = state.GamesPlayed;
        GamesWon = state.GamesWon;
        GamesLost = state.GamesLost;
        HighestWinningStreak = state.HighestWinningStreak;
        HighestLosingStreak = state.HighestLosingStreak;
        CurrentStreak = state.CurrentStreak;
        CumulativeScore = state.CumulativeScore;
        HighestScore = state.HighestScore;
        AverageScore = state.AverageScore;
        CumulativeGameTime = state.CumulativeGameTime;
        AverageGameTime = state.AverageGameTime;
    }

    /// <summary>
    /// 取得當前統計狀態以供保存。
    /// Gets current statistics state for saving.
    /// </summary>
    /// <returns>當前統計狀態 (Current statistics state)</returns>
    public GameStatisticsState GetState()
    {
        return new GameStatisticsState(
            GamesPlayed,
            GamesWon,
            GamesLost,
            HighestWinningStreak,
            HighestLosingStreak,
            CurrentStreak,
            CumulativeScore,
            HighestScore,
            AverageScore,
            CumulativeGameTime,
            AverageGameTime
        );
    }
}