using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FluentResults;

/// <summary>
/// Extension methods for Result for Logging
/// </summary>
public static class ResultLogging
{
    extension<TResult>(TResult result) where TResult : IResultBase
    {
        /// <summary>
        /// Log the result. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult Log(LogLevel logLevel = LogLevel.Information)
        {
            return result.Log(string.Empty, null, logLevel);
        }

        /// <summary>
        /// Log the result. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult Log(string context, LogLevel logLevel = LogLevel.Information)
        {
            return result.Log(context, null, logLevel);
        }

        /// <summary>
        /// Log the result with a specific logger context. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult Log(string context, string content, LogLevel logLevel = LogLevel.Information)
        {
            var logger = Result.Settings.Logger;

            logger.Log(context, content, result, logLevel);

            return (TResult)result;
        }

        /// <summary>
        /// Log the result only when it is successful. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult LogIfSuccess(LogLevel logLevel = LogLevel.Information)
        {
            if(result.IsSuccess)
                return result.Log(logLevel);

            return (TResult)result;
        }

        /// <summary>
        /// Log the result with a specific logger context only when it is successful. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult LogIfSuccess(string context, string content = null, LogLevel logLevel = LogLevel.Information)
        {
            if(result.IsSuccess)
                return result.Log(context, content, logLevel);

            return (TResult)result;
        }
        /// <summary>
        /// Log the result only when it is failed. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult LogIfFailed(LogLevel logLevel = LogLevel.Error)
        {
            if(result.IsFailed)
                return result.Log(logLevel);

            return (TResult)result;
        }

        /// <summary>
        /// Log the result with a specific logger context only when it is failed. Configure the logger via Result.Setup(..)
        /// </summary>
        public TResult LogIfFailed(string context, string content = null, LogLevel logLevel = LogLevel.Error)
        {
            if(result.IsFailed)
                return result.Log(context, content, logLevel);

            return (TResult)result;
        }
    }

    /// <param name="result">Result without value</param>
    extension(IResultBase result)
    {
        /// <summary>
        /// Log the result with a typed context. Configure the logger via Result.Setup(..)
        /// </summary>
        public IResultBase LogWithContext<TContext>(LogLevel logLevel = LogLevel.Information)
        {
            return result.LogWithContext<TContext>(null, logLevel);
        }

        /// <summary>
        /// Log the result with a typed context. Configure the logger via Result.Setup(..)
        /// </summary>
        public IResultBase LogWithContext<TContext>(string content, LogLevel logLevel = LogLevel.Information)
        {
            var logger = Result.Settings.Logger;

            logger.Log<TContext>(content, result, logLevel);

            return (IResultBase)result;
        }


        /// <summary>
        /// Log the result with a typed context only when it is successful. Configure the logger via Result.Setup(..)
        /// </summary>
        public IResultBase LogWithContextIfSuccess<TContext>(string content = null, LogLevel logLevel = LogLevel.Information)
        {
            if(result.IsSuccess)
                return result.LogWithContext<TContext>(content, logLevel);

            return (IResultBase)result;
        }


        /// <summary>
        /// Log the result with a typed context only when it is failed. Configure the logger via Result.Setup(..)
        /// </summary>
        public IResultBase LogWithContextIfFailed<TContext>(string content = null, LogLevel logLevel = LogLevel.Error)
        {
            if(result.IsFailed)
                return result.LogWithContext<TContext>(content, logLevel);

            return (IResultBase)result;
        }
    }
}
