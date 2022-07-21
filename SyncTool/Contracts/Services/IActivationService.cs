using System.Threading.Tasks;

namespace SyncTool.Contracts.Services;

public interface IActivationService
{
    Task ActivateAsync(object activationArgs);
}
