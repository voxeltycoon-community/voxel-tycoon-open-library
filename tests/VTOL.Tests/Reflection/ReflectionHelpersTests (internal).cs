using System;
using System.Collections.Generic;
using System.Reflection;

using NUnit.Framework;

using VTOL.Utils;

namespace VTOL.Reflection
{
	/// <summary>
	/// This class holds test cases for <see cref="ReflectionHelpers"/>. The main purpose of <see cref="ReflectionHelpers"/> is to make reflection more accessible.
	/// This class is devided over 2 files, one which holds tests that only run tests on internal created test classes and another file which runs tests on
	/// external voxel tycoon classes. 
	/// </summary>
	[TestFixture]
	internal partial class ReflectionHelpersTests
	{
		#region -- GetPrivateFieldValue --

		#region - Exception Testing -

		[Test]
		public void GetPrivateFieldValue_ThrowsException_WithNoObject()
		{
			//Arrange
			ParentClass parentClass = null;

			//Assert
			Assert.Catch<TargetException>(
				() => parentClass.GetPrivateFieldValue<int, ParentClass>("_myPrivateInteger")
			);
		}

		[Test]
		public void GetPrivateFieldValue_ThrowsException_WithNoFieldName()
		{
			//Arrange
			ParentClass parentClass = new ParentClass();

			//Assert
			Assert.Catch<ArgumentNullException>(
				() => parentClass.GetPrivateFieldValue<int, ParentClass>(null));
		}

		[Test]
		public void GetPrivateFieldValue_ThrowsException_TypeMismatch()
		{
			//Arrange
			ParentClass parentClass = new ParentClass();

			//Assert
			Assert.Catch<TypeMismatchException>(
				() => parentClass.GetPrivateFieldValue<string, ParentClass>("_myPrivateInteger")
			);
		}

		[Test]
		public void GetPrivateFieldValue_ThrowsException_FieldDoesNotExist()
		{
			//Arrange
			ParentClass parentClass = new ParentClass();

			//Assert
			Assert.Catch<MemberNotFoundException>(
				() => parentClass.GetPrivateFieldValue<string, ParentClass>("_myField")
			);
		}

		#endregion

		#region - Success Testing -

		private static IEnumerable<object[]> GetPrivateFieldValue_GetExpectedValue_CasesParent
		{
			get
			{
				// === Always about GetPrivateFieldValue<ParentClass>(...) ===

				// Static field with instance value.
				yield return new object[] { new ParentClass(), "_privateStaticInteger",   300 };
				yield return new object[] { new ParentClass(), "protectedStaticInteger", 200 };
				// Static field with null object.
				yield return new object[] { null, "_privateStaticInteger",   300 };
				yield return new object[] { null, "protectedStaticInteger", 200 };
				
				// Instance field with instance value.
				yield return new object[] { new ParentClass(), "_myPrivateInteger",  30 };
				yield return new object[] { new ParentClass(), "myProtectedInteger", 20 };
				// Instance field with sub instance value.
				yield return new object[] { new SubClassA(), "_myPrivateInteger",  30 };
				yield return new object[] { new SubClassA(), "myProtectedInteger", 20 };
				// Instance field with sub instance value and new implementation.
				yield return new object[] { new SubClassB(), "_myPrivateInteger",  30 };
				yield return new object[] { new SubClassB(), "myProtectedInteger", 20 };
			}
		}
		
		[Test]
		[TestCaseSource(nameof(GetPrivateFieldValue_GetExpectedValue_CasesParent))]
		public void GetPrivateFieldValue_GetExpectedValue_WithParentClassSource(
			ParentClass instance,
			string fieldName,
			int expectedValue
		)
		{
			// Arrange
			/* Arrangement done in the parameters. */

			// Act
			int actualValue = instance.GetPrivateFieldValue<int, ParentClass>(fieldName);

			// Assert
			Assert.AreEqual(expectedValue, actualValue);
		}

		private static IEnumerable<object[]> GetPrivateFieldValue_GetExpectedValue_CasesSubB
		{
			get
			{
				// === Always about GetPrivateFieldValue<SubClassB>(...) ===

				// Instance field with sub instance value and new implementation.
				yield return new object[] { new SubClassB(), "myProtectedInteger", 80 };
				yield return new object[] { new SubClassB(), "_myPrivateInteger",  70 };
				// Static field with sub instance value and new implementation.
				yield return new object[] { new SubClassB(), "protectedStaticInteger", 800 };
				yield return new object[] { new SubClassB(), "_privateStaticInteger",  700 };
			}
		}

		[Test]
		[TestCaseSource(nameof(GetPrivateFieldValue_GetExpectedValue_CasesSubB))]
		public void GetPrivateFieldValue_GetExpectedValue_WithSubClassBSource(
			SubClassB instance,
			string fieldName,
			int expectedValue
		)
		{
			// Arrange
			/* Arrangement done in the parameters. */

			// Act
			int actualValue = instance.GetPrivateFieldValue<int, SubClassB>(fieldName);

			// Assert
			Assert.AreEqual(expectedValue, actualValue);
		}

		#endregion

		#endregion

		#region -- SetPrivateFieldValue --

		#region - Exception Testing -

		[Test]
		public void SetPrivateFieldValue_ThrowsException_WithNoObject()
		{
			//Arrange
			ParentClass parentClass = null;

			//Assert
			Assert.Catch<TargetException>(
				() => parentClass.SetPrivateFieldValue("_myPrivateInteger", 1));
		}

		[Test]
		public void SetPrivateFieldValue_ThrowsException_WithNoFieldName()
		{
			//Arrange
			ParentClass parentClass = new ParentClass();

			//Assert
			Assert.Catch<ArgumentNullException>(
				() => parentClass.SetPrivateFieldValue(null, 1));
		}

		[Test]
		public void SetPrivateFieldValue_ThrowsException_TypeMismatch()
		{
			//Arrange
			ParentClass parentClass = new ParentClass();

			//Assert
			Assert.Catch<TypeMismatchException>(
				() => parentClass.SetPrivateFieldValue("_myPrivateInteger", "myString"));
		}

		[Test]
		public void SetPrivateFieldValue_ThrowsException_FieldDoesNotExist()
		{
			//Arrange
			ParentClass parentClass = new ParentClass();

			//Assert
			Assert.Catch<MemberNotFoundException>(
				() => parentClass.SetPrivateFieldValue("_myField", 1));
		}

		#endregion

		#region - Success Testing -

		private static IEnumerable<object[]> SetPrivateFieldValue_GetExpectedValue_CasesParent
		{
			get
			{
				// === Always about SetPrivateFieldValue<ParentClass>(...) ===

				// Static field with instance value.
				yield return new object[] { new ParentClass(), "_privateStaticInteger" };
				yield return new object[] { new ParentClass(), "protectedStaticInteger" };
				// Static field with null object.
				yield return new object[] { null, "_privateStaticInteger" };
				yield return new object[] { null, "protectedStaticInteger" };

				// Instance field with instance value.
				yield return new object[] { new ParentClass(), "_myPrivateInteger" };
				yield return new object[] { new ParentClass(), "myProtectedInteger" };
				// Instance field with sub instance value.
				yield return new object[] { new SubClassA(), "_myPrivateInteger" };
				yield return new object[] { new SubClassA(), "myProtectedInteger" };
				// Instance field with sub instance value and new implementation.
				yield return new object[] { new SubClassB(), "_myPrivateInteger" };
				yield return new object[] { new SubClassB(), "myProtectedInteger" };
			}
		}

		[Test]
		[TestCaseSource(nameof(SetPrivateFieldValue_GetExpectedValue_CasesParent))]
		public void SetPrivateFieldValue_GetExpectedValue_WithParentClassSource(
			ParentClass instance,
			string fieldName
			)
		{
			//Arrange
			int expectedValue = 1;

			//Act
			instance.SetPrivateFieldValue(fieldName, expectedValue);
			int actualValue = instance.GetPrivateFieldValue<int, ParentClass>(fieldName);

			//Assert
			Assert.AreEqual(expectedValue, actualValue);
		}

		private static IEnumerable<object[]> SetPrivateFieldValue_GetExpectedValue_CasesSubB
		{
			get
			{
				// === Always about SetPrivateFieldValue<SubClassB>(...) ===

				// Instance field with sub instance value and new implementation.
				yield return new object[] { new SubClassB(), "myProtectedInteger" };
				yield return new object[] { new SubClassB(), "_myPrivateInteger" };
				// Static field with sub instance value and new implementation.
				yield return new object[] { new SubClassB(), "protectedStaticInteger" };
				yield return new object[] { new SubClassB(), "_privateStaticInteger" };
			}
		}

		[Test]
		[TestCaseSource(nameof(SetPrivateFieldValue_GetExpectedValue_CasesSubB))]
		public void SetPrivateFieldValue_GetExpectedValue_WithSubClassBSource(
			SubClassB instance,
			string fieldName
			)
		{
			//Arrange
			int expectedValue = 1;

			//Act
			instance.SetPrivateFieldValue(fieldName, expectedValue);
			int actualValue = instance.GetPrivateFieldValue<int, SubClassB>(fieldName);

			//Assert
			Assert.AreEqual(expectedValue, actualValue);
		}

		#endregion

		#endregion

		#region GetPropertyValue

		#region - Exception Testing -



		#endregion

		#region - Success Testing -



		#endregion

		#endregion

		#region SetPropertyValue

		#region - Exception Testing -



		#endregion

		#region - Success Testing -



		#endregion

		#endregion

		#region Test Classes
		//These classes are used for simulating a realistic situation towards test cases.

		public class ParentClass
		{
			public int MyInteger { get; private set; }

			public static int MyStaticInteger { get; private set; }

			protected int myProtectedInteger = 20;
			private int _myPrivateInteger = 30; //Is not directly referred to, but is used in TestCaseSources

			protected static int protectedStaticInteger = 200;
			private static int _privateStaticInteger = 300; //Is not directly referred to, but is used in TestCaseSources
		}

		public class SubClassA : ParentClass
		{

		}

		public class SubClassB : ParentClass
		{
			protected static new int protectedStaticInteger = 800;
			private static int _privateStaticInteger = 700; //Is not directly referred to, but is used in TestCaseSources

			protected new int myProtectedInteger = 80;
			private int _myPrivateInteger = 70; //Is not directly referred to, but is used in TestCaseSources

			public new int MyInteger { get; }

			public new int MyStaticInteger { get; private set; }
		}

		#endregion


		/*
		[Test]
        public void SetReadOnlyProperty_ThrowsException_WithNoObject()
        {
            //Arrange
            SubClassA subClass = null;

            //Assert
            Assert.Catch<ArgumentNullException>(() => subClass.SetReadOnlyProperty("MyInteger", 10));
        }

        [Test]
        public void SetReadOnlyProperty_ThrowsException_WithNoPropertyName()
        {
            //Arrange
            SubClassA subClass = new SubClassA();

            //Assert
            Assert.Catch<ArgumentNullException>(() => subClass.SetReadOnlyProperty(null, 10));
        }

        [Test]
        public void SetReadOnlyProperty_ThrowsException_PropertyIsNotAssignable()
        {
            //Arrange
            SubClassA subClass = new SubClassA();

            //Assert
            Assert.Catch<ArgumentException>(() => subClass.SetReadOnlyProperty("MyInteger", "myString"));
        }

        [Test]
        public void SetReadOnlyPropertyWithSubClassHidingProperty_ThrowsException_WithNoSetAccessor()
        {
            //Arrange
            SubClassB subClass = new SubClassB();

            //Assert
            Assert.Catch<ArgumentNullException>(() => subClass.SetReadOnlyProperty("MyInteger", 10));
        }

        [Test]
        public void SetReadOnlyProperty_SetValue_WithGivenValue()
        {
            //Arrange
            int expected = 10;
            SubClassA subClass = new SubClassA();

            //Act
            subClass.SetReadOnlyProperty("MyInteger", expected);
            int actual = subClass.MyInteger;

            //Assert
            Assert.AreEqual(expected, actual);
        }
		*/

	}
}
