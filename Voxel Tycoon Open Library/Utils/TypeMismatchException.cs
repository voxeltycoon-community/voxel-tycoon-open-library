using System;
using System.Runtime.Serialization;

namespace VTOL.Utils
{

	/// <summary>
	/// The exception which gets thrown when an object's type is not of the same
	/// type or assignable to the one which we expect. This can happen when a
	/// methods return value is not of the same type as our expected generic.
	/// </summary>
	[Serializable]
	public class TypeMismatchException : Exception
	{

		/// <summary>
		/// Initializes a new exception which can be thrown when a returned type
		/// is not of the same type or assignle from our expected type.
		/// </summary>
		public TypeMismatchException() : base() { }

		/// <summary>
		/// Initializes a new exception which can be thrown when a returned type
		/// is not of the same type or assignable from our expected type.
		/// </summary>
		/// <param name="message">An explanation of which types could not be
		/// matched.</param>
		public TypeMismatchException(string message) : base(message) { }

		/// <summary>
		/// Initializes a new exception which can be thrown when a returned type
		/// is not of the same type or assignable from our expected type.
		/// </summary>
		/// <param name="expectedType">The type which was expected from
		/// a function.</param>
		/// <param name="actualType">The type which we received from a function.
		/// </param>
		public TypeMismatchException(Type expectedType, Type actualType)
		: base($"'{actualType?.Name}' is not of type '{expectedType?.Name}'.") { }

		/// <summary>
		/// Initializes a new exception which can be thrown when a returned type
		/// is not of the same type or assignable from our expected type.
		/// </summary>
		/// <param name="message">An explanation of which types could not be
		/// matched.</param>
		/// <param name="inner">Another exception which caused the reflection
		/// operation to be invalid or irrelevant.</param>
		public TypeMismatchException(string message, Exception inner)
			: base(message, inner) { }

		/// <summary>
		/// Restores a previously serialized type mismatch exception.
		/// </summary>
		/// <param name="info">The previously serialized information from
		/// this object. This contains all the data that was retrieved.</param>
		/// <param name="context">The information about the deserialization
		/// process itself. This will describe from which source it came
		/// and how it was retrieved.</param>
		/// <exception cref="ArgumentNullException">If any of the provided
		/// objects does not exist.</exception>
		protected TypeMismatchException(
			SerializationInfo info,
			StreamingContext context
		) : base(info, context) { }

	}

}
