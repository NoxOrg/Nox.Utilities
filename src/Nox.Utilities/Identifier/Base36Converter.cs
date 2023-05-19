using System.Text;

namespace Nox.Utilities.Identifier;

public static class Base36Converter
{
    private const int Base = 36;
    private const string Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public static string ToBase36(int value)
    {
        uint unsignedValue = (uint)value;

        var result = new StringBuilder(64);

        while (unsignedValue > 0)
        {
            result.Append( Chars[(int)(unsignedValue % Base)] );
            unsignedValue /= Base;
        }

        return result.ToString();
    }
}