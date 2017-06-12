using System;
using System.Collections.Generic;
using System.Text;

namespace BitAuto.CarChannel.BLL.Data
{
	public class EntityCompare
	{
		public static int BrandDefaultCompare(BrandEntity be1, BrandEntity be2)
		{
			if (be1.IsImport && !be2.IsImport)
				return 1;
			else if (!be1.IsImport && be2.IsImport)
				return -1;
			else
			{
				return String.Compare(be1.Spell, be2.Spell);
			}
		}
	}
}
