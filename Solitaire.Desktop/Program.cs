using System;
using System.Diagnostics;
using Avalonia;

namespace Solitaire.Desktop;

/// <summary>
/// 桌面平台程式進入點類別（Windows、macOS、Linux）。
/// Desktop platform program entry point class (Windows, macOS, Linux).
/// </summary>
sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    /// <summary>
    /// 應用程式主要進入點。初始化 Avalonia 並啟動應用程式。
    /// Application main entry point. Initializes Avalonia and starts the application.
    /// </summary>
    /// <param name="args">命令列參數 (Command line arguments)</param>
    [STAThread]
    public static void Main(string[] args)
    {
        // 新增控制台追蹤監聽器以便除錯
        // Add console trace listener for debugging
        Trace.Listeners.Add(new ConsoleTraceListener());
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    /// <summary>
    /// 建立 Avalonia 應用程式建構器。配置跨平台 UI 框架。
    /// Builds Avalonia application builder. Configures cross-platform UI framework.
    /// </summary>
    /// <returns>Avalonia 應用程式建構器 (Avalonia app builder)</returns>
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()  // 自動偵測平台並使用適當的後端 (Auto-detect platform and use appropriate backend)
            .LogToTrace();        // 將日誌輸出到追蹤 (Output logs to trace)
}
