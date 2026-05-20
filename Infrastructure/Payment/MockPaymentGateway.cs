using System;
using System.Threading.Tasks;
namespace AirportSystem.Avalonia.Infrastructure.Payment;
public class MockPaymentGateway : IPaymentGateway
{
    private readonly Random _random = new();
    public Task<bool> ProcessPaymentAsync(double amount) => Task.FromResult(_random.NextDouble() < 0.8);
    public Task<bool> ProcessRefundAsync(double amount) => Task.FromResult(true);
}
