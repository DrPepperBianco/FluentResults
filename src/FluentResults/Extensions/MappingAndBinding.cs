using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FluentResults;

using Factories;

/// <summary>
/// Extension methods for Mapping and Binding results
/// </summary>
public static class ResultMappingAndBinding
{
#if false
    /// <remarks/>
    extension(IResult result)
    {
        /// <summary>
        /// Convert result to result with value that may fail.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="bind">Transformation that may fail.</param>
        public IResult<TNewValue> Bind<TNewValue>(Func<IResult<TNewValue>> bind)
        {
            if(result.IsSuccess)
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
        /// <param name="bind">Transformation that may fail.</param>
        public async Task<IResult<TNewValue>> Bind<TNewValue>(Func<Task<IResult<TNewValue>>> bind)
        {
            if(result.IsSuccess)
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
        /// <param name="bind">Transformation that may fail.</param>
        public async ValueTask<IResult<TNewValue>> Bind<TNewValue>(Func<ValueTask<IResult<TNewValue>>> bind)
        {
            if(result.IsSuccess)
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
        /// <param name="action">Action that may fail.</param>
        public IResult Bind(Func<IResult> action)
        {
            if(result.IsSuccess)
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
        /// <param name="action">Action that may fail.</param>
        public  async Task<IResult> Bind(Func<Task<IResult>> action)
        {
            if(result.IsSuccess)
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
        public async ValueTask<IResult> Bind(Func<ValueTask<IResult>> action)
        {
            if(result.IsSuccess)
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
    }

    /// <param name="result">The result</param>
    extension<TValue>(IResult<TValue> result)
    {
        /// <summary>
        /// Convert result with value to result with another value. Use valueConverter parameter to specify the value transformation logic.
        /// </summary>
        public IResult<TNewValue> Map<TNewValue>(Func<TValue, TNewValue> mapLogic)
        {
            if(result.IsSuccess)
            {
                if(mapLogic is null)
                    throw new ArgumentException("If result is success then valueConverter should not be null");

                return ResultFactory
                    .CreateEmptyResult(mapLogic(result.ValueOrDefault))
                    .WithReasons(result.Reasons);
            }
            else
            {
                return ResultFactory
                    .CreateEmptyResult<TNewValue>(default!)
                    .WithReasons(result.Reasons);
            }
        }
        /// <summary>
        /// Convert result with value to result with another value that may fail.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = result
        ///     .Bind(GetWhichMayFail)
        ///     .Bind(ProcessWhichMayFail)
        ///     .Bind(FormattingWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="result"/>
        /// <param name="bind">Transformation that may fail.</param>
        public IResult<TNewValue> Bind<TNewValue>(Func<TValue, IResult<TNewValue>> bind)
        {
            if(result.IsSuccess)
            {
                var converted = bind(result.ValueOrDefault);
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
        /// Convert result with value to result with another value that may fail asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="result"/>
        /// <param name="bind">Transformation that may fail.</param>
        public async Task<IResult<TNewValue>> Bind<TNewValue>(Func<TValue, Task<IResult<TNewValue>>> bind)
        {
            if(result.IsSuccess)
            {
                var converted = await bind(result.ValueOrDefault);
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
        /// Convert result with value to result with another value that may fail asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="result"/>
        /// <param name="bind">Transformation that may fail.</param>
        public async ValueTask<IResult<TNewValue>> Bind<TNewValue>(Func<TValue, ValueTask<IResult<TNewValue>>> bind)
        {
            if(result.IsSuccess)
            {
                var converted = await bind(result.ValueOrDefault);
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
        /// Execute an action which returns a <see cref="IResult"/> synchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var done = await result.Bind(ActionWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="result"/>
        /// <param name="action">Action that may fail.</param>
        public IResult Bind(Func<TValue, IResult> action)
        {
            if(result.IsSuccess)
            {
                var converted = action(result.ValueOrDefault);
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
        /// Execute an action which returns a <see cref="IResult"/> asynchronously.
        /// </summary>
        /// <example>
        /// <code>
        ///  var done = await result.Bind(ActionWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="result"/>
        /// <param name="action">Action that may fail.</param>
        public async Task<IResult> Bind(Func<TValue, Task<IResult>> action)
        {
            if(result.IsSuccess)
            {
                var converted = await action(result.ValueOrDefault);
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
        ///  var done = await result.Bind(ActionWhichMayFail);
        /// </code>
        /// </example>
        /// <param name="result"/>
        /// <param name="action">Action that may fail.</param>
        public async ValueTask<IResult> Bind(Func<TValue, ValueTask<IResult>> action)
        {
            if(result.IsSuccess)
            {
                var converted = await action(result.ValueOrDefault);
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
    }
#endif


    /// <summary>
    /// Convert result to result with value that may fail.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="result"/>
    /// <param name="bind">Transformation that may fail.</param>
    public static IResult<TNewValue> Bind<TNewValue>(this IResult result, Func<IResult<TNewValue>> bind)
    {
        if(result.IsSuccess)
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
    public static async Task<IResult<TNewValue>> Bind<TNewValue>(this IResult result, Func<Task<IResult<TNewValue>>> bind)
    {
        if(result.IsSuccess)
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
    public static async ValueTask<IResult<TNewValue>> Bind<TNewValue>(this IResult result, Func<ValueTask<IResult<TNewValue>>> bind)
    {
        if(result.IsSuccess)
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
    public static IResult Bind(this IResult result, Func<IResult> action)
    {
        if(result.IsSuccess)
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
    public static async Task<IResult> Bind(this IResult result, Func<Task<IResult>> action)
    {
        if(result.IsSuccess)
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
    /// <param name="result"/>
    /// <param name="action">Action that may fail.</param>
    public static async ValueTask<IResult> Bind(this IResult result, Func<ValueTask<IResult>> action)
    {
        if(result.IsSuccess)
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


    ////////////////////////




    /// <summary>
    /// Convert result with value to result with another value. Use valueConverter parameter to specify the value transformation logic.
    /// </summary>
    public static IResult<TNewValue> Map<TValue, TNewValue>(this IResult<TValue> result, Func<TValue, TNewValue> mapLogic)
    {
        if(result.IsSuccess)
        {
            if(mapLogic is null)
                throw new ArgumentException("If result is success then valueConverter should not be null");

            return ResultFactory
                .CreateEmptyResult(mapLogic(result.ValueOrDefault))
                .WithReasons(result.Reasons);
        }
        else
        {
            return ResultFactory
                .CreateEmptyResult<TNewValue>(default!)
                .WithReasons(result.Reasons);
        }
    }
    /// <summary>
    /// Convert result with value to result with another value that may fail.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = result
    ///     .Bind(GetWhichMayFail)
    ///     .Bind(ProcessWhichMayFail)
    ///     .Bind(FormattingWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="result"/>
    /// <param name="bind">Transformation that may fail.</param>
    public static IResult<TNewValue> Bind<TValue, TNewValue>(this IResult<TValue> result, Func<TValue, IResult<TNewValue>> bind)
    {
        if(result.IsSuccess)
        {
            var converted = bind(result.ValueOrDefault);
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
    /// Convert result with value to result with another value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="result"/>
    /// <param name="bind">Transformation that may fail.</param>
    public static async Task<IResult<TNewValue>> Bind<TValue, TNewValue>(this IResult<TValue> result, Func<TValue, Task<IResult<TNewValue>>> bind)
    {
        if(result.IsSuccess)
        {
            var converted = await bind(result.ValueOrDefault);
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
    /// Convert result with value to result with another value that may fail asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var bakeryDtoResult = await result.Bind(GetWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="result"/>
    /// <param name="bind">Transformation that may fail.</param>
    public static async ValueTask<IResult<TNewValue>> Bind<TValue, TNewValue>(this IResult<TValue> result, Func<TValue, ValueTask<IResult<TNewValue>>> bind)
    {
        if(result.IsSuccess)
        {
            var converted = await bind(result.ValueOrDefault);
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
    /// Execute an action which returns a <see cref="IResult"/> synchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var done = await result.Bind(ActionWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="result"/>
    /// <param name="action">Action that may fail.</param>
    public static IResult Bind<TValue>(this IResult<TValue> result, Func<TValue, IResult> action)
    {
        if(result.IsSuccess)
        {
            var converted = action(result.ValueOrDefault);
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
    /// Execute an action which returns a <see cref="IResult"/> asynchronously.
    /// </summary>
    /// <example>
    /// <code>
    ///  var done = await result.Bind(ActionWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="result"/>
    /// <param name="action">Action that may fail.</param>
    public static async Task<IResult> Bind<TValue>(this IResult<TValue> result, Func<TValue, Task<IResult>> action)
    {
        if(result.IsSuccess)
        {
            var converted = await action(result.ValueOrDefault);
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
    ///  var done = await result.Bind(ActionWhichMayFail);
    /// </code>
    /// </example>
    /// <param name="result"/>
    /// <param name="action">Action that may fail.</param>
    public static async ValueTask<IResult> Bind<TValue>(this IResult<TValue> result, Func<TValue, ValueTask<IResult>> action)
    {
        if(result.IsSuccess)
        {
            var converted = await action(result.ValueOrDefault);
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




}
