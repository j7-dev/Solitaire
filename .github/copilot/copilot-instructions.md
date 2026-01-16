# GitHub Copilot 指令

## 優先準則

為此代碼庫生成代碼時：

1. **版本相容性**：始終檢測並遵守此專案中使用的語言、框架和程式庫的確切版本
2. **上下文檔案**：優先使用 .github/copilot 目錄中定義的模式和標準
3. **代碼庫模式**：當上下文檔案未提供特定指引時，掃描代碼庫以尋找已建立的模式
4. **架構一致性**：維護我們的 MVVM 架構風格和已建立的邊界
5. **代碼品質**：在所有生成的代碼中優先考慮可維護性、效能、安全性和可測試性

## 技術版本檢測

在生成代碼之前，掃描代碼庫以識別：

1. **語言版本**：檢測使用中的程式語言確切版本
   - 檢查專案檔案、配置檔案和套件管理器
   - 尋找特定語言的版本指示器（例如 .NET 專案中的 `<LangVersion>`）
   - 絕不使用超出檢測版本的語言功能

2. **框架版本**：識別所有框架的確切版本
   - 檢查 .csproj、global.json、Directory.Build.props 等
   - 生成與這些特定版本相容的代碼
   - 絕不建議使用檢測到的框架版本中不可用的功能

3. **程式庫版本**：記錄關鍵程式庫和相依性的確切版本
   - 生成與這些特定版本相容的代碼
   - 絕不使用檢測版本中不可用的 API 或功能

## 專案技術棧

此專案使用以下技術和版本（基於代碼庫分析）：

### 核心技術
- **.NET SDK**: 9.0.0
- **C# 語言版本**: latest（.NET 9.0 支援的最新版本）
- **目標框架**: net9.0
- **可空性**: 啟用（`<Nullable>enable</Nullable>`）

### 框架和程式庫
- **Avalonia UI**: 11.3.2（跨平台 UI 框架）
- **CommunityToolkit.Mvvm**: 8.0.0（MVVM 模式支援）
- **Avalonia.Xaml.Interactivity**: 11.0.0（行為支援）

### 平台支援
此專案為跨平台應用程式，支援：
- Windows Desktop
- macOS
- Linux
- iOS
- Android
- WebAssembly (Browser)
- AndroidTV
- tvOS
- VisionOS

## 上下文檔案

優先使用 .github/copilot 目錄中的以下檔案（如果存在）：

- **architecture.md**: 系統架構準則
- **tech-stack.md**: 技術版本和框架詳細資訊
- **coding-standards.md**: 代碼風格和格式化標準
- **folder-structure.md**: 專案組織準則
- **exemplars.md**: 要遵循的範例代碼模式

## 代碼庫掃描指令

當上下文檔案未提供特定指引時：

1. 識別與正在修改或創建的檔案相似的檔案
2. 分析以下模式：
   - 命名慣例
   - 代碼組織
   - 錯誤處理
   - 日誌記錄方法
   - 文檔風格
   - 測試模式
   
3. 遵循代碼庫中找到的最一致的模式
4. 當存在衝突的模式時，優先使用較新檔案或具有更高測試覆蓋率的檔案中的模式
5. 絕不引入代碼庫中未找到的模式

## 架構模式

### MVVM 架構

此專案嚴格遵循 Model-View-ViewModel (MVVM) 架構模式：

#### 資料夾結構
```
Solitaire/
├── Models/          # 業務邏輯和資料模型
├── Views/           # AXAML UI 定義檔案
├── ViewModels/      # 視圖模型（UI 邏輯和狀態）
├── Controls/        # 自訂 UI 控制項
├── Converters/      # 值轉換器
├── Behaviors/       # UI 行為
├── Utils/           # 工具類別和助手
├── Assets/          # 資源檔案（圖片、字型等）
└── Styles/          # 樣式定義
```

#### ViewModel 模式
- 所有 ViewModel 必須繼承自 `ViewModelBase`
- 使用 `CommunityToolkit.Mvvm` 的 `[ObservableObject]` 屬性
- 使用 `[ObservableProperty]` 屬性用於可觀察屬性
- 使用 partial class 以支援原始碼生成器
- ViewModel 不應直接參照 View

範例模式：
```csharp
using CommunityToolkit.Mvvm.ComponentModel;

namespace Solitaire.ViewModels;

[ObservableObject]
public abstract partial class ViewModelBase 
{
}
```

```csharp
public partial class CasinoViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase? _currentView;
}
```

#### 命令模式
- 使用 `CommunityToolkit.Mvvm` 的 `RelayCommand` 或 `AsyncRelayCommand`
- 在建構函式中初始化命令
- 使用 `ICommand` 介面類型

範例：
```csharp
public ICommand NavigateToTitleCommand { get; }

public CasinoViewModel()
{
    NavigateToTitleCommand = new RelayCommand(() =>
    {
        CurrentView = TitleInstance;
        Save();
    });
}
```

### 模型模式
- 模型應該是簡單的資料類別或列舉
- 使用 `record` 類型用於不可變資料
- 為公共 API 提供 XML 文檔註釋

範例：
```csharp
namespace Solitaire.Models;

/// <summary>
/// Represents a card's suit.
/// </summary>
public enum CardSuit
{
    /// <summary>
    /// Hearts. 
    /// </summary>
    Hearts,

    /// <summary>
    /// Diamonds.
    /// </summary>
    Diamonds,

    /// <summary>
    /// Clubs.
    /// </summary>
    Clubs,

    /// <summary>
    /// Spades.
    /// </summary>
    Spades
}
```

### 控制項模式
- 自訂控制項應繼承自適當的 Avalonia 基礎類別
- 使用 `TemplatedControl` 用於完全可自訂的控制項
- 保持控制項簡單且可重複使用

範例：
```csharp
using Avalonia.Controls.Primitives;

namespace Solitaire.Controls;

public class PlayingCard : TemplatedControl
{
}
```

## 代碼品質標準

### 可維護性
- 撰寫具有清晰命名的自文檔化代碼
- 遵循代碼庫中明顯的命名和組織慣例
- 遵循已建立的模式以保持一致性
- 保持函式專注於單一職責
- 限制函式複雜度和長度以匹配現有模式

### 效能
- 遵循現有的記憶體和資源管理模式
- 匹配現有處理計算密集操作的模式
- 遵循已建立的非同步操作模式
- 一致地應用快取以匹配現有模式
- 根據代碼庫中明顯的模式進行優化

### 安全性
- 遵循現有的輸入驗證模式
- 應用代碼庫中使用的相同清理技術
- 使用與現有模式匹配的參數化查詢
- 遵循已建立的驗證和授權模式
- 根據現有模式處理敏感資料

### 可測試性
- 遵循已建立的可測試代碼模式
- 匹配代碼庫中使用的依賴注入方法
- 應用相同的管理依賴關係模式
- 遵循已建立的模擬和測試替身模式
- 匹配現有測試中使用的測試風格

## 命名慣例

基於代碼庫分析，遵循這些命名慣例：

### 命名空間
- 使用 `Solitaire` 作為根命名空間
- 子命名空間反映資料夾結構：`Solitaire.Models`、`Solitaire.ViewModels`、`Solitaire.Views` 等
- 使用 PascalCase

### 類別和介面
- 使用 PascalCase
- ViewModel 類別應以 `ViewModel` 結尾（例如：`CasinoViewModel`、`CardGameViewModel`）
- 介面應以 `I` 開頭（例如：`IRuntimeStorageProvider`、`ICommand`）
- 抽象類別無特定後綴，但清楚標示為 `abstract`

### 方法和屬性
- 使用 PascalCase
- 屬性應使用名詞或名詞片語
- 方法應使用動詞或動詞片語
- 布林屬性可以以 `Is`、`Has`、`Can` 開頭

### 欄位
- 私有欄位使用 camelCase，並以下底線開頭（例如：`_currentView`、`_moveStack`）
- 公共欄位使用 PascalCase（罕見，優先使用屬性）
- 常數使用 PascalCase

### 參數和區域變數
- 使用 camelCase
- 使用描述性名稱
- 避免單字母變數名稱，除非在簡短的 lambda 或 LINQ 查詢中

### 檔案命名
- 檔案名稱應與主要類別名稱匹配
- 使用 `.axaml` 用於 Avalonia XAML 檔案
- 使用 `.axaml.cs` 用於 XAML code-behind 檔案
- 使用 `.cs` 用於純 C# 檔案

## 文檔要求

### XML 文檔註釋
- 為所有公共類別、介面、方法、屬性和列舉提供 XML 文檔註釋
- 使用 `<summary>` 標籤描述目的
- 使用 `<param>` 標籤記錄參數
- 使用 `<returns>` 標籤記錄返回值
- 使用 `<value>` 標籤用於屬性
- 使用 `<exception>` 標籤記錄可能拋出的例外

範例模式：
```csharp
/// <summary>
/// Base class for a ViewModel for a card game.
/// </summary>
public abstract partial class CardGameViewModel : ViewModelBase
{
    /// <summary>
    /// Gets the card suit.
    /// </summary> 
    /// <value>The card suit.</value>
    public CardSuit Suit { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CardGameViewModel"/> class.
    /// </summary>
    protected CardGameViewModel(CasinoViewModel casinoViewModel)
    {
    }
}
```

### 程式碼註釋
- 僅在解釋非顯而易見的行為時使用行內註釋
- 避免說明顯而易見的內容的註釋
- 使用註釋解釋「為什麼」而不是「什麼」
- 保持註釋簡潔且相關

範例：
```csharp
//  The suit can be worked out from the numeric value of the CardType enum.
var enumVal = (int)CardType;
```

## .NET 特定準則

### C# 語言功能
- 檢測並嚴格遵守使用中的特定 .NET 版本（.NET 9.0）
- 僅使用與檢測版本相容的 C# 語言功能
- 啟用可空參考類型（`<Nullable>enable</Nullable>`）
- 適當使用可空性註釋（`?`、`!`）

### 非同步/等待模式
- 使用 `async`/`await` 用於 I/O 綁定操作
- 非同步方法應返回 `Task` 或 `Task<T>`
- 避免 `async void`，除非用於事件處理程式
- 使用 `ConfigureAwait(false)` 時要謹慎（在 UI 應用程式中通常不需要）

範例模式：
```csharp
public static async Task<CasinoViewModel> CreateOrLoadFromDisk()
{
    var ret = new CasinoViewModel();
    var state = await PlatformProviders.CasinoStorage.LoadObject("mainSettings");
    if (state is not null)
    {
        ret.SettingsInstance.ApplyState(state.Settings);
    }
    return ret;
}
```

### LINQ 使用
- 遵循代碼庫中出現的 LINQ 使用模式
- 優先使用方法語法而不是查詢語法（基於代碼庫觀察）
- 適當使用延遲執行
- 考慮複雜查詢的效能影響

### 集合類型
- 使用 `ImmutableArray<T>` 用於不可變集合
- 使用 `List<T>` 用於可變列表
- 使用 `Stack<T>` 用於堆疊操作
- 使用 `Dictionary<TKey, TValue>` 用於鍵值對
- 考慮使用集合表達式（C# 12+）

範例：
```csharp
public ImmutableArray<PlayingCardViewModel>? Deck;
private readonly Stack<CardOperation[]> _moveStack = new();
```

### 依賴注入
- 遵循代碼庫中明顯的依賴注入方法
- 透過建構函式注入依賴關係
- 使用介面進行抽象
- 避免服務定位器模式

範例：
```csharp
public PlayingCardViewModel(CardGameViewModel? cardGameInstance)
{
    CardGameInstance = cardGameInstance;
}
```

### 錯誤處理
- 遵循現有的錯誤處理模式
- 使用可空性而不是例外用於預期的失敗情況
- 為意外錯誤保留例外
- 提供有意義的錯誤訊息

## Avalonia UI 特定準則

### AXAML 檔案
- 使用 `.axaml` 副檔名用於 Avalonia XAML 檔案
- 遵循現有的 XAML 結構和命名空間
- 使用數據綁定連接 View 和 ViewModel
- 啟用編譯綁定：`<AvaloniaUseCompiledBindingsByDefault>true</AvaloniaUseCompiledBindingsByDefault>`

### 控制項開發
- 繼承自適當的 Avalonia 基礎類別
- 使用 `TemplatedControl` 用於自訂控制項
- 遵循現有控制項的模式

### 樣式和主題
- 將樣式放在 `Styles/` 目錄中
- 使用資源字典進行共享樣式
- 遵循現有的主題模式

### 資源管理
- 將資產放在 `Assets/` 目錄中
- 在專案檔案中使用 `<AvaloniaResource>` 包含資產
- 遵循現有的資源命名慣例

## 版本控制準則

### Git 提交訊息
- 使用清晰、描述性的提交訊息
- 以動詞開頭（Add、Update、Fix、Remove 等）
- 保持第一行簡短（50 個字元或更少）
- 如果需要，提供詳細的描述

### 分支策略
- 遵循專案使用的分支策略
- 為功能和修復使用描述性分支名稱

## 一般最佳實務

### 代碼組織
- 遵循現有代碼中出現的命名慣例
- 匹配相似檔案的代碼組織模式
- 應用與現有模式一致的錯誤處理
- 遵循代碼庫中看到的相同測試方法
- 匹配現有代碼的日誌記錄模式
- 使用與代碼庫中看到的相同配置方法

### 跨平台考慮
- 記住此應用程式在多個平台上執行
- 避免平台特定的代碼，除非在平台特定的專案中
- 使用 Avalonia 的跨平台 API
- 在適用的情況下測試多個平台

### 效能考慮
- 對 UI 操作使用非同步模式
- 避免在 UI 執行緒上進行阻塞操作
- 適當使用 `Dispatcher.UIThread.InvokeAsync`
- 考慮大型集合的記憶體使用

範例：
```csharp
Dispatcher.UIThread.InvokeAsync(async () =>
{
    desktop.MainWindow.DataContext = await CasinoViewModel.CreateOrLoadFromDisk();
});
```

## 專案特定指引

### 遊戲邏輯
- 遊戲邏輯應在 ViewModel 中
- 使用命令模式進行使用者互動
- 維護可撤銷操作的狀態
- 使用堆疊進行撤銷/重做功能

### 狀態管理
- 使用 `PersistentState` 進行應用程式狀態持久化
- 透過 `PlatformProviders` 保存和載入狀態
- 實作 `GetState()` 和 `ApplyState()` 方法用於可序列化狀態

### 平台提供者
- 使用 `PlatformProviders` 進行平台特定功能
- 實作 `IRuntimeStorageProvider` 用於儲存
- 保持平台特定代碼隔離

## 重要提醒

1. **版本相容性優先**：始終確保代碼與檢測到的版本（.NET 9.0、Avalonia 11.3.2、C# latest）相容
2. **一致性優於最佳實務**：當有疑問時，優先考慮與現有代碼的一致性，而不是外部最佳實務或較新的語言功能
3. **遵循 MVVM 嚴格**：維護 Model、View 和 ViewModel 之間的清晰分離
4. **使用 CommunityToolkit.Mvvm**：利用工具包進行 MVVM 模式實作
5. **啟用可空性**：始終考慮可空參考類型並適當處理它們
6. **記錄公共 API**：為所有公共成員提供完整的 XML 文檔
7. **跨平台意識**：記住此應用程式在多個平台上執行
8. **非同步優先**：對 I/O 操作使用非同步模式
9. **不可變性**：適當使用不可變集合（`ImmutableArray<T>`）
10. **清晰命名**：使用描述性、自文檔化的名稱

## 掃描代碼庫

在生成任何代碼之前：
1. 徹底掃描代碼庫
2. 無例外地尊重現有架構邊界
3. 匹配周圍代碼的風格和模式
4. 當有疑問時，優先考慮與現有代碼的一致性
