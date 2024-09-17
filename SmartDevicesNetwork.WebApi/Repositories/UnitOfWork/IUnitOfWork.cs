using System.Threading;
using System.Threading.Tasks;
using SmartDevicesNetwork.WebApi.Repositories.Interfaces;

namespace SmartDevicesNetwork.WebApi.Repositories.UnitOfWork;

public interface IUnitOfWork
{
    IActionsRepository ActionsRepository { get; }
    IDevicesRepository DevicesRepository { get; }
    INetworkRepository NetworkRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}