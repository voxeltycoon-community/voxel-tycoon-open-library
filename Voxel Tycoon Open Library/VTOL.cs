using HarmonyLib;
using JetBrains.Annotations;
using VoxelTycoon.Modding;

namespace VTOL 
{

	/// <summary>
	/// The main library entry point.
	/// </summary>
	[UsedImplicitly]
	public class VTOL : Mod
	{
		private Harmony _harmony;
		private const string HarmonyID = "vtol.patch";

		/// <summary>
		/// Method called on mod initialization (while loading a save)
		/// </summary>
		protected override void Initialize()
		{
			_harmony = new Harmony(HarmonyID);
			_harmony.PatchAll();
		}

		/// <summary>
		/// Method called on mod de-initialization (at the end of the loaded game - before exiting VoxelTycoon or loading another save file)
		/// </summary>
		protected override void Deinitialize()
		{
			_harmony.UnpatchAll(HarmonyID);
			_harmony = null;
		}
	}

}
