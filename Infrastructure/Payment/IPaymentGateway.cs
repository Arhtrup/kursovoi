using System.Threading.Tasks;
namespace AirportSystem.Avalonia.Infrastructure.Payment;
public interface IPaymentGateway
{
    Task<bool> ProcessPaymentAsync(double amount);
    Task<bool> ProcessRefundAsync(double amount);
}
