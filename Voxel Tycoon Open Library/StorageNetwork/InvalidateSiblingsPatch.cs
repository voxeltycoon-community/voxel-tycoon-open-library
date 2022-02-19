using System.Collections.Generic;
using HarmonyLib;
using VoxelTycoon.Buildings;

namespace VTOL.StorageNetwork
{
	/// <summary>
	/// This class is used to control which <see cref="StorageNetworkBuilding"/> can be connected with eachother.
	/// Voxel Tycoon only dictates that certain buildings can connect with eachother based on their type. With this patch a system is introduced where it is also possible to filter connections based on <see cref="Building.AssetId"/>.
	/// Filters are methods which have the functionality to allow or disallow a connection between two <see cref="StorageNetworkBuilding"/>. These methods are made by the user and can be registered with <see cref="ConnectionController.RegisterConnectionFilter(int, OnStorageNetworkUpdate, int)"/>.
	/// Every time a new <see cref="StorageNetworkBuilding"/> is placed, Voxel Tycoon will update the Storage Network, but only for the <see cref="StorageNetworkBuilding"/> which are in range of the placed building.
	/// To update the Storage Network, Voxel Tycoon will use <see cref="StorageBuildingManager.FindSiblings(StorageNetworkBuilding)"/> for each <see cref="StorageNetworkBuilding"/> that needs an update and returns a list with all connections.
	/// Before this list is returned, <see cref="InvalidateSiblingsPatch"/> allows all filters registered with the <see cref="Building.AssetId"/> of the current building to cycle through this list and decide if a connection should be allowed or not.
	/// </summary>
	[HarmonyPatch(typeof(StorageBuildingManager))]
	[HarmonyPatch("FindSiblings")]
	internal static class InvalidateSiblingsPatch
	{
		static void Postfix(StorageNetworkBuilding building) 
		{
			if (building.Id == 0 || 
				//If building.Id is 0, this means the building is still a ghost (building mode), and the storage network doesnt need to update. Unfortunately because of an inefficiency in Voxel Tycoon it is constantly updating the network. This way we prevent the filters from happening while the building is still a ghost.
				!ConnectionController.Current.TryGetConnectionFilters(building.AssetId, out IList<PriorityConnectionFilter> connectionFilters))
			{
				return;
			}

			List<StorageBuildingSibling> findSiblingsResult = StorageBuildingManager.Current.FindSiblingsResult;
			PotentialConnectionArgs connections = new PotentialConnectionArgs(building, findSiblingsResult);

			foreach (PriorityConnectionFilter filter in connectionFilters)
			{
				filter.ConnectionFilter(connections);
			}

			List<StorageBuildingSibling> approvedConnections = new List<StorageBuildingSibling>(connections.RemoveCanceled());

			findSiblingsResult.Clear();
			findSiblingsResult.Capacity = approvedConnections.Count + connections.AddedConnections.Count;
			findSiblingsResult.AddRange(approvedConnections);
			findSiblingsResult.AddRange(connections.AddedConnections);
		}
	}
}
