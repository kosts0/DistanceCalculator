namespace DistanceCalculator.Exceptions;

public class IntegrationException : Exception
{
    public string ErrorCode { get; }

    public IntegrationException(string message, string errorCode = "INTEGRATION_ERROR")
        : base(message)
    {
        ErrorCode = errorCode;
    }

    public IntegrationException(string message, Exception innerException, string errorCode = "INTEGRATION_ERROR")
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}