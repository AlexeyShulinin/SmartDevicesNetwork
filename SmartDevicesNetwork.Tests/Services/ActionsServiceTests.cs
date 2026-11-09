using Imposter.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using SmartDevicesNetwork.WebApi.Enums;
using SmartDevicesNetwork.WebApi.Models.Requests;
using SmartDevicesNetwork.WebApi.Repositories.Interfaces;
using SmartDevicesNetwork.WebApi.Repositories.Models;
using SmartDevicesNetwork.WebApi.Repositories.UnitOfWork;
using SmartDevicesNetwork.WebApi.Resources;
using SmartDevicesNetwork.WebApi.Services;
using CacheConstants = SmartDevicesNetwork.WebApi.Shared.CacheConstants;

namespace SmartDevicesNetwork.Tests.Services;

public class ActionsServiceTests
{
    // Minimal localizer that returns the resource value passed as the localized string.
    private class TestLocalizer : IStringLocalizer<ApiMessages>
    {
        public LocalizedString this[string name] => new LocalizedString(name, name);
        public LocalizedString this[string name, params object[] arguments] => new LocalizedString(name, string.Format(name, arguments));
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => Array.Empty<LocalizedString>();
        // ReSharper disable once UnusedMember.Local
        public IStringLocalizer WithCulture(System.Globalization.CultureInfo _) => this;
    }

    [Fact]
    public async Task PerformAction_ShouldReturnFailed_WhenRequestCountIsDivisibleByTen()
    {
        // Arrange device returned from repository
        var device = new DeviceDtoModel { DeviceId = 1, Name = "Device 1", Status = "Offline", Type = "Test" };

        var devicesRepositoryImposter = IDevicesRepository.Imposter();
        var actionsRepositoryImposter = IActionsRepository.Imposter();
        var networkRepositoryImposter = INetworkRepository.Imposter();

        devicesRepositoryImposter.ByIdAsync(Arg<int>.Is(1), Arg<CancellationToken>.Any()).Returns(Task.FromResult(device));
        devicesRepositoryImposter.Update(Arg<DeviceDtoModel>.Any(), Arg<string>.Any());

        actionsRepositoryImposter.Add(Arg<ActionsDtoModel>.Any());

        var imposter = IUnitOfWork.Imposter();
        imposter.DevicesRepository.Getter().Returns(devicesRepositoryImposter.Instance());
        imposter.NetworkRepository.Getter().Returns(networkRepositoryImposter.Instance());
        imposter.ActionsRepository.Getter().Returns(actionsRepositoryImposter.Instance());

        imposter.SaveChangesAsync(Arg<CancellationToken>.Any()).Returns(Task.FromResult(1));

        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var localizer = new TestLocalizer();

        var service = new ActionsService(imposter.Instance(), memoryCache, localizer);

        // Act
        var result = await service.PerformActionAsync(1, new ActionRequest(Actions.On), CancellationToken.None);

        // Assert response and side effects
        Assert.Equal("Failed", result.Status);
        Assert.Equal(ApiMessages.DeviceSwitchedOnErrorMessage, result.Message);
        Assert.Equal("Offline", device.Status);

        Assert.True(memoryCache.TryGetValue(CacheConstants.ActionRequestCountKey, out int updatedCount));
        Assert.Equal(1, updatedCount);

        // Verify repository interactions
        devicesRepositoryImposter.Update(Arg<DeviceDtoModel>.Any(), Arg<string>.Any()).Called(Count.Once());
        actionsRepositoryImposter.Add(Arg<ActionsDtoModel>.Any()).Called(Count.Once());
        imposter.SaveChangesAsync(Arg<CancellationToken>.Any()).Called(Count.Once());
    }

    [Fact]
    public async Task PerformAction_ShouldReturnFailed_WhenActionIsUnknown()
    {
        var devicesRepositoryImposter = IDevicesRepository.Imposter();
        var actionsRepositoryImposter = IActionsRepository.Imposter();
        var networkRepositoryImposter = INetworkRepository.Imposter();

        devicesRepositoryImposter.Update(Arg<DeviceDtoModel>.Any(), Arg<string>.Any());
        actionsRepositoryImposter.Add(Arg<ActionsDtoModel>.Any());

        var imposter = IUnitOfWork.Imposter();
        imposter.DevicesRepository.Getter().Returns(devicesRepositoryImposter.Instance());
        imposter.NetworkRepository.Getter().Returns(networkRepositoryImposter.Instance());
        imposter.ActionsRepository.Getter().Returns(actionsRepositoryImposter.Instance());
        imposter.SaveChangesAsync(Arg<CancellationToken>.Any()).Returns(Task.FromResult(1));

        var service = new ActionsService(imposter.Instance(), new MemoryCache(new MemoryCacheOptions()), new TestLocalizer());

        var result = await service.PerformActionAsync(1, new ActionRequest((Actions)999), CancellationToken.None);

        Assert.Equal("Failed", result.Status);
        Assert.Equal(string.Format(ApiMessages.ActionNotFoundErrorMessage, 999), result.Message);

        devicesRepositoryImposter.ByIdAsync(Arg<int>.Any(), Arg<CancellationToken>.Any()).Called(Count.Never());
        devicesRepositoryImposter.Update(Arg<DeviceDtoModel>.Any(), Arg<string>.Any()).Called(Count.Never());
        actionsRepositoryImposter.Add(Arg<ActionsDtoModel>.Any()).Called(Count.Never());
        imposter.SaveChangesAsync(Arg<CancellationToken>.Any()).Called(Count.Never());
    }

    [Fact]
    public async Task PerformAction_ShouldReturnFailed_WhenDeviceIsNotFound()
    {
        var devicesRepositoryImposter = IDevicesRepository.Imposter();
        var actionsRepositoryImposter = IActionsRepository.Imposter();
        var networkRepositoryImposter = INetworkRepository.Imposter();

        devicesRepositoryImposter.ByIdAsync(Arg<int>.Is(1), Arg<CancellationToken>.Any())
            .Returns(Task.FromResult<DeviceDtoModel>(null!));
        devicesRepositoryImposter.Update(Arg<DeviceDtoModel>.Any(), Arg<string>.Any());
        actionsRepositoryImposter.Add(Arg<ActionsDtoModel>.Any());

        var imposter = IUnitOfWork.Imposter();
        imposter.DevicesRepository.Getter().Returns(devicesRepositoryImposter.Instance());
        imposter.NetworkRepository.Getter().Returns(networkRepositoryImposter.Instance());
        imposter.ActionsRepository.Getter().Returns(actionsRepositoryImposter.Instance());
        imposter.SaveChangesAsync(Arg<CancellationToken>.Any()).Returns(Task.FromResult(1));

        var service = new ActionsService(imposter.Instance(), new MemoryCache(new MemoryCacheOptions()), new TestLocalizer());

        var result = await service.PerformActionAsync(1, new ActionRequest(Actions.On), CancellationToken.None);

        Assert.Equal("Failed", result.Status);
        Assert.Equal(ApiMessages.DeviceNotFoundErrorMessage, result.Message);

        devicesRepositoryImposter.Update(Arg<DeviceDtoModel>.Any(), Arg<string>.Any()).Called(Count.Never());
        actionsRepositoryImposter.Add(Arg<ActionsDtoModel>.Any()).Called(Count.Never());
        imposter.SaveChangesAsync(Arg<CancellationToken>.Any()).Called(Count.Never());
    }

    [Fact]
    public async Task PerformAction_ShouldReturnSuccess_WhenRequestCountIsNotDivisibleByTen()
    {
        var device = new DeviceDtoModel { DeviceId = 1, Name = "Device 1", Status = "Offline", Type = "Test" };

        var devicesRepositoryImposter = IDevicesRepository.Imposter();
        var actionsRepositoryImposter = IActionsRepository.Imposter();
        var networkRepositoryImposter = INetworkRepository.Imposter();

        devicesRepositoryImposter.ByIdAsync(Arg<int>.Is(1), Arg<CancellationToken>.Any()).Returns(Task.FromResult(device));
        devicesRepositoryImposter.Update(Arg<DeviceDtoModel>.Any(), Arg<string>.Any());
        actionsRepositoryImposter.Add(Arg<ActionsDtoModel>.Any());

        var imposter = IUnitOfWork.Imposter();
        imposter.DevicesRepository.Getter().Returns(devicesRepositoryImposter.Instance());
        imposter.NetworkRepository.Getter().Returns(networkRepositoryImposter.Instance());
        imposter.ActionsRepository.Getter().Returns(actionsRepositoryImposter.Instance());
        imposter.SaveChangesAsync(Arg<CancellationToken>.Any()).Returns(Task.FromResult(1));

        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        memoryCache.Set(CacheConstants.ActionRequestCountKey, 9);
        var service = new ActionsService(imposter.Instance(), memoryCache, new TestLocalizer());

        var result = await service.PerformActionAsync(1, new ActionRequest(Actions.On), CancellationToken.None);

        Assert.Equal("Success", result.Status);
        Assert.Equal(ApiMessages.DeviceSwitchedOnSuccessMessage, result.Message);
        Assert.Equal("Online", device.Status);

        Assert.True(memoryCache.TryGetValue(CacheConstants.ActionRequestCountKey, out int cachedCount));
        Assert.Equal(9, cachedCount);

        devicesRepositoryImposter.Update(Arg<DeviceDtoModel>.Any(), Arg<string>.Any()).Called(Count.Once());
        actionsRepositoryImposter.Add(Arg<ActionsDtoModel>.Any()).Called(Count.Once());
        imposter.SaveChangesAsync(Arg<CancellationToken>.Any()).Called(Count.Once());
    }
}