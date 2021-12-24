using VoxelTycoon;
using VoxelTycoon.Buildings;

namespace VTOL
{
    /// <summary>
    /// Class with utility methods concerning the Storage Network in Voxel Tycoon
    /// </summary>
    public static class VTOLStorageNetworkUtils
    {
        /// <summary>
        /// Creates a <code>StorageBuildingSibling</code>.
        /// </summary>
        /// <param name="source">The building you want to create a connection from.</param>
        /// <param name="target">The building you want to create a connection to.</param>
        /// <param name="ignoreDistance">Ignore the distance between source and target.</param>
        /// <returns><code>StorageBuildingSibling</code></returns>
        public static StorageBuildingSibling CreateStorageBuildingSibling(StorageNetworkBuilding source, StorageNetworkBuilding target, bool ignoreDistance = false)
        {
            float distance = ignoreDistance ? 0f : Xz.Distance((Xz)source.Position, (Xz)target.Position);

            return new StorageBuildingSibling()
            {
                Building = target,
                Distance = distance
            };
        }
    }
}
