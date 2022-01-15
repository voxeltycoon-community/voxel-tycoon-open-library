using HarmonyLib;
using System.Collections.Generic;
using VoxelTycoon.Buildings;
using VTOL.StorageNetwork;

namespace VTOL.Patches
{
    [HarmonyPatch(typeof(StorageBuildingManager))]
    [HarmonyPatch("FindSiblings")]
    internal static class InvalidateSiblings
    {
        static void Postfix(StorageNetworkBuilding building)
        {
            if (!ConnectionController.Current.TryGetListeners(building.AssetId, out SortedSet<PriorityListener> listeners))
            {
                return;
            }

            List<StorageBuildingSibling> findSiblingsResult = StorageBuildingManager.Current.FindSiblingsResult;
            PotentialConnectionArgs connections = new PotentialConnectionArgs(findSiblingsResult);

            foreach (PriorityListener listener in listeners)
            {
                listener.Listener(connections);
            }

            List<StorageBuildingSibling> approvedConnections = new List<StorageBuildingSibling>(connections.RemoveCanceled());

            findSiblingsResult.Clear();
            findSiblingsResult.Capacity = approvedConnections.Count + connections.AddedConnections.Count;
            findSiblingsResult.AddRange(approvedConnections);
            findSiblingsResult.AddRange(connections.AddedConnections);
        }
    }
}
