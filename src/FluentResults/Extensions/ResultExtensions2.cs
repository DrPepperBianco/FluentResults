using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FluentResults;

/// <summary>
/// Extension methods for IResultBase
/// </summary>
public static class ResultExtensions2
{
    /// <summary>
    /// Creates a clone of this result
    /// </summary>
    /// <remarks>
    /// This is needed for immutable fluent syntax.
    /// </remarks>
    public static TResult CloneResult<TResult>(this TResult result)
        where TResult : IResultBase
    {
        // If available use clone method
        if(result is IDeepClonable<TResult> clonable)
        {
            return clonable.DeepClone();
        }
        // Otherwise create clone manually (Type of result must have
        // default constructor for that).
        else
        {
            return ((TResult)Activator.CreateInstance(result.GetType()))
                .WithReasons(result.Reasons);
        }
    }

    /// <summary>
    /// Maps all reasons of the result via reasonMapper
    /// </summary>
    public static TResult MapReasons<TResult, TReason>(this TResult result, Func<TReason, TReason> reasonMapper)
        where TResult : IResultBase
        where TReason : IReason
    {
        var output = result.CloneResult();
        output.Reasons.Clear();
        output.Reasons.AddRange(
            result.Reasons.Select(reason => reason switch
            {
                TReason tReason => reasonMapper(tReason),
                _ => reason
            }));
        return output;
    }


    /// <summary>
    /// Map all errors of the result via errorMapper
    /// </summary>
    /// <remarks>
    /// Uses <see cref="IDeepClonable{T}.DeepClone"/> if available.
    /// </remarks>
    public static TResult MapErrors<TResult>(this TResult result, Func<IError, IError> errorMapper)
        where TResult : IResultBase =>
        result.IsSuccess()
        ? result
        : result.MapReasons(errorMapper);

    /// <summary>
    /// Map all successes of the result via successMapper
    /// </summary>
    /// <remarks>
    /// Uses <see cref="IDeepClonable{T}.DeepClone"/> if available.
    /// </remarks>
    public static TResult MapSuccesses<TResult>(this TResult result, Func<ISuccess, ISuccess> successMapper)
        where TResult : IResultBase =>
        result.MapReasons(successMapper);

    /// <summary>
    /// Create an IResult{TValue} from the given value.
    /// </summary>
    public static Result<TNewValue> ToResult<TNewValue>(this IResultBase result, TNewValue? newValue = default) =>
        new Result<TNewValue>()
        .WithValue(newValue)
        .WithReasons(result.Reasons);

    /// <summary>
    /// Convert result (with value) to result without value
    /// </summary>
    /// <remarks>
    /// Initially, that method was only for result with value, 
    /// but now it works for any result.
    /// </remarks>
    public static Result ToResultWithoutValue(this IResultBase resultBase) => 
        new Result()
        .WithReasons(resultBase.Reasons);

    /// <summary>
    /// Convert result with value to result with another value. Use valueConverter parameter to specify the value transformation logic.
    /// </summary>
    public static Result<TNewValue> ToResult<TValue, TNewValue>(this IResult<TValue> result, Func<TValue, TNewValue>? valueConverter = null) =>
        Map(result, valueConverter);



    #region Varianten von `Bind(Func<IResult<TValue>>)`

    /// <summary>
    /// Convert result to result with value that may fail.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="resultBase">The resultBase that is bound.</param>
    /// <param name="bind">Transformation that may fail.</param>
    public static Result<TNewValue> Bind<TNewValue>(this IResultBase resultBase, Func<IResult<TNewValue>> bind)
    {
        var result = new Result<TNewValue>();
        result.WithReasons(resultBase.Reasons);

        if(resultBase.IsSuccess())
        {
            var converted = bind();
            result.WithValue(converted.ValueOrDefault);
            result.WithReasons(converted.Reasons);
        }

        return result;
    }

    /// <summary>
    /// Convert result to result with value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="resultBase">The resultBase that is bound.</param>
    /// <param name="bind">Transformation that may fail.</param>
    public static async Task<Result<TNewValue>> Bind<TNewValue>(this IResultBase resultBase, Func<Task<IResult<TNewValue>>> bind)
    {
        var result = new Result<TNewValue>();
        result.WithReasons(resultBase.Reasons);

        if(resultBase.IsSuccess())
        {
            var converted = await bind();
            result.WithValue(converted.ValueOrDefault);
            result.WithReasons(converted.Reasons);
        }

        return result;
    }

    /// <summary>
    /// Convert result to result with value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="resultBase">The resultBase that is bound.</param>
    /// <param name="bind">Transformation that may fail.</param>
    public static async Task<Result<TNewValue>> Bind<TNewValue>(this IResultBase resultBase, Func<Task<Result<TNewValue>>> bind)
    {
        var result = new Result<TNewValue>();
        result.WithReasons(resultBase.Reasons);

        if(resultBase.IsSuccess())
        {
            var converted = await bind();
            result.WithValue(converted.ValueOrDefault);
            result.WithReasons(converted.Reasons);
        }

        return result;
    }

    /// <summary>
    /// Convert result to result with value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="resultBase">The resultBase that is bound.</param>
    /// <param name="bind">Transformation that may fail.</param>
    public static async ValueTask<Result<TNewValue>> Bind<TNewValue>(this IResultBase resultBase, Func<ValueTask<IResult<TNewValue>>> bind)
    {
        var result = new Result<TNewValue>();
        result.WithReasons(resultBase.Reasons);

        if(resultBase.IsSuccess())
        {
            var converted = await bind();
            result.WithValue(converted.ValueOrDefault);
            result.WithReasons(converted.Reasons);
        }

        return result;
    }

    /// <summary>
    /// Convert result to result with value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="resultBase">The resultBase that is bound.</param>
    /// <param name="bind">Transformation that may fail.</param>
    public static async ValueTask<Result<TNewValue>> Bind<TNewValue>(this IResultBase resultBase, Func<ValueTask<Result<TNewValue>>> bind)
    {
        var result = new Result<TNewValue>();
        result.WithReasons(resultBase.Reasons);

        if(resultBase.IsSuccess())
        {
            var converted = await bind();
            result.WithValue(converted.ValueOrDefault);
            result.WithReasons(converted.Reasons);
        }

        return result;
    }

    #endregion


    #region Varianten von `Bind(Action)`

    /// <summary>
    /// Execute an action which returns a <see cref="Result"/>.
    /// </summary>
    /// <example>
    /// <code>
    ///  var done = result.Bind(ActionWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="resultBase">The resultBase that is bound.</param>
    /// <param name="action">Action that may fail.</param>
    public static Result Bind(this IResultBase resultBase, Func<IResultWithoutValue> action)
    {
        var result = new Result();
        result.WithReasons(resultBase.Reasons);

        if(resultBase.IsSuccess())
        {
            var converted = action();
            result.WithReasons(converted.Reasons);
        }

        return result;
    }

    /// <summary>
    /// Execute an action which returns a <see cref="Result"/> asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var done = result.Bind(ActionWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="resultBase">The resultBase that is bound.</param>
    /// <param name="action">Action that may fail.</param>
    public static async Task<Result> Bind(this IResultBase resultBase, Func<Task<IResultWithoutValue>> action)
    {
        var result = new Result();
        result.WithReasons(resultBase.Reasons);

        if(resultBase.IsSuccess())
        {
            var converted = await action();
            result.WithReasons(converted.Reasons);
        }

        return result;
    }

    /// <summary>
    /// Execute an action which returns a <see cref="Result"/> asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var done = result.Bind(ActionWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="resultBase">The resultBase that is bound.</param>
    /// <param name="action">Action that may fail.</param>
    public static async Task<Result> Bind(this IResultBase resultBase, Func<Task<Result>> action)
    {
        var result = new Result();
        result.WithReasons(resultBase.Reasons);

        if(resultBase.IsSuccess())
        {
            var converted = await action();
            result.WithReasons(converted.Reasons);
        }

        return result;
    }

    /// <summary>
    /// Execute an action which returns a <see cref="Result"/> asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var done = result.Bind(ActionWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="resultBase">The resultBase that is bound.</param>
    /// <param name="action">Action that may fail.</param>
    public static async ValueTask<Result> Bind(this IResultBase resultBase, Func<ValueTask<IResultWithoutValue>> action)
    {
        var result = new Result();
        result.WithReasons(resultBase.Reasons);

        if(resultBase.IsSuccess())
        {
            var converted = await action();
            result.WithReasons(converted.Reasons);
        }

        return result;
    }

    /// <summary>
    /// Execute an action which returns a <see cref="Result"/> asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var done = result.Bind(ActionWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="resultBase">The resultBase that is bound.</param>
    /// <param name="action">Action that may fail.</param>
    public static async ValueTask<Result> Bind(this IResultBase resultBase, Func<ValueTask<Result>> action)
    {
        var result = new Result();
        result.WithReasons(resultBase.Reasons);

        if(resultBase.IsSuccess())
        {
            var converted = await action();
            result.WithReasons(converted.Reasons);
        }

        return result;
    }

    #endregion


    #region Varianten von `Map()`

    /// <summary>
    /// Convert result with value to result with another value. Use valueConverter parameter to specify the value transformation logic.
    /// </summary>
    public static Result<TNewValue> Map<TValue, TNewValue>(this IResult<TValue> @this, Func<TValue, TNewValue> mapLogic)
    {
        if(@this.IsSuccess() && mapLogic == null)
            throw new ArgumentException("If result is success then valueConverter should not be null");

        return new Result<TNewValue>()
               .WithValue(@this.IsFailed() ? default : mapLogic(@this.Value))
               .WithReasons(@this.Reasons);
    }

    #endregion


    #region Varianten von `Bind(Func<TValue, IResult<TNewValue>>)`


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
    /// <param name="this">The result that is bound.</param>
    /// <param name="bind">Transformation that may fail.</param>
    public static Result<TNewValue> Bind<TValue, TNewValue>(this IResult<TValue> @this, Func<TValue, IResult<TNewValue>> bind)
    {
        var result = new Result<TNewValue>();
        result.WithReasons(@this.Reasons);

        if(@this.IsSuccess())
        {
            var converted = bind(@this.Value);
            result.WithValue(converted.ValueOrDefault);
            result.WithReasons(converted.Reasons);
        }

        return result;
    }

    /// <summary>
    /// Convert result with value to result with another value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="this">The result that is bound.</param>
    /// <param name="bind">Transformation that may fail.</param>
    public static async Task<Result<TNewValue>> Bind<TValue, TNewValue>(this IResult<TValue> @this, Func<TValue, Task<IResult<TNewValue>>> bind)
    {
        var result = new Result<TNewValue>();
        result.WithReasons(@this.Reasons);

        if(@this.IsSuccess())
        {
            var converted = await bind(@this.Value);
            result.WithValue(converted.ValueOrDefault);
            result.WithReasons(converted.Reasons);
        }

        return result;
    }

    /// <summary>
    /// Convert result with value to result with another value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="this">The result that is bound.</param>
    /// <param name="bind">Transformation that may fail.</param>
    public static async Task<Result<TNewValue>> Bind<TValue, TNewValue>(this IResult<TValue> @this, Func<TValue, Task<Result<TNewValue>>> bind)
    {
        var result = new Result<TNewValue>();
        result.WithReasons(@this.Reasons);

        if(@this.IsSuccess())
        {
            var converted = await bind(@this.Value);
            result.WithValue(converted.ValueOrDefault);
            result.WithReasons(converted.Reasons);
        }

        return result;
    }

    /// <summary>
    /// Convert result with value to result with another value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="this">The result that is bound.</param>
    /// <param name="bind">Transformation that may fail.</param>
    public static async ValueTask<Result<TNewValue>> Bind<TValue, TNewValue>(this IResult<TValue> @this, Func<TValue, ValueTask<IResult<TNewValue>>> bind)
    {
        var result = new Result<TNewValue>();
        result.WithReasons(@this.Reasons);

        if(@this.IsSuccess())
        {
            var converted = await bind(@this.Value);
            result.WithValue(converted.ValueOrDefault);
            result.WithReasons(converted.Reasons);
        }

        return result;
    }

    /// <summary>
    /// Convert result with value to result with another value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="this">The result that is bound.</param>
    /// <param name="bind">Transformation that may fail.</param>
    public static async ValueTask<Result<TNewValue>> Bind<TValue, TNewValue>(this IResult<TValue> @this, Func<TValue, ValueTask<Result<TNewValue>>> bind)
    {
        var result = new Result<TNewValue>();
        result.WithReasons(@this.Reasons);

        if(@this.IsSuccess())
        {
            var converted = await bind(@this.Value);
            result.WithValue(converted.ValueOrDefault);
            result.WithReasons(converted.Reasons);
        }

        return result;
    }

    #endregion


    #region Varianten von `Bind(Func<TValue, IResultBase>)`

    /// <summary>
    /// Execute an action which returns a <see cref="Result"/>.
    /// </summary>
    /// <example>
    /// <code>
    ///  var done = result.Bind(ActionWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="this">The result that is bound.</param>
    /// <param name="action">Action that may fail.</param>
    public static Result Bind<TValue>(this IResult<TValue> @this, Func<TValue, IResultWithoutValue> action)
    {
        var result = new Result();
        result.WithReasons(@this.Reasons);

        if(@this.IsSuccess())
        {
            var converted = action(@this.Value);
            result.WithReasons(converted.Reasons);
        }

        return result;
    }

    /// <summary>
    /// Execute an action which returns a <see cref="Result"/> asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var done = await result.Bind(ActionWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="this">The result that is bound.</param>
    /// <param name="action">Action that may fail.</param>
    public static async Task<Result> Bind<TValue>(this IResult<TValue> @this, Func<TValue, Task<IResultWithoutValue>> action)
    {
        var result = new Result();
        result.WithReasons(@this.Reasons);

        if(@this.IsSuccess())
        {
            var converted = await action(@this.Value);
            result.WithReasons(converted.Reasons);
        }

        return result;
    }

    /// <summary>
    /// Execute an action which returns a <see cref="Result"/> asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var done = await result.Bind(ActionWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="this">The result that is bound.</param>
    /// <param name="action">Action that may fail.</param>
    public static async Task<Result> Bind<TValue>(this IResult<TValue> @this, Func<TValue, Task<Result>> action)
    {
        var result = new Result();
        result.WithReasons(@this.Reasons);

        if(@this.IsSuccess())
        {
            var converted = await action(@this.Value);
            result.WithReasons(converted.Reasons);
        }

        return result;
    }

    /// <summary>
    /// Execute an action which returns a <see cref="Result"/> asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var done = await result.Bind(ActionWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="this">The result that is bound.</param>
    /// <param name="action">Action that may fail.</param>
    public static async ValueTask<Result> Bind<TValue>(this IResult<TValue> @this, Func<TValue, ValueTask<IResultWithoutValue>> action)
    {
        var result = new Result();
        result.WithReasons(@this.Reasons);

        if(@this.IsSuccess())
        {
            var converted = await action(@this.Value);
            result.WithReasons(converted.Reasons);
        }

        return result;
    }

    /// <summary>
    /// Execute an action which returns a <see cref="Result"/> asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var done = await result.Bind(ActionWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="this">The result that is bound.</param>
    /// <param name="action">Action that may fail.</param>
    public static async ValueTask<Result> Bind<TValue>(this IResult<TValue> @this, Func<TValue, ValueTask<Result>> action)
    {
        var result = new Result();
        result.WithReasons(@this.Reasons);

        if(@this.IsSuccess())
        {
            var converted = await action(@this.Value);
            result.WithReasons(converted.Reasons);
        }

        return result;
    }
    #endregion


    /// <summary>
    /// Deconstruct Result
    /// </summary>
    public static void Deconstruct<TValue>(this IResult<TValue> @this, out bool isSuccess, out bool isFailed, out TValue value)
    {
        isSuccess = @this.IsSuccess();
        isFailed = @this.IsFailed();
        value = @this.IsSuccess() ? @this.Value : default;
    }

    /// <summary>
    /// Deconstruct Result
    /// </summary>
    public static void Deconstruct<TValue>(this IResult<TValue> @this, out bool isSuccess, out bool isFailed, out TValue value, out IReadOnlyList<IError> errors)
    {
        isSuccess = @this.IsSuccess();
        isFailed = @this.IsFailed();
        value = @this.IsSuccess() ? @this.Value : default;
        errors = @this.IsFailed() ? @this.Errors() : default;
    }
}

