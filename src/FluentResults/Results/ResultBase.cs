using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace
namespace FluentResults;

/// <summary>
/// Default implementation of <see cref="IResultBase"/> generics
/// </summary>
public static partial class ResultBaseExt
{
    /// <summary>
    /// Add a reason (success or successes)
    /// </summary>
    public static TResult WithReason<TResult>(this TResult result, IReason reason)
        where TResult : IResultBase
    {
        result.Reasons.Add(reason);
        return (TResult)result;
    }

    /// <summary>
    /// Add multiple reasons (success or successes)
    /// </summary>
    public static TResult WithReasons<TResult>(this TResult result, IEnumerable<IReason> reasons)
        where TResult : IResultBase
    {
        result.Reasons.AddRange(reasons);
        return (TResult)result;
    }

    /// <summary>
    /// Add an successes
    /// </summary>
    public static TResult WithError<TResult>(this TResult result, string errorMessage)
        where TResult : IResultBase
    {
        return result.WithError(Result.Settings.ErrorFactory(errorMessage));
    }

    /// <summary>
    /// Add an successes
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult WithError<TResult>(this TResult result, IError error)
        where TResult : IResultBase
    {
        return result.WithReason(error);
    }

    /// <summary>
    /// Add multiple errors
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult WithErrors<TResult>(this TResult result, IEnumerable<IError> errors)
        where TResult : IResultBase
    {
        return result.WithReasons(errors);
    }

    /// <summary>
    /// Add multiple errors
    /// </summary>
    public static TResult WithErrors<TResult>(this TResult result, IEnumerable<string> errors)
        where TResult : IResultBase
    {
        return result.WithReasons(errors.Select(errorMessage => Result.Settings.ErrorFactory(errorMessage)));
    }

    /// <summary>
    /// Add an successes
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult WithError<TResult, TError>(this TResult result)
        where TResult : IResultBase
        where TError : IError, new()
    {
        return result.WithError(new TError());
    }

    /// <summary>
    /// Add a success
    /// </summary>
    public static TResult WithSuccess<TResult>(this TResult result, string successMessage)
        where TResult : IResultBase
    {
        return result.WithSuccess(Result.Settings.SuccessFactory(successMessage));
    }

    /// <summary>
    /// Add a success
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult WithSuccess<TResult>(this TResult result, ISuccess success)
        where TResult : IResultBase
    {
        return result.WithReason(success);
    }

    /// <summary>
    /// Add a success
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult WithSuccess<TResult, TSuccess>(this TResult result)
        where TResult : IResultBase
        where TSuccess : Success, new()
    {
        return result.WithSuccess(new TSuccess());
    }

    /// <summary>
    /// Add multiple successes
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult WithSuccesses<TResult>(this TResult result, IEnumerable<ISuccess> successes)
        where TResult : IResultBase
    {
        return result.WithReasons(successes);
    }

    #region Log
    /// <summary>
    /// Log the result. Configure the logger via Result.Setup(..)
    /// </summary>
    public static TResult Log<TResult>(this TResult result, LogLevel logLevel = LogLevel.Information)
        where TResult : IResultBase
    {
        return result.Log(string.Empty, null, logLevel);
    }

    /// <summary>
    /// Log the result. Configure the logger via Result.Setup(..)
    /// </summary>
    public static TResult Log<TResult>(this TResult result, string context, LogLevel logLevel = LogLevel.Information)
        where TResult : IResultBase
    {
        return result.Log(context, null, logLevel);
    }

    /// <summary>
    /// Log the result with a specific logger context. Configure the logger via Result.Setup(..)
    /// </summary>
    public static TResult Log<TResult>(this TResult result, string context, string content, LogLevel logLevel = LogLevel.Information)
        where TResult : IResultBase
    {
        var logger = Result.Settings.Logger;

        logger.Log(context, content, result, logLevel);

        return (TResult)result;
    }

    /// <summary>
    /// Log the result with a typed context. Configure the logger via Result.Setup(..)
    /// </summary>
    public static TResult Log<TResult, TContext>(this TResult result, LogLevel logLevel = LogLevel.Information)
        where TResult : IResultBase
    {
        return result.Log<TResult, TContext>(null, logLevel);
    }

    /// <summary>
    /// Log the result with a typed context. Configure the logger via Result.Setup(..)
    /// </summary>
    public static TResult Log<TResult, TContext>(this TResult result, string content, LogLevel logLevel = LogLevel.Information)
        where TResult : IResultBase
    {
        var logger = Result.Settings.Logger;

        logger.Log<TContext>(content, result, logLevel);

        return (TResult)result;
    }

    /// <summary>
    /// Log the result only when it is successful. Configure the logger via Result.Setup(..)
    /// </summary>
    public static TResult LogIfSuccess<TResult>(this TResult result, LogLevel logLevel = LogLevel.Information)
        where TResult : IResultBase
    {
        if(result.IsSuccess())
            return result.Log(logLevel);

        return (TResult)result;
    }

    /// <summary>
    /// Log the result with a specific logger context only when it is successful. Configure the logger via Result.Setup(..)
    /// </summary>
    public static TResult LogIfSuccess<TResult>(this TResult result, string context, string content = null, LogLevel logLevel = LogLevel.Information)
        where TResult : IResultBase
    {
        if(result.IsSuccess())
            return result.Log(context, content, logLevel);

        return (TResult)result;
    }

    /// <summary>
    /// Log the result with a typed context only when it is successful. Configure the logger via Result.Setup(..)
    /// </summary>
    public static TResult LogIfSuccess<TResult, TContext>(this TResult result, string content = null, LogLevel logLevel = LogLevel.Information)
        where TResult : IResultBase
    {
        if(result.IsSuccess())
            return result.Log<TResult, TContext>(content, logLevel);

        return (TResult)result;
    }

    /// <summary>
    /// Log the result only when it is failed. Configure the logger via Result.Setup(..)
    /// </summary>
    public static TResult LogIfFailed<TResult>(this TResult result, LogLevel logLevel = LogLevel.Error)
        where TResult : IResultBase
    {
        if(result.IsFailed())
            return result.Log(logLevel);

        return (TResult)result;
    }

    /// <summary>
    /// Log the result with a specific logger context only when it is failed. Configure the logger via Result.Setup(..)
    /// </summary>
    public static TResult LogIfFailed<TResult>(this TResult result, string context, string content = null, LogLevel logLevel = LogLevel.Error)
        where TResult : IResultBase
    {
        if(result.IsFailed())
            return result.Log(context, content, logLevel);

        return (TResult)result;
    }

    /// <summary>
    /// Log the result with a typed context only when it is failed. Configure the logger via Result.Setup(..)
    /// </summary>
    public static TResult LogIfFailed<TResult, TContext>(this TResult result, string content = null, LogLevel logLevel = LogLevel.Error)
        where TResult : IResultBase
    {
        if(result.IsFailed())
            return result.Log<TResult, TContext>(content, logLevel);

        return (TResult)result;
    }
    #endregion
}