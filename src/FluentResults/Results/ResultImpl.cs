#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentResults.Implementations;

/// <summary>
/// Implementierung von <see cref="IResult{TValue}"/>
/// </summary>
public sealed class ResultImpl<TValue>(TValue? valueOrDefault) : 
    IResult<TValue>
{
    /// <inheritdoc/>
    public TValue? ValueOrDefault { get; } = valueOrDefault;

    /// <inheritdoc/>
    public List<IReason> Reasons { get; } = new();

    /// <summary>
    /// ToString implementation
    /// </summary>
    public override string ToString()
    {
        // The first part is identical to ToString in ResultBaseImpl.
        var reasonsString = Reasons.Any()
            ? $", Reasons='{ReasonFormat.ReasonsToString(Reasons)}'"
            : string.Empty;

        var baseString = $"Result: IsSuccess='{this.IsSuccess()}'{reasonsString}";
        var valueString = ValueOrDefault.ToLabelValueStringOrEmpty("Value");
        return $"{baseString}, {valueString}";
    }

}
