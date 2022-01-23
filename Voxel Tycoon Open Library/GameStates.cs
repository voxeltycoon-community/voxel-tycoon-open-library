namespace VTOL 
{
	/// <summary>
	/// These are the states used in <see cref="VoxelTycoon.Modding.Mod"/>. The states discribe in which part of the life cycle the game currently is.
	/// VTOL is automatically updating these states in <see cref="Vtol.GameState"/>. When certain operations need to happen in a specific state, this can be used to check the current state.
	/// </summary>
	public enum GameStates
	{
		Initialize,
		OnModsInitialized,
		OnGameStarting,
		OnGameStarted,
		OnDeinitialize
	}
}
