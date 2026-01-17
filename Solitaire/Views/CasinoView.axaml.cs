using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Platform;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Solitaire.Controls;
using Solitaire.ViewModels;

#pragma warning disable CS0618 // Type or member is obsolete

namespace Solitaire.Views;

/// <summary>
/// Interaction logic for CasinoView.xaml
/// 賭場視圖。行動平台和 WebAssembly 的主視圖容器。
/// Casino view. Main view container for mobile platforms and WebAssembly.
/// 處理返回鍵和安全區域（劉海屏、刪節螢幕）等平台特定功能。
/// Handles platform-specific features like back button and safe areas (notch screens, cutout displays).
/// </summary>
public partial class CasinoView : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CasinoView"/> class.
    /// 初始化賭場視圖的新實例。
    /// </summary>
    public CasinoView()
    {
        InitializeComponent();
    }

    /// <summary>
    /// 當控制項附加到視覺樹時調用。設定返回鍵處理和安全區域。
    /// Called when control is attached to visual tree. Sets up back button handling and safe areas.
    /// </summary>
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        if (TopLevel.GetTopLevel(this) is { } topLevel)
        {
            // 註冊 Escape 鍵作為返回鍵
            // Register Escape key as back button
            topLevel.PlatformSettings?.HotkeyConfiguration.Back.Add(new KeyGesture(Key.Escape));

            topLevel.BackRequested += TopLevelOnBackRequested;
            
            // 處理安全區域（避開劉海屏等）
            // Handle safe areas (avoid notches, etc.)
            if (topLevel is { InsetsManager: { } insetsManager })
            {
                insetsManager.SafeAreaChanged += InsetsManagerOnSafeAreaChanged;
                InsetsManagerOnSafeAreaChanged(insetsManager, new SafeAreaChangedArgs(insetsManager.SafeAreaPadding));
            }
            else
            {
                InsetsManagerOnSafeAreaChanged(this, new SafeAreaChangedArgs(default));
            }
        }
    }

    /// <summary>
    /// 當控制項從視覺樹分離時調用。清理事件處理器。
    /// Called when control is detached from visual tree. Cleans up event handlers.
    /// </summary>
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);

        if (TopLevel.GetTopLevel(this) is { } topLevel)
        {
            topLevel.BackRequested -= TopLevelOnBackRequested;

            if (topLevel is { InsetsManager: { } insetsManager })
            {
                insetsManager.SafeAreaChanged -= InsetsManagerOnSafeAreaChanged;
            }
        }
    }

    /// <summary>
    /// 當安全區域變更時調用。調整內容邊距以適應劉海屏等。
    /// Called when safe area changes. Adjusts content padding to accommodate notches, etc.
    /// </summary>
    private void InsetsManagerOnSafeAreaChanged(object? sender, SafeAreaChangedArgs e)
    {
        // Apply "10,10,10,0" as a minimum padding.
        // 應用最小邊距 "10,10,10,0"，確保內容不會太靠近螢幕邊緣。
        RootContentControl.Padding = new Thickness(
            Math.Max(10, e.SafeAreaPadding.Left),
            Math.Max(10, e.SafeAreaPadding.Top),
            Math.Max(10, e.SafeAreaPadding.Right),
            e.SafeAreaPadding.Bottom);
    }

    /// <summary>
    /// 當按下返回鍵時調用。導航回標題頁（主選單）。
    /// Called when back button is pressed. Navigates back to title page (main menu).
    /// </summary>
    private void TopLevelOnBackRequested(object? sender, RoutedEventArgs e)
    {
        (DataContext as CasinoViewModel)?.NavigateToTitleCommand.Execute(null);
    }
}