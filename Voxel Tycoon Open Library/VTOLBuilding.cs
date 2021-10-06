using System.Collections.Generic;
using UnityEngine;
using VoxelTycoon;
using VoxelTycoon.Buildings;

namespace VTOL
{
    /// <summary>
    /// Class with utility methods concerning buildings in Voxel Tycoon
    /// </summary>
    public static class VTOLBuilding
    {
        /// <summary>
        /// Attempts to find a building at the cursor position.
        /// </summary>
        /// <typeparam name="T">Type of building to look for.</typeparam>
        /// <param name="buildingUnderCursor">The returned building at the cursor position, otherwise <code>null</code>.</param>
        /// <returns>If a building is at the cursor position.</returns>
        public static bool GetBuildingUnderCursor<T>(out T buildingUnderCursor) where T : Building
        {
            buildingUnderCursor = ObjectRaycaster.Get<T>(null);

            return (buildingUnderCursor != null);
        }
        
        /// <summary>
        /// Will search for buildings within a given range from another building.
        /// </summary>
        /// <typeparam name="T">Type of building to look for.</typeparam>
        /// <param name="buildingOrigin">Position of the origin of the building to look from.</param>
        /// <param name="buildingSize">The total size of the building.</param>
        /// <param name="range">Number of cells to look in X and Z direction.</param>
        /// <returns>Returns an array of buildings within a given range.</returns>
        public static T[] GetBuildingsWithinRange<T>(Vector3 buildingOrigin, Vector3 buildingSize, int range) where T : Building
        {
            List<T> buildings = new List<T>();

            Vector3 overlapBoxSize = buildingSize + new Vector3(range * 2, 0, range * 2);
            int resultsArrayLength = ((int)overlapBoxSize.x) * ((int)overlapBoxSize.z);
            Collider[] results = new Collider[resultsArrayLength];

            Physics.OverlapBoxNonAlloc(buildingOrigin, overlapBoxSize / 2, results, Quaternion.identity, 1 << (int)Layer.Buildings);

            foreach (Collider collider in results)
            {
                if (collider != null)
                {
                    T building = collider.GetComponent<T>();

                    if (building != null)
                    {
                        buildings.Add(building);
                    }
                }
            }

            return buildings.ToArray();
        }
    }
}
