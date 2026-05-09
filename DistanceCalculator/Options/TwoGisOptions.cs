namespace DistanceCalculator.Options;

public class TwoGisOptions
{
    public const string SectionName = "TwoGis";

    public string EndpointHost { get; set; } = "https://routing.api.2gis.com";
    public string ApiKey { get; set; } = string.Empty;
    public string Version { get; set; } = "2.0";
}