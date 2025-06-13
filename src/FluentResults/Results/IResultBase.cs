using FluentResults;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace
namespace FluentResults;

/// <summary>
/// Definition of a ResultBase
/// </summary>
public interface IResultBase
{
    /// <summary>
    /// Get all reasons (errors and successes)
    /// </summary>
    List<IReason> Reasons { get; }
}

/// <summary>
/// Direkte Erweiterungs-Methoden für <see cref="IResultBase"/>
/// </summary>
public static partial class ResultBaseExt
{
    /// <summary>
    /// Is true if Reasons contains at least one error
    /// </summary>
    public static bool IsFailed(this IResultBase result) =>
        result.Reasons.OfType<IError>().Any();

    /// <summary>
    /// Is true if Reasons contains no errors
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSuccess(this IResultBase result) =>
        !IsFailed(result);

    /// <summary>
    /// Get all errors
    /// </summary>
    public static IEnumerable<IError> GetErrors(this IResultBase result) =>
        result.Reasons.OfType<IError>();

    /// <summary>
    /// Get all successes
    /// </summary>
    public static IEnumerable<ISuccess> GetSuccesses(this IResultBase result) =>
        result.Reasons.OfType<ISuccess>();
}