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
    /// <param name="result">Result with or without value</param>
    extension<TResult>(TResult result) where TResult : IResultBase
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
}
