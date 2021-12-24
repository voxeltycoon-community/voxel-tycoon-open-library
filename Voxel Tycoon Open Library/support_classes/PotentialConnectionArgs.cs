using System;
using System.Collections;
using System.Collections.Generic;
using VoxelTycoon.Buildings;

namespace VTOL
{
    /// <summary>
    /// This class is used to collect all the Siblings found by <code>VoxelTycoon.Buildings.StorageBuildingManager.FindSiblings()</code>
    /// and allow them to be removed based on specified conditions.
    /// </summary>
    public class PotentialConnectionArgs
    {
        private readonly List<StorageBuildingSibling> _siblings = new List<StorageBuildingSibling>();
        private readonly List<StorageBuildingSibling> _addedConnections = new List<StorageBuildingSibling>();
        private readonly List<Connection> _connections;
        private HashSet<int> _buildingIds;

        internal PotentialConnectionArgs(List<StorageBuildingSibling> siblings)
        {
            _siblings = siblings;
            _connections = new List<Connection>(_siblings.Count);

            foreach (StorageBuildingSibling sibling in _siblings)
            {
                _connections.Add(new Connection(sibling.Building));
            }
        }

        public IEnumerable<Connection> Connections => _connections;
        internal List<StorageBuildingSibling> AddedConnections => _addedConnections;

        /// <summary>
        /// Create a connection with a building not detected by the by the base game detection.
        /// </summary>
        /// <param name="storageBuildingSibling">The <code>StorageBuildingSibling</code> containing the information to create a connection.</param>
        /// <remarks>The normal detection is done by <code>VoxelTycoon.Buildings.StorageBuildingManager.FindSiblings()</code>.</remarks>
        /// <remarks>A <code>StorageBuildingSibling</code> can be created with <code>VTOLStorageNetworkUtils.CreateStorageBuildingSibling()</code>.</remarks>
        public void AddConnection(StorageBuildingSibling storageBuildingSibling)
        {
            RegisterBuildingIds();

            int id = storageBuildingSibling.Building.Id;

            if (_buildingIds.Contains(id))
            {
                throw new InvalidOperationException($"Building with ID: {id} was already detected or has already been added.");
            }

            _addedConnections.Add(storageBuildingSibling);
            _buildingIds.Add(id);
        }
        
        /// <summary>
        /// Removes all the StorageBuildingSiblings that have been canceled.
        /// </summary>
        /// <returns>Returns a list with the remaining StorageBuildingSiblings.</returns>
        internal List<StorageBuildingSibling> RemoveCanceled()
        {
            if (_connections.Count != _siblings.Count)
            {
                throw new ArgumentException();
            }

            for (int i = _connections.Count - 1; i >= 0; i--)
            {
                if (_connections[i].IsCanceled)
                {
                    if (_connections[i].Building.Id != _siblings[i].Building.Id)
                    {
                        throw new ArgumentException();
                    }

                    _siblings.RemoveAt(i);
                }
            }

            return _siblings;
        }

        private void RegisterBuildingIds()
        {
            if (_buildingIds == null)
            {
                _buildingIds = new HashSet<int>();

                foreach (StorageBuildingSibling sibling in _siblings)
                {
                    _buildingIds.Add(sibling.Building.Id);
                }
            }
        }
    }

    /// <summary>
    /// Class used by <code>PotentialConnectionArgs</code> to store each building detected, combined with if it will be canceled or not.
    /// </summary>
    public class Connection
    {
        internal Connection(StorageNetworkBuilding building)
        {
            Building = building;
        }

        public bool IsCanceled { get; set;}
        public StorageNetworkBuilding Building { get; private set; }
    }
}