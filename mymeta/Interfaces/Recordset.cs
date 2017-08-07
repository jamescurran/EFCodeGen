using System;
using System.Collections.Generic;
using System.Reflection;

namespace MyMeta
{
	public class Recordset
	{
		public void Open(object value, object missing, CursorTypeEnum adOpenStatic, LockTypeEnum adLockOptimistic, int i)
		{
			throw new System.NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fieldList">Optional. A single name, or an array of names or ordinal positions of the fields in the new record.</param>
		/// <param name="values">Optional. A single value, or an array of values for the fields in the new record. If Fieldlist is an array, Values must also be an array with the same number of members; otherwise, an error occurs. The order of field names must match the order of field values in each array.</param>
		public void AddNew(object fieldList, object values)
		{
			throw new NotImplementedException();
		}

		public FieldCollection Fields { get; private set; } = new FieldCollection();
		public bool EOF { get; set; }
		public bool BOF { get; set; }

		public void MoveFirst()
		{
			throw new NotImplementedException();
		}

		internal void MoveNext()
		{
			throw new NotImplementedException();
		}

		public void Close()
		{
			throw new NotImplementedException();
		}
	}
}