namespace SmartDevicesNetwork.WebApi.Repositories.Models;

public class NetworkLinkDtoModel
{
    public int LinkId { get; set; }
    public int SourceId { get; set; }
    public int TargetId { get; set; }
    public string LinkType { get; set; }
}