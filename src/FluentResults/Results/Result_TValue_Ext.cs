using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace
namespace FluentResults;


/// <summary>
/// Extensions methods for <see cref="IResult{TValue}"/>
/// </summary>
public static partial class Result_TValue_Ext 
{
    /// <summary>
    /// Set value
    /// </summary>
    public static IResult<TValue> WithValue<TValue>(this IResultBase result, TValue value)
    {
        return ResultFactory
            .CreateEmptyResult(value)
            .WithReasons(result.Reasons);
    }

    /// <summary>
    /// Map all errors of the result via errorMapper
    /// </summary>
    public static IResult<TValue> MapErrors<TValue>(this IResult<TValue> result, Func<IError, IError> errorMapper)
    {
        if (result.IsSuccess())
            return result;

        return ResultFactory
            .CreateEmptyResult(result.ValueOrDefault)
            .WithErrors(result.GetErrors().Select(errorMapper))
            .WithSuccesses(result.GetSuccesses());
    }

    /// <summary>
    /// Map all successes of the result via successMapper
    /// </summary>
    public static IResult<TValue> MapSuccesses<TValue>(this IResult<TValue> result, Func<ISuccess, ISuccess> successMapper)
    {
        return ResultFactory
            .CreateEmptyResult(result.ValueOrDefault)
            .WithErrors(result.GetErrors())
            .WithSuccesses(result.GetSuccesses().Select(successMapper));
    }

    /// <summary>
    /// Convert result with value to result without value
    /// </summary>
    public static IResultBase ToResult<TValue>(this IResult<TValue> result)
    {
        return ResultFactory
            .CreateEmptyResult()
            .WithReasons(result.Reasons);
    }

    /// <summary>
    /// Convert result with value to result with another value. Use valueConverter parameter to specify the value transformation logic.
    /// </summary>
    public static IResult<TNewValue> ToResult<TValue, TNewValue>(this IResult<TValue> result, Func<TValue, TNewValue> valueConverter = null)
    {
        return Map(result, valueConverter);
    }

    /// <summary>
    /// Convert result with value to result with another value. Use valueConverter parameter to specify the value transformation logic.
    /// </summary>
    public static IResult<TNewValue> Map<TValue, TNewValue>(this IResult<TValue> result, Func<TValue, TNewValue> mapLogic)
    {
        if(result.IsSuccess())
        {
            if(mapLogic is null)
                throw new ArgumentException("If result is success then valueConverter should not be null");

            return ResultFactory
                .CreateEmptyResult(mapLogic(result.ValueOrDefault))
                .WithReasons(result.Reasons);
        }
        else
        {
            return ResultFactory
                .CreateEmptyResult<TNewValue>(default!)
                .WithReasons(result.Reasons);
        }
    }

    /// <summary>
    /// Convert result with value to result with another value that may fail.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = result
    ///     .Bind(GetWhichMayFail)
    ///     .Bind(ProcessWhichMayFail)
    ///     .Bind(FormattingWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="result"/>
    /// <param name="bind">Transformation that may fail.</param>
    public static IResult<TNewValue> Bind<TValue, TNewValue>(this IResult<TValue> result, Func<TValue, IResult<TNewValue>> bind)
    {
        if(result.IsSuccess())
        {
            var converted = bind(result.ValueOrDefault);
            return ResultFactory
                .CreateEmptyResult(converted.ValueOrDefault)
                .WithReasons(result.Reasons)
                .WithReasons(converted.Reasons);
        }
        else
        {
            return ResultFactory
                .CreateEmptyResult<TNewValue>(default!)
                .WithReasons(result.Reasons);
        }
    }
    
    /// <summary>
    /// Convert result with value to result with another value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="result"/>
    /// <param name="bind">Transformation that may fail.</param>
    public static async Task<IResult<TNewValue>> Bind<TValue, TNewValue>(this IResult<TValue> result, Func<TValue, Task<IResult<TNewValue>>> bind)
    {
        if(result.IsSuccess())
        {
            var converted = await bind(result.ValueOrDefault);
            return ResultFactory
                .CreateEmptyResult(converted.ValueOrDefault)
                .WithReasons(result.Reasons)
                .WithReasons(converted.Reasons);
        }
        else
        {
            return ResultFactory
                .CreateEmptyResult<TNewValue>(default!)
                .WithReasons(result.Reasons);
        }
    }
    
    /// <summary>
    /// Convert result with value to result with another value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="result"/>
    /// <param name="bind">Transformation that may fail.</param>
    public static async ValueTask<IResult<TNewValue>> Bind<TValue, TNewValue>(this IResult<TValue> result, Func<TValue, ValueTask<IResult<TNewValue>>> bind)
    {
        if(result.IsSuccess())
        {
            var converted = await bind(result.ValueOrDefault);
            return ResultFactory
                .CreateEmptyResult(converted.ValueOrDefault)
                .WithReasons(result.Reasons)
                .WithReasons(converted.Reasons);
        }
        else
        {
            return ResultFactory
                .CreateEmptyResult<TNewValue>(default!)
                .WithReasons(result.Reasons);
        }
    }

    /// <summary>
    /// Execute an action which returns a <see cref="IResultBase"/> synchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var done = await result.Bind(ActionWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="result"/>
    /// <param name="action">Action that may fail.</param>
    public static IResultBase Bind<TValue>(this IResult<TValue> result, Func<TValue, IResultBase> action)
    {
        if(result.IsSuccess())
        {
            var converted = action(result.ValueOrDefault);
            return ResultFactory
                .CreateEmptyResult()
                .WithReasons(result.Reasons)
                .WithReasons(converted.Reasons);
        }
        else
        {
            return ResultFactory
                .CreateEmptyResult()
                .WithReasons(result.Reasons);
        }
    }

    /// <summary>
    /// Execute an action which returns a <see cref="IResultBase"/> asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var done = await result.Bind(ActionWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="result"/>
    /// <param name="action">Action that may fail.</param>
    public static async Task<IResultBase> Bind<TValue>(this IResult<TValue> result, Func<TValue, Task<IResultBase>> action)
    {
        if(result.IsSuccess())
        {
            var converted = await action(result.ValueOrDefault);
            return ResultFactory
                .CreateEmptyResult()
                .WithReasons(result.Reasons)
                .WithReasons(converted.Reasons);
        }
        else
        {
            return ResultFactory
                .CreateEmptyResult()
                .WithReasons(result.Reasons);
        }
    }
    
    /// <summary>
    /// Execute an action which returns a <see cref="Result"/> asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var done = await result.Bind(ActionWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="result"/>
    /// <param name="action">Action that may fail.</param>
    public static async ValueTask<IResultBase> Bind<TValue>(this IResult<TValue> result, Func<TValue, ValueTask<IResultBase>> action)
    {
        if(result.IsSuccess())
        {
            var converted = await action(result.ValueOrDefault);
            return ResultFactory
                .CreateEmptyResult()
                .WithReasons(result.Reasons)
                .WithReasons(converted.Reasons);
        }
        else
        {
            return ResultFactory
                .CreateEmptyResult()
                .WithReasons(result.Reasons);
        }
    }

    /// <summary>
    /// Convertes the result to an <see cref="IResult{TValue}"/>.
    /// </summary>
    /// <remarks>
    /// Warning: There is no actual value assigned! If the result is successful 
    /// the Value might me null (even though the property might me not nullable!).
    /// Maybe prefer the overload with DefaultValue parameter.
    /// </remarks>
    public static IResult<TValue> ToResult<TValue>(this IResultBase result)
    {
        return ResultFactory
            .CreateEmptyResult<TValue>(default!)
            .WithReasons(result.Reasons);
    }

    /// <summary>
    /// Creates a result of object from the typed result. Value types will be boxed.
    /// </summary>
    public static IResult<object> ToObjectResult<TValue>(this IResult<TValue> result)
    {
        return ResultFactory
            .CreateEmptyResult<object>((object)result.ValueOrDefault)
            .WithReasons(result.Reasons);
    }

    /// <summary>
    /// Creates a result from a value.
    /// </summary>
    public static IResult<TValue> ToResult<TValue>(this TValue value)
    {
        return ResultFactory
            .CreateEmptyResult(value);
    }

    /// <summary>
    /// Creates a failed typed result from an error.
    /// </summary>
    public static IResult<TValue> ToResult<TValue>(this IError error)
    {
        return ResultFactory
            .CreateEmptyResult<TValue>(default!)
            .WithError(error);
    }


    /// <summary>
    /// Creates a failed typed result from a list of errors.
    /// </summary>
    public static IResult<TValue> ToResult<TValue>(this IEnumerable<IError> error)
    {
        return ResultFactory
            .CreateEmptyResult<TValue>(default!)
            .WithErrors(error);
    }


    /// <summary>
    /// Deconstruct Result
    /// </summary>
    public static void Deconstruct<TValue>(this IResult<TValue> result, out bool isSuccess, out bool isFailed, out TValue value)
    {
        isSuccess = result.IsSuccess();
        isFailed = result.IsFailed();
        value = isSuccess ? result.ValueOrDefault : default;
    }

    /// <summary>
    /// Deconstruct Result
    /// </summary>
    public static void Deconstruct<TValue>(this IResult<TValue> result, out bool isSuccess, out bool isFailed, out TValue value, out List<IError> errors)
    {
        Deconstruct(result, out isSuccess, out isFailed, out value);
        errors = isFailed ? [.. result.GetErrors()] : [];
    }
}