using FluentResults.Extensions;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using FluentResults.Results.Factory;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    internal static class ResultHelper
    {
        public static IResultBase Merge(IEnumerable<IResultBase> results)
        {
            return results.Aggregate(
                ResultFactory.CreateEmptyResult(),
                (r, result) => r.WithReasons(result.Reasons));
        }

        public static IResult<IReadOnlyList<TValue>> MergeWithValue<TValue>(
            IEnumerable<IResult<TValue>> results)
        {
            var merged = ResultHelper.Merge(results);

            IReadOnlyList<TValue> value =
                merged.IsSuccess() ?
                [.. results.Select(x => x.ValueOrDefault)] :
                [];

            return merged.WithValue(value);
        }

        public static bool HasError<TError>(
            List<IError> errors,
            Func<TError, bool> predicate,
            out IReadOnlyList<TError> result)
            where TError : IError
        {
            result = [.. errors.OfType<TError>().Where(predicate)];
            if(result.Any())
            {
                return true;
            }

            // Recursive call:
            foreach(var error in errors)
            {
                if(HasError(error.Reasons ?? new List<IError>(), predicate, out result))
                {
                    return true;
                }
            }

            // Return empty list and false
            result = [];
            return false;
        }

        public static bool HasException<TException>(
            List<IError> errors,
            Func<TException, bool> predicate,
            out IEnumerable<IError> result)
            where TException : Exception
        {
            var foundErrors = errors.OfType<ExceptionalError>()
                .Where(e => e.Exception is TException rootExceptionOfTException
                            && predicate(rootExceptionOfTException))
                .ToList();

            if (foundErrors.Any())
            {
                result = foundErrors;
                return true;
            }

            foreach (var error in errors)
                if (HasException(error.Reasons ?? new List<IError>(), predicate, out var fErrors))
                {
                    result = fErrors;
                    return true;
                }

            result = Array.Empty<IError>();
            return false;
        }

        public static bool HasSuccess<TSuccess>(
            List<ISuccess> successes,
            Func<TSuccess, bool> predicate,
            out IEnumerable<TSuccess> result) where TSuccess : ISuccess
        {
            var foundSuccesses = successes.OfType<TSuccess>()
                .Where(predicate)
                .ToList();
            if (foundSuccesses.Any())
            {
                result = foundSuccesses;
                return true;
            }

            result = Array.Empty<TSuccess>();
            return false;
        }
    }
}