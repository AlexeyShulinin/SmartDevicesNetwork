using System.Collections.Generic;

namespace SmartDevicesNetwork.WebApi.Options;

public class CultureOptions
{
    public IList<string> SupportedCultures { get; set; }
    public string DefaultCulture { get; set; }
}