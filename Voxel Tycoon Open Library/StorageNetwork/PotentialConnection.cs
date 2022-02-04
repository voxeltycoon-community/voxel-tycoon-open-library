using VoxelTycoon.Buildings;

namespace VTOL.StorageNetwork
{
	/// <summary>
	/// Class used by <see cref="PotentialConnectionArgs"/> to store each building detected. Also, this class keeps track of a connection should be canceled once all filters have been executed.
	/// </summary>
	public class PotentialConnection
	{
		internal PotentialConnection(StorageNetworkBuilding building)
		{
			Building = building;
		}

		public bool IsCanceled { get; set; }
		public StorageNetworkBuilding Building { get; }
	}
}
