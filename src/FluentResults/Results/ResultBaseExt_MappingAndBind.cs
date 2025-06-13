using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// Implementation of a Result
    /// </summary>
    partial class ResultBaseExt_MappingAndBind
    {
        /// <summary>
        /// Map all errors of the result via errorMapper
        /// </summary>
        /// <param name="errorMapper"></param>
        /// <returns></returns>
        public static IResultBase MapErrors(this IResultBase result, Func<IError, IError> errorMapper)
        {
            if (result.IsSuccess())
                return result;

            return ResultFactory
                .CreateEmptyResult()
                .WithErrors(result.GetErrors().Select(errorMapper))
                .WithSuccesses(result.GetSuccesses());
        }

        /// <summary>
        /// Map all successes of the result via successMapper
        /// </summary>
        public static IResultBase MapSuccesses(this IResultBase result, Func<ISuccess, ISuccess> successMapper)
        {
            return ResultFactory
                .CreateEmptyResult()
                .WithErrors(result.GetErrors())
                .WithSuccesses(result.GetSuccesses().Select(successMapper));
        }

        /// <summary>
        /// Convert result without value to a result containing a value
        /// </summary>
        /// <typeparam name="TNewValue">Type of the value</typeparam>
        /// <param name="newValue">Value to add to the new result</param>
        public static IResult<TNewValue> ToResult<TNewValue>(this IResultBase result, TNewValue newValue = default)
        {
            return ResultFactory
                .CreateEmptyResult(newValue)
                .WithReasons(result.Reasons);
        }

        /// <summary>
        /// Convert result to result with value that may fail.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="result" />
        /// <param name="bind">Transformation that may fail.</param>
        public static IResult<TNewValue> Bind<TNewValue>(this IResultBase result, Func<IResult<TNewValue>> bind)
        {
            if(result.IsSuccess())
            {
                var converted = bind();
                return ResultFactory
                    .CreateEmptyResult(converted.ValueOrDefault)
                    .WithReasons(result.Reasons)
                    .WithReasons(converted.Reasons);
            }
            else
            {
                return ResultFactory
                    .CreateEmptyResult<TNewValue>(default!)
                    .WithReasons(result.Reasons);
            }
        }
        
        /// <summary>
        /// Convert result to result with value that may fail asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="result"/>
        /// <param name="bind">Transformation that may fail.</param>
        public static async Task<IResult<TNewValue>> Bind<TNewValue>(this IResultBase result, Func<Task<IResult<TNewValue>>> bind)
        {
            if(result.IsSuccess())
            {
                var converted = await bind();
                return ResultFactory
                    .CreateEmptyResult(converted.ValueOrDefault)
                    .WithReasons(result.Reasons)
                    .WithReasons(converted.Reasons);
            }
            else
            {
                return ResultFactory
                    .CreateEmptyResult<TNewValue>(default!)
                    .WithReasons(result.Reasons);
            }
        }
        
        /// <summary>
        /// Convert result to result with value that may fail asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="result"/>
        /// <param name="bind">Transformation that may fail.</param>
        public static async ValueTask<IResult<TNewValue>> Bind<TNewValue>(this IResultBase result, Func<ValueTask<IResult<TNewValue>>> bind)
        {
            if(result.IsSuccess())
            {
                var converted = await bind();
                return ResultFactory
                    .CreateEmptyResult(converted.ValueOrDefault)
                    .WithReasons(result.Reasons)
                    .WithReasons(converted.Reasons);
            }
            else
            {
                return ResultFactory
                    .CreateEmptyResult<TNewValue>(default!)
                    .WithReasons(result.Reasons);
            }
        }
        
        /// <summary>
        /// Execute an action which returns a <see cref="Result"/>.
        /// </summary>
        /// <example>
        /// <code>
        ///  var done = result.Bind(ActionWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="result"/>
        /// <param name="action">Action that may fail.</param>
        public static IResultBase Bind(this IResultBase result, Func<IResultBase> action)
        {
            if(result.IsSuccess())
            {
                var converted = action();
                return ResultFactory
                    .CreateEmptyResult()
                    .WithReasons(result.Reasons)
                    .WithReasons(converted.Reasons);
            }
            else
            {
                return ResultFactory
                    .CreateEmptyResult()
                    .WithReasons(result.Reasons);
            }
        }
        
        /// <summary>
        /// Execute an action which returns a <see cref="Result"/> asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var done = result.Bind(ActionWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="result"/>
        /// <param name="action">Action that may fail.</param>
        public static async Task<IResultBase> Bind(this IResultBase result, Func<Task<IResultBase>> action)
        {
            if(result.IsSuccess())
            {
                var converted = await action();
                return ResultFactory
                    .CreateEmptyResult()
                    .WithReasons(result.Reasons)
                    .WithReasons(converted.Reasons);
            }
            else
            {
                return ResultFactory
                    .CreateEmptyResult()
                    .WithReasons(result.Reasons);
            }
        }
        
        /// <summary>
        /// Execute an action which returns a <see cref="Result"/> asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var done = result.Bind(ActionWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="action">Action that may fail.</param>
        public static async ValueTask<IResultBase> Bind(this IResultBase result, Func<ValueTask<IResultBase>> action)
        {
            if(result.IsSuccess())
            {
                var converted = await action();
                return ResultFactory
                    .CreateEmptyResult()
                    .WithReasons(result.Reasons)
                    .WithReasons(converted.Reasons);
            }
            else
            {
                return ResultFactory
                    .CreateEmptyResult()
                    .WithReasons(result.Reasons);
            }
        }


        /// <summary>
        /// Explicit conversion from <see cref="IError"/> to a <see cref="IResultBase"/>
        /// </summary>
        /// <param name="error">The error</param>
        public static IResultBase ToResult(this IError error)
        {
            // return Fail(error);
            return ResultFactory
                .CreateEmptyResult()
                .WithError(error);
        }

        /// <summary>
        /// Explicit conversion from <see cref="IReadOnlyList{IError}"/> to a <see cref="IResultBase"/>
        /// </summary>
        /// <param name="errors">The errors</param>
        public static IResultBase ToResult(this IReadOnlyList<IError> errors)
        {
            return ResultFactory
                .CreateEmptyResult()
                .WithErrors(errors);
        }
    }
}