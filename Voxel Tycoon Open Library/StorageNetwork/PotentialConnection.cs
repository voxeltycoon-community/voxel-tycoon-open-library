using VoxelTycoon.Buildings;

namespace VTOL.StorageNetwork
{
	/// <summary>
	/// Class used by <see cref="PotentialConnectionArgs"/> to store each building detected. Also, this class keeps track of a connection should be canceled once all filters have been executed.
	/// </summary>
	public class PotentialConnection
	{
		internal PotentialConnection(StorageNetworkBuilding building) => Building = building;

		/// <summary>
		/// If this connection should be canceled or not.
		/// </summary>
		/// <remarks>This might be overwritten by a filter with a higher priority.</remarks>
		public bool IsCanceled { get; set; }

		/// <summary>
		/// The building <see cref="PotentialConnectionArgs.Source"/> is potentially connecting with.
		/// </summary>
		public StorageNetworkBuilding Building { get; }
	}
}
