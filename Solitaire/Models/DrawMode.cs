namespace Solitaire.Models;

/// <summary>
/// The DrawMode, i.e. how many cards to draw.
/// 抽牌模式，即每次抽取的牌數。
/// </summary>
public enum DrawMode
{
    /// <summary>
    /// Draw one card.
    /// 每次抽一張牌。
    /// </summary>
    DrawOne = 1,

    /// <summary>
    /// Draw three cards.
    /// 每次抽三張牌。
    /// </summary>
    DrawThree = 3
}