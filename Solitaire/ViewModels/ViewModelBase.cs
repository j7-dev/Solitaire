using CommunityToolkit.Mvvm.ComponentModel;

namespace Solitaire.ViewModels;

/// <summary>
/// 視圖模型基類。所有視圖模型都繼承自此類別。
/// Base class for all view models. All view models inherit from this class.
/// 使用 CommunityToolkit.Mvvm 的 ObservableObject 特性來自動實作 INotifyPropertyChanged。
/// Uses CommunityToolkit.Mvvm's ObservableObject attribute to automatically implement INotifyPropertyChanged.
/// </summary>
[ObservableObject]
public abstract partial class ViewModelBase 
{
}