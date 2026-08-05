namespace Gateway.OpenTelemetry.Core.Abstractions;

/// <summary>
/// Enriches OpenTelemetry resources.
/// </summary>
public interface IResourceEnricher
{
    void Enrich(
        IDictionary<string, object?> attributes);
}
