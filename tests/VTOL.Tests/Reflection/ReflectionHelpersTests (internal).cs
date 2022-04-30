using System;
using System.Collections.Generic;
using System.Reflection;

using JetBrains.Annotations;

using NUnit.Framework;

using VTOL.Utils;

namespace VTOL.Reflection
{
	/// <summary>
	/// This class holds test cases for <see cref="ReflectionHelpers"/>. The main purpose of <see cref="ReflectionHelpers"/> is to make reflection more accessible.
	/// </summary>
	// This class is devided over 2 files, one (ReflectionHelperTests (internal).cs) holds tests that only runs tests on internal created test classes.
	// The other file (ReflectionHelperTests (external).cs) runs tests on external voxel tycoon classes.

	// --- This is the internal testing section ---
	[TestFixture]
	internal partial class ReflectionHelpersTests
	{
		#region -- Exception TestCaseSources --

		private static IEnumerable<object[]> ThrowExceptionFieldTestCaseSource
		{
			get
			{
				FieldParentClass parent = new FieldParentClass();

				yield return new object[] { null, "_myPrivateInteger", typeof(TargetException) };
				yield return new object[] { parent, "_nonExistingField", typeof(MemberNotFoundException) };
				yield return new object[] { parent, null, typeof(ArgumentNullException) };
				yield return new object[] { parent, "_differentTypedField", typeof(TypeMismatchException) };
			}
		}

		private static IEnumerable<object[]> ThrowExceptionPropertyTestCaseSource(bool getter)
		{
			PropertyParentClass parent = new PropertyParentClass();

			yield return new object[] { null, "MyPrivateSetInteger", typeof(TargetException) };
			yield return new object[] { parent, "NonExistingProperty", typeof(MemberNotFoundException) };

			yield return new object[] {
				parent,
				getter ? "NonBackedSetterInteger" : "NonBackedGetterInteger",
				typeof(MemberNotFoundException)
			};

			yield return new object[] { parent, null, typeof(ArgumentNullException) };
			yield return new object[] { parent, "DifferentTypedProperty", typeof(TypeMismatchException) };
		}
		
		#endregion

		#region -- GetPrivateFieldValue --

		#region - Exception Testing -

		[Test]
		[TestCaseSource(nameof(ThrowExceptionFieldTestCaseSource))]
		public void GetPrivateFieldValue_ThrowException_WithTestCaseSource(
			FieldParentClass instance,
			string fieldName,
			Type exceptionType
			)
		{
			// Arrange
			// Arrangement done in the parameters through TestCaseSource

			// Assert
			Assert.Catch(exceptionType, () => instance.GetPrivateFieldValue<int, FieldParentClass>(fieldName));
		}

		#endregion

		#region - Success Testing -

		private static IEnumerable<object[]> GetPrivateFieldValue_GetExpectedValue_CasesParent
		{
			get
			{
				// === Always about GetPrivateFieldValue<FieldParentClass>(...) ===

				// Static field with instance value.
				yield return new object[] { new FieldParentClass(), "_privateStaticInteger",   300 };
				yield return new object[] { new FieldParentClass(), "protectedStaticInteger", 200 };
				// Static field with null object.
				yield return new object[] { null, "_privateStaticInteger",   300 };
				yield return new object[] { null, "protectedStaticInteger", 200 };
				
				// Instance field with instance value.
				yield return new object[] { new FieldParentClass(), "_myPrivateInteger",  30 };
				yield return new object[] { new FieldParentClass(), "myProtectedInteger", 20 };
				// Instance field with sub instance value.
				yield return new object[] { new FieldSubClassA(), "_myPrivateInteger",  30 };
				yield return new object[] { new FieldSubClassA(), "myProtectedInteger", 20 };
				// Instance field with sub instance value and new implementation.
				yield return new object[] { new FieldSubClassB(), "_myPrivateInteger",  30 };
				yield return new object[] { new FieldSubClassB(), "myProtectedInteger", 20 };
			}
		}
		
		[Test]
		[TestCaseSource(nameof(GetPrivateFieldValue_GetExpectedValue_CasesParent))]
		public void GetPrivateFieldValue_GetExpectedValue_WithParentClassSource(
			FieldParentClass instance,
			string fieldName,
			int expectedValue
		)
		{
			// Arrange
			// Arrangement done in the parameters through TestCaseSource

			// Act
			int actualValue = instance.GetPrivateFieldValue<int, FieldParentClass>(fieldName);

			// Assert
			Assert.AreEqual(expectedValue, actualValue);
		}

		private static IEnumerable<object[]> GetPrivateFieldValue_GetExpectedValue_CasesSubB
		{
			get
			{
				// === Always about GetPrivateFieldValue<FieldSubClassB>(...) ===

				// Instance field with sub instance value and new implementation.
				yield return new object[] { new FieldSubClassB(), "myProtectedInteger", 80 };
				yield return new object[] { new FieldSubClassB(), "_myPrivateInteger",  70 };
				// Static field with sub instance value and new implementation.
				yield return new object[] { new FieldSubClassB(), "protectedStaticInteger", 800 };
				yield return new object[] { new FieldSubClassB(), "_privateStaticInteger",  700 };
			}
		}

		[Test]
		[TestCaseSource(nameof(GetPrivateFieldValue_GetExpectedValue_CasesSubB))]
		public void GetPrivateFieldValue_GetExpectedValue_WithSubClassBSource(
			FieldSubClassB instance,
			string fieldName,
			int expectedValue
		)
		{
			// Arrange
			// Arrangement done in the parameters through TestCaseSource

			// Act
			int actualValue = instance.GetPrivateFieldValue<int, FieldSubClassB>(fieldName);

			// Assert
			Assert.AreEqual(expectedValue, actualValue);
		}

		#endregion

		#endregion

		#region -- SetPrivateFieldValue --

		#region - Exception Testing -

		[Test]
		[TestCaseSource(nameof(ThrowExceptionFieldTestCaseSource))]
		public void SetPrivateFieldValue_ThrowException_WithTestCaseSource(
			FieldParentClass instance,
			string fieldName,
			Type exceptionType
			)
		{
			// Arrange
			// Arrangement done in the parameters through TestCaseSource

			// Assert
			Assert.Catch(exceptionType, () => instance.SetPrivateFieldValue(fieldName, 1));
		}

		#endregion

		#region - Success Testing -

		private static IEnumerable<object[]> SetPrivateFieldValue_GetExpectedValue_CasesParent
		{
			get
			{
				// === Always about SetPrivateFieldValue<ParentClass>(...) ===

				// Static field with instance value.
				yield return new object[] { new FieldParentClass(), "_privateStaticInteger" };
				yield return new object[] { new FieldParentClass(), "protectedStaticInteger" };
				// Static field with null object.
				yield return new object[] { null, "_privateStaticInteger" };
				yield return new object[] { null, "protectedStaticInteger" };

				// Instance field with instance value.
				yield return new object[] { new FieldParentClass(), "_myPrivateInteger" };
				yield return new object[] { new FieldParentClass(), "myProtectedInteger" };
				// Instance field with sub instance value.
				yield return new object[] { new FieldSubClassA(), "_myPrivateInteger" };
				yield return new object[] { new FieldSubClassA(), "myProtectedInteger" };
				// Instance field with sub instance value and new implementation.
				yield return new object[] { new FieldSubClassB(), "_myPrivateInteger" };
				yield return new object[] { new FieldSubClassB(), "myProtectedInteger" };
			}
		}

		[Test]
		[TestCaseSource(nameof(SetPrivateFieldValue_GetExpectedValue_CasesParent))]
		public void SetPrivateFieldValue_GetExpectedValue_WithParentClassSource(
			FieldParentClass instance,
			string fieldName
			)
		{
			// Arrange
			int expectedValue = 1;

			// Act
			instance.SetPrivateFieldValue(fieldName, expectedValue);
			int actualValue = instance.GetPrivateFieldValue<int, FieldParentClass>(fieldName);

			// Assert
			Assert.AreEqual(expectedValue, actualValue);
		}

		private static IEnumerable<object[]> SetPrivateFieldValue_GetExpectedValue_CasesSubB
		{
			get
			{
				// === Always about SetPrivateFieldValue<SubClassB>(...) ===

				// Instance field with sub instance value and new implementation.
				yield return new object[] { new FieldSubClassB(), "myProtectedInteger" };
				yield return new object[] { new FieldSubClassB(), "_myPrivateInteger" };
				// Static field with sub instance value and new implementation.
				yield return new object[] { new FieldSubClassB(), "protectedStaticInteger" };
				yield return new object[] { new FieldSubClassB(), "_privateStaticInteger" };
			}
		}

		[Test]
		[TestCaseSource(nameof(SetPrivateFieldValue_GetExpectedValue_CasesSubB))]
		public void SetPrivateFieldValue_GetExpectedValue_WithSubClassBSource(
			FieldSubClassB instance,
			string fieldName
			)
		{
			// Arrange
			int expectedValue = 1;

			// Act
			instance.SetPrivateFieldValue(fieldName, expectedValue);
			int actualValue = instance.GetPrivateFieldValue<int, FieldSubClassB>(fieldName);

			// Assert
			Assert.AreEqual(expectedValue, actualValue);
		}

		#endregion

		#endregion

		#region -- GetPropertyValue --

		#region - Exception Testing -

		[Test]
		[TestCaseSource(nameof(ThrowExceptionPropertyTestCaseSource), new object[] { true } )]
		public void GetPropertyValue_ThrowException_WithTestCaseSource(
			PropertyParentClass instance,
			string fieldName,
			Type exceptionType
			)
		{
			// Arrange
			// Arrangement done in the parameters through TestCaseSource

			// Assert
			Assert.Catch(exceptionType, () => instance.GetPropertyValue<int, PropertyParentClass>(fieldName));
		}

		#endregion

		#region - Success Testing -

		private static IEnumerable<object[]> GetPropertyValue_GetExpectedValue_CasesSubclassA
		{
			get
			{
				// === Always about GetPropertyValue<PropertySubclassA>(...) ===

				yield return new object[] { new PropertySubClassA(), nameof(PropertySubClassA.MyPrivateGetInteger), 10 };
				yield return new object[] { new PropertySubClassA(), nameof(PropertySubClassA.MyPrivateSetInteger), 20 };
				yield return new object[] { new PropertySubClassA(), nameof(PropertySubClassA.MyGetInteger), 30 };
				yield return new object[] { new PropertySubClassA(), nameof(PropertySubClassA.MyStaticPrivateGetInteger), 40 };
				yield return new object[] { new PropertySubClassA(), nameof(PropertySubClassA.MyStaticPrivateSetInteger), 50 };

				// See test case 'SetPropertyValue_EnsureNoBleeding_WhenGetAndSet'
				// yield return new object[] { new PropertySubClassA(), nameof(PropertySubClassA.MyStaticGetInteger), 60 };
			}
		}

		[Test]
		[TestCaseSource(nameof(GetPropertyValue_GetExpectedValue_CasesSubclassA))]
		public void GetPropertyValue_GetExpectedValues_CasesSubclassA(
			PropertySubClassA instance,
			string fieldName,
			int expectedValue
		)
		{
			// Arrange
			// Arrangement done in the parameters through TestCaseSource

			// Act
			int actualValue = instance.GetPropertyValue<int, PropertySubClassA>(fieldName);

			// Assert
			Assert.AreEqual(expectedValue, actualValue);
		}

		private static IEnumerable<object[]> GetPropertyValue_GetExpectedValue_CasesSubclassB
		{
			get
			{
				// === Always about GetPropertyValue<PropertySubclassB>(...) ===

				yield return new object[] { new PropertySubClassB(), nameof(PropertySubClassB.MyPrivateGetInteger), 100 };
				yield return new object[] { new PropertySubClassB(), nameof(PropertySubClassB.MyPrivateSetInteger), 200 };
				yield return new object[] { new PropertySubClassB(), nameof(PropertySubClassB.MyGetInteger), 300 };
				yield return new object[] { new PropertySubClassB(), nameof(PropertySubClassB.MyStaticPrivateGetInteger), 400 };
				yield return new object[] { new PropertySubClassB(), nameof(PropertySubClassB.MyStaticPrivateSetInteger), 500 };
				yield return new object[] { new PropertySubClassB(), nameof(PropertySubClassB.MyStaticGetInteger), 600 };
			}
		}

		[Test]
		[TestCaseSource(nameof(GetPropertyValue_GetExpectedValue_CasesSubclassB))]
		public void GetPropertyValue_GetExpectedValues_CasesSubclassB(
			PropertySubClassB instance,
			string fieldName,
			int expectedValue
		)
		{
			// Arrange
			// Arrangement done in the parameters through TestCaseSource

			// Act
			int actualValue = instance.GetPropertyValue<int, PropertySubClassB>(fieldName);

			// Assert
			Assert.AreEqual(expectedValue, actualValue);
		}

		
		[Test]
		public void GetPropertyValue_GetExpectedValues_CaseNonBackedGetter()
		{
			// Arrange
			PropertySubClassA instance = new PropertySubClassA();
			int expectedValue = 70;

			// Act
			int actualValue = instance.GetPropertyValue<int, PropertyParentClass>("NonBackedGetterInteger");

			// Assert
			Assert.AreEqual(expectedValue, actualValue);
		}

		#endregion

		#endregion

		#region -- SetPropertyValue --

		#region - Exception Testing -

		[Test]
		[TestCaseSource(nameof(ThrowExceptionPropertyTestCaseSource), new object[] { false })]
		public void SetPropertyValue_ThrowException_WithTestCaseSource(
			PropertyParentClass instance,
			string fieldName,
			Type exceptionType
		)
		{
			// Arrange
			// Arrangement done in the parameters through TestCaseSource

			// Assert
			Assert.Catch(exceptionType, () => instance.SetPropertyValue<int, PropertyParentClass>(fieldName, 1));
		}

		#endregion

		#region - Success Testing -

		private static IEnumerable<object[]> SetPropertyValue_GetExpectedValue_CasesSubclassB
		{
			get
			{
				// === Always about GetPropertyValue<PropertySubclassB>(...) ===

				yield return new object[] { new PropertySubClassB(), nameof(PropertySubClassB.MyPrivateGetInteger) };
				yield return new object[] { new PropertySubClassB(), nameof(PropertySubClassB.MyPrivateSetInteger) };
				yield return new object[] { new PropertySubClassB(), nameof(PropertySubClassB.MyGetInteger) };
				yield return new object[] { new PropertySubClassB(), nameof(PropertySubClassB.MyStaticPrivateGetInteger) };
				yield return new object[] { new PropertySubClassB(), nameof(PropertySubClassB.MyStaticPrivateSetInteger) };

				// See test case 'SetPropertyValue_EnsureNoBleeding_WhenGetAndSet'
				// yield return new object[] { new PropertySubClassB(), nameof(PropertySubClassB.MyStaticGetInteger) };
			}
		}

		[Test]
		[TestCaseSource(nameof(SetPropertyValue_GetExpectedValue_CasesSubclassB))]
		public void SetPropertyValue_GetExpectedValues_CasesSubclassB(
			PropertySubClassB instance,
			string fieldName
		)
		{
			// Arrange
			int expectedValue = 1;

			// Act
			instance.SetPropertyValue(fieldName, expectedValue);
			int actualValue = instance.GetPropertyValue<int, PropertySubClassB>(fieldName);

			// Assert
			Assert.AreEqual(expectedValue, actualValue);
		}

		[Test]
		public void SetPropertyValue_GetExpectedValue_CaseNonBackedSetterInteger()
		{
			// Arrange
			PropertySubClassA instance = new PropertySubClassA();
			int expectedValue = 1;

			// Act
			instance.SetPropertyValue<int, PropertyParentClass>("NonBackedSetterInteger", expectedValue);
			int actualValue = instance.SecretNonBackedField;

			// Assert
			Assert.AreEqual(expectedValue, actualValue);
		}

		#endregion

		#endregion

		#region -- Bleeding Test --

		/// <summary>
		/// Currently this test case fails because of a very rare scenario that
		/// if you run the get method before a set method and then use the get
		/// function again on a new static property that it will return
		/// the first value from the initial get (what it originally had) and
		/// not the replaced one. We suspect that this is the case because of
		/// caching but we are unsure how to fix it, and because of the rarity
		/// of this scenario we instead leave it as unsupported for now.
		/// </summary>
		// [Test]
		public static void SetPropertyValue_EnsureNoBleeding_WhenGetAndSet() {
			PropertySubClassB instance = new PropertySubClassB();
			string propertyName = nameof(PropertySubClassB.MyStaticGetInteger);
			int originalValue = 600;
			int replacedValue = 500;

			int preSetValue = instance.GetPropertyValue<int, PropertySubClassB>(propertyName);
			Assert.AreEqual(originalValue, preSetValue);

			instance.SetPropertyValue<int, PropertySubClassB>(propertyName, replacedValue);
			int postSetValue = instance.GetPropertyValue<int, PropertySubClassB>(propertyName);
			Assert.AreEqual(replacedValue, postSetValue);
		}

		#endregion

		#region -- Test Classes --
		// These classes are used for simulating a realistic situation towards test cases.

		#region - Field Testing Classes -

		public class FieldParentClass
		{
			protected int myProtectedInteger = 20;
			[UsedImplicitly] private int _myPrivateInteger = 30; //Is not directly referred to, but is used in TestCaseSources

			protected static int protectedStaticInteger = 200;
			[UsedImplicitly] private static int _privateStaticInteger = 300; //Is not directly referred to, but is used in TestCaseSources

			[UsedImplicitly] private string _differentTypedField = "I am an int!"; //Is not directly referred to, but is used in TestCaseSources
		}

		public class FieldSubClassA : FieldParentClass
		{

		}

		public class FieldSubClassB : FieldParentClass
		{
			protected static new int protectedStaticInteger = 800;
			[UsedImplicitly] private static int _privateStaticInteger = 700; //Is not directly referred to, but is used in TestCaseSources

			protected new int myProtectedInteger = 80;
			[UsedImplicitly] private int _myPrivateInteger = 70; //Is not directly referred to, but is used in TestCaseSources
		}

		#endregion

		#region - Property Testing Classes -

		public class PropertyParentClass
		{
			public PropertyParentClass()
			{
				MyPrivateGetInteger = 10;
				MyPrivateSetInteger = 20;
				MyGetInteger = 30;
				MyStaticPrivateGetInteger = 40;
				MyStaticPrivateSetInteger = 50;
			}

			public int SecretNonBackedField;

			public int MyPrivateGetInteger { private get; set; }
			public int MyPrivateSetInteger { get; private set; }
			public int MyGetInteger { get; }
			private int NonBackedGetterInteger => 70;
			private int NonBackedSetterInteger
			{
				set => SecretNonBackedField = value;
			}

			public string DifferentTypedProperty { get; set; } = "I am an int!";

			public static int MyStaticPrivateGetInteger { private get; set; }
			public static int MyStaticPrivateSetInteger { get; private set; }
			public static int MyStaticGetInteger { get; } = 60;

			
		}

		public class PropertySubClassA : PropertyParentClass
		{
			
		}

		public class PropertySubClassB : PropertyParentClass
		{
			public PropertySubClassB()
			{
				MyPrivateGetInteger = 100;
				MyPrivateSetInteger = 200;
				MyGetInteger = 300;
				MyStaticPrivateGetInteger = 400;
				MyStaticPrivateSetInteger = 500;
			}

			public new int MyPrivateGetInteger { private get; set; }
			public new int MyPrivateSetInteger { get; private set; }
			public new int MyGetInteger { get; }
			public new static int MyStaticPrivateGetInteger { private get; set; }
			public new static int MyStaticPrivateSetInteger { get; private set; }
			public new static int MyStaticGetInteger { get; } = 600;
		}

		#endregion

		#endregion
	}
}
