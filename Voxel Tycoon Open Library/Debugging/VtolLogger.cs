using System;
using VoxelTycoon;

namespace VTOL.Debugging
{
	/// <summary>
	/// This class has functionality to help with logging to the Player.log-file in a clear manner. It replaces the use of Unity's default <see cref="UnityEngine.Debug.Log(object)"/>.
	/// </summary>
	/// <remarks>The <see cref="Logger{T}"/> class has a nicer output than <see cref="UnityEngine.Debug.Log(object)"/>. Use <see cref="Logger.Log(string)"/> to write to the Player.log-file.</remarks>
	internal static class VtolLogger
	{
		private static Lazy<Logger<Vtol>> _logger = new Lazy<Logger<Vtol>>(() => new Logger<Vtol>());

		/// <summary>
		/// This can be used to write to the Player.log-file.
		/// </summary>
		/// <param name="message">The message that should appear in the log-file.</param>
		internal static void Log(string message) => _logger.Value.Log(message);
	}
}
