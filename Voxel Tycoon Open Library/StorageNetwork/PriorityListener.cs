using System;
using System.Collections.Generic;
using System.Text;

namespace VTOL.StorageNetwork
{
    /// <summary>
    /// When a listener is registered, it is stored in PriorityListener. PriorityListener is used to sort every registered listener based on priority
    /// </summary>
    internal struct PriorityListener : IComparable<PriorityListener>
    {
        public PriorityListener(int assetId, OnStorageNetworkUpdate listener, int priority)
        {
            AssetId = assetId;
            Listener = listener;
            Priority = priority;
        }

        public int AssetId { get; private set; }
        public int Priority { get; private set; }
        public OnStorageNetworkUpdate Listener { get; private set; }

        public int CompareTo(PriorityListener other)
        {
            return Priority.CompareTo(other.Priority);
        }
    }
}
