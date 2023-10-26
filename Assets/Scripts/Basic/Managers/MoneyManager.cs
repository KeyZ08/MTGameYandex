using System;
using UnityEngine;

public static class MoneyManager
{
    public static int Money
    {
        get { return GetMoney(); }
        private set { Save(value); }
    }

    public static void Add(int count)
    {
        if (count < 0) throw new ArgumentException("Значение не должно быть < 0");
        Money += count;
    }

    public static void Remove(int count)
    {
        if (count < 0) throw new ArgumentException("Значение не должно быть < 0");
        if (count > Money) throw new ArgumentException("Нельзя вычесть больше чем есть на данный момент");
        Money -= count;
    }

    private static void Save(int value)
    {
        PlayerPrefs.SetInt("PlayerMoney", value);
    }

    private static int GetMoney()
    {
        return PlayerPrefs.GetInt("PlayerMoney", 0);
    }
}