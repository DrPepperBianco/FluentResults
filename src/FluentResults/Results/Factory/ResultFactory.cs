using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace FluentResults.Factory;


/// <summary>
/// Creates <see cref="IResultBase"/> and <see cref="IResult{TValue}"/>
/// </summary>
internal static class ResultFactory
{
    /// <summary>
    /// Creates an empty result object.
    /// </summary>
    /// <remarks>
    /// By Design this is also a success object.
    /// </remarks>
    public static IResultBase CreateEmptyResult() => new ResultBaseImpl();

    /// <summary>
    /// Creates an empty Result object with the given 
    /// <paramref name="valueOrDefault"/>.
    /// </summary>
    /// <remarks>
    /// If <paramref name="valueOrDefault"/> is a subtype of <typeparamref name="TValue"/>
    /// than we create an IResult of that subtype, to that co-variance will work
    /// on the object.
    /// </remarks>
    public static IResult<TValue> CreateEmptyResult<TValue>(TValue valueOrDefault) =>
        valueOrDefault switch
        {
            // Creates an empty object
            null => new ResultImpl<TValue>(default),

            // Value types cannot be co-variant, therefor we use
            // the type directly.
            // (This also applies, if TValue is object, but the
            // actual value is a value type!)
            var value when typeof(TValue).IsValueType || value.GetType().IsValueType =>
                new ResultImpl<TValue>(value),

            // If type is exactly, just create object
            var value when value.GetType() == typeof(TValue) => 
                new ResultImpl<TValue>(value),

            // If type is different, create Result object with inherited type
            // (for co-variance)
            var value => (IResult<TValue>)Activator.CreateInstance(
                typeof(ResultImpl<>).MakeGenericType(value.GetType()),
                value)
        };

}
