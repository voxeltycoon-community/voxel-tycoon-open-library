using System;
using System.Collections.Generic;
using VoxelTycoon;

namespace VTOL
{
    /// <summary>
    /// This class provides methods to allow for adjusting Storage Network connections.
    /// </summary>
    public class VTOLStorageNetworkManager : LazyManager<VTOLStorageNetworkManager>
    {
        private readonly IDictionary<int, IList<Listener>> _listeners = new Dictionary<int, IList<Listener>>();
        private readonly List<PriorityListener> _priorityListeners = new List<PriorityListener>();

        private bool _isDirty;

        /// <summary>
        /// Registers a method which decides if a connection between two Storage Network Buildings should be allowed or not.
        /// </summary>
        /// <param name="assetId">The AssetId of one of the StorageNetworkBuildings which is part of the connection to be adjusted.</param>
        /// <param name="listener">The method which decides if a connection should be allowed.</param>
        /// <param name="priority">Listeners with a higher priority can override adjustments made by listeners with a lower priority.</param>
        public void RegisterListener(int assetId, Listener listener, int priority)
        {
            PriorityListener priorityListener = new PriorityListener()
            {
                AssetId = assetId,
                Priority = priority,
                Listener = listener
            };

            _priorityListeners.Add(priorityListener);

            _isDirty = true;
        }

        /// <summary>
        /// Gets the listeners associated with the specified AssetId.
        /// </summary>
        /// <param name="assetId">The specified AssetId</param>
        /// <param name="listeners">The list with all listeners. <code>Null</code> if none are found.</param>
        /// <returns>True if listeners are registered with specified AssetId. Otherwise false.</returns>
        public bool GetListeners(int assetId, out IList<Listener> listeners)
        {
            SortListeners();
            
            return _listeners.TryGetValue(assetId, out listeners);
        }

        /// <summary>
        /// Creates a dictionary with AssetId as a key. Attaches a list with Listeners sorted by priority. Lowest priority first.
        /// </summary>
        private void SortListeners()
        {
            if (_isDirty)
            {
                _priorityListeners.Sort();
                _listeners.Clear();

                foreach (PriorityListener priorityListener in _priorityListeners)
                {
                    AddListener(priorityListener.AssetId, priorityListener.Listener);
                }

                _isDirty = false;
            }
        }

        /// <summary>
        /// Adds a listener to the dictionary with specified AssetId.
        /// </summary>
        /// <param name="assetId">The AssetId of one of the StorageNetworkBuildings which is part of the connection to be adjusted.</param>
        /// <param name="listener">The method which decides if a connection should be allowed.</param>
        private void AddListener(int assetId, Listener listener)
        {
            if (!_listeners.ContainsKey(assetId))
            {
                _listeners.Add(assetId, new List<Listener>());
            }

            _listeners[assetId].Add(listener);
        }

        /// <summary>
        /// When a listener is registered, it is stored in PriorityListener. PriorityListener is used to sort every registered listener based on priority
        /// </summary>
        private struct PriorityListener : IComparable<PriorityListener>
        {
            public int AssetId { get; set; }
            public int Priority { get; set; }
            public Listener Listener { get; set; }

            public int CompareTo(PriorityListener other)
            {
                return Priority.CompareTo(other.Priority);
            }
        }
    }

    /// <summary>
    /// The delegate to store the methods which decide if a connection should be allowed.
    /// </summary>
    public delegate void Listener(PotentialConnectionArgs potentialConnections);
}
