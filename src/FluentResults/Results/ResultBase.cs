using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    public interface IResultBase
    {
        /// <summary>
        /// Get all reasons (errors and successes)
        /// </summary>
        List<IReason> Reasons { get; }
    }

    public abstract class ResultBase : IResultBase
    {
        /// <inheritdoc/>
        public List<IReason> Reasons { get; }

        protected ResultBase()
        {
            Reasons = new List<IReason>();
        }
    }

    /// <summary>
    /// Describes a type, that has deep clone mechanism
    /// </summary>
    public interface IDeepClonable<out T>
    {
        /// <summary>
        /// Creates a deep clone
        /// </summary>
        public T DeepClone();
    }

    public abstract class ResultBase<TResult> : ResultBase
        where TResult : ResultBase<TResult>

    {
        /// <summary>
        /// Add an error
        /// </summary>
        public TResult WithError<TError>()
            where TError : IError, new()
        {
            return (TResult)this.WithError(new TError());
        }

        /// <summary>
        /// Add a success
        /// </summary>
        public TResult WithSuccess<TSuccess>()
            where TSuccess : ISuccess, new()
        {
            return (TResult)this.WithSuccess(new TSuccess());
        }

        /// <summary>
        /// Log the result. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult Log(LogLevel logLevel = LogLevel.Information)
        {
            return Log(string.Empty, null, logLevel);
        }

        /// <summary>
        /// Log the result. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult Log(string context, LogLevel logLevel = LogLevel.Information)
        {
            return Log(context, null, logLevel);
        }

        /// <summary>
        /// Log the result with a specific logger context. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult Log(string context, string content, LogLevel logLevel = LogLevel.Information)
        {
            var logger = Result.Settings.Logger;

            logger.Log(context, content, this, logLevel);

            return (TResult)this;
        }

        /// <summary>
        /// Log the result with a typed context. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult Log<TContext>(LogLevel logLevel = LogLevel.Information)
        {
            return Log<TContext>(null, logLevel);
        }

        /// <summary>
        /// Log the result with a typed context. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult Log<TContext>(string content, LogLevel logLevel = LogLevel.Information)
        {
            var logger = Result.Settings.Logger;

            logger.Log<TContext>(content, this, logLevel);

            return (TResult)this;
        }

        /// <summary>
        /// Log the result only when it is successful. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult LogIfSuccess(LogLevel logLevel = LogLevel.Information)
        {
            if (this.IsSuccess())
                return Log(logLevel);

            return (TResult)this;
        }

        /// <summary>
        /// Log the result with a specific logger context only when it is successful. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult LogIfSuccess(string context, string content = null, LogLevel logLevel = LogLevel.Information)
        {
            if (this.IsSuccess())
                return Log(context, content, logLevel);

            return (TResult)this;
        }

        /// <summary>
        /// Log the result with a typed context only when it is successful. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult LogIfSuccess<TContext>(string content = null, LogLevel logLevel = LogLevel.Information)
        {
            if (this.IsSuccess())
                return Log<TContext>(content, logLevel);

            return (TResult)this;
        }

        /// <summary>
        /// Log the result only when it is failed. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult LogIfFailed(LogLevel logLevel = LogLevel.Error)
        {
            if (this.IsFailed())
                return Log(logLevel);

            return (TResult)this;
        }

        /// <summary>
        /// Log the result with a specific logger context only when it is failed. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult LogIfFailed(string context, string content = null, LogLevel logLevel = LogLevel.Error)
        {
            if (this.IsFailed())
                return Log(context, content, logLevel);

            return (TResult)this;
        }

        /// <summary>
        /// Log the result with a typed context only when it is failed. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult LogIfFailed<TContext>(string content = null, LogLevel logLevel = LogLevel.Error)
        {
            if (this.IsFailed())
                return Log<TContext>(content, logLevel);

            return (TResult)this;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var reasonsString = Reasons.Any()
                                    ? $", Reasons='{ReasonFormat.ReasonsToString(Reasons)}'"
                                    : string.Empty;

            return $"Result: IsSuccess='{this.IsSuccess()}'{reasonsString}";
        }
    }
}