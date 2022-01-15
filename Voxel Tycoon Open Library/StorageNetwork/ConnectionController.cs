using System;
using System.Collections.Generic;
using VoxelTycoon;
using VoxelTycoon.Buildings;

namespace VTOL.StorageNetwork
{
    /// <summary>
    /// This class provides methods to allow for adjusting Storage Network connections.
    /// </summary>
    public class ConnectionController : LazyManager<ConnectionController>
    {
        private readonly IDictionary<int, SortedSet<PriorityListener>> _listeners = new Dictionary<int, SortedSet<PriorityListener>>();
        //private readonly List<PriorityListener> _priorityListeners = new List<PriorityListener>();

        /// <summary>
        /// Registers a method which decides if a connection between two <see cref="StorageNetworkBuilding"/> should be allowed or not.
        /// </summary>
        /// <param name="assetId">The AssetId of one of the StorageNetworkBuildings which is part of the connection to be adjusted.</param>
        /// <param name="listener">The method which decides if a connection should be allowed.</param>
        /// <param name="modName">The name of the mod registering the method.</param>
        /// <param name="reasoning">What is the reasoning behind the method.</param>
        /// <param name="priority">Listeners with a higher priority can override adjustments made by listeners with a lower priority.</param>
        /// <exception cref="InvalidOperationException">When trying to register when the game is done loading.</exception>
        /// <remarks>Choosing a proper priority mainly depends on mod compatibilty. <see cref="ConnectionController)"/> is logging a list with priorities and reasons. This will help choosing a proper priority.
        /// Be aware that you cannot unregister a method. Make sure that your method includes a check to see if it needs to execute a cancellation.</remarks>
        public void Register(int assetId, OnStorageNetworkUpdate listener, int priority = 0)
        {
            if (VTOL.GameState == GameStates.OnGameStarted)
            {
                throw new InvalidOperationException("You are not allowed to register after the game is completely loaded and started. Try to register in an earlier state.");
            }

            PriorityListener priorityListener = new PriorityListener(assetId, listener, priority);

            AddListener(assetId, priorityListener);
        }

        /// <summary>
        /// Tries to get all the listeners associated with the specified AssetId.
        /// </summary>
        /// <param name="assetId">The specified AssetId</param>
        /// <param name="listeners">The list with all listeners. <code>Null</code> if none are found.</param>
        /// <returns>True if listeners are registered with specified AssetId. Otherwise false.</returns>
        internal bool TryGetListeners(int assetId, out SortedSet<PriorityListener> listeners)
        {
            return _listeners.TryGetValue(assetId, out listeners);
        }

        /// <summary>
        /// Adds a listener to the dictionary with specified AssetId.
        /// </summary>
        /// <param name="assetId">The AssetId of one of the StorageNetworkBuildings which is part of the connection to be adjusted.</param>
        /// <param name="listener">The method which decides if a connection should be allowed.</param>
        private void AddListener(int assetId, PriorityListener listener)
        {
            if (!_listeners.TryGetValue(assetId, out SortedSet<PriorityListener> listeners))
            {
                listeners = _listeners[assetId] = new SortedSet<PriorityListener>();
            }
            
            listeners.Add(listener);
        }
    }

    /// <summary>
    /// The delegate to store the methods which decide if a connection should be allowed.
    /// </summary>
    public delegate void OnStorageNetworkUpdate(PotentialConnectionArgs potentialConnections);
}
