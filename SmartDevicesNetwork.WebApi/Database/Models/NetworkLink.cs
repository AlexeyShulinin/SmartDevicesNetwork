namespace SmartDevicesNetwork.WebApi.Database.Models;

public class NetworkLink
{
    public int LinkId { get; set; }
    public int SourceId { get; set; }
    public int TargetId { get; set; }
    public string LinkType { get; set; }

    public Device SourceDevice { get; set; }
    public Device TargetDevice { get; set; }
}