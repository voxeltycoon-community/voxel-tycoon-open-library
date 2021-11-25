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
        /// Allows for setting a property which has a <code>private set;</code>-accessor.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="obj">The object holding the property.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value the property should be set to.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="propertyName"/> does not exist.</exception>
        /// <exception cref="ArgumentException">If value is not an instance of the property type.</exception>
        public static void SetReadOnlyProperty<T>([NotNull] this object obj, [NotNull] string propertyName, T value)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            
            PropertyInfo property = obj.GetType().GetProperty(propertyName);

            if (property == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            // IsAssignableFrom returns true when the type is a subclass or the same class
            if (!property.PropertyType.IsAssignableFrom(typeof(T)))
            {
                throw new ArgumentException($"{value} is not of type {property.PropertyType}.");
            }

            property = property.DeclaringType.GetProperty(propertyName);
            property.GetSetMethod(true).Invoke(obj, new object[] { value });
        }
    }
}
