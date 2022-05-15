using System;
using System.Runtime.Serialization;

namespace VTOL.Reflection
{

	/// <summary>
	/// The exception which gets thrown when a member was not present
	/// within a class while reflection does try to access it.
	/// </summary>
	[Serializable]
	public class MemberNotFoundException : ReflectionException
	{

		/// <summary>
		/// Initializes a new exception which gets thrown when a member
		/// was not within a class.
		/// </summary>
		public MemberNotFoundException() : base() { }

		/// <summary>
		/// Initializes a new exception which gets thrown when a member
		/// was not within a class.
		/// </summary>
		/// <param name="message">An explanation of why this reflection
		/// operation could not be executed as expected.</param>
		public MemberNotFoundException(string message) : base(message) { }

		/// <summary>
		/// Initializes a new exception which gets thrown when a member
		/// was not within a class.
		/// </summary>
		/// <param name="memberName">The name of the member to retrieve,
		/// this can be a field name, property name, method name, etc.</param>
		/// <param name="containingClass">The class which did not contain
		/// the expected member.</param>
		public MemberNotFoundException(string memberName, Type containingClass)
			: base($"Could not find '{memberName}' in '{containingClass?.Name}'")
		{ }

		/// <summary>
		/// Initializes a new exception which gets thrown when a member
		/// was not within a class.
		/// </summary>
		/// <param name="message">An explanation of why this reflection
		/// operation could not be executed as expected.</param>
		/// <param name="inner">Another exception which caused the reflection
		/// operation to be invalid or irrelevant.</param>
		public MemberNotFoundException(string message, Exception inner)
			: base(message, inner) { }

		/// <summary>
		/// Restores a previously serialized member not found exception.
		/// </summary>
		/// <param name="info">The previously serialized information from
		/// this object. This contains all the data that was retrieved.</param>
		/// <param name="context">The information about the deserialization
		/// process itself. This will describe from which source it came
		/// and how it was retrieved.</param>
		/// <exception cref="ArgumentNullException">If any of the provided
		/// objects does not exist.</exception>
		protected MemberNotFoundException(
			SerializationInfo info,
			StreamingContext context
		) : base(info, context) { }

	}

}
