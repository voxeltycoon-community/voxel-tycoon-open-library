using System.Runtime.CompilerServices;
using HarmonyLib;
using JetBrains.Annotations;
using VoxelTycoon.Modding;

[assembly: InternalsVisibleTo("VTOL.Tests")]
namespace VTOL
{
	/// <summary>
	/// The main library entry point.
	/// </summary>
	[UsedImplicitly]
	public class Vtol : Mod
	{
		private Harmony _harmony;
		private const string _harmonyID = "vtol.patch";

		/// <summary>
		/// The GameState Voxel Tycoon is currently in.
		/// </summary>
		/// <remarks>The game states are based on the virtual methods provided in <see cref="VoxelTycoon.Modding.Mod"/>.</remarks>
		public static GameStates GameState { get; internal set; }

		/// <summary>
		/// Method called when this mod is being registered. This is before all mods are available.
		/// </summary>
		protected override void Initialize() 
		{
			GameState = GameStates.Initialize;

			_harmony = new Harmony(_harmonyID);
			_harmony.PatchAll();
		}

		/// <summary>
		/// Method called when all mods are registered.
		/// From here you can access other mods.
		/// </summary>
		protected override void OnModsInitialized()
		{
			GameState = GameStates.OnModsInitialized;
		}

		/// <summary>
		/// Method called before the save or new game is being loaded/generated.
		/// </summary>
		protected override void OnGameStarting()
		{
			GameState = GameStates.OnGameStarting;
		}

		/// <summary>
		/// Method called after the save or new game has been loaded/generated.
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
			
			_harmony.UnpatchAll(_harmonyID);
			_harmony = null;
		}
	}
}
