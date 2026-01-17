using Avalonia.Controls;

namespace Solitaire.Views;

/// <summary>
/// 主視窗。桌面平台（Windows、macOS、Linux）的主視窗容器。
/// Main window. Main window container for desktop platforms (Windows, macOS, Linux).
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// 初始化主視窗的新實例。
    /// Initializes a new instance of the main window.
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }
}