using System;

namespace VTOL.StorageNetwork
{
	/// <summary>
	/// When a connectiong filter is registered, it is stored in PriorityListener. PriorityListener is used to sort every registered listener based on priority
	/// </summary>
	internal struct PriorityConnectionFilter : IComparable<PriorityConnectionFilter>
	{
		public PriorityConnectionFilter(int assetId, OnStorageNetworkUpdate connectionFilter, int priority)
		{
			AssetId = assetId;
			ConnectionFilter = connectionFilter;
			Priority = priority;
		}

		public int AssetId { get; }
		public int Priority { get; }
		public OnStorageNetworkUpdate ConnectionFilter { get; private set; }

		public int CompareTo(PriorityConnectionFilter other) => Priority.CompareTo(other.Priority);
	}
}
