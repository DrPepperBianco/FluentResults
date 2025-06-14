#nullable enable
#pragma warning disable CS1591 // No warning for missing comments on `extension`
using System;
using System.Collections.Generic;
using System.Data;
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
    #region Classic Exytension methods for properties IsSuccess, IsFailed and Value
    // These extension methods are for usage in projects, that doesn’t support new
    // extension.
    // All other methods are automatically extension methods by design. But properties
    // are not automatically extension methods.

    /// <summary>
    /// Is true if Reasons contains no errors
    /// </summary>
    public static bool GetIsFailed(this IResult result) =>
        get_IsFailed(result);

    /// <summary>
    /// Is true if Reasons contains no errors
    /// </summary>
    public static bool GetIsSuccess(this IResult result) =>
        get_IsSuccess(result);

    /// <summary>
    /// Get the Value. If result is failed then an Exception is thrown because a failed result has no value. Opposite see property ValueOrDefault.
    /// </summary>
    public static TValue GetValue<TValue>(this IResult<TValue> result) =>
        get_Value<TValue>(result);

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

#pragma warning restore CS1591
