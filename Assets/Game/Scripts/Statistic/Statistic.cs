using System;
using System.Collections.Generic;
using System.Linq;
using UnityJSON;
using YG;

public static class Statistic
{
    private static List<StatisticDay> _statistics;

    public static List<StatisticDay> Statistics
    {
        get { return GetStatistic(); }
    }

    public static StatisticDay Current
    {
        get 
        {
            var day = Statistics.Where(x=>x.Date == DateTime.Now.Date).FirstOrDefault();
            if (day == null)
            {
                day = new StatisticDay(DateTime.Now.Date);
                _statistics.Add(day);
                Save();
            }
            return day;
        }
    }

    private static List<StatisticDay> GetStatistic()
    {
        var value = YandexGame.savesData.Statistic;
        if (value == null || value == "")
        {
            _statistics = new List<StatisticDay>();
            Save();
        }
        else
        {
            _statistics = JSON.Deserialize<List<StatisticDay>>(value);
            if (_statistics.Count > 7)
            {
                int rm_nums = _statistics.Count - 7;
                for (var i = 0; i < rm_nums; i++)
                {
                    _statistics.RemoveAt(0);
                }
            }
        }
        return _statistics;
    }

    public static void Save()
    {
        var json = JSON.Serialize(_statistics);
        YandexGame.savesData.Statistic = json;
        YandexGame.SaveProgress();
    }
}


/// <summary>
/// Содержит данные о статистике за день,
/// каждый индекс в массиве соответсвует числу умножения 
/// </summary>
public class StatisticDay
{
    public JsonDateTime Date;
    //int[11]
    public int[] Right;
    public int[] Incorrect;
    // фактическое количество предметов
    public int RightCount; 
    public int IncorrectCount;

    public StatisticDay() { }

    public StatisticDay(DateTime date)
    {
        Date = date.Date;
        Right = new int[11];
        Incorrect = new int[11];
    }

    public void AddRight(int[] num)
    {
        var hash = new HashSet<int>();
        for (int i = 0; i < num.Length; i++)
        {
            if (num[i] < 0 || num[i] > 10) continue;
            if (!hash.Contains(i))
            {
                Right[num[i]] += 1;
                hash.Add(i);
            }
        }
        RightCount += 1;
        Statistic.Save();
    }

    public void AddIncorrect(int[] num)
    {
        var hash = new HashSet<int>();
        for (int i = 0; i < num.Length; i++)
        {
            if (num[i] < 0 || num[i] > 10) continue;
            if (!hash.Contains(i))
            {
                Incorrect[num[i]] += 1;
                hash.Add(i);
            }
        }
        IncorrectCount += 1;
        Statistic.Save();
    }

    public override string ToString()
    {
        return $"Date: {Date}, Right: [{string.Join(", ", Right)}], Incorrect: [{string.Join(", ", Incorrect)}], RightCount: {RightCount}, IncorrectCount: {IncorrectCount}";
    }
}