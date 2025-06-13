using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// Objects from Error class cause a failed result
    /// </summary>
    public class Error : IError
    {
        /// <summary>
        /// Message of the error
        /// </summary>
        public string Message { get; protected set; }

        /// <summary>
        /// Metadata of the error
        /// </summary>
        public Dictionary<string, object> Metadata { get; }

        /// <summary>
        /// Get the reasons of an error
        /// </summary>
        public List<IError> Reasons { get; }

        /// <summary>
        /// Creates a new instance of <see cref="Error"/>
        /// </summary>
        protected Error()
        {
            Metadata = new Dictionary<string, object>();
            Reasons = new List<IError>();
        }

        /// <summary>
        /// Creates a new instance of <see cref="Error"/>
        /// </summary>
        /// <param name="message">Description of the error</param>
        public Error(string message)
            : this()
        {
            Message = message;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Error"/>
        /// </summary>
        /// <param name="message">Description of the error</param>
        /// <param name="causedBy">The root cause of the <see cref="Error"/></param>
        public Error(string message, IError causedBy)
            : this(message)
        {
            if (causedBy == null)
                throw new ArgumentNullException(nameof(causedBy));

            Reasons.Add(causedBy);
        }

        /// <summary>
        /// ToString override
        /// </summary>
        public override string ToString()
        {
            return new ReasonStringBuilder()
                .WithReasonType(GetType())
                .WithInfo(nameof(Message), Message)
                .WithInfo(nameof(Metadata), string.Join("; ", Metadata))
                .WithInfo(nameof(Reasons), ReasonFormat.ErrorReasonsToString(Reasons))
                .Build();
        }
    }

    internal class ReasonFormat
    {
        public static string ErrorReasonsToString(IReadOnlyCollection<IError> errorReasons)
        {
            return string.Join("; ", errorReasons);
        }

        public static string ReasonsToString(IReadOnlyCollection<IReason> errorReasons)
        {
            return string.Join("; ", errorReasons);
        }
    }
}
