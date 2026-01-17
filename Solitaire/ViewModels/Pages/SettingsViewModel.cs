using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Solitaire.Models;
using Solitaire.Utils;
using System;
using Avalonia.Reactive;

namespace Solitaire.ViewModels.Pages;

/// <summary>
/// 設定頁面視圖模型。管理遊戲設定，如難度和抽牌模式。
/// Settings page view model. Manages game settings such as difficulty and draw mode.
/// </summary>
public partial class SettingsViewModel : ViewModelBase
{
    /// <summary>
    /// 難度等級（簡單、中等、困難）。
    /// Difficulty level (Easy, Medium, Hard).
    /// </summary>
    [ObservableProperty] private Difficulty _difficulty = Difficulty.Easy;
    
    /// <summary>
    /// 抽牌模式（一張或三張）。
    /// Draw mode (one card or three cards).
    /// </summary>
    [ObservableProperty] private DrawMode _drawMode = DrawMode.DrawOne;
    
    /// <summary>
    /// 抽牌模式的顯示文字。
    /// Display text for draw mode.
    /// </summary>
    [ObservableProperty] private string? _drawModeText;
    
    /// <summary>
    /// 難度等級的顯示文字。
    /// Display text for difficulty level.
    /// </summary>
    [ObservableProperty] private string? _difficultyText;
    
    /// <summary>
    /// 返回標題頁的命令。
    /// Command to return to title page.
    /// </summary>
    public ICommand NavigateToTitleCommand { get; }
    
    /// <summary>
    /// 切換抽牌模式的命令（在一張和三張之間切換）。
    /// Command to toggle draw mode (between one and three cards).
    /// </summary>
    public ICommand DrawModeCommand { get; } 

    /// <summary>
    /// 切換難度的命令（簡單→中等→困難→簡單）。
    /// Command to cycle through difficulty levels (Easy→Medium→Hard→Easy).
    /// </summary>
    public ICommand DifficultyCommand { get; } 

    
    /// <summary>
    /// 初始化設定視圖模型。
    /// Initializes the settings view model.
    /// </summary>
    /// <param name="casinoViewModel">賭場視圖模型引用 (Casino view model reference)</param>
    public SettingsViewModel(CasinoViewModel casinoViewModel)
    {
        var casinoViewModel1 = casinoViewModel;

        // 設定返回標題頁的命令，並保存設定
        // Set up command to return to title page and save settings
        NavigateToTitleCommand = new RelayCommand(() =>
        {
            casinoViewModel1.CurrentView = casinoViewModel1.TitleInstance;
            casinoViewModel1.Save();
        });
        

        // 設定切換抽牌模式的命令
        // Set up command to toggle draw mode
        DrawModeCommand = new RelayCommand(() =>
        {
            DrawMode = DrawMode == DrawMode.DrawOne ? DrawMode.DrawThree : DrawMode.DrawOne;
        });


        // 設定循環切換難度的命令
        // Set up command to cycle through difficulty levels
        DifficultyCommand = new RelayCommand(() =>
        {
            Difficulty = Difficulty switch
            {
                Difficulty.Easy => Difficulty.Medium,
                Difficulty.Medium => Difficulty.Hard,
                Difficulty.Hard => Difficulty.Easy,
                _ => throw new ArgumentOutOfRangeException()
            };
        });
        
        // 監聽抽牌模式變更，更新顯示文字
        // Watch for draw mode changes and update display text
        this.WhenAnyValue(x => x.DrawMode)
            .Subscribe(new AnonymousObserver<DrawMode>(x =>
            {
                DrawModeText = $"{DrawMode.ToString()
                    .Replace("Draw", "")} Card{(DrawMode == DrawMode.DrawThree? "s" : "")}" ;
            }));

        // 監聽難度變更，更新顯示文字
        // Watch for difficulty changes and update display text
        this.WhenAnyValue(x => x.Difficulty)
            .Subscribe(new AnonymousObserver<Difficulty>(x =>
            {
                DifficultyText = $"{Difficulty}";
            }));
    }

    /// <summary>
    /// 應用保存的狀態。
    /// Applies saved state.
    /// </summary>
    /// <param name="state">要應用的狀態 (State to apply)</param>
    public void ApplyState(SettingsState state)
    {
        Difficulty = state.Difficulty;
        DrawMode = state.DrawMode;
    }

    /// <summary>
    /// 取得當前狀態以供保存。
    /// Gets current state for saving.
    /// </summary>
    /// <returns>當前設定狀態 (Current settings state)</returns>
    public SettingsState GetState()
    {
        return new SettingsState(Difficulty, DrawMode);
    }
}