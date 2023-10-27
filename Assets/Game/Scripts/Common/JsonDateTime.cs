using System;

public struct JsonDateTime : IComparable<JsonDateTime>
{
    public int day;
    public int month;
    public int year;

    public static implicit operator DateTime(JsonDateTime jdt)
        => DateTime.Parse($"{jdt.year}-{jdt.month}-{jdt.day}");

    public static implicit operator JsonDateTime(DateTime dt) 
        => new JsonDateTime() { day = dt.Day, month = dt.Month, year = dt.Year, };

    public override string ToString()
    {
        return $"{year}-{month}-{day}";
    }

    public int CompareTo(JsonDateTime obj)
    {
        var num1 = year * 10000 + month * 100 + day;
        var num2 = obj.year * 10000 + obj.month * 100 + obj.day;
        return num1 - num2;
    }
}
