﻿using System.Collections.Generic;
using HarmonyLib;
using VoxelTycoon.Buildings;

namespace VTOL.StorageNetwork.ConnectionManager
{
	/// <summary>
	/// This class is used to control which <see cref="StorageNetworkBuilding"/> can be connected with eachother.
	/// Voxel Tycoon only dictates that certain buildings can connect with eachother based on their type. With this patch a system is introduced where it is also possible to filter connections based on custom logic.
	/// Filters are classes implementing <see cref="IConnectionFilter"/>, and have the functionality to allow or disallow a connection between two <see cref="StorageNetworkBuilding"/>. 
	/// These filter classes are made by the user and can be registered with <see cref="ConnectionController.RegisterConnectionFilter(IConnectionFilter, double)"/>.
	/// Every time a new <see cref="StorageNetworkBuilding"/> is placed, Voxel Tycoon will update the Storage Network, but only for the <see cref="StorageNetworkBuilding"/> which are in range of the placed building.
	/// To update the Storage Network, Voxel Tycoon will use <see cref="StorageBuildingManager.FindSiblings(StorageNetworkBuilding)"/> for each <see cref="StorageNetworkBuilding"/> that needs an update and returns a list with all potential connections.
	/// Before this list with potential connections is returned, <see cref="InvalidateSiblingsPatch"/> allows all filters to cycle through this list and decide if a connection should be allowed or not.
	/// </summary>
	[HarmonyPatch(typeof(StorageBuildingManager))]
	[HarmonyPatch("FindSiblings")]
	internal static class InvalidateSiblingsPatch
	{
		internal static void Postfix(StorageNetworkBuilding building) 
		{
			if (!building.IsBuilt ||
				!ConnectionController.Current.GetConnectionFilters(out IList<PriorityConnectionFilter> connectionFilters))
			{
				return;
			}

			List<StorageBuildingSibling> findSiblingsResult = StorageBuildingManager.Current.FindSiblingsResult;
			PotentialConnectionArgs connections = new PotentialConnectionArgs(building, findSiblingsResult);

			foreach (PriorityConnectionFilter filter in connectionFilters)
			{
				if (filter.ConnectionFilter.IsRelevant(building))
				{
					filter.ConnectionFilter.OnConnect(connections); 
				}
			}

			List<StorageBuildingSibling> approvedConnections = new List<StorageBuildingSibling>(connections.RemoveCanceled());

			findSiblingsResult.Clear();
			findSiblingsResult.Capacity = approvedConnections.Count + connections.AddedConnections.Count;
			findSiblingsResult.AddRange(approvedConnections);
			findSiblingsResult.AddRange(connections.AddedConnections);
		}
	}
}
