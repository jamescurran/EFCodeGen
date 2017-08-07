using System;
using System.Reflection;
using System.Windows.Forms;

namespace MyMeta
{
	public class FieldCollection
	{
		internal void Append(string fieldname, DataTypeEnum dataTypeEnum, int length, FieldAttributeEnum adFldIsNullable, Missing value)
		{
			throw new NotImplementedException();
		}

		public Field this[int inx]
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }

		}

		public Field this[string name]
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }

		}
	}
}