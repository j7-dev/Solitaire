// ReSharper disable InconsistentNaming
namespace Solitaire.Models;

/// <summary>
/// The Card Types.
/// 撲克牌類型列舉。包含所有 52 張標準撲克牌（紅心、方塊、梅花、黑桃各 13 張）。
/// </summary>
public enum CardType
{
    //  Hearts（紅心）
    HA,
    H2,
    H3,
    H4,
    H5,
    H6,
    H7,
    H8,
    H9,
    H10,
    HJ,
    HQ,
    HK,

    //  Diamonds（方塊）
    DA,
    D2,
    D3,
    D4,
    D5,
    D6,
    D7,
    D8,
    D9,
    D10,
    DJ,
    DQ,
    DK,

    //  Clubs（梅花）
    CA,
    C2,
    C3,
    C4,
    C5,
    C6,
    C7,
    C8,
    C9,
    C10,
    CJ,
    CQ,
    CK,

    //  Spades（黑桃）
    SA,
    S2,
    S3,
    S4,
    S5,
    S6,
    S7,
    S8,
    S9,
    S10,
    SJ,
    SQ,
    SK
}