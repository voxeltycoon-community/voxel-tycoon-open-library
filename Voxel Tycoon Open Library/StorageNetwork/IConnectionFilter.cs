using VoxelTycoon.Buildings;

namespace VTOL.StorageNetwork
{
	/// <summary>
	/// A class which is used to build a connection filter should implement this interface.
	/// <para>The methods implemented with this interface are used by <see cref="InvalidateSiblingsPatch"/> to determine if a filter should be executed (<see cref="IsRelevant(StorageNetworkBuilding)"/>) and execute the filter itself (<see cref="OnConnect(PotentialConnectionArgs)"/>).</para>
	/// </summary>
	public interface IConnectionFilter
	{
		/// <summary>
		/// This method is used to check if the filter should be executed.
		/// </summary>
		/// <param name="source">The <see cref="StorageNetworkBuilding"/> of which the connections will be filtered.</param>
		/// <returns>True if this filter (<see cref="OnConnect(PotentialConnectionArgs)"/>) should be executed, otherwise false.</returns>
		bool IsRelevant(StorageNetworkBuilding source);

		/// <summary>
		/// This method is the actual filter, used to filter through all potential connections and decide if a connection should be canceled or not. This method will be execute when <see cref="IsRelevant(StorageNetworkBuilding)"/> returns true.
		/// </summary>
		/// <param name="potentialConnectionArgs">Class with all the required functionality to alter potential connections.</param>
		/// <remarks><see cref="PotentialConnectionArgs"/> is automatically created by <see cref="InvalidateSiblingsPatch"/> and passed to every connection filter that is executed.</remarks>
		void OnConnect(PotentialConnectionArgs potentialConnectionArgs);
	}
}
