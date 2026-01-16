using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Solitaire.Models;
using Solitaire.Utils;

namespace Solitaire.ViewModels.Pages;

/// <summary>
/// 統計頁面視圖模型。顯示所有遊戲（克朗代克、蜘蛛、空當）的統計資料。
/// Statistics page view model. Displays statistics for all games (Klondike, Spider, FreeCell).
/// </summary>
public partial class StatisticsViewModel : ViewModelBase
{
    /// <summary>
    /// 返回標題頁的命令。
    /// Command to return to title page.
    /// </summary>
    public ICommand NavigateToTitleCommand { get; }
    
    /// <summary>
    /// 重置克朗代克接龍統計的命令。
    /// Command to reset Klondike Solitaire statistics.
    /// </summary>
    public ICommand ResetKlondikeStatsCommand { get; }
    
    /// <summary>
    /// 重置空當接龍統計的命令。
    /// Command to reset FreeCell Solitaire statistics.
    /// </summary>
    public ICommand ResetFreeCellStatsCommand { get; }
    
    /// <summary>
    /// 重置蜘蛛接龍統計的命令。
    /// Command to reset Spider Solitaire statistics.
    /// </summary>
    public ICommand ResetSpiderStatsCommand { get; }

     

    /// <summary>
    /// 克朗代克接龍的統計實例。
    /// Klondike Solitaire statistics instance.
    /// </summary>
    [ObservableProperty] private GameStatisticsViewModel _klondikeStatsInstance;
    
    /// <summary>
    /// 蜘蛛接龍的統計實例。
    /// Spider Solitaire statistics instance.
    /// </summary>
    [ObservableProperty] private GameStatisticsViewModel _spiderStatsInstance;
    
    /// <summary>
    /// 空當接龍的統計實例。
    /// FreeCell Solitaire statistics instance.
    /// </summary>
    [ObservableProperty] private GameStatisticsViewModel _freeCellStatsInstance;


    /// <summary>
    /// 初始化統計視圖模型。為每個遊戲創建統計實例。
    /// Initializes the statistics view model. Creates statistics instances for each game.
    /// </summary>
    /// <param name="casinoViewModel">賭場視圖模型引用 (Casino view model reference)</param>
    public StatisticsViewModel(CasinoViewModel casinoViewModel)
    {
        var casinoViewModel1 = casinoViewModel;

        // 設定返回標題頁的命令，並保存資料
        // Set up command to return to title page and save data
        NavigateToTitleCommand = new RelayCommand(() =>
        {
            casinoViewModel1.CurrentView = casinoViewModel1.TitleInstance;
            casinoViewModel1.Save();
        });
        
        // 為每個遊戲創建統計實例
        // Create statistics instances for each game
        _spiderStatsInstance = new GameStatisticsViewModel(casinoViewModel.SpiderInstance);
        _klondikeStatsInstance = new GameStatisticsViewModel(casinoViewModel.KlondikeInstance);
        _freeCellStatsInstance = new GameStatisticsViewModel(casinoViewModel.FreeCellInstance);
        
        
        // 設定重置克朗代克統計的命令
        // Set up command to reset Klondike statistics
        ResetKlondikeStatsCommand = new RelayCommand(() =>
        {
            KlondikeStatsInstance?.ResetCommand?.Execute(null);
        });
        
        // 設定重置蜘蛛統計的命令
        // Set up command to reset Spider statistics
        ResetSpiderStatsCommand = new RelayCommand(() =>
        {
            SpiderStatsInstance?.ResetCommand?.Execute(null);
        });
        
        // 設定重置空當統計的命令
        // Set up command to reset FreeCell statistics
        ResetFreeCellStatsCommand = new RelayCommand(() =>
        {
            FreeCellStatsInstance?.ResetCommand?.Execute(null);
        });
    }
}