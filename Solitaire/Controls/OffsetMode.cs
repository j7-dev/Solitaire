namespace Solitaire.Controls;

/// <summary>
/// The offset mode - how we offset individual cards in a stack.
/// 偏移模式 - 定義如何偏移堆疊中的個別牌。
/// 用於控制牌堆疊的視覺佈局，決定哪些牌要顯示偏移效果。
/// </summary>
public enum OffsetMode
{
    /// <summary>
    /// Offset every card.
    /// 每張牌都偏移。所有牌都會有間隔顯示。
    /// </summary>
    EveryCard,

    /// <summary>
    /// Offset every Nth card.
    /// 每 N 張牌偏移一次。用於大堆疊時減少視覺擁擠。
    /// </summary>
    EveryNthCard,

    /// <summary>
    /// Offset only the top N cards.
    /// 只偏移頂部 N 張牌。堆疊底部的牌會疊在一起。
    /// </summary>
    TopNCards,

    /// <summary>
    /// Offset only the bottom N cards.
    /// 只偏移底部 N 張牌。堆疊頂部的牌會疊在一起。
    /// </summary>
    BottomNCards,

    /// <summary>
    /// Use the offset values specified in the playing card class (see
    /// PlayingCardViewModel.FaceDownOffset and PlayingCardViewModel.FaceUpOffset).
    /// 使用牌類別中指定的偏移值（見 PlayingCardViewModel.FaceDownOffset 和 PlayingCardViewModel.FaceUpOffset）。
    /// 允許每張牌有自己的偏移設定。
    /// </summary>
    UseCardValues
}