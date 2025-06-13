using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentResults;

/// <summary>
/// Etensions methods to interact with the Reasons
/// </summary>
public static class ResultReasonHandling
{
    /// <param name="result">Result</param>
    extension(IResultBase result)
    {
        /// <summary>
        /// Check if the result object contains an successes from a specific type
        /// </summary>
        public bool HasError<TError>() where TError : IError
        {
            return result.HasError<TError>(e => true, out _);
        }

        /// <summary>
        /// Check if the result object contains an successes from a specific type
        /// </summary>
        public bool HasError<TError>(out IReadOnlyList<TError> error) where TError : IError
        {
            return result.HasError<TError>(e => true, out error);
        }

        /// <summary>
        /// Check if the result object contains an successes from a specific type and with a specific condition
        /// </summary>
        public bool HasError<TError>(Func<TError, bool> predicate) where TError : IError
        {
            return result.HasError<TError>(predicate, out _);
        }

        /// <summary>
        /// Check if the result object contains an successes from a specific type and with a specific condition
        /// </summary>
        public bool HasError<TError>(Func<TError, bool> predicate, out IReadOnlyList<TError> error) where TError : IError
        {
            if(predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return ResultHelper.HasError(result.Errors.ToList(), predicate, out error);
        }

        /// <summary>
        /// Check if the result object contains an successes with a specific condition
        /// </summary>
        public bool HasError(Func<IError, bool> predicate)
        {
            return result.HasError(predicate, out _);
        }

        /// <summary>
        /// Check if the result object contains an successes with a specific condition
        /// </summary>
        public bool HasError(Func<IError, bool> predicate, out IReadOnlyList<IError> error)
        {
            if(predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return ResultHelper.HasError(result.Errors.ToList(), predicate, out error);
        }

        /// <summary>
        /// Check if the result object contains an exception from a specific type
        /// </summary>
        public bool HasException<TException>() where TException : Exception
        {
            return HasException<TException>(result, out _);
        }

        /// <summary>
        /// Check if the result object contains an exception from a specific type
        /// </summary>
        public bool HasException<TException>(out IEnumerable<IError> error) where TException : Exception
        {
            return HasException<TException>(result, e => true, out error);
        }

        /// <summary>
        /// Check if the result object contains an exception from a specific type and with a specific condition
        /// </summary>
        public bool HasException<TException>(Func<TException, bool> predicate) where TException : Exception
        {
            return HasException(result, predicate, out _);
        }

        /// <summary>
        /// Check if the result object contains an exception from a specific type and with a specific condition
        /// </summary>
        public bool HasException<TException>(Func<TException, bool> predicate, out IEnumerable<IError> error) where TException : Exception
        {
            if(predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return ResultHelper.HasException(result.Errors.ToList(), predicate, out error);
        }

        /// <summary>
        /// Check if the result object contains a success from a specific type
        /// </summary>
        public bool HasSuccess<TSuccess>() where TSuccess : ISuccess
        {
            return HasSuccess<TSuccess>(result, success => true, out _);
        }

        /// <summary>
        /// Check if the result object contains a success from a specific type
        /// </summary>
        public bool HasSuccess<TSuccess>(out IEnumerable<TSuccess> successes) where TSuccess : ISuccess
        {
            return HasSuccess<TSuccess>(result, success => true, out successes);
        }

        /// <summary>
        /// Check if the result object contains a success from a specific type and with a specific condition
        /// </summary>
        public bool HasSuccess<TSuccess>(Func<TSuccess, bool> predicate) where TSuccess : ISuccess
        {
            return HasSuccess(result, predicate, out _);
        }

        /// <summary>
        /// Check if the result object contains a success from a specific type and with a specific condition
        /// </summary>
        public bool HasSuccess<TSuccess>(Func<TSuccess, bool> predicate, out IEnumerable<TSuccess> successes) where TSuccess : ISuccess
        {
            return ResultHelper.HasSuccess(result.Successes.ToList(), predicate, out successes);
        }

        /// <summary>
        /// Check if the result object contains a success with a specific condition
        /// </summary>
        public bool HasSuccess(Func<ISuccess, bool> predicate, out IEnumerable<ISuccess> successes)
        {
            return ResultHelper.HasSuccess(result.Successes.ToList(), predicate, out successes);
        }

        /// <summary>
        /// Check if the result object contains a success with a specific condition
        /// </summary>
        public bool HasSuccess(Func<ISuccess, bool> predicate)
        {
            return ResultHelper.HasSuccess(result.Successes.ToList(), predicate, out _);
        }
    }
}
