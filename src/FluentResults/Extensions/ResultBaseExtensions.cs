using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentResults;

public static class ResultBaseExtensions
{
    #region IResultBase - Basic Interface

    /// <summary>
    /// Is true if Reasons contains at least one error
    /// </summary>
    public static bool IsFailed(this IResultBase result) =>
        result.EnumerateErrors().Any();

    /// <summary>
    /// Is true if Reasons contains no errors
    /// </summary>
    public static bool IsSuccess(this IResultBase result) =>
        !result.IsFailed();

    /// <inheritdoc cref="Errors"/>
    public static IEnumerable<IError> EnumerateErrors(this IResultBase result) =>
        result.Reasons.OfType<IError>();

    /// <summary>
    /// Get all errors
    /// </summary>
    public static IReadOnlyList<IError> Errors(this IResultBase result) =>
        result.EnumerateErrors().ToArray();

    /// <inheritdoc cref="Successes"/>
    public static IEnumerable<ISuccess> EnumerateSuccesses(this IResultBase result) =>
        result.Reasons.OfType<ISuccess>();

    /// <summary>
    /// Get all successes
    /// </summary>
    public static IReadOnlyList<ISuccess> Successes(this IResultBase result) =>
        result.EnumerateSuccesses().ToArray();

    #endregion


    #region IResultBase - Advanced Interface


    /// <summary>
    /// Check if the result object contains an error from a specific type
    /// </summary>
    public static bool HasError<TError>(this IResultBase resultBase) where TError : IError
    {
        return HasError<TError>(resultBase, out _);
    }

    /// <summary>
    /// Check if the result object contains an error from a specific type
    /// </summary>
    public static bool HasError<TError>(this IResultBase resultBase, out IEnumerable<TError> result) where TError : IError
    {
        return HasError<TError>(resultBase, e => true, out result);
    }

    /// <summary>
    /// Check if the result object contains an error from a specific type and with a specific condition
    /// </summary>
    public static bool HasError<TError>(this IResultBase resultBase, Func<TError, bool> predicate) where TError : IError
    {
        return HasError<TError>(resultBase, predicate, out _);
    }

    /// <summary>
    /// Check if the result object contains an error from a specific type and with a specific condition
    /// </summary>
    public static bool HasError<TError>(this IResultBase resultBase, Func<TError, bool> predicate, out IEnumerable<TError> result) where TError : IError
    {
        if(predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        return ResultHelper.HasError(resultBase.Errors(), predicate, out result);
    }

    /// <summary>
    /// Check if the result object contains an error with a specific condition
    /// </summary>
    public static bool HasError(this IResultBase resultBase, Func<IError, bool> predicate)
    {
        return HasError(resultBase, predicate, out _);
    }

    /// <summary>
    /// Check if the result object contains an error with a specific condition
    /// </summary>
    public static bool HasError(this IResultBase resultBase, Func<IError, bool> predicate, out IEnumerable<IError> result)
    {
        if(predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        return ResultHelper.HasError(resultBase.Errors(), predicate, out result);
    }

    /// <summary>
    /// Check if the result object contains an exception from a specific type
    /// </summary>
    public static bool HasException<TException>(this IResultBase resultBase) where TException : Exception
    {
        return HasException<TException>(resultBase, out _);
    }

    /// <summary>
    /// Check if the result object contains an exception from a specific type
    /// </summary>
    public static bool HasException<TException>(this IResultBase resultBase, out IEnumerable<IError> result) where TException : Exception
    {
        return HasException<TException>(resultBase, error => true, out result);
    }

    /// <summary>
    /// Check if the result object contains an exception from a specific type and with a specific condition
    /// </summary>
    public static bool HasException<TException>(this IResultBase resultBase, Func<TException, bool> predicate) where TException : Exception
    {
        return HasException(resultBase, predicate, out _);
    }

    /// <summary>
    /// Check if the result object contains an exception from a specific type and with a specific condition
    /// </summary>
    public static bool HasException<TException>(this IResultBase resultBase, Func<TException, bool> predicate, out IEnumerable<IError> result) where TException : Exception
    {
        if(predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        return ResultHelper.HasException(resultBase.Errors(), predicate, out result);
    }



    /// <summary>
    /// Check if the result object contains a success from a specific type
    /// </summary>
    public static bool HasSuccess<TSuccess>(this IResultBase resultBase) where TSuccess : ISuccess
    {
        return HasSuccess<TSuccess>(resultBase, success => true, out _);
    }

    /// <summary>
    /// Check if the result object contains a success from a specific type
    /// </summary>
    public static bool HasSuccess<TSuccess>(this IResultBase resultBase, out IEnumerable<TSuccess> result) where TSuccess : ISuccess
    {
        return HasSuccess<TSuccess>(resultBase, success => true, out result);
    }

    /// <summary>
    /// Check if the result object contains a success from a specific type and with a specific condition
    /// </summary>
    public static bool HasSuccess<TSuccess>(this IResultBase resultBase, Func<TSuccess, bool> predicate) where TSuccess : ISuccess
    {
        return HasSuccess(resultBase, predicate, out _);
    }

    /// <summary>
    /// Check if the result object contains a success from a specific type and with a specific condition
    /// </summary>
    public static bool HasSuccess<TSuccess>(this IResultBase resultBase, Func<TSuccess, bool> predicate, out IEnumerable<TSuccess> result) where TSuccess : ISuccess
    {
        return ResultHelper.HasSuccess(resultBase.Successes(), predicate, out result);
    }

    /// <summary>
    /// Check if the result object contains a success with a specific condition
    /// </summary>
    public static bool HasSuccess(this IResultBase resultBase, Func<ISuccess, bool> predicate, out IEnumerable<ISuccess> result)
    {
        return ResultHelper.HasSuccess(resultBase.Successes(), predicate, out result);
    }

    /// <summary>
    /// Check if the result object contains a success with a specific condition
    /// </summary>
    public static bool HasSuccess(this IResultBase resultBase, Func<ISuccess, bool> predicate)
    {
        return ResultHelper.HasSuccess(resultBase.Successes(), predicate, out _);
    }

    /// <summary>
    /// Deconstruct Result 
    /// </summary>
    public static void Deconstruct(this IResultBase resultBase, out bool isSuccess, out bool isFailed)
    {
        isSuccess = resultBase.IsSuccess();
        isFailed = !isSuccess;
    }

    /// <summary>
    /// Deconstruct Result
    /// </summary>
    public static void Deconstruct(this IResultBase resultBase, out bool isSuccess, out bool isFailed, out IReadOnlyList<IError> errors)
    {
        isSuccess = resultBase.IsSuccess();
        isFailed = !isSuccess;
        errors = isFailed ? resultBase.Errors() : Array.Empty<IError>();
    }

    #endregion


    #region IResultBase - Fluent Syntax

    /// <summary>
    /// Add a reason (success or error)
    /// </summary>
    public static TResult WithReason<TResult>(this TResult result, IReason reason)
        where TResult : IResultBase
    {
        result.Reasons.Add(reason);
        return result;
    }

    /// <summary>
    /// Add multiple reasons (success or error)
    /// </summary>
    public static TResult WithReasons<TResult>(this TResult result, IEnumerable<IReason> reasons)
        where TResult : IResultBase
    {
        result.Reasons.AddRange(reasons);
        return result;
    }

    /// <summary>
    /// Add an error
    /// </summary>
    public static TResult WithError<TResult>(this TResult result, string errorMessage)
        where TResult : IResultBase
    {
        return result.WithError(Result.Settings.ErrorFactory(errorMessage));
    }

    /// <summary>
    /// Add an error
    /// </summary>
    public static TResult WithError<TResult>(this TResult result, IError error)
        where TResult : IResultBase
    {
        return result.WithReason(error);
    }

    /// <summary>
    /// Add multiple errors
    /// </summary>
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
    public static TResult WithSuccess<TResult>(this TResult result, ISuccess success)
        where TResult : IResultBase
    {
        return result.WithReason(success);
    }

    public static TResult WithSuccesses<TResult>(this TResult result, IEnumerable<ISuccess> successes)
        where TResult : IResultBase
    {
        foreach(var success in successes)
        {
            result.WithSuccess(success);
        }

        return result;
    }


    #endregion

}
