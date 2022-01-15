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

		public static GameStates GameState { get; private set; }

		/// <summary>
		/// Method called on mod initialization
		/// </summary>
		protected override void OnModsInitialized()
        {
			GameState = GameStates.OnModsInitialized;
        }
		
		/// <summary>
		/// Method called on mod initialization (while loading a save)
		/// </summary>
		protected override void Initialize()
		{
			GameState = GameStates.Initialize;
			
			_harmony = new Harmony(HarmonyID);
			_harmony.PatchAll();
		}

		/// <summary>
		/// Method called on game starting
		/// </summary>
		protected override void OnGameStarting()
		{
			GameState = GameStates.OnGameStarting;
		}

		/// <summary>
		/// Method called when the process of starting and loading the game has been finished
		/// </summary>
		protected override void OnGameStarted()
        {
			GameState = GameStates.OnGameStarted;
		}

		/// <summary>
		/// Method called on mod de-initialization (at the end of the loaded game - before exiting VoxelTycoon or loading another save file)
		/// </summary>
		protected override void Deinitialize()
		{
			GameState = GameStates.OnDeinitialize;
			
			_harmony.UnpatchAll(HarmonyID);
			_harmony = null;
		}
	}

}
