using System;
using System.Reflection;

using NUnit.Framework;

namespace VTOL
{
	class VtolTests
	{
		[Test]
		public void ExecuteOnGameStarting()
		{
			Vtol vtol = new Vtol();

			MethodInfo method = typeof(Vtol).GetMethod("OnGameStarting", BindingFlags.NonPublic | BindingFlags.Instance);
			Assert.NotNull(method);
			method.Invoke(vtol, Array.Empty<object>());
		}
	}
}
