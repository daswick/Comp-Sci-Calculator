using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum hex { Ze, On, Tw, Th, Fo, Fi, Si, Se, Ei, Ni, A, B, C, D, E, F };

class Hex : BaseNum<hex>
{
	public Hex(int len = 0)
	{
		num = new hex[len + 1];

		for (int i = 0; i < len; i++)
			num[i] = hex.Ze;

		radix = 16;

		negative = false;
	}

	public Hex(Hex h)
	{
		num = new hex[h.GetLength()];

		for (int i = 0; i < num.Length; i++)
			num[i] = h.num[i];

		negative = h.negative;
	}

	public static Hex RadixComplement(Hex h)
	{
		Hex copy = ~h;
		copy++;
		return copy;
	}

	public static Hex Parse(string s)
	{
		Hex h = new Hex();
		if (s[0] == '1')
			h.negative = true;

		s = s.Substring(2, s.Length - 2);

		for (int i = s.Length - 1; i >= 0; i--)
		{
			hex temp;
			int n;
			bool isNum = int.TryParse(s[i].ToString(), out n);

			if (isNum)
				temp = (hex)n;
			else
				temp = (hex)(s[i] - 55);

			h.SetEntry(s.Length - i - 1, temp);
		}

		return h;
	}

	public static Hex ConvertIntToHex(int n)
	{
		Hex result = new Hex();
		byte count = 0;

		if (n < 0)
		{
			result.negative = true;
			n *= -1;
		}

		while (n > 0)
		{
			int temp = n >> 4;

			int rem;
			if (temp == 0)
				rem = n;
			else
				rem = n % 16;

			result.SetEntry(count++, (hex)rem);
			n = n >> 4;
		}

		return result;
	}

	public static int ConvertHexToInt(Hex h)
	{
		int result = 0;

		for (int i = h.GetLength() - 1; i >= 0; i--)
			result += (int)h.GetEntry(i) * (int)(Math.Pow(16, i));

		return result;
	}

	public static Hex ConvertBinToHex(Binary b)
	{
		Hex h;
		if (b.GetLength() % 4 == 0)
			h = new Hex((b.GetLength() / 4) - 1);
		else
			h = new Hex(b.GetLength() / 4);

		for (int i = 0; i < h.GetLength(); i++)
		{
			string s = "0b";

			for (int j = (i * 4 + 3); j >= (i * 4); j--)
				s += b.GetEntry(j) ? "1" : "0";

			Binary sub = Binary.Parse(s);

			h.SetEntry(i, (hex)Binary.ConvertBinToInt(sub));
		}

		return h;
	}

	private static Hex ConvertBase(BaseNum<hex> obj)
	{
		Hex copy = new Hex(obj.GetLength() - 1);

		for (int i = 0; i < obj.GetLength(); i++)
			copy.SetEntry(i, obj.GetEntry(i));

		copy.negative = obj.isNegative();

		return copy;
	}

	private static Hex fill(Hex h, int num)
	{
		Hex h1 = new Hex(num);
		return ConvertBase(Concat(h1, h));
	}

	private static Hex inc(Hex h)
	{
		for (int i = 0; i < h.GetLength(); i++)
		{
			if (h.GetEntry(i) == hex.F)
				h.SetEntry(i, hex.Ze);
			else
			{
				h.SetEntry(i, (hex)((int)h.GetEntry(i) + 1));
				return h;
			}
		}

		return ConvertBase(Concat(ConvertIntToHex(1), h));
	}

	private static Hex dec(Hex h)
	{
		for (int i = 0; i < h.GetLength(); i++)
		{
			if (h.GetEntry(i) == hex.Ze)
				h.SetEntry(i, hex.F);
			else
			{
				h.SetEntry(i, (hex)((int)h.GetEntry(i) - 1));
				break;
			}
		}

		if (h.GetEntry(h.GetLength() - 1) == hex.Ze && h.GetLength() > 1)
			h = ConvertBase(SubBase(h, 0, h.GetLength() - 1));

		return h;
	}

	public static bool operator >(Hex lhs, Hex rhs)
	{
		if (lhs.isNegative() && !rhs.isNegative())
			return false;
		else if (!lhs.isNegative() && rhs.isNegative())
			return true;

		if (lhs.GetLength() != rhs.GetLength())
			return lhs.GetLength() > rhs.GetLength();

		for (int i = lhs.GetLength() - 1; i >= 0; i--)
		{
			if (lhs.GetEntry(i) > rhs.GetEntry(i))
				return true;
			else if (lhs.GetEntry(i) < rhs.GetEntry(i))
				return false;
		}

		return false;
	}

	public static bool operator <(Hex lhs, Hex rhs)
	{
		if (lhs.isNegative() && !rhs.isNegative())
			return true;
		else if (!lhs.isNegative() && rhs.isNegative())
			return false;

		if (lhs.GetLength() != rhs.GetLength())
			return lhs.GetLength() < rhs.GetLength();

		for (int i = lhs.GetLength() - 1; i >= 0; i--)
		{
			if (lhs.GetEntry(i) < rhs.GetEntry(i))
				return true;
			else if (lhs.GetEntry(i) > rhs.GetEntry(i))
				return false;
		}

		return false;
	}

	public static bool operator >=(Hex lhs, Hex rhs)
	{
		if (lhs.isNegative() && !rhs.isNegative())
			return false;
		else if (!lhs.isNegative() && rhs.isNegative())
			return true;

		if (lhs.GetLength() != rhs.GetLength())
			return lhs.GetLength() > rhs.GetLength();

		for (int i = lhs.GetLength() - 1; i >= 0; i--)
		{
			if (lhs.GetEntry(i) > rhs.GetEntry(i))
				return true;
			else if (lhs.GetEntry(i) < rhs.GetEntry(i))
				return false;
		}

		return true;
	}

	public static bool operator <=(Hex lhs, Hex rhs)
	{
		if (lhs.isNegative() && !rhs.isNegative())
			return false;
		else if (!lhs.isNegative() && rhs.isNegative())
			return true;

		if (lhs.GetLength() != rhs.GetLength())
			return lhs.GetLength() < rhs.GetLength();

		for (int i = lhs.GetLength() - 1; i >= 0; i--)
		{
			if (lhs.GetEntry(i) < rhs.GetEntry(i))
				return true;
			else if (lhs.GetEntry(i) > rhs.GetEntry(i))
				return false;
		}

		return true;
	}

	public static bool operator ==(Hex lhs, Hex rhs)
	{
		if (lhs.GetLength() != rhs.GetLength())
			return false;

		for (int i = 0; i < lhs.GetLength(); i++)
			if (lhs.GetEntry(i) != rhs.GetEntry(i))
				return false;

		return true;
	}

	public static bool operator !=(Hex lhs, Hex rhs)
	{
		if (lhs.GetLength() != rhs.GetLength())
			return true;

		for (int i = 0; i < lhs.GetLength(); i++)
			if (lhs.GetEntry(i) != rhs.GetEntry(i))
				return true;

		return false;
	}

	public static Hex operator ++(Hex h)
	{
		if (h == ConvertIntToHex(-1))
			return ConvertIntToHex(0);

		if (h.isNegative())
			return dec(h);
		else
			return inc(h);
	}

	public static Hex operator --(Hex h)
	{
		if (h == ConvertIntToHex(0))
			return ConvertIntToHex(-1);

		if (h.isNegative())
			return inc(h);
		else
			return dec(h);
	}

	public static Hex operator +(Hex lhs, Hex rhs)
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

		Hex sum = new Hex(len - 1);

		int carry = 0;

		for (int i = 0; i < len; i++)
		{
			hex h1 = lhs.GetEntry(i);
			hex h2 = rhs.GetEntry(i);

			int digit = (int)h1 + (int)h2 + carry;

			sum.SetEntry(i, (hex)(digit % 16));

			carry = digit / 16;
		}

		if (carry > 0)
			sum = ConvertBase(Concat(ConvertIntToHex(carry), sum));
		sum.negative = lhs.negative;

		return sum;
	}

	public static Hex operator -(Hex lhs, Hex rhs)
	{
		if (lhs == rhs)
			return ConvertIntToHex(0);
		else if (lhs.isNegative() && !rhs.isNegative())
		{
			Hex copy = new Hex(lhs);
			copy.negative = false;

			Hex temp = copy + rhs;
			temp.negative = true;
			return temp;
		}
		else if (rhs.isNegative() && !lhs.isNegative())
		{
			Hex copy = new Hex(rhs);
			copy.negative = false;
			return lhs + copy;
		}
		else if (lhs.isNegative() && rhs.isNegative())
		{
			Hex copy1 = new Hex(lhs);
			copy1.negative = false;

			Hex copy2 = new Hex(rhs);
			rhs.negative = false;

			return copy2 - copy1;
		}
		else if (lhs <= rhs)
		{
			Hex temp = rhs - lhs;
			temp.negative = true;
			return temp;
		}

		if (rhs.GetLength() != lhs.GetLength())
			rhs = fill(rhs, lhs.GetLength() - rhs.GetLength() - 1);

		rhs = RadixComplement(rhs);

		Hex result = lhs + rhs;

		result = ConvertBase(SubBase(result, 0, result.GetLength() - 1));

		if (result.GetEntry(result.GetLength() - 1) == hex.Ze && result.GetLength() > 1)
		{
			for (int i = result.GetLength() - 1; i >= 0; i--)
			{
				if (result.GetEntry(i) > 0)
				{
					result = ConvertBase(SubBase(result, 0, i + 1));
					break;
				}
			}
		}

		return result;
	}

	public static Hex operator ~(Hex h)
	{
		Hex copy = new Hex(h);

		for (int i = 0; i < copy.GetLength(); i++)
			copy.SetEntry(i, (hex)(15 - (int)h.GetEntry(i)));

		return copy;
	}

	public static Hex operator %(Hex h1, Hex h2)
	{
		if (h1 < h2)
			return h1;
		else if (h1 == h2)
			return ConvertIntToHex(0);

		Hex copy = new Hex(h1);

		while (copy > h2)
			copy = copy - h2;

		return copy;
	}

	public override string ToString()
	{
		string output = negative ? "1x" : "0x";

		for (int i = num.Length - 1; i >= 0; i--)
		{
			if ((int)num[i] < 10)
				output += (int)num[i];
			else
				output += num[i].ToString();
		}
		return output;
	}
}