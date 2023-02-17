using System.IO.Hashing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Nox.Utilities.Identifier;

    //Represents a Nox unique identifier
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
public partial struct Nuid: IComparable, IComparable<Nuid>, IEquatable<Nuid>
{
    internal readonly int _id = 0;

    public Nuid(string input)
    {
        _id = ToInt32(input);
    }

    public int ToInt32()
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
        return BitConverter.ToInt32(hash);
    }
    
    public int CompareTo(object? value)
    {
        if (value == null) return 1;
        if (!(value is Nuid))
        {
            throw new ArgumentException("Object must be of type NUID.", nameof(value));
        }

        return (int)value != _id ? 1 : 0;
    }

    public int CompareTo(Nuid other)
    {
        return other.ToInt32() != _id ? 1 : 0;
    }

    public bool Equals(Nuid other)
    {
        return _id == other._id;
    }

    public override int GetHashCode()
    {
        return _id;
    }
    
    public static bool operator ==(Nuid a, Nuid b) => EqualsCore(a, b);

    public static bool operator !=(Nuid a, Nuid b) => !EqualsCore(a, b);
    
    private static bool EqualsCore(in Nuid left, in Nuid right)
    {
        ref int rA = ref Unsafe.AsRef(in left._id);
        ref int rB = ref Unsafe.AsRef(in right._id);

        // Compare each element

        return rA == rB;
    }
}