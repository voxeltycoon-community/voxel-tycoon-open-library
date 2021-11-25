using NUnit.Framework;
using System;

namespace VTOL
{
    /// <summary>
    /// This class holds test cases for the VTOLReflectionHelpers-class. The main purpose of VTOLReflectionHelpers is to make reflection more accessible.
    /// </summary>
    internal class VTOLReflectionHelpersTests
    {
        [Test]
        public void SetReadOnlyProperty_ThrowsException_WithNoObject()
        {
            SubClassA subClass = null;

            Assert.Catch<ArgumentNullException>(() => subClass.SetReadOnlyProperty("MyInteger", 10));
        }

        [Test]
        public void SetReadOnlyProperty_ThrowsException_WithNoPropertyName()
        {
            SubClassA subClass = new SubClassA();

            Assert.Catch<ArgumentNullException>(() => subClass.SetReadOnlyProperty(null, 10));
        }

        [Test]
        public void SetReadOnlyProperty_ThrowsException_PropertyIsNotAssignable()
        {
            SubClassA subClass = new SubClassA();

            Assert.Catch<ArgumentException>(() => subClass.SetReadOnlyProperty("MyInteger", "myString"));
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

        [Test]
        public void SetReadOnlyProperty_SetValueWhereSubClassHidesBaseVariable_WithGivenValue()
        {
            //Arrange
            int expected = 10;
            SubClassB subClass = new SubClassB();

            //Act
            subClass.SetReadOnlyProperty("MyInteger", expected);
            int actual = subClass.MyInteger;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// This base class is here for simulating a realistic situation towards test cases.
        /// </summary>
        private class ParentClass
        {
            public int MyInteger { get; private set; }
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
            public new int MyInteger { get; private set; }
        }
    }
}
