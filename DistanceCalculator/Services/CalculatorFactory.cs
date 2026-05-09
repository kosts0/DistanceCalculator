using DistanceCalculator.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace DistanceCalculator.Services;

public interface ICalculatorFactory
{
    IDistanceCalculator GetCalculator(CalculatorType type);
}

public class CalculatorFactory : ICalculatorFactory
{
    private readonly IServiceProvider _serviceProvider;
    private static readonly Dictionary<CalculatorType, Func<IServiceProvider, IDistanceCalculator>> _creators = new()
    {
        [CalculatorType.Haversine] = sp => sp.GetRequiredService<HaversineDistanceCalculator>(),
        [CalculatorType.TwoGis] = sp => sp.GetRequiredService<TwoGisDistanceCalculator>()
    };

    public CalculatorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IDistanceCalculator GetCalculator(CalculatorType type)
    {
        if (_creators.TryGetValue(type, out var creator))
        {
            return creator(_serviceProvider);
        }

        throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown calculator type");
    }
}