using FluentResults.Results.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentResults;

/// <summary>
/// Contains extension methods für IResult and IResult{T} regarding
/// mapping of reasons.
/// </summary>
public static class ResultReasonsMappings
{
    /// <summary>Mappings für IResultBase</summary>
    extension(IResultBase result)
    {

        /// <summary>
        /// Map all errors of the result via errorMapper
        /// </summary>
        /// <param name="errorMapper"></param>
        /// <returns></returns>
        public IResultBase MapErrors(Func<IError, IError> errorMapper)
        {
            if(result.IsSuccess)
                return result;

            return ResultFactory
                .CreateEmptyResult()
                .WithErrors(result.Errors.Select(errorMapper))
                .WithSuccesses(result.Successes);
        }

        /// <summary>
        /// Map all successes of the result via successMapper
        /// </summary>
        public IResultBase MapSuccesses(Func<ISuccess, ISuccess> successMapper)
        {
            return ResultFactory
                .CreateEmptyResult()
                .WithErrors(result.Errors)
                .WithSuccesses(result.Successes.Select(successMapper));
        }
    }

    /// <summary>Mappings für IResult{TValue}</summary>
    extension<TValue>(IResult<TValue> result)
    {

        /// <summary>
        /// Map all errors of the result via errorMapper
        /// </summary>
        public IResult<TValue> MapErrors(Func<IError, IError> errorMapper)
        {
            if(result.IsSuccess)
                return result;

            return ResultFactory
                .CreateEmptyResult(result.ValueOrDefault)
                .WithErrors(result.Errors.Select(errorMapper))
                .WithSuccesses(result.Successes);
        }

        /// <summary>
        /// Map all successes of the result via successMapper
        /// </summary>
        public IResult<TValue> MapSuccesses(Func<ISuccess, ISuccess> successMapper)
        {
            return ResultFactory
                .CreateEmptyResult(result.ValueOrDefault)
                .WithErrors(result.Errors)
                .WithSuccesses(result.Successes.Select(successMapper));
        }
    }
}
