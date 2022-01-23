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
		//We're using a SortedSet, because a SortedList uses a KeyValuePair
		private readonly IDictionary<int, ISet<PriorityConnectionFilter>> _connectionFilters = new Dictionary<int, ISet<PriorityConnectionFilter>>();

		/// <summary>
		/// Registers a method which decides if a connection between two <see cref="StorageNetworkBuilding"/> should be allowed or not.
		/// </summary>
		/// <param name="assetId">The AssetId of one of the StorageNetworkBuildings which is part of the connection to be adjusted.</param>
		/// <param name="connectionFilter">The method which decides if a connection should be canceled or not.</param>
		/// <param name="priority">Filters with a higher priority can override adjustments made by filters with a lower priority.</param>
		/// <exception cref="InvalidOperationException">When trying to register while the game is done loading.</exception>
		public void Register(int assetId, OnStorageNetworkUpdate connectionFilter, int priority = 0)
		{
			if (VTOL.GameState > GameStates.OnGameStarting)
			{
				throw new InvalidOperationException("You are not allowed to register after the game is completely loaded and started.");
			}

			PriorityConnectionFilter priorityListener = new PriorityConnectionFilter(assetId, connectionFilter, priority);

			AddListener(assetId, priorityListener);
		}

		/// <summary>
		/// Tries to get all the listeners associated with the specified AssetId.
		/// </summary>
		/// <param name="assetId">The specified AssetId</param>
		/// <param name="connectionFilters">The list with all listeners. <code>Null</code> if none are found.</param>
		/// <returns>True if listeners are registered with specified AssetId. Otherwise false.</returns>
		internal bool TryGetConnectionFilters(int assetId, out ISet<PriorityConnectionFilter> connectionFilters) => _connectionFilters.TryGetValue(assetId, out connectionFilters);

		/// <summary>
		/// Adds a listener to the dictionary with specified AssetId.
		/// </summary>
		/// <param name="assetId">The AssetId of one of the StorageNetworkBuildings which is part of the connection to be adjusted.</param>
		/// <param name="connectionFilter">The method which decides if a connection should be allowed.</param>
		private void AddListener(int assetId, PriorityConnectionFilter connectionFilter)
		{
			if (!_connectionFilters.TryGetValue(assetId, out ISet<PriorityConnectionFilter> connectionFilters))
			{
				connectionFilters = _connectionFilters[assetId] = new SortedSet<PriorityConnectionFilter>();
			}
			
			connectionFilters.Add(connectionFilter);
		}
	}

	/// <summary>
	/// The delegate to store the methods which decide if a connection should be allowed.
	/// </summary>
	public delegate void OnStorageNetworkUpdate(PotentialConnectionArgs potentialConnections);
}
