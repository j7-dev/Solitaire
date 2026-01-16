using System.Text.Json.Serialization;
using Solitaire.Models;

namespace Solitaire;

/// <summary>
/// JSON 序列化上下文。使用 System.Text.Json 的原始碼生成功能。
/// JSON serialization context. Uses System.Text.Json's source generation feature.
/// 提供高效能的 JSON 序列化/反序列化，並支援 AOT 編譯。
/// Provides high-performance JSON serialization/deserialization and supports AOT compilation.
/// </summary>
[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,  // 使用駝峰命名法 (Use camelCase naming)
    IncludeFields = true,      // 包含欄位 (Include fields)
    WriteIndented = false)]    // 不縮排輸出（減少檔案大小）(No indentation - reduces file size)
[JsonSerializable(typeof(PersistentState))]  // 註冊要序列化的類型 (Register type to serialize)
public partial class JsonContext : JsonSerializerContext
{
    
}