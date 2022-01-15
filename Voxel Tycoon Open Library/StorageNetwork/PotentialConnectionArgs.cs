using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using VoxelTycoon.Buildings;

namespace VTOL.StorageNetwork
{
    /// <summary>
    /// This class is used to collect all the Siblings found by <see cref="VoxelTycoon.Buildings.StorageBuildingManager.FindSiblings(StorageNetworkBuilding)"/>
    /// and allows them to be removed based on specified conditions.
    /// </summary>
    public class PotentialConnectionArgs
    {
        private readonly List<StorageBuildingSibling> _siblings;
        private readonly List<StorageBuildingSibling> _addedConnections = new List<StorageBuildingSibling>();
        private readonly List<PotentialConnection> _connections;
        private HashSet<int> _buildingIds;

        internal PotentialConnectionArgs(List<StorageBuildingSibling> siblings)
        {
            _siblings = siblings;
            _connections = new List<PotentialConnection>(_siblings.Count);

            foreach (StorageBuildingSibling sibling in _siblings)
            {
                _connections.Add(new PotentialConnection(sibling.Building));
            }
        }

        public IEnumerable<PotentialConnection> Connections => _connections;
        internal List<StorageBuildingSibling> AddedConnections => _addedConnections;

        /// <summary>
        /// Create a connection with a building not detected by the base game detection.
        /// </summary>
        /// <param name="storageBuildingSibling">The <see cref="StorageBuildingSibling"/> containing the information to create a connection.</param>
        /// <remarks>The normal detection is done by <see cref="StorageBuildingManager.FindSiblings(StorageNetworkBuilding)"/>.
        /// A <see cref="StorageBuildingSibling"/> can be created with <see cref="StorageNetworkUtils.CreateSiblingOfBuilding(StorageNetworkBuilding, StorageNetworkBuilding, bool)"/>.</remarks>
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
        /// <returns>Returns a list with the StorageBuildingSiblings which are not canceled.</returns>
        internal List<StorageBuildingSibling> RemoveCanceled()
        {
            if (_connections.Count != _siblings.Count)
            {
                Debug.Assert(_connections.Count == _siblings.Count);
            }

            for (int i = _connections.Count - 1; i >= 0; i--)
            {
                if (_connections[i].IsCanceled)
                {
                    if (_connections[i].Building.Id != _siblings[i].Building.Id)
                    {
                        Trace.Assert(_connections[i].Building.Id == _siblings[i].Building.Id);
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

    
}