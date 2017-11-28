using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Binary : BaseNum<bool>
{
	public Binary(int len = 0)
	{
		num = new bool[len + 1];

		radix = 2;

		for (int i = 0; i < len; i++)
			num[i] = false;

		negative = false;
	}

	public Binary(Binary b)
	{
		num = new bool[b.GetLength()];

		for (int i = 0; i < b.GetLength(); i++)
			num[i] = b.num[i];

		radix = 2;

		negative = b.negative;
	}

	public static Binary ShrinkToSize(Binary result)
	{
		if (!result.GetEntry(result.GetLength() - 1) && result.GetLength() > 1)
		{
			for (int i = result.GetLength() - 1; i >= 0; i--)
			{
				if (result.GetEntry(i))
				{
					result = ConvertBase(SubBase(result, 0, i + 1));
					break;
				}
			}
		}

		return result;
	}

	public static Binary RadixComplement(Binary b)
	{
		Binary copy = ~b;
		copy++;
		return copy;
	}

	public static Binary Parse(string s)
	{
		Binary b = new Binary();
		if (s[0] == '1')
			b.negative = true;

		s = s.Substring(2, s.Length - 2);

		for (int i = s.Length - 1; i >= 0; i--)
		{
			int a = int.Parse(s[i].ToString());

			b.SetEntry(s.Length - i - 1, (a == 1) ? true : false);
		}

		return b;
	}

	public static Binary ConvertIntToBin(int n)
	{
		Binary result = new Binary();
		byte count = 0;

		if (n < 0)
		{
			result.negative = true;
			n *= -1;
		}

		while (n > 0)
		{
			int temp = n >> 1;

			int rem;
			if (temp == 0)
				rem = n;
			else
				rem = n % 2;

			bool value = (rem == 1);
			result.SetEntry(count++, value);
			n = n >> 1;
		}

		return result;
	}

	public static int ConvertBinToInt(Binary b)
	{
		int result = 0;

		for (int i = b.GetLength() - 1; i >= 0; i--)
			if (b.GetEntry(i))
				result += (int)(Math.Pow(2, i));

		if (b.isNegative())
			result *= -1;

		return result;
	}

	public static Binary ConvertHexToBin(Hex h)
	{
		int n = (int)h.GetEntry(0);
		Binary b = ConvertIntToBin(n);
		b = fill(b, 3 - b.GetLength());

		for (int i = 1; i < h.GetLength(); i++)
		{
			int a = (int)h.GetEntry(i);

			Binary sub = ConvertIntToBin(a);

			sub = fill(sub, 3 - sub.GetLength());

			b = ConvertBase(Concat(sub, b));
		}

		b = ShrinkToSize(b);

		return b;
	}

	private static Binary ConvertBase(BaseNum<bool> obj)
	{
		Binary copy = new Binary(obj.GetLength() - 1);

		for (int i = 0; i < obj.GetLength(); i++)
			copy.SetEntry(i, obj.GetEntry(i));

		copy.negative = obj.isNegative();

		return copy;
	}

	private static Binary fill(Binary b, int num)
	{

		Binary b1 = new Binary(num);

		return ConvertBase(Concat(b1, b));
	}

	private static Binary inc(Binary b)
	{
		for (int i = 0; i < b.GetLength(); i++)
		{
			if (b.GetEntry(i))
				b.SetEntry(i, false);
			else
			{
				b.SetEntry(i, true);
				return b;
			}
		}

		b = (Binary)Concat(ConvertIntToBin(1), b);

		return b;
	}

	private static Binary dec(Binary b)
	{
		for (int i = 0; i < b.GetLength(); i++)
		{
			if (b.GetEntry(i))
			{
				b.SetEntry(i, false);
				break;
			}
			else
				b.SetEntry(i, true);
		}

		if (!b.GetEntry(b.GetLength() - 1) && b.GetLength() > 1)
			b = (Binary)SubBase(b, 0, b.GetLength() - 1);

		return b;
	}

	public static bool operator >(Binary lhs, Binary rhs)
	{
		if (lhs.isNegative() && !rhs.isNegative())
			return false;
		else if (!lhs.isNegative() && rhs.isNegative())
			return true;

		if (lhs.GetLength() != rhs.GetLength())
			return lhs.GetLength() > rhs.GetLength();

		for (int i = lhs.GetLength() - 1; i >= 0; i--)
		{
			if (lhs.GetEntry(i) && !rhs.GetEntry(i))
				return true;
			else if (!lhs.GetEntry(i) && rhs.GetEntry(i))
				return false;
		}

		return false;
	}

	public static bool operator <(Binary lhs, Binary rhs)
	{
		if (lhs.isNegative() && !rhs.isNegative())
			return true;
		else if (!lhs.isNegative() && rhs.isNegative())
			return false;

		if (lhs.GetLength() != rhs.GetLength())
			return lhs.GetLength() < rhs.GetLength();

		for (int i = lhs.GetLength() - 1; i >= 0; i--)
		{
			if (!lhs.GetEntry(i) && rhs.GetEntry(i))
				return true;
			else if (lhs.GetEntry(i) && !rhs.GetEntry(i))
				return false;
		}

		return false;
	}

	public static bool operator >=(Binary lhs, Binary rhs)
	{
		if (lhs.isNegative() && !rhs.isNegative())
			return false;
		else if (!lhs.isNegative() && rhs.isNegative())
			return true;

		if (lhs.GetLength() != rhs.GetLength())
			return lhs.GetLength() > rhs.GetLength();

		for (int i = lhs.GetLength() - 1; i >= 0; i--)
		{
			if (lhs.GetEntry(i) && !rhs.GetEntry(i))
				return true;
			else if (!lhs.GetEntry(i) && rhs.GetEntry(i))
				return false;
		}

		return true;
	}

	public static bool operator <=(Binary lhs, Binary rhs)
	{
		if (lhs.isNegative() && !rhs.isNegative())
			return true;
		else if (!lhs.isNegative() && rhs.isNegative())
			return false;

		if (lhs.GetLength() != rhs.GetLength())
			return lhs.GetLength() < rhs.GetLength();

		for (int i = lhs.GetLength() - 1; i >= 0; i--)
		{
			if (!lhs.GetEntry(i) && rhs.GetEntry(i))
				return true;
			else if (lhs.GetEntry(i) && !rhs.GetEntry(i))
				return false;
		}

		return true;
	}

	public static bool operator ==(Binary lhs, Binary rhs)
	{
		if (lhs.GetLength() != rhs.GetLength())
			return false;

		for (int i = 0; i < lhs.GetLength(); i++)
			if (lhs.GetEntry(i) != rhs.GetEntry(i))
				return false;

		return true;
	}

	public static bool operator !=(Binary lhs, Binary rhs)
	{
		if (lhs.GetLength() != rhs.GetLength())
			return true;

		for (int i = 0; i < lhs.GetLength(); i++)
			if (lhs.GetEntry(i) == rhs.GetEntry(i))
				return false;

		return true;
	}

	public static Binary operator ++(Binary b)
	{
		if (b == ConvertIntToBin(-1))
			return ConvertIntToBin(0);

		if (b.isNegative())
			return dec(b);
		else
			return inc(b);
	}

	public static Binary operator --(Binary b)
	{
		if (b == ConvertIntToBin(0))
			return ConvertIntToBin(-1);

		if (b.isNegative())
			return inc(b);
		else
			return dec(b);
	}

	public static Binary operator +(Binary lhs, Binary rhs)
	{
		if (lhs.isNegative() ^ rhs.isNegative())
		{
			if (lhs.isNegative())
			{
				lhs.negative = false;
				return (rhs - lhs);
			}
			if (rhs.isNegative())
			{
				rhs.negative = false;
				return (lhs - rhs);
			}
		}

		int len = (lhs.GetLength() > rhs.GetLength()) ? lhs.GetLength() : rhs.GetLength();

		Binary sum = new Binary(len);

		int carry = 0;
		for (int i = 0; i < len; i++)
		{
			bool lbit = lhs.GetEntry(i);
			bool rbit = rhs.GetEntry(i);

			if (lbit ^ rbit ^ (carry == 1))
				sum.SetEntry(i, true);
			else
				sum.SetEntry(i, false);

			if ((lbit && rbit) || ((carry == 1) && (lbit || rbit)))
				carry = 1;
			else
				carry = 0;
		}

		if (carry == 1)
			sum = ConvertBase(Concat(ConvertIntToBin(1), sum));
		sum.negative = lhs.negative;

		return sum;
	}

	public static Binary operator -(Binary lhs, Binary rhs)
	{
		if (lhs == rhs)
			return ConvertIntToBin(0);

		if (lhs.isNegative() && !rhs.isNegative())
		{
			Binary copy = new Binary(lhs);
			copy.negative = false;

			Binary temp = copy + rhs;
			temp.negative = true;
			return temp;
		}
		else if (!lhs.isNegative() && rhs.isNegative())
		{
			Binary copy = new Binary(rhs);
			copy.negative = false;
			return lhs + copy;
		}
		else if (lhs.isNegative() && rhs.isNegative())
		{
			Binary copy1 = new Binary(lhs);
			copy1.negative = false;

			Binary copy2 = new Binary(rhs);
			rhs.negative = false;

			return copy2 - copy1;
		}
		else if (lhs <= rhs)
		{
			Binary temp = rhs - lhs;
			temp.negative = true;
			return temp;
		}

		if(rhs.GetLength() != lhs.GetLength())
			rhs = fill(rhs, lhs.GetLength() - rhs.GetLength());

		rhs = RadixComplement(rhs);

		Binary result = lhs + rhs;

		result = ConvertBase(SubBase(result, 0, result.GetLength() - 1));

		result = ShrinkToSize(result);

		return result;
	}

	public static Binary operator ~(Binary b)
	{
		Binary copy = new Binary(b);

		int len = copy.GetLength();
		for (int i = 0; i < len; i++)
			copy.SetEntry(i, !copy.GetEntry(i));

		return copy;
	}

	public static Binary operator %(Binary lhs, Binary rhs)
	{
		if (lhs < rhs)
			return lhs;
		else if (lhs == rhs)
			return ConvertIntToBin(0);

		Binary copy = new Binary(lhs);
		while (copy > rhs)
			copy = copy - rhs;

		return copy;
	}

	public override string ToString()
	{
		string s = (isNegative()) ? "1b" : "0b";
		for (int i = num.Length - 1; i >= 0; i--)
			s += (GetEntry(i).ToString() == "True") ? 1 : 0;

		return s;
	}
}
