using FluentResults;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace
namespace FluentResults;

/// <summary>
/// Definition of a ResultBase
/// </summary>
public interface IResultBase
{
    /// <summary>
    /// Get all reasons (errors and successes)
    /// </summary>
    List<IReason> Reasons { get; }
}

/// <summary>
/// Most important extensions methods for <see cref="IResultBase"/>
/// </summary>
public static partial class ResultBaseExt
{
    /// <remarks/>
    extension(IResultBase self)
    {
        /// <summary>
        /// Is true if Reasons contains at least one error
        /// </summary>
        public bool IsFailed => self.Reasons.OfType<IError>().Any();

        /// <summary>
        /// Is true if Reasons contains no errors
        /// </summary>
        public bool IsSuccess => !self.IsFailed;

        /// <summary>
        /// Get all errors
        /// </summary>
        public IReadOnlyList<IError> Errors =>
            [.. self.Reasons.OfType<IError>()];

        /// <summary>
        /// Get all successes
        /// </summary>
        public IReadOnlyList<ISuccess> Successes =>
            [.. self.Reasons.OfType<ISuccess>()];
    }
}