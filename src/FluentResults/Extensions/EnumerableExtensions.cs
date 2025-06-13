using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace FluentResults
{
    /// <summary>
    /// Helpful extensions for IEnumerable
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Merge multiple result objects to one result together
        /// </summary>
        public static IResultBase Merge(this IEnumerable<IResultBase> results)
        {
            return ResultHelper.Merge(results);
        }

        /// <summary>
        /// Merge multiple result objects to one result together
        /// </summary>
        public static IResult<IEnumerable<TValue>> Merge<TValue>(this IEnumerable<IResult<TValue>> results)
        {
            return ResultHelper.MergeWithValue(results);
        }
    }
}