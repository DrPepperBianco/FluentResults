using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FluentResults;

/// <summary>
/// Extensions methods of IResult to edit reasons
/// </summary>
public static class ResultReasonEditing
{
#if false
    /// <param name="result">Result with or without value</param>
    extension<TResult>(TResult result) where TResult : IResult
    {
        /// <summary>
        /// Add a reason (success or successes)
        /// </summary>
        public TResult WithReason(IReason reason)
        {
            result.Reasons.Add(reason);
            return (TResult)result;
        }

        /// <summary>
        /// Add multiple reasons (success or successes)
        /// </summary>
        public TResult WithReasons(IEnumerable<IReason> reasons)
        {
            result.Reasons.AddRange(reasons);
            return (TResult)result;
        }

        /// <summary>
        /// Add an successes
        /// </summary>
        public TResult WithError(string errorMessage)
        {
            return result.WithError(Result.Settings.ErrorFactory(errorMessage));
        }

        /// <summary>
        /// Add an successes
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TResult WithError(IError error)
        {
            return result.WithReason(error);
        }

        /// <summary>
        /// Add multiple errors
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TResult WithErrors(IEnumerable<IError> errors)
        {
            return result.WithReasons(errors);
        }

        /// <summary>
        /// Add multiple errors
        /// </summary>
        public TResult WithErrors(IEnumerable<string> errors)
        {
            return result.WithReasons(errors.Select(errorMessage => Result.Settings.ErrorFactory(errorMessage)));
        }


        /// <summary>
        /// Add a success
        /// </summary>
        public TResult WithSuccess(string successMessage)
        {
            return result.WithSuccess(Result.Settings.SuccessFactory(successMessage));
        }

        /// <summary>
        /// Add a success
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TResult WithSuccess(ISuccess success)
        {
            return result.WithReason(success);
        }

        
        /// <summary>
        /// Add multiple successes
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TResult WithSuccesses(IEnumerable<ISuccess> successes)
        {
            return result.WithReasons(successes);
        }
    }
#endif

    /// <summary>
    /// Add a reason (success or successes)
    /// </summary>
    public static TResult WithReason<TResult>(this TResult result, IReason reason)        where TResult : IResult
    {
        result.Reasons.Add(reason);
        return (TResult)result;
    }

    /// <summary>
    /// Add multiple reasons (success or successes)
    /// </summary>
    public static TResult WithReasons<TResult>(this TResult result, IEnumerable<IReason> reasons)        where TResult : IResult
    {
        result.Reasons.AddRange(reasons);
        return (TResult)result;
    }

    /// <summary>
    /// Add an successes
    /// </summary>
    public static TResult WithError<TResult>(this TResult result, string errorMessage)        where TResult : IResult
    {
        return result.WithError(Result.Settings.ErrorFactory(errorMessage));
    }

    /// <summary>
    /// Add an successes
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult WithError<TResult>(this TResult result, IError error)        where TResult : IResult
    {
        return result.WithReason(error);
    }

    /// <summary>
    /// Add multiple errors
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult WithErrors<TResult>(this TResult result, IEnumerable<IError> errors)        where TResult : IResult
    {
        return result.WithReasons(errors);
    }

    /// <summary>
    /// Add multiple errors
    /// </summary>
    public static TResult WithErrors<TResult>(this TResult result, IEnumerable<string> errors)        where TResult : IResult
    {
        return result.WithReasons(errors.Select(errorMessage => Result.Settings.ErrorFactory(errorMessage)));
    }


    /// <summary>
    /// Add a success
    /// </summary>
    public static TResult WithSuccess<TResult>(this TResult result, string successMessage)        where TResult : IResult
    {
        return result.WithSuccess(Result.Settings.SuccessFactory(successMessage));
    }

    /// <summary>
    /// Add a success
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult WithSuccess<TResult>(this TResult result, ISuccess success)        where TResult : IResult
    {
        return result.WithReason(success);
    }


    /// <summary>
    /// Add multiple successes
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult WithSuccesses<TResult>(this TResult result, IEnumerable<ISuccess> successes)        where TResult : IResult
    {
        return result.WithReasons(successes);
    }

}
