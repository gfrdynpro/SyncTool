using System.Threading.Tasks;

namespace SyncTool.Activation;

public interface IActivationHandler
{
    bool CanHandle(object args);

    Task HandleAsync(object args);
}
