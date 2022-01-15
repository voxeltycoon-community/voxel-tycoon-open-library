using VoxelTycoon;
using VoxelTycoon.Buildings;

namespace VTOL.StorageNetwork
{
    /// <summary>
    /// Class with utility methods concerning the Storage Network in Voxel Tycoon
    /// </summary>
    public static class StorageNetworkUtils
    {
        /// <summary>
        /// Creates a <code>StorageBuildingSibling</code>.
        /// </summary>
        /// <param name="building">The building you want to create a connection from.</param>
        /// <param name="sibling">The building you want to create a connection to.</param>
        /// <param name="ignoreDistance">Ignore the distance between source and target.</param>
        /// <returns><see cref="StorageBuildingSibling"/></returns>
        public static StorageBuildingSibling CreateSiblingOfBuilding(StorageNetworkBuilding building, StorageNetworkBuilding sibling, bool ignoreDistance = false)
        {
            float distance = ignoreDistance ? 0f : Xz.Distance((Xz)building.Position, (Xz)sibling.Position);

            return new StorageBuildingSibling()
            {
                Building = sibling,
                Distance = distance
            };
        }
    }
}
