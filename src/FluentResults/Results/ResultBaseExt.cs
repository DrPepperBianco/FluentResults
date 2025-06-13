using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace FluentResults;

/// <summary>
/// Default implementation of <see cref="IResultBase"/>
/// </summary>
public static partial class ResultBaseExt_MappingAndBind
{
    /// <summary>
    /// Check if the result object contains an successes from a specific type
    /// </summary>
    public static bool HasError<TError>(this IResultBase result) where TError : IError
    {
        return result.HasError<TError>(e => true, out _);
    }

    /// <summary>
    /// Check if the result object contains an successes from a specific type
    /// </summary>
    public static bool HasError<TError>(this IResultBase result, out IReadOnlyList<TError> error) where TError : IError
    {
        return result.HasError<TError>(e => true, out error);
    }

    /// <summary>
    /// Check if the result object contains an successes from a specific type and with a specific condition
    /// </summary>
    public static bool HasError<TError>(this IResultBase result, Func<TError, bool> predicate) where TError : IError
    {
        return result.HasError<TError>(predicate, out _);
    }

    /// <summary>
    /// Check if the result object contains an successes from a specific type and with a specific condition
    /// </summary>
    public static bool HasError<TError>(this IResultBase result, Func<TError, bool> predicate, out IReadOnlyList<TError> error) where TError : IError
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        return ResultHelper.HasError(result.GetErrors().ToList(), predicate, out error);
    }

    /// <summary>
    /// Check if the result object contains an successes with a specific condition
    /// </summary>
    public static bool HasError(this IResultBase result, Func<IError, bool> predicate)
    {
        return result.HasError(predicate, out _);
    }

    /// <summary>
    /// Check if the result object contains an successes with a specific condition
    /// </summary>
    public static bool HasError(this IResultBase result, Func<IError, bool> predicate, out IReadOnlyList<IError> error)
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        return ResultHelper.HasError(result.GetErrors().ToList(), predicate, out error);
    }

    /// <summary>
    /// Check if the result object contains an exception from a specific type
    /// </summary>
    public static bool HasException<TException>(this IResultBase result) where TException : Exception
    {
        return HasException<TException>(result, out _);
    }

    /// <summary>
    /// Check if the result object contains an exception from a specific type
    /// </summary>
    public static bool HasException<TException>(this IResultBase result, out IEnumerable<IError> error) where TException : Exception
    {
        return HasException<TException>(result, e => true, out error);
    }

    /// <summary>
    /// Check if the result object contains an exception from a specific type and with a specific condition
    /// </summary>
    public static bool HasException<TException>(this IResultBase result, Func<TException, bool> predicate) where TException : Exception
    {
        return HasException(result, predicate, out _);
    }

    /// <summary>
    /// Check if the result object contains an exception from a specific type and with a specific condition
    /// </summary>
    public static bool HasException<TException>(this IResultBase result, Func<TException, bool> predicate, out IEnumerable<IError> error) where TException : Exception
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        return ResultHelper.HasException(result.GetErrors().ToList(), predicate, out error);
    }

    

    /// <summary>
    /// Check if the result object contains a success from a specific type
    /// </summary>
    public static bool HasSuccess<TSuccess>(this IResultBase result) where TSuccess : ISuccess
    {
        return HasSuccess<TSuccess>(result, success => true, out _);
    }

    /// <summary>
    /// Check if the result object contains a success from a specific type
    /// </summary>
    public static bool HasSuccess<TSuccess>(this IResultBase result, out IEnumerable<TSuccess> successes) where TSuccess : ISuccess
    {
        return HasSuccess<TSuccess>(result, success => true, out successes);
    }

    /// <summary>
    /// Check if the result object contains a success from a specific type and with a specific condition
    /// </summary>
    public static bool HasSuccess<TSuccess>(this IResultBase result, Func<TSuccess, bool> predicate) where TSuccess : ISuccess
    {
        return HasSuccess(result, predicate, out _);
    }

    /// <summary>
    /// Check if the result object contains a success from a specific type and with a specific condition
    /// </summary>
    public static bool HasSuccess<TSuccess>(this IResultBase result, Func<TSuccess, bool> predicate, out IEnumerable<TSuccess> successes) where TSuccess : ISuccess
    {
        return ResultHelper.HasSuccess(result.GetSuccesses().ToList(), predicate, out successes);
    }

    /// <summary>
    /// Check if the result object contains a success with a specific condition
    /// </summary>
    public static bool HasSuccess(this IResultBase result, Func<ISuccess, bool> predicate, out IEnumerable<ISuccess> successes)
    {
        return ResultHelper.HasSuccess(result.GetSuccesses().ToList(), predicate, out successes);
    }

    /// <summary>
    /// Check if the result object contains a success with a specific condition
    /// </summary>
    public static bool HasSuccess(this IResultBase result, Func<ISuccess, bool> predicate)
    {
        return ResultHelper.HasSuccess(result.GetSuccesses().ToList(), predicate, out _);
    }

    /// <summary>
    /// Deconstruct Result 
    /// </summary>
    /// <param name="isSuccess"></param>
    /// <param name="isFailed"></param>
    public static void Deconstruct(this IResultBase result, out bool isSuccess, out bool isFailed)
    {
        isSuccess = result.IsSuccess();
        isFailed = result.IsFailed();
    }

    /// <summary>
    /// Deconstruct Result
    /// </summary>
    public static void Deconstruct(this IResultBase result, out bool isSuccess, out bool isFailed, out IReadOnlyList<IError> errors)
    {
        isSuccess = result.IsSuccess();
        isFailed = result.IsFailed();
        errors = isFailed ? [.. result.GetErrors()] : [];
    }
}
