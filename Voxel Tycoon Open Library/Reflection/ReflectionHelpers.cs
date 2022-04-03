using JetBrains.Annotations;
using System;
using System.Reflection;

using VTOL.Utils;

using SDebug = System.Diagnostics.Debug;

namespace VTOL.Reflection
{
	/// <summary>
	/// Class with methods to help with reflection 
	/// </summary>
	public static class ReflectionHelpers
	{

		private static BindingFlags _fieldFlags = 
			BindingFlags.Instance | 
			BindingFlags.Static | 
			BindingFlags.NonPublic | 
			BindingFlags.FlattenHierarchy;

		private static BindingFlags _propertyFlags =
			BindingFlags.Instance |
			BindingFlags.Static |
			BindingFlags.NonPublic |
			BindingFlags.Public;


		/// <summary>
		/// Gets the <see cref="FieldInfo"/> of a field within a class.
		/// </summary>
		/// <typeparam name="TReturn">The <see cref="Type"/> of the field.
		/// </typeparam>
		/// <param name="instanceType">The type of the object which holds
		/// the field.</param>
		/// <param name="fieldName">The name of the field.</param>
		/// <param name="isGetter">True if you want to <b>get</b> the value
		/// of the field, otherwise false if you want to <b>set</b> the value
		/// of the field.</param>
		/// <returns>The <see cref="FieldInfo"/> itself of the given field,
		/// which allows you to get or set the value in it if it is available.
		/// Otherwise this will return <c>null</c>.</returns>
		/// <exception cref="TypeMismatchException">If the provided field is
		/// not of the expected type.</exception>
		[CanBeNull]
		private static FieldInfo GetFieldInfo<TReturn>(
			[NotNull] Type instanceType,
			[NotNull] string fieldName,
			bool isGetter
		)
		{
			// Specify the minimum binding flags to find our current field.
			// Only differentiating between if we get or set the field value.
			BindingFlags flags = _fieldFlags;
			flags |= isGetter ? BindingFlags.GetField : BindingFlags.SetField;

			// With the given flags we find the definition on the current object.
			// This is the field accessible on this level, not the parent / base.
			// The `FlattenHierarchy` binding flag only influences static
			// variables but not instances.
			FieldInfo fieldInfo = instanceType.GetField(fieldName, flags);
			// Next we make sure this field exists to begin with.
			// If the field is not present then it never existed and the user
			// might have written the field incorrectly or the implementation
			// has changed since the moment of definition.
			if (fieldInfo == null) return null;
			// When we have the field, we make sure that the return type
			// is of the type which we expect (is it a string, bool, etc.)
			// If this is not the right type then the user has given us
			// incorrect information so we warn them about the usage.
			//
			// `IsAssignableFrom` returns true when the type is a subclass
			// or of the same class
			if (!fieldInfo.FieldType.IsAssignableFrom(typeof(TReturn)))
			{
				throw new TypeMismatchException(
					typeof(TReturn),
					fieldInfo.FieldType
				);
			}
			// And finally when we know we have a field of the right type
			// which is what the user wanted then it can finally be returned.
			return fieldInfo;
		}

		/// <summary>
		/// Gets the <see cref="PropertyInfo"/> of a property within a class.
		/// </summary>
		/// <typeparam name="TReturn">The <see cref="Type"/> of the property.
		/// </typeparam>
		/// <param name="instanceType">The type of the object which holds
		/// the field.</param>
		/// <param name="propertyName">The name of the property.</param>
		/// <param name="isGetter">True if you want to <b>get</b> the value
		/// of the property, otherwise false if you want to <b>set</b> the
		/// value of the property.</param>
		/// <returns>The <see cref="PropertyInfo"/> itself of the given
		/// property, which allows you to get or set the value in it if it
		/// is available. Otherwise this will return <c>null</c>.</returns>
		/// <exception cref="TypeMismatchException">If the provided property is
		/// not of the expected type.</exception>
		[CanBeNull]
		private static MethodInfo GetPropertyMethod<TReturn>(
			[NotNull] Type instanceType,
			[NotNull] string propertyName,
			bool isGetter
		)
		{
			// Specify the minimum binding flags to find our current property.
			// Only differentiating between if we get or set the property value.
			BindingFlags flags = _propertyFlags;
			flags |= isGetter ? BindingFlags.GetProperty : BindingFlags.SetProperty;

			// With the given flags we find the definition on the current object.
			// This is the property accessible on this level, not the parent /
			// base. The `FlattenHierarchy` binding flag only influences static
			// variables but not instances.
			PropertyInfo propertyInfo = instanceType.GetProperty(propertyName, flags);
			// Next we make sure this property exists to begin with.
			// If the property is not present then it never existed and the
			// user might have written the property incorrectly or the
			// implementation has changed since the moment of definition.
			if (propertyInfo == null) return null;
			// When we have the property, we make sure that the return type
			// is of the type which we expect (is it a string, bool, etc.)
			// If this is not the right type then the user has given us
			// incorrect information so we warn them about the usage.
			//
			// `IsAssignableFrom` returns true when the type is a subclass
			// or of the same class
			if (!propertyInfo.PropertyType.IsAssignableFrom(typeof(TReturn)))
			{
				throw new TypeMismatchException(
					typeof(TReturn),
					propertyInfo.PropertyType
				);
			}

			#region DeclaringType Reason Explanation
			// While we are able to get the property on this level, sometimes
			// it does not give us full accessibility to the getters or setters.
			// This is because one can make a property itself `public` while
			// defining their getters and setters individually as `private`.
			// So in order to reach the getters and setters of the property
			// then we need the property definition of the parent or defining
			// type.
			//
			// If this property was defined on the current level, then we just
			// do the same operation twice but it does not result in any bugs.
			//
			// Example:
			//
			// public class ParentClass {
			//   public SomeProperty { get; private set; }
			// }
			//
			// public class ChildClass {
			//   private void TestMethod() {
			//      // This property can reach the getter but not the setter.
			//      PropertyInfo prop = typeof(ChildClass).GetProperty("SomeProperty");
			//      // This property can reach both the getter and setter.
			//      PropertyInfo parentProp = prop.DeclaringType.GetProperty("SomeProperty");
			//      // This property can reach both the getter and setter.
			//      PropertyInfo prop = typeof(ParentClass).GetProperty("SomeProperty");
			//   }
			// }
			//
			#endregion

			propertyInfo = propertyInfo.DeclaringType.GetProperty(propertyName, flags);
			// The type should never change after our previous check,
			// but this is just in case to guarantee that it works.
			SDebug.Assert(propertyInfo.PropertyType.IsAssignableFrom(typeof(TReturn)));

			// Given the property we extract the getter or setter functionality
			// as methods which can be invoked. We cannot directly modify the
			// properties but we should do it via these methods instead.
			// After all properties are compiled under the hood as get_Property
			// and set_Property methods instead.
			MethodInfo propMethod = isGetter
				? propertyInfo.GetGetMethod(true)
				: propertyInfo.GetSetMethod(true);
			// And finally we return the method, but be careful that these
			// methods can be missing when you define a property with only
			// a getter or with only a setter.
			return propMethod;
		}


		/// <summary>
		/// Gets the value of a private field.
		/// </summary>
		/// <typeparam name="TReturn">The <see cref="Type"/> of the field.
		/// </typeparam>
		/// <typeparam name="TInstance">The <see cref="Type"/> of the object
		/// which holds the field.</typeparam>
		/// <param name="obj">The object which holds the field.</param>
		/// <param name="fieldName">The name of the field.</param>
		/// <returns>The value of the given field.</returns>
		/// <exception cref="ArgumentNullException">If any of the provided
		/// parameters does not exist.</exception>
		/// <exception cref="TargetException">If an instance field should be
		/// retrieved while the provided <paramref name="obj"/> is null / does
		/// not exist.</exception>
		/// <exception cref="MemberNotFoundException">If no field could be found
		/// with the provided name on the given object.</exception>
		/// <exception cref="TypeMismatchException">If the provided field is
		/// not of the expected type.</exception>
		public static TReturn GetPrivateFieldValue<TReturn, TInstance>(
			[CanBeNull] this TInstance obj, 
			[NotNull] string fieldName
		)
		{
			FieldInfo fieldInfo = GetFieldInfo<TReturn>(
				typeof(TInstance),
				fieldName,
				isGetter: true
			);
			if (fieldInfo == null) {
				throw new MemberNotFoundException(fieldName, typeof(TInstance));
			}

			return (TReturn) fieldInfo.GetValue(obj);
		}

		/// <summary>
		/// Sets a value to a private field.
		/// </summary>
		/// <typeparam name="TValue">The <see cref="Type"/> of the field you
		/// want to set the value to.</typeparam>
		/// <typeparam name="TInstance">The <see cref="Type"/> of the object
		/// which holds the field.</typeparam>
		/// <param name="obj">The object which hold the field.</param>
		/// <param name="fieldName">The name of the field.</param>
		/// <param name="value">The value which the field should be set to.
		/// </param>
		/// <returns>The instance of the given object for chaining purposes.
		/// </returns>
		/// <exception cref="ArgumentNullException">If any of the provided
		/// parameters does not exist.</exception>
		/// <exception cref="TargetException">If an instance field should be
		/// retrieved while the provided <paramref name="obj"/> is null / does
		/// not exist.</exception>
		/// <exception cref="MemberNotFoundException">If no field could be found
		/// with the provided name on the given object.</exception>
		/// <exception cref="TypeMismatchException">If the provided field is
		/// not of the expected type.</exception>
		public static TInstance SetPrivateFieldValue<TValue, TInstance>(
			[CanBeNull] this TInstance obj,
			[NotNull] string fieldName,
			TValue value
		)
		{
			FieldInfo fieldInfo = GetFieldInfo<TValue>(
				typeof(TInstance),
				fieldName,
				isGetter: false
			);
			if (fieldInfo == null)
			{
				throw new MemberNotFoundException(fieldName, typeof(TInstance));
			}

			fieldInfo.SetValue(obj, value);
			return obj;
		}


		/// <summary>
		/// Gets the value of the provided property. This will try to find a
		/// getter component on the property when available or otherwise look
		/// at the backing field in order to retrieve the value for us.
		/// </summary>
		/// <typeparam name="TReturn">The type of the value which will be
		/// retrieved from the property.
		/// </typeparam>
		/// <typeparam name="TInstance">The type of object which will contain
		/// property.</typeparam>
		/// <param name="obj">The instance which will hold the expected property.
		/// Or <c>null</c> when trying to access a static property.
		/// </param>
		/// <param name="propertyName">The name of the property which should be
		/// retrieved.</param>
		/// <returns>The value of the given property.</returns>
		/// <exception cref="ArgumentNullException">If the provided property
		/// name does not exist.</exception>
		/// <exception cref="TargetException">If an instance property should be
		/// retrieved while the provided <paramref name="obj"/> is null / does
		/// not exist.</exception>
		/// <exception cref="MemberNotFoundException">When the provided property
		/// does not have a getter method nor a backing field which we can access.
		/// This means that there is no way for us to get the value of this
		/// property. For more info how this can be approached, try decompiling
		/// the class and look at the implementation for alternatives.
		/// </exception>
		/// <exception cref="TypeMismatchException">If the provided property is
		/// not of the expected type.</exception>
		public static TReturn GetPropertyValue<TReturn, TInstance>(
			[CanBeNull] this TInstance obj, 
			[NotNull] string propertyName
		)
		{
			//
			// When a property is defined and it contains a getter,
			// then this will retrieve the getter as a method.
			//
			// Functions when:
			// 1. There is a property visible within the given object with
			//    the specified name. Meaning that either, it is defined within
			//    this object itself, or it is defined in the parent but with a
			//    protected or higher accessibility level.
			// 2. There is a getter specified.
			//    Let it be an automatic { get; }-getter or a manually
			//    implemented one.
			//
			MethodInfo propertyMethod = GetPropertyMethod<TReturn>(
				typeof(TInstance),
				propertyName,
				isGetter: true
			);
			if (propertyMethod != null) {
				return (TReturn) propertyMethod.Invoke(obj, Array.Empty<object>());
			}

			//
			// When the getter does not exist, then we can try to look at the
			// backing field of the getter instead. A backing field is only
			// present when a property uses the auto implemented syntax. If
			// no backing field is present, then it cannot be reached.
			//
			// Example:
			// // This getter can be reached (because it has a backing field):
			// protected string SomeProperty { private set; }
			// // This getter cannot be reached (because it does not exist):
			// protected string SomeProperty { set => Console.Write(value); }
			//
			FieldInfo backingField = GetFieldInfo<TReturn>(
				typeof(TInstance),
				$"<{propertyName}>k__BackingField",
				isGetter: false
			);
			if (backingField == null)
			{
				throw new MemberNotFoundException(
					$"No accessible getter could be found for the property '{nameof(propertyName)}'. "
					+  "This can happen when the provided property only has a self-implemented setter. "
					+ $"For more information, you can decompile the '{typeof(TInstance).Name}' class "
					+  "or check the comments for this method."
				);
			}

			// Otherwise we give the value stored within the backing field.
			return (TReturn) backingField.GetValue(obj);
		}

		/// <summary>
		/// Sets the value of the provided property. This will try to find a
		/// setter component on the property when available or otherwise look
		/// at the backing field in order to force the value to the one specified
		/// as a parameter.
		/// </summary>
		/// <typeparam name="TValue">The type of the value which will be set
		/// by stored within the property.</typeparam>
		/// <typeparam name="TInstance">The type of object which will contain
		/// property.</typeparam>
		/// <param name="obj">The instance which will hold the expected property.
		/// Or <c>null</c> when trying to access a static property.
		/// </param>
		/// <param name="propertyName">The name of the property which should be
		/// modified.</param>
		/// <param name="value">The new value which should be assigned to the
		/// property.</param>
		/// <returns>The provided instance for chaining purposes.</returns>
		/// <exception cref="ArgumentNullException">If the provided property
		/// name does not exist.</exception>
		/// <exception cref="TargetException">If an instance property should be
		/// modified while the provided <paramref name="obj"/> is null / does
		/// not exist.</exception>
		/// <exception cref="MemberNotFoundException">When the provided property
		/// does not have a setter method nor a backing field which we can access.
		/// This means that there is no way for us to set the value of this
		/// property. For more info how this can be approached, try decompiling
		/// the class and look at the implementation for alternatives.
		/// </exception>
		/// <exception cref="TypeMismatchException">If the provided property is
		/// not of the expected type.</exception>
		public static TInstance SetPropertyValue<TValue, TInstance>(
			[CanBeNull] this TInstance obj,
			[NotNull]   string propertyName, 
			[CanBeNull] TValue value
		)
		{
			//
			// When a property is defined and it contains a setter,
			// then this will retrieve the setter as a method.
			//
			// Functions when:
			// 1. There is a property visible within the given object with
			//    the specified name. Meaning that either, it is defined within
			//    this object itself, or it is defined in the parent but with a
			//    protected or higher accessibility level.
			// 2. There is a setter specified.
			//    Let it be an automatic { set; }-setter or a manually
			//    implemented one.
			//
			MethodInfo propertyMethod = GetPropertyMethod<TValue>(
				typeof(TInstance),
				propertyName,
				isGetter: false
			);
			if (propertyMethod != null) {
				propertyMethod.Invoke(obj, new object[] { value });
				return obj;
			}

			//
			// When the setter does not exist, then we can try to look at the
			// backing field of the setter instead. A backing field is only
			// present when a property uses the auto implemented syntax. If
			// no backing field is present, then it cannot be reached.
			//
			// Example:
			// // This setter can be reached (because it has a backing field):
			// protected string SomeProperty { private get; }
			// // This setter cannot be reached (because it does not exist):
			// protected string SomeProperty { get => "Hello World!"; }
			//
			FieldInfo propertyField = GetFieldInfo<TValue>(
				typeof(TInstance),
				$"<{propertyName}>k__BackingField",
				isGetter: false
			);
			if (propertyField == null)
			{
				throw new MemberNotFoundException(
					$"No accessible setter could be found for the property '{nameof(propertyName)}'. "
					+  "This can happen when the provided property only has a self-implemented getter. "
					+ $"For more information, you can decompile the '{typeof(TInstance).Name}' class "
					+ "or check the comments for this method."
				);
			}

			return obj;
		}

	}
}
