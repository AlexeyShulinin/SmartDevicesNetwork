using System.Collections.Generic;

namespace SmartDevicesNetwork.WebApi.Models.Responses;

public record NetworkResponse(IEnumerable<NetworkNodeResponse> Nodes, IEnumerable<NetworkLinkResponse> Links);