using FluentResults;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FluentResults;

/// <summary>
/// Contains Extensions methods for Task{IResultBase}, ValueTask{IResultBase},
/// Task{IResult{TValue}} and ValueTask{IResult{TValue}} that corresponds to
/// the other existing extensions methods in <see cref="ResultMappingAndBinding"/>,
/// <see cref="ResultReasonsMappings"/> and <see cref="ResultConversions"/> etc.
/// </summary>
public static class TaskExtensions
{
    #region Extensions for Task<IResultBase>, ValueTask<IResultBase>, Task<IResult<T>>, and ValueTask<IResult<T>>

    /// <summary>
    /// Map all errors of the result via errorMapper
    /// </summary>
    /// <param name="resultTask">The current result</param>
    /// <param name="errorMapper">Function to transform the errors</param>
    public static async Task<IResult> MapErrors(this Task<IResult> resultTask, Func<IError, IError> errorMapper)
    {
        var result = await resultTask;
        return result.MapErrors(errorMapper);
    }

    /// <summary>
    /// Map all errors of the result via errorMapper
    /// </summary>
    /// <param name="resultTask">The current result</param>
    /// <param name="errorMapper">Function to transform the errors</param>
    public static async ValueTask<IResult> MapErrors(this ValueTask<IResult> resultTask, Func<IError, IError> errorMapper)
    {
        var result = await resultTask;
        return result.MapErrors(errorMapper);
    }

    /// <summary>
    /// Map all errors of the result via errorMapper
    /// </summary>
    /// <param name="resultTask">The current result</param>
    /// <param name="errorMapper">Function to transform the errors</param>
    public static async Task<IResult<T>> MapErrors<T>(this Task<IResult<T>> resultTask, Func<IError, IError> errorMapper)
    {
        var result = await resultTask;
        return result.MapErrors(errorMapper);
    }

    /// <summary>
    /// Map all errors of the result via errorMapper
    /// </summary>
    /// <param name="resultTask">The current result</param>
    /// <param name="errorMapper">Function to transform the errors</param>
    public static async ValueTask<IResult<T>> MapErrors<T>(this ValueTask<IResult<T>> resultTask, Func<IError, IError> errorMapper)
    {
        var result = await resultTask;
        return result.MapErrors(errorMapper);
    }

    /// <summary>
    /// Map all successes of the result via successMapper
    /// </summary>
    /// <param name="resultTask">The current result</param>
    /// <param name="errorMapper">Function to transform the successes</param>
    public static async Task<IResult> MapSuccesses(this Task<IResult> resultTask, Func<ISuccess, ISuccess> errorMapper)
    {
        var result = await resultTask;
        return result.MapSuccesses(errorMapper);
    }

    /// <summary>
    /// Map all successes of the result via successMapper
    /// </summary>
    /// <param name="resultTask">The current result</param>
    /// <param name="errorMapper">Function to transform the successes</param>
    public static async ValueTask<IResult> MapSuccesses(this ValueTask<IResult> resultTask, Func<ISuccess, ISuccess> errorMapper)
    {
        var result = await resultTask;
        return result.MapSuccesses(errorMapper);
    }

    /// <summary>
    /// Map all successes of the result via successMapper
    /// <param name="resultTask">The current result</param>
    /// <param name="errorMapper">Function to transform the successes</param>
    /// </summary>
    public static async Task<IResult<T>> MapSuccesses<T>(this Task<IResult<T>> resultTask, Func<ISuccess, ISuccess> errorMapper)
    {
        var result = await resultTask;
        return result.MapSuccesses(errorMapper);
    }

    /// <summary>
    /// Map all successes of the result via successMapper
    /// </summary>
    /// <param name="resultTask">The current result</param>
    /// <param name="errorMapper">Function to transform the successes</param>
    public static async ValueTask<IResult<T>> MapSuccesses<T>(this ValueTask<IResult<T>> resultTask, Func<ISuccess, ISuccess> errorMapper)
    {
        var result = await resultTask;
        return result.MapSuccesses(errorMapper);
    }

    /// <summary>
    /// Convert result with value to result with another value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="resultTask">The current result</param>
    /// <param name="bind">Transformation that may fail.</param>
    public static async Task<IResult<TNew>> Bind<TOld, TNew>(this Task<IResult<TOld>> resultTask, Func<TOld, Task<IResult<TNew>>> bind)
    {
        var result = await resultTask;
        return await result.Bind(bind);
    }

    /// <summary>
    /// Convert result with value to result with another value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="resultTask">The current result</param>
    /// <param name="bind">Transformation that may fail.</param>
    public static async ValueTask<IResult<TNew>> Bind<TOld, TNew>(this ValueTask<IResult<TOld>> resultTask, Func<TOld, ValueTask<IResult<TNew>>> bind)
    {
        var result = await resultTask;
        return await result.Bind(bind);
    }

    /// <summary>
    /// Convert result with value to result with another value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="resultTask">The current result</param>
    /// <param name="bind">Transformation that may fail.</param>
    public static async Task<IResult<TNew>> Bind<TOld, TNew>(this Task<IResult<TOld>> resultTask, Func<TOld, IResult<TNew>> bind)
    {
        var result = await resultTask;
        return result.Bind(bind);
    }
    /// <summary>
    /// Convert result with value to result with another value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="resultTask">The current result</param>
    /// <param name="bind">Transformation that may fail.</param>

    public static async ValueTask<IResult<TNew>> Bind<TOld, TNew>(this ValueTask<IResult<TOld>> resultTask, Func<TOld, IResult<TNew>> bind)
    {
        var result = await resultTask;
        return result.Bind(bind);
    }

    /// <summary>
    /// Convert result with value to result with another value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="resultTask">The current result</param>
    /// <param name="bind">Transformation that may fail.</param>
    public static async Task<IResult> Bind<TOld>(this Task<IResult<TOld>> resultTask, Func<TOld, Task<IResult>> bind)
    {
        var result = await resultTask;
        return await result.Bind(bind);
    }

    /// <summary>
    /// Convert result with value to result with another value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="resultTask">The current result</param>
    /// <param name="bind">Transformation that may fail.</param>
    public static async Task<IResult> Bind<TOld>(this Task<IResult<TOld>> resultTask, Func<TOld, IResult> bind)
    {
        var result = await resultTask;
        return result.Bind(bind);
    }

    /// <summary>
    /// Convert result with value to result with another value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="resultTask">The current result</param>
    /// <param name="bind">Transformation that may fail.</param>
    public static async ValueTask<IResult> Bind<TOld>(this ValueTask<IResult<TOld>> resultTask, Func<TOld, IResult> bind)
    {
        var result = await resultTask;
        return result.Bind(bind);
    }

    /// <summary>
    /// Convert result with value to result with another value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="resultTask">The current result</param>
    /// <param name="bind">Transformation that may fail.</param>
    public static async ValueTask<IResult> Bind<TOld>(this ValueTask<IResult<TOld>> resultTask, Func<TOld, ValueTask<IResult>> bind)
    {
        var result = await resultTask;
        return await result.Bind(bind);
    }

    /// <summary>
    /// Convert result with value to result with another value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="resultTask">The current result</param>
    /// <param name="bind">Transformation that may fail.</param>
    public static async Task<IResult<TNew>> Bind<TNew>(this Task<IResult> resultTask, Func<Task<IResult<TNew>>> bind)
    {
        var result = await resultTask;
        return await result.Bind(bind);
    }

    /// <summary>
    /// Convert result with value to result with another value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="resultTask">The current result</param>
    /// <param name="bind">Transformation that may fail.</param>
    public static async ValueTask<IResult<TNew>> Bind<TNew>(this ValueTask<IResult> resultTask, Func<ValueTask<IResult<TNew>>> bind)
    {
        var result = await resultTask;
        return await result.Bind(bind);
    }

    /// <summary>
    /// Convert result with value to result with another value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="resultTask">The current result</param>
    /// <param name="bind">Transformation that may fail.</param>
    public static async Task<IResult> Bind(this Task<IResult> resultTask, Func<Task<IResult>> bind)
    {
        var result = await resultTask;
        return await result.Bind(bind);
    }

    /// <summary>
    /// Convert result with value to result with another value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="resultTask">The current result</param>
    /// <param name="bind">Transformation that may fail.</param>
    public static async ValueTask<IResult> Bind(this ValueTask<IResult> resultTask, Func<ValueTask<IResult>> bind)
    {
        var result = await resultTask;
        return await result.Bind(bind);
    }

    /// <summary>
    /// Convert result with value to result with another value. Use valueConverter parameter to specify the value transformation logic.
    /// </summary>
    public static async Task<IResult<TNewValue>> Map<TOldValue, TNewValue>(this Task<IResult<TOldValue>> resultTask, Func<TOldValue, TNewValue> valueConverter)
    {
        var result = await resultTask;
        return result.Map(valueConverter);
    }

    /// <summary>
    /// Convert result with value to result with another value. Use valueConverter parameter to specify the value transformation logic.
    /// </summary>
    public static async Task<IResult<TNewValue>> Map<TOldValue, TNewValue>(this ValueTask<IResult<TOldValue>> resultTask, Func<TOldValue, TNewValue> valueConverter)
    {
        var result = await resultTask;
        return result.Map(valueConverter);
    }

    /// <summary>
    /// Convert result without value to a result containing a value
    /// </summary>
    /// <typeparam name="TValue">Type of the value</typeparam>
    /// <param name="resultTask">The current result</param>
    /// <param name="value">Value to add to the new result</param>
    public static async Task<IResult<TValue>> ToResult<TValue>(this Task<IResult> resultTask, TValue value)
    {
        var result = await resultTask;
        return result.ToResult(value);
    }

    /// <summary>
    /// Convert result without value to a result containing a value
    /// </summary>
    /// <typeparam name="TValue">Type of the value</typeparam>
    /// <param name="resultTask">The current result</param>
    /// <param name="value">Value to add to the new result</param>
    public static async Task<IResult<TValue>> ToResult<TValue>(this ValueTask<IResult> resultTask, TValue value)
    {
        var result = await resultTask;
        return result.ToResult(value);
    }

    #endregion

}
