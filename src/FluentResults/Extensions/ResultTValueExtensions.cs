using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace FluentResults.Extensions
{
    /// <summary>
    /// Extensions methods for <see cref="IResult{TValue}"/>
    /// </summary>
    /// <remarks>
    /// Adds methods that are defined in 
    /// <see cref="IResult{TValue}"/> so that they are also
    /// available in <see cref="IResult{TValue}"/>.
    /// </remarks>
    public static class ResultTValueExtensions
    {
        /// <summary>
        /// Unwraps the value of the result.
        /// </summary>
        /// <remarks>
        /// if <paramref name="result"/> is successful, it returns its value, otherwise
        /// <paramref name="onError"/> is called with the erroneous result.
        /// </remarks>
        /// <typeparam name="TValue">
        /// The underlying type of the result.
        /// </typeparam>
        /// <param name="result">
        /// The result, that is to be unwrapped.
        /// </param>
        /// <param name="onError">
        /// Mapper for the result in case of error. 
        /// (Could also throw an exception instead.)
        /// </param>
        /// <returns>
        /// Return <c>result.Value</c> if result is successful 
        /// and the result of the call to <paramref name="onError"/>
        /// if the result is not successful.
        /// </returns>
        static public TValue Unwrap<TValue>(this IResult<TValue> result, Func<IResult<TValue>, TValue> onError)
        {
            if(result?.IsSuccess() is true)
            {
                return result.ValueOrDefault!;
            }
            else
            {
                return onError(result);
            }
        }
    }
}
