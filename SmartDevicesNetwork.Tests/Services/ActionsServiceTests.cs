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

namespace SmartDevicesNetwork.Tests.Services;

public class ActionsServiceTests
{
    // Minimal localizer that returns the resource value passed as the localized string.
    private class TestLocalizer : IStringLocalizer<ApiMessages>
    {
        public LocalizedString this[string name] => new LocalizedString(name, name);
        public LocalizedString this[string name, params object[] arguments] => new LocalizedString(name, string.Format(name, arguments));
        public System.Collections.Generic.IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) => System.Array.Empty<LocalizedString>();
        public IStringLocalizer WithCulture(System.Globalization.CultureInfo culture) => this;
    }

    [Fact]
    public async Task PerformActionTestShouldReturnFailedStatus()
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

        // Act: perform On action (note: method includes a short Task.Delay ~5s)
        var result = await service.PerformActionAsync(1, new ActionRequest(Actions.On), CancellationToken.None);

        // Assert response
        Assert.Equal("Failed", result.Status);
        Assert.Equal(ApiMessages.DeviceSwitchedOnErrorMessage, result.Message);

        // Verify repository interactions
        devicesRepositoryImposter.Update(Arg<DeviceDtoModel>.Any(), Arg<string>.Any()).Called(Count.Once());
        actionsRepositoryImposter.Add(Arg<ActionsDtoModel>.Any()).Called(Count.Once());
        imposter.SaveChangesAsync(Arg<CancellationToken>.Any()).Called(Count.Once());
    }
}