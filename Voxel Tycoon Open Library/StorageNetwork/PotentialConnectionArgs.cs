using System;
using System.Collections.Generic;
using System.Diagnostics;
using VoxelTycoon.Buildings;

namespace VTOL.StorageNetwork
{
	/// <summary>
	/// This class is used to collect all the Siblings found by <see cref="VoxelTycoon.Buildings.StorageBuildingManager.FindSiblings(StorageNetworkBuilding)"/>
	/// All the Connection Filter methods will receive this class as their Event Args, allowing them to alter the connections through this class.
	/// </summary>
	public class PotentialConnectionArgs
	{
		private readonly IList<StorageBuildingSibling> _siblings;
		private readonly IList<StorageBuildingSibling> _addedConnections = new List<StorageBuildingSibling>();
		private readonly IList<PotentialConnection> _connections;
		private Lazy<ISet<int>> _buildingIds;
		private bool _isClosed;

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
		/// <exception cref="InvalidOperationException">The potential connections have been processed or a building with a similar id is already a potential connection.</exception>
		/// <remarks>A <see cref="StorageBuildingSibling"/> can be created with <see cref="StorageNetworkUtils.CreateSiblingOf(StorageNetworkBuilding, StorageNetworkBuilding, bool)"/>.</remarks>
		public void AddConnection(StorageBuildingSibling storageBuildingSibling)
		{
			if (_isClosed)
			{
				throw new InvalidOperationException("You are not allowed to add any new connections after the potential connections have been processed.");
			}
			
			int id = storageBuildingSibling.Building.Id;

			if (_buildingIds.Value.Contains(id))
			{
				throw new InvalidOperationException($"Building with ID: {id} was already detected or has already been added.");
			}

			_addedConnections.Add(storageBuildingSibling);
			_buildingIds.Value.Add(id);
		}

		/// <summary>
		/// Removes all the StorageBuildingSiblings that have been canceled.
		/// </summary>
		/// <returns>Returns a list with the StorageBuildingSiblings which are not canceled.</returns>
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
