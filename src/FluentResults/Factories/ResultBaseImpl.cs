using FluentResults;
using System.Collections.Generic;
using System.Linq;

namespace FluentResults.Factories;

/// <summary>
/// Standard-Implementierung von <see cref="IResult"/>
/// </summary>
internal sealed class ResultBaseImpl : IResult
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
