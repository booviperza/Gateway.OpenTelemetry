namespace Gateway.OpenTelemetry.Core.Abstractions;

/// <summary>
/// Enriches an OpenTelemetry Activity.
/// </summary>
/// <typeparam name="TContext">
/// Context type.
/// </typeparam>
public interface IActivityEnricher<in TContext>
{
    /// <summary>
    /// Returns true if this enricher can enrich the specified context.
    /// </summary>
    bool CanEnrich(TContext context);

    /// <summary>
    /// Enriches the Activity.
    /// </summary>
    void Enrich(
        TContext context,
        Activity activity);
}
