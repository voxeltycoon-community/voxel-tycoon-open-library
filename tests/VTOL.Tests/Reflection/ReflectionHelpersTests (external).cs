using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace VTOL.Reflection
{
	/// <summary>
	/// This class holds test cases for <see cref="ReflectionHelpers"/>. The main purpose of <see cref="ReflectionHelpers"/> is to make reflection more accessible.
	/// </summary>
	// This class is devided over 2 files, one (ReflectionHelperTests (internal).cs) which holds tests that only run tests on internal created test classes.
	// The other file (ReflectionHelperTests (external).cs) runs tests on external voxel tycoon classes.

	// --- This is the external testing part ---
	[TestFixture]
	internal partial class ReflectionHelpersTests
	{

	}
}
