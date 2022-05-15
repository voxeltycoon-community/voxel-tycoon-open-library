using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using VoxelTycoon;
using VoxelTycoon.Buildings;

namespace VTOL.StorageNetwork.ConnectionManager
{
	/// <summary>
	/// This class provides methods to allow for adjusting Storage Network connections.
	/// </summary>
	public class ConnectionController : LazyManager<ConnectionController>
	{
		private Lazy<List<PriorityConnectionFilter>> _connectionFilters = new Lazy<List<PriorityConnectionFilter>>();
		private bool _isModified;

		private List<PriorityConnectionFilter> ConnectionFilters => _connectionFilters.Value;

		/// <summary>
		/// Registers a <see cref="IConnectionFilter"/> to the connection filter system.
		/// </summary>
		/// <typeparam name="TFilter">Type of class which holds the <see cref="IConnectionFilter"/> functionality.</typeparam>
		/// <param name="priority">(Optional) The priority of the filter. Default value is 0.</param>
		/// <exception cref="InvalidOperationException">When trying to register while the game is done loading.</exception>
		/// <remarks>Filters with a higher priority will be executed after filters with a lower priority. Meaning the alterations made by a filter with a higher priority cannot be overwritten by a filter with a lower priority.</remarks>
		public void RegisterConnectionFilter<TFilter>(double priority = 0)
			where TFilter : IConnectionFilter, new() 
			=> RegisterConnectionFilter(new TFilter(), priority);

		private void RegisterConnectionFilter(IConnectionFilter connectionFilter, double priority = 0)
		{
			if (Vtol.GameState > GameStates.OnGameStarting)
			{
				throw new InvalidOperationException();
			}

			PriorityConnectionFilter priorityListener = new PriorityConnectionFilter(connectionFilter, priority);

			ConnectionFilters.Add(priorityListener);
			_isModified = true;
		}

		/// <summary>
		/// Gets all the connection filters.
		/// </summary>
		/// <param name="connectionFilters">The list with all connection filters.</param>
		/// <returns>True if there are any filters registered, otherwise false.</returns>
		internal bool GetConnectionFilters(out IList<PriorityConnectionFilter> connectionFilters)
		{
			connectionFilters = ConnectionFilters;

			if (!_connectionFilters.IsValueCreated)
			{
				return false;
			}

			TrySort();

			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void TrySort()
		{
			if (_isModified)
			{
				ConnectionFilters.Sort();
				_isModified = false;
			}
		}
	}
}
