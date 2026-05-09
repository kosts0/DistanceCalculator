using System.Text.Json.Serialization;
using DistanceCalculator.Enums;

namespace DistanceCalculator.Dtos;

public class ErrorResponse
{
    public string Status { get; init; } = "ERROR";
    public ErrorDetails Error { get; init; } = null!;

    public ErrorResponse() { }

    public ErrorResponse(string message)
    {
        Error = new ErrorDetails(message);
    }
}

public class ErrorDetails
{
    public string Message { get; init; } = string.Empty;

    public ErrorDetails() { }

    public ErrorDetails(string message)
    {
        Message = message;
    }
}

public class SuccessDistanceResponse
{
    public string Status { get; init; } = "SUCCESS";
    public DistanceData Data { get; init; } = null!;

    public SuccessDistanceResponse() { }

    public SuccessDistanceResponse(double distanceInKm, CalculatorType calculatorType)
    {
        Data = new DistanceData(distanceInKm, calculatorType);
    }
}

public class DistanceData
{
    public double DistanceInKm { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CalculatorType CalculatorType { get; init; }

    public DistanceData() { }

    public DistanceData(double distanceInKm, CalculatorType calculatorType)
    {
        DistanceInKm = Math.Round(distanceInKm, 3);
        CalculatorType = calculatorType;
    }
}