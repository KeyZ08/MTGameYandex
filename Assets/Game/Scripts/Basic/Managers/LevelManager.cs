using Game;
using System;
using YG;

public static class LevelManager
{
    public static int LastLevel
    {
        get
        {
            var last = YandexGame.savesData.LastLevel;
            if (last < 0 || last > Map.Levels.Count) throw new ArgumentOutOfRangeException();
            return last;
        }
        set
        {
            if (value < 0 || value > Map.Levels.Count) throw new ArgumentOutOfRangeException();
            YandexGame.savesData.LastLevel = value;
            YandexGame.SaveProgress();
        }
    }

    public static int ActiveLevel
    {
        get 
        {
            var level = YandexGame.savesData.ActiveLevel;
            if (level == int.MinValue)
            {
                YandexGame.savesData.ActiveLevel = 0;
                YandexGame.SaveProgress();
                return 0;
            }
            if (level < 0 || level > Map.Levels.Count) throw new ArgumentOutOfRangeException();
            if (level == Map.Levels.Count) level -= 1;
            return level;
        }
        set
        {
            if (value < 0 || value > Map.Levels.Count) throw new ArgumentOutOfRangeException();
            if (value == Map.Levels.Count) value -= 1;
            YandexGame.savesData.ActiveLevel = value;
            YandexGame.SaveProgress();
        }
    }

    public static int LastTraining
    {
        get 
        {
            var tr = YandexGame.savesData.LastTraining;
            return tr;
        }
        set
        {
            if (value < 0 || value > Map.Levels.Count) throw new ArgumentOutOfRangeException();
            if (value == Map.Levels.Count) value -= 1;
            YandexGame.savesData.LastTraining = value;
            YandexGame.SaveProgress();
        }
    }

    public static int LastInteractiveTraining
    {
        get 
        {
            var tr = YandexGame.savesData.LastInteractiveTraining;
            return tr;
        }
        set
        {
            if (value < 0 || value > Map.Levels.Count) throw new ArgumentOutOfRangeException();
            if (value == Map.Levels.Count) value -= 1;
            YandexGame.savesData.LastInteractiveTraining = value;
            YandexGame.SaveProgress();
        }
    }

    public static string InfinityGameSettings
    {
        get 
        {
            var tr = YandexGame.savesData.InfinityGameSettings;
            return tr;
        }
        set
        {
            YandexGame.savesData.InfinityGameSettings = value;
            YandexGame.SaveProgress();
        }
    }
}
