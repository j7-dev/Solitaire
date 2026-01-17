using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Solitaire.ViewModels;
using Solitaire.Views;

namespace Solitaire;

/// <summary>
/// 應用程式類別。Avalonia 應用程式的進入點和生命週期管理。
/// Application class. Entry point and lifecycle management for the Avalonia application.
/// </summary>
public class App : Application
{
    /// <summary>
    /// 初始化應用程式。載入 XAML 資源和樣式。
    /// Initializes the application. Loads XAML resources and styles.
    /// </summary>
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    /// <summary>
    /// 當框架初始化完成時調用。根據平台類型設定主視窗或主視圖。
    /// Called when framework initialization is completed. Sets up main window or main view based on platform type.
    /// </summary>
    public override void OnFrameworkInitializationCompleted()
    {
        // 桌面平台（Windows、macOS、Linux）
        // Desktop platforms (Windows, macOS, Linux)
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();

            // 在 UI 執行緒非同步載入或創建 CasinoViewModel
            // Asynchronously load or create CasinoViewModel on UI thread
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                desktop.MainWindow.DataContext = await CasinoViewModel.CreateOrLoadFromDisk();
            });
        }
        // 行動平台和 WebAssembly（iOS、Android、瀏覽器）
        // Mobile platforms and WebAssembly (iOS, Android, browser)
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new CasinoView();
            
            // 在 UI 執行緒非同步載入或創建 CasinoViewModel
            // Asynchronously load or create CasinoViewModel on UI thread
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                singleViewPlatform.MainView.DataContext = await CasinoViewModel.CreateOrLoadFromDisk();
            });
        }

        base.OnFrameworkInitializationCompleted();
    }
}