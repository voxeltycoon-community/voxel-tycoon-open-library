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
		private readonly IDictionary<int, List<PriorityConnectionFilter>> _connectionFilters = new Dictionary<int, List<PriorityConnectionFilter>>();
		private bool _isDirty;

		/// <summary>
		/// Registers a method which decides if a connection between two <see cref="StorageNetworkBuilding"/> should be allowed or not.
		/// </summary>
		/// <param name="assetId">The AssetId of one of the StorageNetworkBuildings which is part of the connection to be adjusted.</param>
		/// <param name="connectionFilter">The method which decides if a connection should be canceled or not.</param>
		/// <param name="priority">Filters with a higher priority can override adjustments made by filters with a lower priority.</param>
		/// <exception cref="InvalidOperationException">When trying to register while the game is done loading.</exception>
		public void RegisterConnectionFilter(int assetId, OnStorageNetworkUpdate connectionFilter, int priority = 0)
		{
			if (Vtol.GameState > GameStates.OnGameStarting)
			{
				throw new InvalidOperationException($"You are not allowed to register after state OnGameStarting. The current state is {Vtol.GameState}.");
			}

			PriorityConnectionFilter priorityListener = new PriorityConnectionFilter(assetId, connectionFilter, priority);

			AddConnectionFilter(assetId, priorityListener);
			_isDirty = true;
		}

		/// <summary>
		/// Tries to get all the listeners associated with the specified AssetId.
		/// </summary>
		/// <param name="assetId">The specified AssetId</param>
		/// <param name="connectionFilters">The list with all listeners. <code>Null</code> if none are found.</param>
		/// <returns>True if listeners are registered with specified AssetId. Otherwise false.</returns>
		internal bool TryGetConnectionFilters(int assetId, out IList<PriorityConnectionFilter> connectionFilters)
		{
			if (_isDirty)
			{
				SortConnectionFilters();
			}
			
			bool isFound = _connectionFilters.TryGetValue(assetId, out List<PriorityConnectionFilter> tempConnectionFilters);

			connectionFilters = tempConnectionFilters;

			return isFound;
		}

		/// <summary>
		/// Adds a listener to the dictionary with specified AssetId.
		/// </summary>
		/// <param name="assetId">The AssetId of one of the StorageNetworkBuildings which is part of the connection to be adjusted.</param>
		/// <param name="connectionFilter">The method which decides if a connection should be allowed.</param>
		private void AddConnectionFilter(int assetId, PriorityConnectionFilter connectionFilter)
		{
			if (!_connectionFilters.TryGetValue(assetId, out List<PriorityConnectionFilter> connectionFilters))
			{
				connectionFilters = _connectionFilters[assetId] = new List<PriorityConnectionFilter>();
			}
			
			connectionFilters.Add(connectionFilter);
		}

		private void SortConnectionFilters()
		{
			foreach (List<PriorityConnectionFilter> connectionFilters in _connectionFilters.Values)
			{
				connectionFilters.Sort();
			}
		}
	}

	/// <summary>
	/// The delegate to store the methods which decide if a connection should be allowed.
	/// </summary>
	public delegate void OnStorageNetworkUpdate(PotentialConnectionArgs potentialConnections);
}
