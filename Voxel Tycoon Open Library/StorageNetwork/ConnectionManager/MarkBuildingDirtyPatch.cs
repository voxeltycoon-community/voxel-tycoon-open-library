using HarmonyLib;
using VoxelTycoon.Buildings;

namespace VTOL.StorageNetwork.ConnectionManager
{
	/// <summary>
	/// This patch makes sure that a <see cref="StorageNetworkBuilding"/> updates it's Storage Network in the same frame it has been built.
	/// <para>After some investigating we found that a <see cref="StorageNetworkBuilding"/> is not updating it's Storage Network in the frame it has been build. Instead it would only update whenever another <see cref="StorageNetworkBuilding"/> is build within range.
	/// For the Connection Filters to work correctly it is necessary that the network is updated after building too. This patch makes sure this happens.</para>
	/// </summary>
	[HarmonyPatch(typeof(StorageNetworkBuilding))]
	[HarmonyPatch("OnBuilt")]
	internal static class MarkBuildingDirtyPatch
	{
		static bool Prefix(StorageNetworkBuilding __instance)
		{
			__instance.MarkSiblingsDirty();

			return true;
		}
	}
}
