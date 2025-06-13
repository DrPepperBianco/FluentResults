#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentResults;

/// <summary>
/// Definition of a result with a value of type <typeparamref name="TValue"/>
/// </summary>
/// <typeparam name="TValue">The type of the value</typeparam>
public interface IResult<out TValue> : IResultBase
{
    /// <summary>
    /// Get the Value. If result is failed then a default value is returned. Opposite see property Value.
    /// </summary>
    /// <remarks>
    /// Unless <typeparamref name="TValue"/> is explicitly nullable this interface
    /// assumes, that <see cref="ValueOrDefault"/> is not null, as long as 
    /// <see cref="ResultBaseExt.get_IsFailed(IResultBase) "/> is false.
    /// </remarks>
    TValue? ValueOrDefault { get; }
}

/// <summary>
/// Erweiterungen für <see cref="IResult{TValue}"/>
/// </summary>
public static partial class ResultExt
{
    /// <remarks/>
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
