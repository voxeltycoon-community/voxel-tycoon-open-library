using System;
using System.Collections.Generic;
using System.Diagnostics;
using VoxelTycoon.Buildings;
using VTOL.Debugging;

namespace VTOL.StorageNetwork.ConnectionManager
{
	/// <summary>
	/// This class is used to collect all the Siblings found by <see cref="VoxelTycoon.Buildings.StorageBuildingManager.FindSiblings(StorageNetworkBuilding)"/>
	/// All the Connection Filters (<see cref="IConnectionFilter.OnConnect(PotentialConnectionArgs)"/>) will receive this class as their argument, allowing them to alter the connections through this class.
	/// </summary>
	public class PotentialConnectionArgs
	{
		private readonly IList<StorageBuildingSibling> _siblings;
		private readonly IList<StorageBuildingSibling> _addedConnections = new List<StorageBuildingSibling>();
		private readonly IList<PotentialConnection> _connections;
		private Lazy<ISet<int>> _buildingIds;
		private bool _isClosed;

		private ISet<int> BuildingIds => _buildingIds.Value;

		internal PotentialConnectionArgs(StorageNetworkBuilding storageNetworkBuilding, IList<StorageBuildingSibling> siblings)
		{
			Source = storageNetworkBuilding;
			_siblings = siblings;
			_connections = new List<PotentialConnection>(_siblings.Count);
			_buildingIds = new Lazy<ISet<int>>(RegisterBuildingIds);

			foreach (StorageBuildingSibling sibling in _siblings)
			{
				_connections.Add(new PotentialConnection(sibling.Building));
			}
		}

		/// <summary>
		/// The <see cref="StorageNetworkBuilding"/> of which the connections are being filtered.
		/// </summary>
		public StorageNetworkBuilding Source { get; private set; }

		/// <summary>
		/// IEnumerable for cycling through the list with potential connections.
		/// </summary>
		public IEnumerable<PotentialConnection> Connections => _connections;
		internal IList<StorageBuildingSibling> AddedConnections => _addedConnections;

		/// <summary>
		/// Create a connection with a building not detected by <see cref="StorageBuildingManager.FindSiblings(StorageNetworkBuilding)"/>.
		/// </summary>
		/// <param name="storageBuildingSibling">The <see cref="StorageBuildingSibling"/> containing the information to create a connection.</param>
		/// <returns>True if connection has been added, otherwise false.</returns>
		/// <remarks>A <see cref="StorageBuildingSibling"/> can be created with <see cref="StorageNetworkUtils.CreateSiblingOf(StorageNetworkBuilding, StorageNetworkBuilding, bool)"/>.</remarks>
		public bool TryAddConnection(StorageBuildingSibling storageBuildingSibling)
		{
			if (_isClosed)
			{
				throw new InvalidOperationException("You are not allowed to add a new connection after all filters have been executed.");
			}
			
			int id = storageBuildingSibling.Building.Id;

			if (BuildingIds.Contains(id))
			{
				VtolLogger.Log($"Building {storageBuildingSibling.Building.DisplayName} with id {id} was already detected, or has already been added as a connection. Ignoring addition.");
				
				return false;
			}

			_addedConnections.Add(storageBuildingSibling);
			BuildingIds.Add(id);

			return true;
		}

		/// <summary>
		/// Removes all the StorageBuildingSiblings that have been canceled.
		/// </summary>
		/// <returns>Returns the altered list with the StorageBuildingSiblings which haven't been canceled.</returns>
		// ReSharper disable once ReturnTypeCanBeEnumerable.Global
		internal IList<StorageBuildingSibling> RemoveCanceled()
		{
			Trace.Assert(_connections.Count == _siblings.Count);

			for (int i = _connections.Count - 1; i >= 0; i--)
			{
				if (_connections[i].IsCanceled)
				{
					Trace.Assert(_connections[i].Building.Id == _siblings[i].Building.Id);

					_siblings.RemoveAt(i);
				}
			}

			_isClosed = true;

			return _siblings;
		}

		//When adding a completely new connection, meaning creating a connection with a building which Voxel Tycoon is not detecting, the to be connected building cannot already been detected by the normal Voxel Tycoon detection.
		//After all, since the filters arent checking the newly added connections (_addedConnections), this would allow to force a connection with an already detected building, bypassing other registered filters.
		//To check if a building was already detected, a list with id's is created from the list of detected buildings (_siblings). This only has to happen when a custom connection is added with AddConnection(), and only once.
		//This is why _buildingIds is using the Lazy-class.
		private ISet<int> RegisterBuildingIds()
		{
			ISet<int> buildingIds = new HashSet<int>();
			
			foreach (StorageBuildingSibling sibling in _siblings) 
			{
				buildingIds.Add(sibling.Building.Id);
			}

			return buildingIds;
		}
	}
}
