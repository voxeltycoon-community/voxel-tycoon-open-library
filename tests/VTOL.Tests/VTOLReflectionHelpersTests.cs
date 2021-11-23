using NUnit.Framework;
using System;

namespace VTOL
{
    internal class VTOLReflectionHelpersTests
    {
        [Test]
        public void SetReadOnlyProperty_ThrowsException_WithNoPropertyName()
        {
            SubClass subClass = new SubClass();

            Assert.Catch<ArgumentNullException>(() => { subClass.SetReadOnlyProperty(null, 10); });
        }

        [Test]
        public void SetReadOnlyProperty_ThrowsException_PropertyIsNotAssignable()
        {
            SubClass subClass = new SubClass();

            Assert.Catch<ArgumentException>(() => { subClass.SetReadOnlyProperty("MyInteger", "myString"); });
        }

        [Test]
        public void SetReadOnlyProperty_SetValue_GivenValue()
        {
            //Arrange
            int expected = 10;
            SubClass subClass = new SubClass();
            
            //Act
            subClass.SetReadOnlyProperty("MyInteger", expected);
            int actual = subClass.MyInteger;

            //Assert
            Assert.AreEqual(expected, actual);
        }
    }

    internal class ParentClass
    {
        public int MyInteger { get; private set; }
    }

    internal class SubClass : ParentClass
    {

    }
}
