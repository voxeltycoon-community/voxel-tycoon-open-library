using System;
using System.Reflection;
using System.Runtime.Serialization;
using VoxelTycoon;

using SDebug = System.Diagnostics.Debug;

namespace VTOL.Testing
{
	/// <summary>
	/// This class contains methods which help with setting up a testing environment.
	/// </summary>
	internal static class TestHelpers
	{
		/// <summary>
		/// Creates a <see cref="LazyManager{T}"/> without using the constructor and sets the required <see cref="LazyManager{T}.Current"/>
		/// field with an instance of type T.
		/// </summary>
		/// <typeparam name="T">Type of object of which a <see cref="LazyManager{T}"/> object should be created.</typeparam>
		/// <returns>Returns an uninitialized object of type T</returns>
		/// <remarks>We cannot call the default constructor because it will try to create a Unity GameObject,
		/// while we run the tests outside of the unity environment</remarks>
		public static T CreateLazyManagerMockObject<T>() where T : LazyManager<T>, new()
		{
			Type mockType = typeof(T);
			Type lazyManagerType = typeof(LazyManager<T>);

			T mockObject = (T) FormatterServices.GetUninitializedObject(mockType);

			FieldInfo singletonField = lazyManagerType.GetField("_current", BindingFlags.Static | BindingFlags.NonPublic);
			SDebug.Assert(singletonField != null);
			singletonField.SetValue(null, mockObject);

			return mockObject;
		}
	}
}
