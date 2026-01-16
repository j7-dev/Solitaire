using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Solitaire.Models;
using Solitaire.Utils;
using Solitaire.ViewModels.Pages;

namespace Solitaire.ViewModels;

/// <summary>
/// The casino view model.
/// 賭場視圖模型。這是應用程式的主視圖模型，管理所有遊戲和頁面之間的導航。
/// 類似於遊戲大廳或主選單，包含所有遊戲模式和設定。
/// </summary>
public partial class CasinoViewModel : ViewModelBase
{
    /// <summary>
    /// 當前顯示的視圖。可以是標題頁、遊戲頁面、設定頁面等。
    /// The currently displayed view. Can be title page, game page, settings page, etc.
    /// </summary>
    [ObservableProperty] private ViewModelBase? _currentView;


    
    /// <summary>
    /// Initializes a new instance of the <see cref="CasinoViewModel"/> class.
    /// 初始化賭場視圖模型的新實例。創建所有子視圖模型。
    /// </summary>
    public CasinoViewModel()
    {
        // 創建所有遊戲和頁面的視圖模型實例
        // Create view model instances for all games and pages
        SettingsInstance = new SettingsViewModel(this);
        KlondikeInstance = new KlondikeSolitaireViewModel(this);
        SpiderInstance = new SpiderSolitaireViewModel(this);
        FreeCellInstance = new FreeCellSolitaireViewModel(this);
        TitleInstance = new TitleViewModel(this);
        StatisticsInstance = new StatisticsViewModel(this);
        
        // 預設顯示標題頁（主選單）
        // Default to showing the title page (main menu)
        CurrentView = TitleInstance;

        // 設定導航到標題頁的命令
        // Set up command to navigate to title page
        NavigateToTitleCommand = new RelayCommand(() =>
        {
            CurrentView = TitleInstance;
            Save();
        });
    }
    
    /// <summary>
    /// 統計頁面視圖模型。顯示所有遊戲的統計資料。
    /// Statistics page view model. Displays statistics for all games.
    /// </summary>
    public StatisticsViewModel StatisticsInstance { get; }

    /// <summary>
    /// 標題頁視圖模型。主選單頁面。
    /// Title page view model. Main menu page.
    /// </summary>
    public TitleViewModel TitleInstance { get; }
    
    /// <summary>
    /// 設定頁面視圖模型。管理遊戲設定。
    /// Settings page view model. Manages game settings.
    /// </summary>
    public SettingsViewModel SettingsInstance { get; }
    
    /// <summary>
    /// 蜘蛛接龍遊戲視圖模型。
    /// Spider Solitaire game view model.
    /// </summary>
    public SpiderSolitaireViewModel SpiderInstance { get; }
    
    /// <summary>
    /// 空當接龍遊戲視圖模型。
    /// FreeCell Solitaire game view model.
    /// </summary>
    public FreeCellSolitaireViewModel FreeCellInstance { get; }
    
    /// <summary>
    /// 克朗代克接龍遊戲視圖模型（經典接龍）。
    /// Klondike Solitaire game view model (classic solitaire).
    /// </summary>
    public KlondikeSolitaireViewModel KlondikeInstance { get; }

    /// <summary>
    /// 導航到標題頁的命令。
    /// Command to navigate to the title page.
    /// </summary>
    public ICommand NavigateToTitleCommand { get; }

    /// <summary>
    /// Saves this instance.
    /// 保存當前狀態到持久化存儲。包含設定和統計資料。
    /// </summary>
    public async void Save()
    {
        // 創建持久化狀態物件，包含所有需要保存的資料
        // Create persistent state object containing all data to save
        var state = new PersistentState(
            SettingsInstance.GetState(),
            StatisticsInstance.KlondikeStatsInstance.GetState(),
            StatisticsInstance.SpiderStatsInstance.GetState(),
            StatisticsInstance.FreeCellStatsInstance.GetState());
        await PlatformProviders.CasinoStorage.SaveObject(state, "mainSettings");
    }

    /// <summary>
    /// Loads this instance.
    /// 從持久化存儲載入狀態。如果是首次啟動，則使用預設值。
    /// </summary>
    /// <returns>賭場視圖模型實例 (Casino view model instance)</returns>
    public static async Task<CasinoViewModel> CreateOrLoadFromDisk()
    {
        // 創建新實例
        // Create new instance
        var ret = new CasinoViewModel();
        
        // 嘗試從磁碟載入保存的狀態
        // Try to load saved state from disk
        var state = await PlatformProviders.CasinoStorage.LoadObject("mainSettings");
        if (state is not null)
        {
            // 如果找到保存的狀態，應用到各個視圖模型
            // If saved state found, apply to view models
            ret.SettingsInstance.ApplyState(state.Settings);
            ret.StatisticsInstance.KlondikeStatsInstance.ApplyState(state.KlondikeStatsInstance);
            ret.StatisticsInstance.SpiderStatsInstance.ApplyState(state.SpiderStatsInstance);
            ret.StatisticsInstance.FreeCellStatsInstance.ApplyState(state.FreeCellStatsInstance);
        }
        return ret;
    }
}