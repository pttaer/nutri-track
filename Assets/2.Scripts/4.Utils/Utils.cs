using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

public class Utils
{
    public static T GetComponentAs<T>(Transform parent, string path) where T : Component
    {
        Transform transform = parent.Find(path);
        if (transform == null)
        {
            Debug.LogErrorFormat("Failed to find Transform for path '{0}'", path);
            return null;
        }
        T component = transform.GetComponent<T>();
        if (component == null)
        {
            Debug.LogErrorFormat("Failed to get Component of type '{0}' for path '{1}'", typeof(T).Name, path);
            return null;
        }
        return component;
    }

    public static string ConcatenateStrings(params string[] strings)
    {
        StringBuilder sb = new StringBuilder();
        foreach (string s in strings)
        {
            sb.Append(s);
        }
        return sb.ToString();
    }

    public static string StringConcat(string[] path)
    {
        return string.Concat(path);
    }

    public static Color ParseColorFromString(string color)
    {
        if (!color.StartsWith("#"))
        {
            color = "#" + color;
        }

        Color result;
        if (ColorUtility.TryParseHtmlString(color, out result))
        {
            return result;
        }
        return Color.white;
    }

    public static DateTime GetFirstDayOfWeek(DateTime dayInWeek)
    {
        DayOfWeek firstDay = DayOfWeek.Monday;
        DateTime firstDayInWeek = dayInWeek.Date;
        while (firstDayInWeek.DayOfWeek != firstDay)
        {
            firstDayInWeek = firstDayInWeek.AddDays(-1);
        }

        return firstDayInWeek;
    }

    public static DateTime GetFirstDayOfWeek(DateTime dayInWeek, CultureInfo cultureInfo)
    {
        DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
        DateTime firstDayInWeek = dayInWeek.Date;
        while (firstDayInWeek.DayOfWeek != firstDay)
        {
            firstDayInWeek = firstDayInWeek.AddDays(-1);
        }

        return firstDayInWeek;
    }

    #region CONVERSION

    // Conversion methods
    public static bool ToBool(object value)
    {
        if (value == null || !bool.TryParse(value.ToString(), out bool result))
        {
            throw new ArgumentException($"Failed to convert '{value}' to bool");
        }
        return result;
    }

    public static byte ToByte(object value)
    {
        if (value == null || !byte.TryParse(value.ToString(), out byte result))
        {
            throw new ArgumentException($"Failed to convert '{value}' to byte");
        }
        return result;
    }

    public static char ToChar(object value)
    {
        if (value == null || !char.TryParse(value.ToString(), out char result))
        {
            throw new ArgumentException($"Failed to convert '{value}' to char");
        }
        return result;
    }

    public static DateTime ToDateTime(object value)
    {
        if (value == null || !DateTime.TryParse(value.ToString(), out DateTime result))
        {
            throw new ArgumentException($"Failed to convert '{value}' to DateTime");
        }
        return result;
    }

    public static decimal ToDecimal(object value)
    {
        if (value == null || !decimal.TryParse(value.ToString(), out decimal result))
        {
            throw new ArgumentException($"Failed to convert '{value}' to decimal");
        }
        return result;
    }

    public static double ToDouble(object value)
    {
        if (value == null || !double.TryParse(value.ToString(), out double result))
        {
            throw new ArgumentException($"Failed to convert '{value}' to double");
        }
        return result;
    }

    public static short ToInt16(object value)
    {
        if (value == null || !short.TryParse(value.ToString(), out short result))
        {
            throw new ArgumentException($"Failed to convert '{value}' to short");
        }
        return result;
    }

    public static int ToInt32(object value)
    {
        if (value == null || !int.TryParse(value.ToString(), out int result))
        {
            throw new ArgumentException($"Failed to convert '{value}' to int");
        }
        return result;
    }

    public static long ToInt64(object value)
    {
        if (value == null || !long.TryParse(value.ToString(), out long result))
        {
            throw new ArgumentException($"Failed to convert '{value}' to long");
        }
        return result;
    }

    public static float ToSingle(object value)
    {
        if (value == null || !float.TryParse(value.ToString(), out float result))
        {
            throw new ArgumentException($"Failed to convert '{value}' to float");
        }
        return result;
    }

    public static string ToString(object value)
    {
        return value?.ToString();
    }

    public static ushort ToUInt16(object value)
    {
        if (value == null || !ushort.TryParse(value.ToString(), out ushort result))
        {
            throw new ArgumentException($"Failed to convert '{value}' to ushort");
        }
        return result;
    }

    public static uint ToUInt32(object value)
    {
        if (value == null || !uint.TryParse(value.ToString(), out uint result))
        {
            throw new ArgumentException($"Failed to convert '{value}' to uint");
        }
        return result;
    }

    public static ulong ToUInt64(object value)
    {
        if (value == null || !ulong.TryParse(value.ToString(), out ulong result))
        {
            throw new ArgumentException($"Failed to convert '{value}' to ulong");
        }
        return result;
    }

    #endregion CONVERSION

    #region LOG

    public static void Log(string message)
    {
        Console.WriteLine($"[LOG] {message}");
    }

    public static void LogWarning(string message)
    {
        Console.WriteLine($"[WARNING] {message}");
    }

    public static void LogError(string message)
    {
        Console.WriteLine($"[ERROR] {message}");
    }

    public static void LogList<T>(List<T> list)
    {
        Console.Write("[LIST] ");
        foreach (var item in list)
        {
            Console.Write($"{item}, ");
        }
        Console.WriteLine();
    }

    #endregion LOG
}