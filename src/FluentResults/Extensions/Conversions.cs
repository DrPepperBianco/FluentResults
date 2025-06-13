using System;
using System.Collections.Generic;
using System.Text;

namespace FluentResults;

using Factories;

/// <summary>
/// Extension methods for Result, regarding conversions
/// </summary>
public static class ResultConversions
{
    extension(IResult result)
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
        public IResult ToResult()
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
            isFailed = result.IsFailed;
            errors = isFailed ? [.. result.Errors] : [];
        }
    }

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
        public TValue Unwrap(Func<IResult<TValue>, TValue> onError)
        {
            if(result?.IsSuccess is true)
            {
                return result.ValueOrDefault!;
            }
            else
            {
                return onError(result);
            }
        }

        /// <summary>
        /// Unwraps the value of the result or returns <paramref name="otherValue"/>.
        /// </summary>
        /// <remarks>
        /// if <paramref name="result"/> is successful, it returns its value, otherwise
        /// <paramref name="otherValue"/> is returned.
        /// </remarks>
        /// <typeparam name="TValue">
        /// The underlying type of the result.
        /// </typeparam>
        /// <param name="result">
        /// The result, that is to be unwrapped.
        /// </param>
        /// <param name="otherValue">
        /// Die other value, that should be used, if the result IsFailed.
        /// </param>
        /// <returns>
        /// Return <c>result.Value</c> if result is successful 
        /// and <paramref name="otherValue"/> otherwise.
        /// </returns>
        public TValue UnwrapOr(TValue otherValue)
        {
            if(result?.IsSuccess is true)
            {
                return result.ValueOrDefault!;
            }
            else
            {
                return otherValue;
            }
        }
    }

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

    extension(IError error)
    {
        /// <summary>
        /// Explicit conversion from <see cref="IError"/> to a <see cref="IResult"/>
        /// </summary>
        public IResult ToResult()
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

    extension(IEnumerable<IError> errors)
    {
        /// <summary>
        /// Explicit conversion from <see cref="IEnumerable{IError}"/> to a <see cref="IResult"/>
        /// </summary>
        public IResult ToResult()
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

    extension(IEnumerable<IResult> results)
    {
        /// <summary>
        /// Merge multiple result objects to one result together
        /// </summary>
        public IResult Merge()
        {
            return ResultHelper.Merge(results);
        }
    }

    extension<TValue>(IEnumerable<IResult<TValue>> results)
    {
        /// <summary>
        /// Merge multiple result objects to one result together
        /// </summary>
        public IResult<IReadOnlyList<TValue>> Merge()
        {
            return ResultHelper.MergeWithValue(results);
        }
    }
}
