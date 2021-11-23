using JetBrains.Annotations;
using System;
using System.Reflection;

namespace VTOL
{
    /// <summary>
    /// Class with methods to help with reflection 
    /// </summary>
    public static class VTOLReflectionHelpers
    {
        /// <summary>
        /// Allows for setting a property which has a private set accessor.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="obj">The object holding the property.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value the property should be set to.</param>
        public static void SetReadOnlyProperty<T>([NotNull] this object obj, [NotNull] string propertyName, T value)
        {
            PropertyInfo property = obj.GetType().GetProperty(propertyName);

            if (property == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (property.PropertyType.IsAssignableFrom(typeof(T)))
            {
                throw new ArgumentException($"{value} is not of type {property.PropertyType}.");
            }

            property = property.DeclaringType.GetProperty(propertyName);
            property.GetSetMethod(true).Invoke(obj, new object[] { value });
        }
    }
}
