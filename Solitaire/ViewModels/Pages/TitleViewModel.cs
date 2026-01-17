using System.Windows.Input;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Solitaire.ViewModels.Pages;

/// <summary>
/// 標題頁視圖模型。作為主選單，提供導航到各種遊戲和設定頁面的命令。
/// Title page view model. Serves as the main menu, providing commands to navigate to various games and settings pages.
/// </summary>
public partial class TitleViewModel : ViewModelBase
{
    
#if DEBUG
    /// <summary>
    /// 設計時建構函式（僅用於 Visual Studio 設計器）。
    /// Design-time constructor (only for Visual Studio designer).
    /// </summary>
    public TitleViewModel() { }
#endif
    
    /// <summary>
    /// 導航到克朗代克接龍的命令。
    /// Command to navigate to Klondike Solitaire.
    /// </summary>
    public ICommand? NavigateToKlondikeCommand { get; }

    /// <summary>
    /// 導航到蜘蛛接龍的命令。
    /// Command to navigate to Spider Solitaire.
    /// </summary>
    public ICommand? NavigateToSpiderCommand { get; }
    
    /// <summary>
    /// 導航到空當接龍的命令。
    /// Command to navigate to FreeCell Solitaire.
    /// </summary>
    public ICommand? NavigateToFreeCellCommand { get; }
    
    /// <summary>
    /// 導航到統計頁面的命令。
    /// Command to navigate to Statistics page.
    /// </summary>
    public ICommand? NavigateToStatisticsCommand { get; }

    /// <summary>
    /// 導航到設定頁面的命令。
    /// Command to navigate to Settings page.
    /// </summary>
    public ICommand? NavigateToSettingsCommand { get; }

    /// <summary>
    /// 初始化標題頁視圖模型。設定所有導航命令。
    /// Initializes the title page view model. Sets up all navigation commands.
    /// </summary>
    /// <param name="casinoViewModel">賭場視圖模型引用 (Casino view model reference)</param>
    public TitleViewModel(CasinoViewModel casinoViewModel)
    {
 
        // 設定導航到克朗代克接龍的命令
        // Set up command to navigate to Klondike Solitaire
        NavigateToKlondikeCommand = new RelayCommand(() =>
        {
            casinoViewModel.CurrentView = casinoViewModel.KlondikeInstance;
            // 在背景執行緒發送新遊戲命令
            // Post new game command on background thread
            Dispatcher.UIThread.Post(() =>
            {
                casinoViewModel.KlondikeInstance.NewGameCommand?.Execute(default);
            }, DispatcherPriority.Background);
        });

        // 設定導航到蜘蛛接龍的命令
        // Set up command to navigate to Spider Solitaire
        NavigateToSpiderCommand = new RelayCommand(() =>
        {
            casinoViewModel.CurrentView = casinoViewModel.SpiderInstance;
            
            Dispatcher.UIThread.Post(() =>
            {
                casinoViewModel.SpiderInstance.NewGameCommand?.Execute(default);
            }, DispatcherPriority.Background);
         });

        // 設定導航到空當接龍的命令
        // Set up command to navigate to FreeCell Solitaire
        NavigateToFreeCellCommand = new RelayCommand(() =>
        {
            casinoViewModel.CurrentView = casinoViewModel.FreeCellInstance;
            
            Dispatcher.UIThread.Post(() =>
            {
                casinoViewModel.FreeCellInstance.NewGameCommand?.Execute(default);
            }, DispatcherPriority.Background);
         });

        // 設定導航到設定頁面的命令
        // Set up command to navigate to Settings page
        NavigateToSettingsCommand = new RelayCommand(() =>
        {
            casinoViewModel.CurrentView = casinoViewModel.SettingsInstance;
        });

        // 設定導航到統計頁面的命令
        // Set up command to navigate to Statistics page
        NavigateToStatisticsCommand = new RelayCommand(() =>
        {
            casinoViewModel.CurrentView = casinoViewModel.StatisticsInstance;
        });
        
    }
}