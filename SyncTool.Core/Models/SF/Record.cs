using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace SyncTool.Core.Models.SF;
public partial class Record
{
    [JsonProperty("attributes")]
    public Attributes Attributes
    {
        get; set;
    }

    [JsonProperty("Id")]
    public string Id
    {
        get; set;
    }

    [JsonProperty("Name")]
    public string Name
    {
        get; set;
    }
}
