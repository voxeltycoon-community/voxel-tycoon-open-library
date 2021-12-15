using HarmonyLib;
using JetBrains.Annotations;
using System;
using System.Reflection;

namespace VTOL
{
    /// <summary>
    /// Class with methods to help with reflection.
    /// Should be used with care.
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

            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
            
            PropertyInfo property = obj.GetType().GetProperty(propertyName);

            if (property == null)
            {
                throw new ArgumentException($"{nameof(propertyName)} does not exist.");
            }

            // IsAssignableFrom returns true when the type is a subclass or the same class
            if (!property.PropertyType.IsAssignableFrom(typeof(T)))
            {
                throw new ArgumentException($"{value} is not of type {property.PropertyType}.");
            }

            property = property.DeclaringType.GetProperty(propertyName);
            MethodInfo setMethod = property.GetSetMethod(true);

            if (setMethod == null)
            {
                throw new ArgumentNullException(nameof(setMethod));
            }

            setMethod.Invoke(obj, new object[] { value });
        }

        /// <summary>
        /// Allows for setting a private field
        /// </summary>
        /// <typeparam name="T">Type of the field.</typeparam>
        /// <param name="obj">The object holding the field.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">The value the field should be set to.</param>
        public static void SetPrivateField<T>([NotNull] this object obj, [NotNull] string fieldName, T value)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            Traverse.Create(obj).Field(fieldName).SetValue(value);
        }

        /// <summary>
        /// Allows for getting a value from a private field.
        /// </summary>
        /// <typeparam name="T">Type of the field.</typeparam>
        /// <param name="obj">The object holding the field.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>Value of field.</returns>
        public static T GetPrivateField<T>([NotNull] this object obj, [NotNull] string fieldName)
        {
            return (T)Traverse.Create(obj).Field(fieldName).GetValue();
        }
    }
}
