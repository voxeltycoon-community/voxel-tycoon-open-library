using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using VoxelTycoon.Buildings;

namespace VTOL
{
    [HarmonyPatch(typeof(StorageBuildingManager))]
    [HarmonyPatch("FindSiblings")]
    internal class FindSiblingsPatch
    {
        static void Postfix(StorageNetworkBuilding building)
        {
            if (VTOLStorageNetworkManager.Current.GetListeners(building.AssetId, out IList<Listener> listeners))
            {
                List<StorageBuildingSibling> findSiblingsResult = StorageBuildingManager.Current.FindSiblingsResult;
                PotentialConnectionArgs connections = new PotentialConnectionArgs(findSiblingsResult);

                foreach (Listener listener in listeners)
                {
                    listener(connections);
                }

                List<StorageBuildingSibling> approvedConnections = new List<StorageBuildingSibling>(connections.RemoveCanceled());

                findSiblingsResult.Clear();
                findSiblingsResult.Capacity = approvedConnections.Count + connections.AddedConnections.Count;
                findSiblingsResult.AddRange(approvedConnections);
                findSiblingsResult.AddRange(connections.AddedConnections);
            }
        }
    }
}
