using System.IO.Hashing;
using System.Text;

namespace Nox.Utilities.Identifier;

public readonly struct Identifier
{
    private readonly int _id = 0;


    public Identifier(string input)
    {
        _id = ToInt32(input);
    }

    public Int32 ToInt32()
    {
        return _id;
    }

    public string ToHex()
    {
        return string.Format("{0:X}", _id).PadLeft(8,'0');
    }

    public string ToBase36()
    {
        return Base36Converter.ToBase36(_id);
    }

    public Guid ToGuid()
    {
        byte[] bytes = new byte[16];
        BitConverter.GetBytes(_id).Reverse().ToArray().CopyTo(bytes, 12);
        return new Guid(bytes);
    }

    public static int ToInt32(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = XxHash32.Hash(bytes).Reverse().ToArray();
        return BitConverter.ToInt32(hash, 0);
    }
}