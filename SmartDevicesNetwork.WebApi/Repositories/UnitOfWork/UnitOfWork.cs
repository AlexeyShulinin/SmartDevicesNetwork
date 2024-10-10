using System.Threading;
using System.Threading.Tasks;
using SmartDevicesNetwork.WebApi.Database;
using SmartDevicesNetwork.WebApi.Repositories.Interfaces;

namespace SmartDevicesNetwork.WebApi.Repositories.UnitOfWork;

public class UnitOfWork(SdnDbContext dbContext) : IUnitOfWork
{
    private IActionsRepository actionsRepository;
    private IDevicesRepository devicesRepository;
    private INetworkRepository networkRepository;

    public IActionsRepository ActionsRepository => actionsRepository ??= new ActionsRepository(dbContext);
    public IDevicesRepository DevicesRepository => devicesRepository ??= new DevicesRepository(dbContext);
    public INetworkRepository NetworkRepository => networkRepository ??= new NetworkRepository(dbContext);
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}