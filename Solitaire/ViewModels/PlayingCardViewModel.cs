using CommunityToolkit.Mvvm.ComponentModel;
using Solitaire.Models;

namespace Solitaire.ViewModels;

/// <summary>
/// The Playing Card represents a Card played in a game - so as
/// well as the card type it also has the face down property etc.
/// 撲克牌視圖模型代表遊戲中的一張牌，不僅包含牌的類型，還包含是否面朝下等屬性。
/// </summary>
public partial class PlayingCardViewModel : ViewModelBase
{
    /// <summary>
    /// 卡片遊戲實例的引用。用於訪問遊戲狀態和邏輯。
    /// Reference to the card game instance. Used to access game state and logic.
    /// </summary>
    public CardGameViewModel? CardGameInstance { get; }

    public PlayingCardViewModel(CardGameViewModel? cardGameInstance)
    {
        CardGameInstance = cardGameInstance;
    }
        
    /// <summary>
    /// Gets the card suit.
    /// 取得牌的花色。
    /// </summary> 
    /// <value>The card suit. 牌的花色。</value>
    public CardSuit Suit
    {
        get
        {
            //  The suit can be worked out from the numeric value of the CardType enum.
            //  花色可以從 CardType 列舉的數值計算出來。
            var enumVal = (int)CardType;
            return enumVal switch
            {
                < 13 => CardSuit.Hearts,    // 0-12: 紅心
                < 26 => CardSuit.Diamonds,  // 13-25: 方塊
                < 39 => CardSuit.Clubs,     // 26-38: 梅花
                _ => CardSuit.Spades        // 39-51: 黑桃
            };
        }
    }

    /// <summary>
    /// Gets the card value.
    /// 取得牌的點數（0-12，對應 A-K）。
    /// </summary>
    /// <value>The card value. 牌的點數。</value>
    public int Value =>
        //  The CardType enum has 13 cards in each suit.
        //  CardType 列舉中每種花色有 13 張牌。
        (int)CardType % 13;

    /// <summary>
    /// Gets the card colour.
    /// 取得牌的顏色。
    /// </summary>
    /// <value>The card colour. 牌的顏色。</value>
    public CardColour Colour =>
        //  The first two suits in the CardType enum are red, the last two are black.
        //  CardType 列舉中前兩種花色是紅色（紅心、方塊），後兩種是黑色（梅花、黑桃）。
        (int)CardType < 26 ? CardColour.Red : CardColour.Black;

    /// <summary>
    /// 牌的類型（如紅心 A、黑桃 K 等）。
    /// The type of the card (e.g., Ace of Hearts, King of Spades, etc.).
    /// </summary>
    [ObservableProperty]  private CardType _cardType  = CardType.SA;
    
    /// <summary>
    /// 是否面朝下。
    /// Whether the card is face down.
    /// </summary>
    [ObservableProperty] private bool _isFaceDown;
    
    /// <summary>
    /// 是否可以被移動或點擊。
    /// Whether the card can be moved or clicked.
    /// </summary>
    [ObservableProperty] private bool _isPlayable;
    
    /// <summary>
    /// 面朝下時的偏移量（用於視覺堆疊效果）。
    /// Offset when face down (for visual stacking effect).
    /// </summary>
    [ObservableProperty] private double _faceDownOffset;
    
    /// <summary>
    /// 面朝上時的偏移量（用於視覺堆疊效果）。
    /// Offset when face up (for visual stacking effect).
    /// </summary>
    [ObservableProperty] private double _faceUpOffset;
 
    /// <summary>
    /// 重置牌的狀態到初始值。
    /// Resets the card state to initial values.
    /// </summary>
    public void Reset()
    {
        IsPlayable = false;
        IsFaceDown = true;
        FaceDownOffset = 0;
        FaceUpOffset = 0;
    }
 
}