using FluentResults.Results.Factory;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentResults;

/// <summary>
/// Extension methods for Result, regarding conversions
/// </summary>
public static class ResultConversions
{
    /// <summary>Conversions, that work for all IResult (with or without value)</summary>
    extension(IResultBase result)
    {
        /// <summary>
        /// Convert result without value to a result containing a value
        /// </summary>
        /// <typeparam name="TNewValue">Type of the value</typeparam>
        /// <param name="newValue">Value to add to the new result</param>
        public IResult<TNewValue> ToResult<TNewValue>(TNewValue newValue = default)
        {
            return ResultFactory
                .CreateEmptyResult(newValue)
                .WithReasons(result.Reasons);
        }

        /// <summary>
        /// Convert result with value (or without value) to result without value
        /// </summary>
        /// <remarks>
        /// Event if called with result without value a new result
        /// object is constructed.
        /// </remarks>
        public IResultBase ToResult()
        {
            return ResultFactory
                .CreateEmptyResult()
                .WithReasons(result.Reasons);
        }
        /// <summary>
        /// Set value
        /// </summary>
        public IResult<TValue> WithValue<TValue>(TValue value)
        {
            return ResultFactory
                .CreateEmptyResult(value)
                .WithReasons(result.Reasons);
        }

        /// <summary>
        /// Deconstruct Result 
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="isFailed"></param>
        public void Deconstruct(out bool isSuccess, out bool isFailed)
        {
            isSuccess = result.IsSuccess;
            isFailed = result.IsFailed;
        }

        /// <summary>
        /// Deconstruct Result
        /// </summary>
        public void Deconstruct(out bool isSuccess, out bool isFailed, out IReadOnlyList<IError> errors)
        {
            isSuccess = result.IsSuccess;
            isFailed = result.IsFailed);
            errors = isFailed ? [.. result.Errors] : [];
        }
    }

    /// <param name="result">Result with value</param>
    extension<TValue>(IResult<TValue> result)
    {
        /// <summary>
        /// Convert result with value to result with another value. Use valueConverter parameter to specify the value transformation logic.
        /// </summary>
        public IResult<TNewValue> ToResult<TNewValue>(Func<TValue, TNewValue> valueConverter = null)
        {
            // Use mapper: see MappingAndBinding.cs
            return result.Map(valueConverter);
        }

        /// <summary>
        /// Convertes the result to an <see cref="IResult{TValue}"/>.
        /// </summary>
        /// <remarks>
        /// Warning: There is no actual value assigned! If the result is successful 
        /// the Value might me null (even though the property might me not nullable!).
        /// Maybe prefer the overload with DefaultValue parameter.
        /// </remarks>
        public IResult<TValue> ToResult()
        {
            return ResultFactory
                .CreateEmptyResult<TValue>(default!)
                .WithReasons(result.Reasons);
        }

        /// <summary>
        /// Creates a result of object from the typed result. Value types will be boxed.
        /// </summary>
        public IResult<object> ToObjectResult()
        {
            return ResultFactory
                .CreateEmptyResult<object>((object)result.ValueOrDefault)
                .WithReasons(result.Reasons);
        }
        /// <summary>
        /// Deconstruct Result
        /// </summary>
        public void Deconstruct(out bool isSuccess, out bool isFailed, out TValue value)
        {
            isSuccess = result.IsSuccess;
            isFailed = result.IsFailed;
            value = isSuccess ? result.ValueOrDefault : default;
        }

        /// <summary>
        /// Deconstruct Result
        /// </summary>
        public void Deconstruct(out bool isSuccess, out bool isFailed, out TValue value, out List<IError> errors)
        {
            Deconstruct(result, out isSuccess, out isFailed, out value);
            errors = isFailed ? [.. result.Errors] : [];
        }
    }

    // <param name="self">The value</param>
    extension<TValue>(TValue self)
    {
        /// <summary>
        /// Creates a result from a value.
        /// </summary>
        public IResult<TValue> ToResult()
        {
            return ResultFactory
                .CreateEmptyResult(self);
        }
    }

    /// <param name="error">The error</param>
    extension(IError error)
    {
        /// <summary>
        /// Explicit conversion from <see cref="IError"/> to a <see cref="IResultBase"/>
        /// </summary>
        public IResultBase ToResult()
        {
            // return Fail(error);
            return ResultFactory
                .CreateEmptyResult()
                .WithError(error);
        }
        /// <summary>
        /// Creates a failed typed result from an error.
        /// </summary>
        public IResult<TValue> ToResult<TValue>()
        {
            return ResultFactory
                .CreateEmptyResult<TValue>(default!)
                .WithError(error);
        }
    }

    /// <param name="errors">The errors</param>
    extension(IEnumerable<IError> errors)
    {
        /// <summary>
        /// Explicit conversion from <see cref="IEnumerable{IError}"/> to a <see cref="IResultBase"/>
        /// </summary>
        public IResultBase ToResult()
        {
            return ResultFactory
                .CreateEmptyResult()
                .WithErrors(errors);
        }
        /// <summary>
        /// Creates a failed typed result from a list of errors.
        /// </summary>
        public IResult<TValue> ToResult<TValue>()
        {
            return ResultFactory
                .CreateEmptyResult<TValue>(default!)
                .WithErrors(errors);
        }
    }
}
