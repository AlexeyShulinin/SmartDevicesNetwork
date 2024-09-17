using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SmartDevicesNetwork.WebApi.Database;
using SmartDevicesNetwork.WebApi.Repositories.Interfaces;

namespace SmartDevicesNetwork.WebApi.Repositories.UnitOfWork;

public class UnitOfWork(SdnDbContext dbContext, IMapper mapper) : IUnitOfWork
{
    private IActionsRepository actionsRepository;
    private IDevicesRepository devicesRepository;
    private INetworkRepository networkRepository;

    public IActionsRepository ActionsRepository => actionsRepository ??= new ActionsRepository(dbContext, mapper);
    public IDevicesRepository DevicesRepository => devicesRepository ??= new DevicesRepository(dbContext, mapper);
    public INetworkRepository NetworkRepository => networkRepository ??= new NetworkRepository(dbContext, mapper);
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}