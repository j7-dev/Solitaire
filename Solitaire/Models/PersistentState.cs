using System;

namespace Solitaire.Models;

/// <summary>
/// 設定狀態記錄。包含遊戲的難度和抽牌模式設定。
/// Settings state record. Contains game difficulty and draw mode settings.
/// </summary>
/// <param name="Difficulty">難度等級 (Difficulty level)</param>
/// <param name="DrawMode">抽牌模式 (Draw mode)</param>
public record SettingsState(
    Difficulty Difficulty,
    DrawMode DrawMode);

/// <summary>
/// 遊戲統計狀態記錄。追蹤各種遊戲統計數據。
/// Game statistics state record. Tracks various game statistics.
/// </summary>
/// <param name="GamesPlayed">已遊玩場數 (Games played)</param>
/// <param name="GamesWon">獲勝場數 (Games won)</param>
/// <param name="GamesLost">失敗場數 (Games lost)</param>
/// <param name="HighestWinningStreak">最高連勝紀錄 (Highest winning streak)</param>
/// <param name="HighestLosingStreak">最高連敗紀錄 (Highest losing streak)</param>
/// <param name="CurrentStreak">目前連勝/連敗 (Current streak)</param>
/// <param name="CumulativeScore">累計分數 (Cumulative score)</param>
/// <param name="HighestScore">最高分數 (Highest score)</param>
/// <param name="AverageScore">平均分數 (Average score)</param>
/// <param name="CumulativeGameTime">累計遊戲時間 (Cumulative game time)</param>
/// <param name="AverageGameTime">平均遊戲時間 (Average game time)</param>
public record GameStatisticsState(
    int GamesPlayed,
    int GamesWon,
    int GamesLost,
    int HighestWinningStreak,
    int HighestLosingStreak,
    int CurrentStreak,
    int CumulativeScore,
    int HighestScore,
    double AverageScore,
    TimeSpan CumulativeGameTime,
    TimeSpan AverageGameTime);

/// <summary>
/// 持久化狀態記錄。包含所有需要在應用程式重啟後保留的資料。
/// Persistent state record. Contains all data that needs to persist across app restarts.
/// </summary>
/// <param name="Settings">設定狀態 (Settings state)</param>
/// <param name="KlondikeStatsInstance">克朗代克接龍統計 (Klondike solitaire statistics)</param>
/// <param name="SpiderStatsInstance">蜘蛛接龍統計 (Spider solitaire statistics)</param>
/// <param name="FreeCellStatsInstance">空當接龍統計 (FreeCell solitaire statistics)</param>
public record PersistentState(
    SettingsState Settings,
    GameStatisticsState KlondikeStatsInstance,
    GameStatisticsState SpiderStatsInstance,
    GameStatisticsState FreeCellStatsInstance);