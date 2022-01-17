using VoxelTycoon.Buildings;

namespace VTOL.StorageNetwork
{
	/// <summary>
	/// Class used by <see cref="PotentialConnectionArgs"/> to store each building detected, combined with if it will be canceled or not.
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
