using System;
using System.Runtime.Serialization;

namespace VTOL.Reflection
{

	/// <summary>
	/// The exception which gets thrown in a method which makes use of Reflection.
	/// This encapsulates all exceptions for runtime reflection based errors,
	/// such as members not existing or an operation being invalid like calling
	/// a property setter when one is not available.
	/// </summary>
	[Serializable]
	public class ReflectionException : Exception
	{

		/// <summary>
		/// Initializes a new exception which can be thrown in a reflection
		/// environment.
		/// </summary>
		public ReflectionException() : base() { }

		/// <summary>
		/// Initializes a new exception which can be thrown in a reflection
		/// environment.
		/// </summary>
		/// <param name="message">An explanation of why this reflection
		/// operation could not be executed as expected.</param>
		public ReflectionException(string message) : base(message) { }

		/// <summary>
		/// Initializes a new exception which can be thrown in a reflection
		/// environment.
		/// </summary>
		/// <param name="message">An explanation of why this reflection
		/// operation could not be executed as expected.</param>
		/// <param name="inner">Another exception which caused the reflection
		/// operation to be invalid or irrelevant.</param>
		public ReflectionException(string message, Exception inner)
			: base(message, inner) { }

		/// <summary>
		/// Restores a previously serialized reflection exception.
		/// </summary>
		/// <param name="info">The previously serialized information from
		/// this object. This contains all the data that was retrieved.</param>
		/// <param name="context">The information about the deserialization
		/// process itself. This will describe from which source it came
		/// and how it was retrieved.</param>
		/// <exception cref="ArgumentNullException">If any of the provided
		/// objects does not exist.</exception>
		protected ReflectionException(
			SerializationInfo info,
			StreamingContext context
		) : base(info, context) { }

	}

}