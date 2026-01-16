using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Solitaire.Converters;

/// <summary>
/// A converter that turns a time span into a small string, only 
/// suitable for up to 24 hours.
/// 時間跨度到短字串的轉換器。將 TimeSpan 轉換為簡短的時間字串。
/// 僅適用於 24 小時以內的時間。格式：HH:MM:SS 或 MM:SS。
/// </summary>
public class TimeSpanToShortStringConverter : IValueConverter
{
    /// <summary>
    /// Converts a value.
    /// 將 TimeSpan 轉換為短字串格式。
    /// </summary>
    /// <param name="value">The value produced by the binding source. 綁定來源產生的值。</param>
    /// <param name="targetType">The type of the binding target property. 綁定目標屬性的類型。</param>
    /// <param name="parameter">The converter parameter to use. 要使用的轉換器參數。</param>
    /// <param name="culture">The culture to use in the converter. 轉換器中使用的文化。</param>
    /// <returns>
    /// A converted value. If the method returns null, the valid null value is used.
    /// 轉換後的值。如果方法返回 null，則使用有效的 null 值。
    /// </returns>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var timeSpan = (TimeSpan)(value ?? throw new ArgumentNullException(nameof(value)));
        // 如果有小時數，顯示 HH:MM:SS；否則只顯示 MM:SS
        // If there are hours, display HH:MM:SS; otherwise display MM:SS only
        return timeSpan.Hours > 0 ? $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}" 
            : $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }

    /// <summary>
    /// Converts a value.
    /// 反向轉換（未實作）。
    /// </summary>
    /// <param name="value">The value that is produced by the binding target. 綁定目標產生的值。</param>
    /// <param name="targetType">The type to convert to. 要轉換到的類型。</param>
    /// <param name="parameter">The converter parameter to use. 要使用的轉換器參數。</param>
    /// <param name="culture">The culture to use in the converter. 轉換器中使用的文化。</param>
    /// <returns>
    /// A converted value. If the method returns null, the valid null value is used.
    /// 轉換後的值。如果方法返回 null，則使用有效的 null 值。
    /// </returns>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}