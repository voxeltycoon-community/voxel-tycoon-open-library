using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using VoxelTycoon;
using VoxelTycoon.Buildings;
using NUnit.Framework;
using VTOL.Testing;

using SDebug = System.Diagnostics.Debug;

namespace VTOL.StorageNetwork.ConnectionManager
{
	internal class InvalidateSiblingsPatchTests
	{
		[SetUp]
		public void InstantiateLazyManagerClasses()
		{
			TestHelpers.CreateLazyManagerMockObject<ConnectionController>();
		}
		
		[SetUp]
		public void InstantiateConnectionController()
		{
			// We cannot call the default constructor because it will try to
			// create a Unity GameObject, while we run the tests outside of
			// the unity environment.
			//
			// So this will manually create a new instance without
			// the constructor and assigns all its required fields.
			Type controllerType = typeof(ConnectionController);

			ConnectionController controller = 
				(ConnectionController) FormatterServices.GetUninitializedObject(controllerType);

			// Assign the different fields which are accessed by the to be tested methods,
			// these are normally set in the constructor.
			FieldInfo connectionFilterField = controllerType.GetField(
				"_connectionFilters",
				BindingFlags.Instance | BindingFlags.NonPublic
			);
			SDebug.Assert(connectionFilterField != null);
			connectionFilterField.SetValue(controller, new Lazy<List<PriorityConnectionFilter>>());

			// Now we assign our custom object to the LazyManager instance value.
			Type lazyManagerType = typeof(LazyManager<ConnectionController>);
			FieldInfo singletonField = lazyManagerType.GetField(
				"_current",
				BindingFlags.Static | BindingFlags.NonPublic
			);
			SDebug.Assert(singletonField != null);
			singletonField.SetValue(null, controller);
		}

		[SetUp]
		public void InstantiateStorageBuildingManager()
		{
			Type managerType = typeof(StorageBuildingManager);

			StorageBuildingManager storageBuildingManager = 
				(StorageBuildingManager) FormatterServices.GetUninitializedObject(managerType);

			MethodInfo[] methods = managerType.GetMethods();

			foreach (MethodInfo method in methods)
			{
				Console.WriteLine(method.Name);
			}

			FieldInfo[] fields = managerType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

			foreach (FieldInfo field in fields)
			{
				Console.WriteLine(field.Name);
			}

			FieldInfo findSiblingsResultField = managerType.GetField($"<{nameof(storageBuildingManager.FindSiblingsResult)}>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);

			SDebug.Assert(findSiblingsResultField != null);

			findSiblingsResultField.SetValue(storageBuildingManager, new List<StorageBuildingSibling>());

			Type lazyManagerType = typeof(LazyManager<StorageBuildingManager>);
			FieldInfo singletonField = lazyManagerType.GetField(
				"_current",
				BindingFlags.Static | BindingFlags.NonPublic
			);
			SDebug.Assert(singletonField != null);
			singletonField.SetValue(null, storageBuildingManager);
		}

		[Test]
		public void Postfix_Ignores_WhenBuildingInGhostMode() 
		{
			// Arrange
			StorageNetworkBuilding testBuilding = new Mine { IsBuilt = false };
			
			SDebug.Assert(ConnectionController.Current != null);
			ConnectionController.Current.RegisterConnectionFilter(new FailFilter());

			// Act
			InvalidateSiblingsPatch.Postfix(testBuilding);

			// Assert
			/* FailFilter fails the test when its code is executed instead. */
		}
		[Test]
		public void Postfix_ExecutesFilters_WhenBuildingIsBuilt()
		{
			// Arrange
			StorageNetworkBuilding testBuilding = new Mine { IsBuilt = true };
			SuccessFilter successFilter = new SuccessFilter();

			SDebug.Assert(ConnectionController.Current != null);
			ConnectionController.Current.RegisterConnectionFilter(successFilter);

			// Act
			InvalidateSiblingsPatch.Postfix(testBuilding);

			// Assert
			Assert.AreEqual(successFilter.CalledRelevant, 1);
			Assert.AreEqual(successFilter.CalledConnect, 1);
		}


		internal class FailFilter : IConnectionFilter {

			/// <inheritdoc />
			public bool IsRelevant(StorageNetworkBuilding source)
			{
				Assert.Fail("Called IsRelevant in Filter class.");
				return false;
			}

			/// <inheritdoc />
			public void OnConnect(PotentialConnectionArgs potentialConnectionArgs)
			{
				Assert.Fail("Called OnConnect in Filter class.");
			}
		}

		internal class SuccessFilter : IConnectionFilter
		{
			public int CalledRelevant { get; private set; }
			public int CalledConnect { get; private set; }

			/// <inheritdoc />
			public bool IsRelevant(StorageNetworkBuilding source)
			{
				CalledRelevant++;
				return true;
			}

			/// <inheritdoc />
			public void OnConnect(PotentialConnectionArgs potentialConnectionArgs)
			{
				CalledConnect++;
			}
		}

	}
}
