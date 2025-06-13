#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentResults;

/// <summary>
/// Definition of a ResultBase
/// </summary>
public interface IResult
{
    /// <summary>
    /// Get all reasons (errors and successes)
    /// </summary>
    List<IReason> Reasons { get; }
}

/// <summary>
/// Definition of a result with a value of type <typeparamref name="TValue"/>
/// </summary>
/// <typeparam name="TValue">The type of the value</typeparam>
public interface IResult<out TValue> : IResult
{
    /// <summary>
    /// Get the Value. If result is failed then a default value is returned. Opposite see property Value.
    /// </summary>
    /// <remarks>
    /// Unless <typeparamref name="TValue"/> is explicitly nullable this interface
    /// assumes, that <see cref="ValueOrDefault"/> is not null, as long as 
    /// <see cref="ResultExt.get_IsFailed(IResult) "/> is false.
    /// </remarks>
    TValue? ValueOrDefault { get; }
}

/// <summary>
/// Most important extensions methods for <see cref="IResult"/> 
/// and <see cref="IResult{TValue}"/>
/// </summary>
public static partial class ResultExt
{
    /// <summary>Extensions for IResult</summary>
    extension(IResult self)
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

    /// <summary>Extensions for IResult{T}</summary>
    extension<TValue>(IResult<TValue> self)
    {
        /// <summary>
        /// Get the Value. If result is failed then an Exception is thrown because a failed result has no value. Opposite see property ValueOrDefault.
        /// </summary>
        public TValue Value
        {
            get
            {
                self.ThrowIfFailed();

                // We assume, that valueOrDefault is not null,
                // if Result is not failed.
                //
                // We don’t check that, because otherwise results like
                // `IResult<string?>` wouldn’t work anymore.
                //
                return self.ValueOrDefault!;
            }
        }
    }

    private static void ThrowIfFailed<TValue>(this IResult<TValue> result)
    {
        if(result.IsFailed)
            throw new InvalidOperationException(
                $"Result is in status failed. Value is not set. Having: {
                    ReasonFormat.ErrorReasonsToString(result.Errors)}");
    }
}
