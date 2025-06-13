using FluentResults;
using System.Collections.Generic;
using System.Linq;

namespace FluentResults.Factory;

/// <summary>
/// Standard-Implementierung von <see cref="IResultBase"/>
/// </summary>
public sealed class ResultBaseImpl : IResultBase
{
    /// <inheritdoc/>
    public List<IReason> Reasons { get; } = new();

    /// <summary>
    /// ToString override
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var reasonsString = Reasons.Any()
            ? $", Reasons='{ReasonFormat.ReasonsToString(Reasons)}'"
            : string.Empty;

        return $"Result: IsSuccess='{this.IsSuccess}'{reasonsString}";
    }
}
