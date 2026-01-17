using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Solitaire.Models;

namespace Solitaire.Converters;

/// <summary>
/// Converter to get the brush for a playing card.
/// 撲克牌圖像轉換器。根據牌的類型取得對應的 DrawingImage。
/// 使用快取字典來提升效能，避免重複查找資源。
/// </summary>
public class PlayingCardToBrushConverter : IValueConverter
{
    // /// <summary>
    // /// A dictionary of brushes for card types.
    // /// </summary>
    /// <summary>
    /// 牌類型圖像的快取字典。
    /// Cache dictionary for card type images.
    /// </summary>
    private static readonly Dictionary<string, DrawingImage> Brushes = new();
    //
    // public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    // {
    //     //  Cast the data.
    //     if (values[0] is not CardType cardType || values[1] is not bool isFaceDown) return null;
    //
    //     var cardName = cardType.ToString();
    //     
    //     if (Brushes.TryGetValue(isFaceDown ? "CardBack" : cardName, out var retDrawingImage))
    //     {
    //         return retDrawingImage;
    //     }
    //     
    //     if (isFaceDown && Application.Current!.TryFindResource("CardBack", out var test1)
    //                    && test1 is DrawingImage backImage)
    //     {
    //         Brushes.Add("CardBack", backImage);
    //         return backImage;
    //     }
    //
    //     if (!Application.Current!.TryFindResource(cardName, out var test) ||
    //         test is not DrawingImage faceImage) return null;
    //     
    //     Brushes.Add(cardName, faceImage);
    //     return faceImage;
    // }


    /// <inheritdoc />
    /// <summary>
    /// 將 CardType 轉換為對應的 DrawingImage。
    /// Converts CardType to corresponding DrawingImage.
    /// </summary>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not CardType cardType) return null;

        var cardName = cardType.ToString();

        // 先檢查快取
        // Check cache first
        if (Brushes.TryGetValue( cardName, out var retDrawingImage))
        {
            return retDrawingImage;
        }
 
        // 從應用程式資源查找圖像
        // Find image from application resources
        if (!Application.Current!.TryFindResource(cardName, out var test) ||
            test is not DrawingImage faceImage) return null;

        // 加入快取
        // Add to cache
        Brushes.Add(cardName, faceImage);
        
        return faceImage;
    }

    /// <inheritdoc />
    /// <summary>
    /// 反向轉換（未實作）。
    /// Converts back (not implemented).
    /// </summary>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}