using VoxelTycoon.Buildings;

namespace VTOL.StorageNetwork
{
	public interface IConnectionFilter
	{
		/// <summary>
		/// This method is used to check if the filter should be executed.
		/// </summary>
		/// <param name="source">The <see cref="StorageNetworkBuilding"/> of which the connections will be filtered.</param>
		/// <returns>True if this filter (<see cref="OnConnect(PotentialConnectionArgs)"/>) should be executed, otherwise false.</returns>
		bool IsRelevant(StorageNetworkBuilding source);

		/// <summary>
		/// This method is the actual filter, used to filter through all potential connections and decide if a connection should be canceled or not.
		/// </summary>
		/// <param name="potentialConnectionArgs">Class with all the required functionality to alter potential connections.</param>
		void OnConnect(PotentialConnectionArgs potentialConnectionArgs);
	}
}
