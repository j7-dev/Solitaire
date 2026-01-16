using Foundation;
using Avalonia;
using Avalonia.iOS;

namespace Solitaire.iOS;

// The UIApplicationDelegate for the application. This class is responsible for launching the 
// User Interface of the application, as well as listening (and optionally responding) to 
// application events from iOS.
/// <summary>
/// iOS 應用程式委派。負責啟動使用者介面並處理 iOS 應用程式事件。
/// iOS application delegate. Responsible for launching user interface and handling iOS app events.
/// </summary>
[Register("AppDelegate")]
public class AppDelegate : AvaloniaAppDelegate<App>
{
    /// <summary>
    /// 自訂 Avalonia 應用程式建構器。可以在此添加 iOS 特定的配置。
    /// Customizes Avalonia app builder. Can add iOS-specific configurations here.
    /// </summary>
    /// <param name="builder">應用程式建構器 (App builder)</param>
    /// <returns>自訂的建構器 (Customized builder)</returns>
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder);
    }
}
