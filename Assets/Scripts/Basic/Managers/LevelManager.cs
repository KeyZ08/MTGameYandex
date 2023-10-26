using Game;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class LevelManager
{
    public static int LastLevel
    {
        get
        {
            var last = PlayerPrefs.GetInt("LastLevel", 0);
            if (last < 0 || last > Map.Levels.Count) throw new ArgumentOutOfRangeException();
            return last;
        }
        set
        {
            if (value < 0 || value > Map.Levels.Count) throw new ArgumentOutOfRangeException();
            PlayerPrefs.SetInt("LastLevel", value);
        }
    }

    public static int ActiveLevel
    {
        get 
        {
            var level = PlayerPrefs.GetInt("ActiveLevel", int.MinValue);
            if (level == int.MinValue)
            {
                PlayerPrefs.SetInt("ActiveLevel", 0);
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
            PlayerPrefs.SetInt("ActiveLevel", value);
        }
    }

    public static int LastTraining
    {
        get 
        {
            var tr = PlayerPrefs.GetInt("LastTraining", -1);
            return tr;
        }
        set
        {
            if (value < 0 || value > Map.Levels.Count) throw new ArgumentOutOfRangeException();
            if (value == Map.Levels.Count) value -= 1;
            PlayerPrefs.SetInt("LastTraining", value);
        }
    }

    public static int LastInteractiveTraining
    {
        get 
        {
            var tr = PlayerPrefs.GetInt("LastInteractiveTraining", -1);
            return tr;
        }
        set
        {
            if (value < 0 || value > Map.Levels.Count) throw new ArgumentOutOfRangeException();
            if (value == Map.Levels.Count) value -= 1;
            PlayerPrefs.SetInt("LastInteractiveTraining", value);
        }
    }

    public static string InfinityGameSettings
    {
        get 
        {
            var tr = PlayerPrefs.GetString("InfinityGameSettings", null);
            return tr;
        }
        set
        {
            PlayerPrefs.SetString("InfinityGameSettings", value);
        }
    }
}
