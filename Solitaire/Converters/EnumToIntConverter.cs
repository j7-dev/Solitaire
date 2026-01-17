using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;

namespace Solitaire.Converters;

/// <summary>
/// 列舉到整數轉換器。將列舉值轉換為其整數表示。
/// Enum to integer converter. Converts enum values to their integer representation.
/// 用於在 XAML 中顯示列舉的數值。
/// Used to display enum numeric values in XAML.
/// </summary>
public class EnumToIntConverter : IValueConverter
{
    /// <summary>
    /// 將列舉值轉換為整數。
    /// Converts enum value to integer.
    /// </summary>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value?.GetType() is { IsEnum: true })
        {
            return (int)value;
        }
        return 0;
    }

    /// <summary>
    /// 反向轉換（未實作）。
    /// Converts back (not implemented).
    /// </summary>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}

