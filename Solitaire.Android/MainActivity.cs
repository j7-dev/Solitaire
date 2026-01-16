using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;

namespace Solitaire.Android;

/// <summary>
/// Android 平台主要 Activity。Avalonia Android 應用程式的進入點。
/// Main Activity for Android platform. Entry point for Avalonia Android application.
/// </summary>
[Activity(
    Label = "Solitaire.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,  // 標記為啟動 Activity (Mark as launcher activity)
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]  // 處理配置變更 (Handle configuration changes)
public class MainActivity : AvaloniaMainActivity<App>
{
    /// <summary>
    /// 自訂 Avalonia 應用程式建構器。可以在此添加 Android 特定的配置。
    /// Customizes Avalonia app builder. Can add Android-specific configurations here.
    /// </summary>
    /// <param name="builder">應用程式建構器 (App builder)</param>
    /// <returns>自訂的建構器 (Customized builder)</returns>
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder);
    }
}
