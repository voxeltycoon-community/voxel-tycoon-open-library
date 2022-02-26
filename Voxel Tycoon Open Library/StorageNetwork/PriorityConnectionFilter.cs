using System;

namespace VTOL.StorageNetwork
{
	/// <summary>
	/// When a connection filter is registered, it is stored in an instance of <see cref="PriorityConnectionFilter"/>. <see cref="PriorityConnectionFilter"/> is used to sort every registered filter based on their priority.
	/// </summary>
	internal struct PriorityConnectionFilter : IComparable<PriorityConnectionFilter>
	{
		public PriorityConnectionFilter(IConnectionFilter connectionFilter, double priority)
		{
			ConnectionFilter = connectionFilter;
			Priority = priority;
		}

		/// <summary>
		/// The priority of the filter.
		/// <para>Filters with a higher priority will be executed after filters with a lower priority. Meaning the alterations made by a filter with a higher priority cannot be overwritten by a filter with a lower priority.</para>
		/// </summary>
		public double Priority { get; private set; }

		/// <summary>
		/// The class with the filter logic.
		/// </summary>
		public IConnectionFilter ConnectionFilter { get; private set; }
		
		public int CompareTo(PriorityConnectionFilter other) => Priority.CompareTo(other.Priority);
	}
}
