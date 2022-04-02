using NUnit.Framework;
using System;
using VTOL.Reflection;

namespace VTOL
{
    /// <summary>
    /// This class holds test cases for the VTOLReflectionHelpers-class. The main purpose of VTOLReflectionHelpers is to make reflection more accessible.
    /// </summary>
    internal class VTOLReflectionHelpersTests
    {
        #region SetReadOnlyProperty tests
        [Test]
        public void SetReadOnlyProperty_ThrowsException_WithNoObject()
        {
            //Arrange
            SubClassA subClass = null;

            //Assert
            Assert.Catch<ArgumentNullException>(() => subClass.SetReadOnlyProperty("MyPublicIntegerProperty", 10));
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
        public void SetReadOnlyProperty_ThrowsException_WithIncorrectPropertyName()
        {
            //Arrange
            SubClassA subClass = new SubClassA();

            //Assert
            Assert.Catch<ArgumentException>(() => subClass.SetReadOnlyProperty("IncorrectPropertyName", 10));
        }

        [Test]
        public void SetReadOnlyProperty_ThrowsException_PropertyIsNotAssignable()
        {
            //Arrange
            SubClassA subClass = new SubClassA();

            //Assert
            Assert.Catch<ArgumentException>(() => subClass.SetReadOnlyProperty("MyPublicIntegerProperty", "Hello World"));
        }

        [Test]
        public void SetReadOnlyPropertyWithSubClassHidingProperty_ThrowsException_WithNoSetAccessor()
        {
            //Arrange
            SubClassB subClass = new SubClassB();

            //Assert
            Assert.Catch<ArgumentNullException>(() => subClass.SetReadOnlyProperty("MyPublicIntegerProperty", 10));
        }

        [Test]
        public void SetReadOnlyProperty_SetValue_WithGivenValue()
        {
            //Arrange
            int expected = 10;
            SubClassA subClass = new SubClassA();

            //Act
            subClass.SetReadOnlyProperty("MyPublicIntegerProperty", expected);
            int actual = subClass.MyPublicIntegerProperty;

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region GetPrivateField tests
        [Test]
        public void GetPrivateField_GetValue_WithNoObject()
        {
            //Arrange
            ParentClass parentClass = null;

            //Assert
            Assert.Catch<NullReferenceException>(() => parentClass.GetPrivateField<int>("_myPrivateIntegerField"));
        }

        [Test]
        public void GetPrivateField_GetValue_WithIncorrectFieldType()
        {
            //Arrange
            ParentClass parentClass = new ParentClass();

            //Assert
            Assert.Catch<InvalidCastException>(() => parentClass.GetPrivateField<string>("_myPrivateIntegerField"));
        }

        [Test]
        public void GetPrivateField_GetValue_WithNoFieldName()
        {
            //Arrange
            ParentClass parentClass = new ParentClass();

            //Assert
            Assert.Catch<ArgumentNullException>(() => parentClass.GetPrivateField<int>(null));
        }

        [Test]
        public void GetPrivateField_GetValue_WithIncorrectFieldName()
        {
            //Arrange
            ParentClass parentClass = new ParentClass();

            //Assert
            Assert.Catch<NullReferenceException>(() => parentClass.GetPrivateField<int>("incorrectFieldName"));
        }

        [Test]
        public void GetPrivateField_GetValue_WithValue()
        {
            //Arrange
            int expected = 10;
            ParentClass parentClass = new ParentClass();
            parentClass.SetPrivateIntegerField(expected);

            //Act
            int actual = parentClass.GetPrivateField<int>("_myPrivateIntegerField");

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region SetPrivateField Tests
        [Test]
        public void SetPrivateField_SetValue_WithNoObject()
        {
            //Arrange
            ParentClass parentClass = null;

            //Assert
            Assert.Catch<ArgumentNullException>(() => parentClass.SetPrivateField("_myPrivateIntegerField", 10));
        }

        [Test]
        public void SetPrivateField_SetValue_WithIncorrectFieldType()
        {
            //Arrange
            ParentClass parentClass = new ParentClass();

            //Assert
            Assert.Catch<ArgumentException>(() => parentClass.SetPrivateField("_myPrivateIntegerField", "Hello World"));
        }

        [Test]
        public void SetPrivateField_SetValue_WithNoFieldName()
        {
            //Arrange
            ParentClass parentClass = new ParentClass();

            //Assert
            Assert.Catch<ArgumentNullException>(() => parentClass.SetPrivateField(null, 10));
        }

        //This one returns null instead of the Exception. Im not sure how to do this differently.
        [Test]
        public void SetPrivateField_SetValue_WithIncorrectFieldName()
        {
            //Arrange
            ParentClass parentClass = new ParentClass();

            //Assert
            Assert.Catch<NullReferenceException>(() => parentClass.SetPrivateField("incorrectFieldName", 10));
        }

        [Test]
        public void SetPrivateField_SetValue_WithValue()
        {
            //Arrange
            int expected = 10;
            ParentClass parentClass = new ParentClass();

            //Act
            parentClass.SetPrivateField("_myPrivateIntegerField", expected);
            int actual = parentClass.GetPrivateField<int>("_myPrivateIntegerField");

            //Assert
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Testing classes
        /// <summary>
        /// This base class is here for simulating a realistic situation towards test cases.
        /// </summary>
        private class ParentClass
        {
            public int MyPublicIntegerProperty { get; private set; }

            private int _myPrivateIntegerField;

            public void SetPrivateIntegerField(int value)
            {
                _myPrivateIntegerField = value;
            }
        }

        /// <summary>
        /// This subclass is here for simulating a realistic situation towards test cases.
        /// </summary>
        private class SubClassA : ParentClass
        {

        }

        /// <summary>
        /// This subclass is here for simulating a realistic situation towards test cases.
        /// </summary>
        private class SubClassB : ParentClass
        {
            public new int MyPublicIntegerProperty { get; }
        }
        #endregion
    }
}
