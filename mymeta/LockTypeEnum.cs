namespace MyMeta
{
	public enum LockTypeEnum
	{
		adLockBatchOptimistic = 4, //	Indicates optimistic batch updates. Required for batch update mode.

		adLockOptimistic = 3, //		Indicates optimistic locking, record by record. The provider uses optimistic locking, locking records only when you call the Update method.

		adLockPessimistic =2, //		Indicates pessimistic locking, record by record. The provider does what is necessary to ensure successful editing of the records, usually by locking records at the data source immediately after editing.
		adLockReadOnly = 1, //		Indicates read-only records. You cannot alter the data.

		adLockUnspecified =-1 //		Does not specify a type of lock. For clones, the clone is created with the same lock type as the original.
	}
}