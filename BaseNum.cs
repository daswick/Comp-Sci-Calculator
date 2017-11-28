using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BaseNum<Object>
{
	public BaseNum(int len = 0)
	{
		num = new Object[len + 1];

		for (int i = 0; i < len; i++)
			num[i] = default(Object);

		negative = false;
	}

	public BaseNum(BaseNum<Object> b)
	{
		num = new Object[b.GetLength()];

		for (int i = 0; i < b.GetLength(); i++)
			num[i] = b.GetEntry(i);

		negative = b.negative;
	}

	public bool isNegative()
	{
		return negative;
	}

	public int GetLength()
	{
		return num.Length;
	}

	public static BaseNum<Object> Concat(BaseNum<Object> lhs, BaseNum<Object> rhs)
	{
		int llen = lhs.GetLength();
		int rlen = rhs.GetLength();

		int newlen = llen + rlen - 1;

		BaseNum<Object> result = new BaseNum<Object>(newlen - 1);

		for (int i = 0; i < rlen; i++)
			result.SetEntry(i, rhs.GetEntry(i));

		for (int j = 0; j < llen; j++)
			result.SetEntry((j + rlen), lhs.GetEntry(j));

		if (lhs.isNegative() || rhs.isNegative())
			result.negative = true;

		return result;
	}

	public static BaseNum<Object> SubBase(BaseNum<Object> b, int start, int end)
	{
		int newlen = end - start;

		BaseNum<Object> sub = new BaseNum<Object>(newlen - 1);

		for (int i = 0; i < newlen; i++)
			sub.SetEntry(i, b.GetEntry(start + i));

		if (b.isNegative())
			sub.negative = true;

		return sub;
	}

	public Object GetEntry(int index)
	{
		if (index < 0)
			throw new ArgumentOutOfRangeException("Cannot access index below 0");

		if (index >= GetLength())
			return default(Object);

		return num[index];
	}

	public void SetEntry(int index, Object value)
	{
		if (index < 0)
			throw new ArgumentOutOfRangeException("Cannot access bits below 0");

		if (num.Length < index + 1)
		{
			BaseNum<Object> temp = new BaseNum<Object>(this);

			num = null;

			num = new Object[index + 1];

			for (int i = 0; i <= index; i++)
				num[i] = temp.GetEntry(i);
		}

		num[index] = value;
	}

	public override bool Equals(object obj)
	{
		var h = obj as BaseNum<Object>;

		if (h == null)
			return false;

		return (this == h);
	}

	public override int GetHashCode()
	{
		return num.GetHashCode();
	}

	protected Object[] num;
	protected bool negative;
	protected byte radix;
}