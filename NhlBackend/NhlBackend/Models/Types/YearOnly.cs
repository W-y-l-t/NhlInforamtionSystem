using System.Globalization;

namespace NhlBackend.Models.Types;

public struct YearOnly
{
    public int Year { get; }

    public YearOnly(int year)
    {
        if (year is < 1 or > 9999)
            throw new ArgumentOutOfRangeException(nameof(year), "Year must be between 1 and 9999.");
        
        Year = year;
    }

    public static bool TryParseExact(
        string s, string format, IFormatProvider provider, DateTimeStyles styles, out YearOnly result)
    {
        result = default;

        if (!int.TryParse(s, NumberStyles.None, provider, out var year))
        {
            return false;
        }
        
        result = new YearOnly(year);
            
        return true;
    }

    public override string ToString() => Year.ToString("D4");

    public bool Equals(YearOnly other) => Year == other.Year;
    public override bool Equals(object? obj) => obj is YearOnly other && Equals(other);
    public override int GetHashCode() => Year.GetHashCode();

    public static YearOnly Parse(string s)
    {
        if (TryParseExact(s, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var y))
        {
            return y;
        }
        
        throw new FormatException("Invalid YearOnly format. Expected 'yyyy'.");
    }
}