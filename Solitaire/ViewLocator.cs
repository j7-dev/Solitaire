using System;
using System.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Solitaire.ViewModels;
using Solitaire.ViewModels.Pages;
using Solitaire.Views;
using Solitaire.Views.Pages;

namespace Solitaire;

/// <summary>
/// 視圖定位器。實作 MVVM 模式中的視圖解析邏輯。
/// View locator. Implements view resolution logic in MVVM pattern.
/// 根據 ViewModel 類型自動創建對應的 View 實例。
/// Automatically creates corresponding View instances based on ViewModel type.
/// </summary>
public class ViewLocator : IDataTemplate
{
    /// <summary>
    /// 根據 ViewModel 建立對應的 View。
    /// Builds the corresponding View based on the ViewModel.
    /// </summary>
    /// <param name="data">ViewModel 實例 (ViewModel instance)</param>
    /// <returns>對應的 View 控制項 (Corresponding View control)</returns>
    public Control? Build(object? data)
    {
        return data switch
        {
            TitleViewModel => new TitleView(),                      // 標題頁（主選單）
            KlondikeSolitaireViewModel => new KlondikeSolitaireView(), // 克朗代克接龍
            FreeCellSolitaireViewModel => new FreeCellSolitaireView(), // 空當接龍
            SpiderSolitaireViewModel => new SpiderSolitaireView(),     // 蜘蛛接龍
            GameStatisticsViewModel => new GameStatisticsView(),       // 遊戲統計
            SettingsViewModel => new SettingsView(),                   // 設定頁
            StatisticsViewModel => new StatisticsView(),               // 統計頁
            CasinoViewModel => new CasinoView(),                       // 賭場主視圖
            null => null,
            // 找不到對應視圖時顯示錯誤訊息
            // Display error message when no corresponding view is found
            _ => new TextBlock
            {
                Text = $"View for {data.GetType().Name} wasn't found"
            }
        };
    }

    /// <summary>
    /// 檢查是否可以為此資料建立視圖。
    /// Checks if a view can be created for this data.
    /// </summary>
    /// <param name="data">要檢查的資料 (Data to check)</param>
    /// <returns>如果是 ViewModelBase 類型則返回 true (Returns true if it's a ViewModelBase type)</returns>
    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}