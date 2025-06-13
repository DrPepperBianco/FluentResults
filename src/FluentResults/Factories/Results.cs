using FluentResults.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public partial class Result
    {
        internal static ResultSettings Settings { get; private set; }

        static Result()
        {
            Settings = new ResultSettingsBuilder().Build();
        }

        /// <summary>
        /// Setup global settings like logging
        /// </summary>
        public static void Setup(Action<ResultSettingsBuilder> setupFunc)
        {
            var settingsBuilder = new ResultSettingsBuilder();
            setupFunc(settingsBuilder);

            Settings = settingsBuilder.Build();
        }

        /// <summary>
        /// Creates an empty <see cref="IResultBase"/>
        /// </summary>
        public static IResultBase Create() => 
            ResultFactory.CreateEmptyResult();

        /// <summary>
        /// Creates an empty <see cref="IResult{TValue}"/>.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="value">Optional value, to initialize the result with a value.</param>
        /// <returns></returns>
        public static IResult<TValue> Create<TValue>(TValue value = default) =>
            ResultFactory.CreateEmptyResult<TValue>(value);

        /// <summary>
        /// Creates a success result
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IResultBase Ok() => Create();

        /// <summary>
        /// Creates a failed result with the given error
        /// </summary>
        public static IResultBase Fail(IError error)
        {
            return Create().WithError(error);
        }

        /// <summary>
        /// Creates a failed result with the given error message. Internally an error object from the error factory is created. 
        /// </summary>
        public static IResultBase Fail(string errorMessage)
        {
            var result = Create();
            result.WithError(Settings.ErrorFactory(errorMessage));
            return result;
        }

        /// <summary>
        /// Creates a failed result with the given error messages. Internally a list of error objects from the error factory is created
        /// </summary>
        public static IResultBase Fail(IEnumerable<string> errorMessages)
        {
            if (errorMessages == null)
                throw new ArgumentNullException(nameof(errorMessages), "The list of error messages cannot be null");

            var result = Create();
            result.WithErrors(errorMessages.Select(Settings.ErrorFactory));
            return result;
        }

        /// <summary>
        /// Creates a failed result with the given errors.
        /// </summary>
        public static IResultBase Fail(IEnumerable<IError> errors)
        {
            if (errors == null)
                throw new ArgumentNullException(nameof(errors), "The list of errors cannot be null");

            var result = Create();
            result.WithErrors(errors);
            return result;
        }

        /// <summary>
        /// Creates a success result with the given value
        /// </summary>
        public static IResult<TValue> Ok<TValue>(TValue value)
        {
            return Create(value);
        }

        /// <summary>
        /// Creates a failed result with the given error
        /// </summary>
        public static IResult<TValue> Fail<TValue>(IError error)
        {
            var result = Create<TValue>(default!);
            result.WithError(error);
            return result;
        }

        /// <summary>
        /// Creates a failed result with the given error message. Internally an error object from the error factory is created. 
        /// </summary>
        public static IResult<TValue> Fail<TValue>(string errorMessage)
        {
            var result = Create<TValue>(default!);
            result.WithError(Settings.ErrorFactory(errorMessage));
            return result;
        }

        /// <summary>
        /// Creates a failed result with the given error messages. Internally a list of error objects from the error factory is created. 
        /// </summary>
        public static IResult<TValue> Fail<TValue>(IEnumerable<string> errorMessages)
        {
            if (errorMessages == null)
                throw new ArgumentNullException(nameof(errorMessages), "The list of error messages cannot be null");

            var result = Create<TValue>(default!);
            result.WithErrors(errorMessages.Select(Settings.ErrorFactory));
            return result;
        }

        /// <summary>
        /// Creates a failed result with the given errors.
        /// </summary>
        public static IResult<TValue> Fail<TValue>(IEnumerable<IError> errors)
        {
            if (errors == null)
                throw new ArgumentNullException(nameof(errors), "The list of errors cannot be null");

            var result = Create<TValue>(default!);
            result.WithErrors(errors);
            return result;
        }

        /// <summary>
        /// Merge multiple result objects to one result object together
        /// </summary>
        public static IResultBase Merge(params IResultBase[] results)
        {
            return ResultHelper.Merge(results);
        }

        /// <summary>
        /// Merge multiple result objects to one result object together. Return one result with a list of merged values.
        /// </summary>
        public static IResult<IEnumerable<TValue>> Merge<TValue>(params IResult<TValue>[] results)
        {
            return ResultHelper.MergeWithValue(results);
        }

        /// <summary>
        /// Create a success/failed result depending on the parameter isSuccess
        /// </summary>
        public static IResultBase OkIf(bool isSuccess, IError error)
        {
            return isSuccess ? Ok() : Fail(error);
        }

        /// <summary>
        /// Create a success/failed result depending on the parameter isSuccess
        /// </summary>
        public static IResultBase OkIf(bool isSuccess, string error)
        {
            return isSuccess ? Ok() : Fail(error);
        }

        /// <summary>
        /// Create a success/failed result depending on the parameter isSuccess
        /// </summary>
        /// <remarks>
        /// Error is lazily evaluated.
        /// </remarks>
        public static IResultBase OkIf(bool isSuccess, Func<IError> errorFactory)
        {
            return isSuccess ? Ok() : Fail(errorFactory.Invoke());
        }

        /// <summary>
        /// Create a success/failed result depending on the parameter isSuccess
        /// </summary>
        /// <remarks>
        /// Error is lazily evaluated.
        /// </remarks>
        public static IResultBase OkIf(bool isSuccess, Func<string> errorMessageFactory)
        {
            return isSuccess ? Ok() : Fail(errorMessageFactory.Invoke());
        }

        /// <summary>
        /// Create a success/failed result depending on the parameter isFailure
        /// </summary>
        public static IResultBase FailIf(bool isFailure, IError error)
        {
            return isFailure ? Fail(error) : Ok();
        }

        /// <summary>
        /// Create a success/failed result depending on the parameter isFailure
        /// </summary>
        public static IResultBase FailIf(bool isFailure, string error)
        {
            return isFailure ? Fail(error) : Ok();
        }

        /// <summary>
        /// Create a success/failed result depending on the parameter isFailure
        /// </summary>
        /// <remarks>
        /// Error is lazily evaluated.
        /// </remarks>
        public static IResultBase FailIf(bool isFailure, Func<IError> errorFactory)
        {
            return isFailure ? Fail(errorFactory.Invoke()) : Ok();
        }

        /// <summary>
        /// Create a success/failed result depending on the parameter isFailure
        /// </summary>
        /// <remarks>
        /// Error is lazily evaluated.
        /// </remarks>
        public static IResultBase FailIf(bool isFailure, Func<string> errorMessageFactory)
        {
            return isFailure ? Fail(errorMessageFactory.Invoke()) : Ok();
        }

        /// <summary>
        /// Executes the action. If an exception is thrown within the action then this exception is transformed via the catchHandler to an Error object
        /// </summary>
        public static IResultBase Try(Action action, Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                action();
                return Ok();
            }
            catch (Exception e)
            {
                return Fail(catchHandler(e));
            }
        }

        /// <summary>
        /// Executes the action. If an exception is thrown within the action then this exception is transformed via the catchHandler to an Error object
        /// </summary>
        public static async Task<IResultBase> Try(Func<Task> action, Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                await action();
                return Ok();
            }
            catch (Exception e)
            {
                return Fail(catchHandler(e));
            }
        }

        /// <summary>
        /// Executes the action. If an exception is thrown within the action then this exception is transformed via the catchHandler to an Error object
        /// </summary>
        public static async ValueTask<IResultBase> Try(Func<ValueTask> action, Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                await action();
                return Ok();
            }
            catch (Exception e)
            {
                return Fail(catchHandler(e));
            }
        }

        /// <summary>
        /// Executes the action. If an exception is thrown within the action then this exception is transformed via the catchHandler to an Error object
        /// </summary>
        public static IResult<T> Try<T>(Func<T> action, Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                return Ok(action());
            }
            catch (Exception e)
            {
                return Fail<T>(catchHandler(e));
            }
        }

        /// <summary>
        /// Executes the action. If an exception is thrown within the action then this exception is transformed via the catchHandler to an Error object
        /// </summary>
        public static async Task<IResult<T>> Try<T>(Func<Task<T>> action, Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                return Ok(await action());
            }
            catch (Exception e)
            {
                return Fail<T>(catchHandler(e));
            }
        }

        /// <summary>
        /// Executes the action. If an exception is thrown within the action then this exception is transformed via the catchHandler to an Error object
        /// </summary>
        public static async ValueTask<IResult<T>> Try<T>(Func<ValueTask<T>> action, Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                return Ok(await action());
            }
            catch (Exception e)
            {
                return Fail<T>(catchHandler(e));
            }
        }

        /// <summary>
        /// Executes the action. If an exception is thrown within the action then this exception is transformed via the catchHandler to an Error object
        /// </summary>
        public static IResultBase Try(Func<IResultBase> action, Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                return action();
            }
            catch (Exception e)
            {
                return Fail(catchHandler(e));
            }

        }

        /// <summary>
        /// Executes the action. If an exception is thrown within the action then this exception is transformed via the catchHandler to an Error object
        /// </summary>
        public static async Task<IResultBase> Try(Func<Task<IResultBase>> action, Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                return await action();
            }
            catch (Exception e)
            {
                return Fail(catchHandler(e));
            }
        }

        /// <summary>
        /// Executes the action. If an exception is thrown within the action then this exception is transformed via the catchHandler to an Error object
        /// </summary>
        public static async ValueTask<IResultBase> Try(Func<ValueTask<IResultBase>> action, Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                return await action();
            }
            catch (Exception e)
            {
                return Fail(catchHandler(e));
            }
        }

        /// <summary>
        /// Executes the action. If an exception is thrown within the action then this exception is transformed via the catchHandler to an Error object
        /// </summary>
        public static IResult<T> Try<T>(Func<IResult<T>> action, Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                return action();
            }
            catch (Exception e)
            {
                return Fail<T>(catchHandler(e));
            }

        }

        /// <summary>
        /// Executes the action. If an exception is thrown within the action then this exception is transformed via the catchHandler to an Error object
        /// </summary>
        public static async Task<IResult<T>> Try<T>(Func<Task<IResult<T>>> action, Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                return await action();
            }
            catch (Exception e)
            {
                return Fail<T>(catchHandler(e));
            }
        }

        /// <summary>
        /// Executes the action. If an exception is thrown within the action then this exception is transformed via the catchHandler to an Error object
        /// </summary>
        public static async ValueTask<IResult<T>> Try<T>(Func<ValueTask<IResult<T>>> action, Func<Exception, IError> catchHandler = null)
        {
            catchHandler = catchHandler ?? Settings.DefaultTryCatchHandler;

            try
            {
                return await action();
            }
            catch (Exception e)
            {
                return Fail<T>(catchHandler(e));
            }
        }

    }
}