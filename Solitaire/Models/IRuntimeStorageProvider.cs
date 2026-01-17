using System.Threading.Tasks;

namespace Solitaire.Models;

/// <summary>
/// 運行時存儲提供者介面。用於定義存儲和載入物件的方法。
/// Runtime storage provider interface. Defines methods for saving and loading objects.
/// </summary>
/// <typeparam name="T">要存儲的物件類型 (The type of object to store)</typeparam>
public interface IRuntimeStorageProvider<T>
{
    /// <summary>
    /// 將物件存儲到指定的鍵。
    /// Saves an object to the specified key.
    /// </summary>
    /// <param name="obj">要存儲的物件 (The object to save)</param>
    /// <param name="key">存儲鍵 (The storage key)</param>
    Task SaveObject(T obj, string key);
    
    /// <summary>
    /// 從指定的鍵載入物件。
    /// Loads an object from the specified key.
    /// </summary>
    /// <param name="key">存儲鍵 (The storage key)</param>
    /// <returns>載入的物件，如果不存在則返回 null (The loaded object, or null if not found)</returns>
    Task<T?> LoadObject(string key);
}