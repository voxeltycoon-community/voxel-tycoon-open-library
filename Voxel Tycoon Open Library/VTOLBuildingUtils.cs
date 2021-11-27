using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using VoxelTycoon;
using VoxelTycoon.Buildings;
using VoxelTycoon.Game;
using VoxelTycoon.Tools.Remover.Handlers;

namespace VTOL
{
    /// <summary>
    /// Class with utility methods concerning buildings in Voxel Tycoon
    /// </summary>
    public static class VTOLBuildingUtils
    {
        /// <summary>
        /// Attempts to find a building at the cursor position.
        /// </summary>
        /// <typeparam name="T">Type of building to look for.</typeparam>
        /// <param name="buildingUnderCursor">The returned building at the cursor position, otherwise <code>null</code>.</param>
        /// <param name="condition">A condition the building should meet.</param>
        /// <returns>If a building is at the cursor position.</returns>
        public static bool TryGetBuildingUnderCursor<T>(out T buildingUnderCursor, Func<Component, bool> condition = null) where T : Building
        {
            if (condition == null)
            {
                Building building = GameUI.Current.BuildingUnderCursor;

                if (building is T)
                {
                    buildingUnderCursor = building as T;
                    return true;
                }
            }

            buildingUnderCursor = ObjectRaycaster.Get<T>(condition);

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
    /// <summary>
    /// Can be overriden to allow the user to change whether the user is allowed to remove building using the buldozer tool.
    /// </summary>
    /// <typeparam name="TBuilding">Type of building to overide.</typeparam>
    public class VTOLBuildingRemoverHandler<TBuilding> : BuildingRemoverHandler<TBuilding> where TBuilding : Building
    {
        /// <summary>
        /// NOT to be overriden.
        /// this overides the base game - honestly I am not entirely sure why this isn't already in the game.
        /// </summary>
        [UsedImplicitly]
        public override bool Match(Component target)
        {
            return target is TBuilding;
        }
        /// <summary>
        /// Can be overriden to allow the user to change whether the user is allowed to remove the building type useing the buldozer tool.
        /// </summary>
        [UsedImplicitly]
        protected override bool CanRemoveInternal(List<TBuilding> targets, out string reason)
        {
            return base.CanRemoveInternal(targets, out reason);
        }
        /// <summary>
        /// Can be overriden to allow the user to change whether the game needs to confirm deletion using the buldozer tool.
        /// </summary>
        [UsedImplicitly]
        protected override bool RequiresConfirmationInternal(TBuilding target, out string confirmationMessage)
        {
            return base.RequiresConfirmationInternal(target, out confirmationMessage);
        }
    }
}
