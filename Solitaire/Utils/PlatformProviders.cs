using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using Solitaire.Models;
using Solitaire.ViewModels;

namespace Solitaire.Utils;

/// <summary>
/// 平台提供者。提供跨平台的工具方法和存儲服務。
/// Platform providers. Provides cross-platform utility methods and storage services.
/// </summary>
public static class PlatformProviders
{
    /// <summary>
    /// 產生下一個隨機雙精度浮點數（0.0 到 1.0 之間）。
    /// 使用加密安全的隨機數生成器。
    /// Generates the next random double (between 0.0 and 1.0).
    /// Uses a cryptographically secure random number generator.
    /// </summary>
    /// <returns>隨機雙精度浮點數 (Random double)</returns>
    public static double NextRandomDouble()
    {
        var nextULong = BitConverter.ToUInt64(RandomNumberGenerator.GetBytes(sizeof(ulong)));

        return (nextULong >> 11) * (1.0 / (1ul << 53));
    }
    
    /// <summary>
    /// 預設設定存儲實作。使用 IsolatedStorage 在本地存儲資料。
    /// Default settings store implementation. Uses IsolatedStorage to store data locally.
    /// </summary>
    private class DefaultSettingsStore<T> : IRuntimeStorageProvider<T>
    {
        /// <summary>
        /// 存儲識別符，基於類型全名。
        /// Storage identifier based on type full name.
        /// </summary>
        private static string Identifier { get; } = typeof(T).FullName?.Replace(".", string.Empty) ?? "default";

        
        /// <inheritdoc />
        /// <summary>
        /// 將物件序列化並保存到獨立存儲。
        /// Serializes and saves object to isolated storage.
        /// </summary>
        public async Task SaveObject(T obj, string key)
        {
            try
            {
                // Get a new isolated store for this user, domain, and assembly.
                // 取得此使用者、網域和組件的新獨立存儲。
                using var isoStore = IsolatedStorageFile.GetUserStoreForApplication();

                //  Create data stream.
                //  創建資料串流並序列化物件。
                await using var isoStream = isoStore.OpenFile(Identifier + key, FileMode.CreateNew, FileAccess.Write);
                await JsonSerializer.SerializeAsync(isoStream, obj, typeof(T), JsonContext.Default);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// 從獨立存儲載入並反序列化物件。
        /// Loads and deserializes object from isolated storage.
        /// </summary>
        public async Task<T?> LoadObject(string key)
        {
            try
            {
                // 取得獨立存儲並打開檔案
                // Get isolated storage and open file
                using var isoStore = IsolatedStorageFile.GetUserStoreForApplication();
                await using var isoStream = isoStore.OpenFile(Identifier + key, FileMode.Open, FileAccess.Read);
                // 反序列化物件
                // Deserialize object
                var storedObj = (T?)await JsonSerializer.DeserializeAsync(isoStream, typeof(T), JsonContext.Default);
                return storedObj ?? default;
            }
            catch (Exception e) when (e.InnerException is FileNotFoundException)
            {
                // Ignore - 檔案不存在是正常的（首次啟動）
                // Ignore - file not found is normal (first launch)
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return default;
        }

    }
    
    /// <summary>
    /// 賭場存儲提供者。用於保存和載入遊戲狀態。
    /// Casino storage provider. Used to save and load game state.
    /// </summary>
    public static IRuntimeStorageProvider<PersistentState> CasinoStorage { get; set; }
        = new DefaultSettingsStore<PersistentState>();
}