using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// Interface to describe Result without value
    /// </summary>
    /// <remarks>
    /// The only reason for this, is for extension methods to 
    /// differntiate between result with value and without value.
    /// </remarks>
    public interface IResultWithoutValue : IResultBase { }

    /// <summary>
    /// Implementation for result without value
    /// </summary>
    public partial class Result : ResultBase<Result>, IDeepClonable<Result>, IResultWithoutValue
    {
        /// <summary>
        /// Default-Constructor
        /// </summary>
        public Result()
        { }

        /// <inheritdoc/>
        public Result DeepClone() => 
            new Result().WithReasons(this.Reasons);
        
        /// <summary>
        /// Implicit cast from Error to Result
        /// </summary>
        public static implicit operator Result(Error error)
        {
            return Fail(error);
        }

        /// <summary>
        /// Implicit cast from List of errors to Result
        /// </summary>
        public static implicit operator Result(List<Error> errors)
        {
            return Fail(errors);
        }
    }

    /// <summary>
    /// Interface for result, that can contain a result value.
    /// </summary>
    public interface IResult<out TValue> : IResultBase
    {
        /// <summary>
        /// Get the Value. If result is failed then an Exception is thrown because a failed result has no value. Opposite see property ValueOrDefault.
        /// </summary>
        TValue Value { get; }

        /// <summary>
        /// Get the Value. If result is failed then a default value is returned. Opposite see property Value.
        /// </summary>
        TValue ValueOrDefault { get; }
    }

    /// <summary>
    /// Result, that can contain a result value.
    /// </summary>
    public class Result<TValue> : ResultBase<Result<TValue>>, IResult<TValue>, IDeepClonable<Result<TValue>>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Result()
        { }

        /// <inheritdoc/>
        public Result<TValue> DeepClone() => 
            new Result<TValue>() { _value = _value }
            .WithReasons(Reasons);

        private TValue _value;

        /// <inheritdoc/>
        public TValue ValueOrDefault => _value;

        /// <inheritdoc/>
        public TValue Value
        {
            get
            {
                ThrowIfFailed();

                return _value;
            }
            private set
            {
                ThrowIfFailed();

                _value = value;
            }
        }

        /// <summary>
        /// Set value
        /// </summary>
        public Result<TValue> WithValue(TValue value)
        {
            Value = value;
            return this;
        }

        /// <summary>
        /// Convert result with value to result with another value. Use valueConverter parameter to specify the value transformation logic.
        /// </summary>
        public Result<TNewValue> ConvertResult<TNewValue>(Func<TValue, TNewValue>? valueConverter = null) =>
            this.ToResult<TValue, TNewValue>(valueConverter);

        /// <inheritdoc/>
        public override string ToString()
        {
            var baseString = base.ToString();
            var valueString = ValueOrDefault.ToLabelValueStringOrEmpty(nameof(Value));
            return $"{baseString}, {valueString}";
        }

        /// <summary>
        /// Implicit cast to result without value
        /// </summary>
        public static implicit operator Result<TValue>(Result result)
        {
            return result.ToResult<TValue>(default);
        }

        /// <summary>
        /// Implicit cast to result of object
        /// </summary>
        public static implicit operator Result<object>(Result<TValue> result)
        {
            return result.ToResult(value => (object)value);
        }

        /// <summary>
        /// Implicit cast to result with another value
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Result<TValue>(TValue value)
        {
            if (value is Result<TValue> r)
                return r;

            return Result.Ok(value);
        }
        
        /// <summary>
        /// Implicit cast from error to result with value
        /// </summary>
        public static implicit operator Result<TValue>(Error error)
        {
            return Result.Fail(error);
        }

        /// <summary>
        /// Implicit cast from List of erros to result of value
        /// </summary>
        public static implicit operator Result<TValue>(List<Error> errors)
        {
            return Result.Fail(errors);
        }

        private void ThrowIfFailed()
        {
            if (this.IsFailed())
                throw new InvalidOperationException($"Result is in status failed. Value is not set. Having: {ReasonFormat.ErrorReasonsToString(this.Errors())}");
        }
    }
}