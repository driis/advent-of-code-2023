global using static System.Console;
global using System.Text.RegularExpressions;
global using System.IO;

public static class Extensions
{
    public static int ToInt(this string value)
    {
        return int.TryParse(value, out int n) ? 
            n : 
            throw new ApplicationException($"{value} could not be parsed as an integer");
    }

        public static int? ToIntNullable(this string value)
    {
        return int.TryParse(value, out int n) ? 
            n : 
            null;
    }
}

public static class Console
{
    public static void Dump<T>(IEnumerable<T> what) 
    {
        foreach(T item in what)
            WriteLine(item);
    }
}