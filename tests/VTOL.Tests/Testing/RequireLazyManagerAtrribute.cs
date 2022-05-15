using System;

using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace VTOL.Testing
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class RequireLazyManagerAtrribute : NUnitAttribute
	{
		private Type _managerType;

		public RequireLazyManagerAtrribute(Type type) => _managerType = type;

		public void BeforeTest(ITest test)
		{

		}

		public void AfterTest(ITest test)
		{

		}
	}
}
