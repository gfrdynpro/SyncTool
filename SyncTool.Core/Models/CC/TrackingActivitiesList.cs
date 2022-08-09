using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SyncTool.Core.Models.CC;
public partial class TrackingActivitiesList
{
    [JsonProperty("tracking_activities")]
    public TrackingActivity[] TrackingActivities
    {
        get; set;
    }

    [JsonProperty("_links")]
    public Links Links
    {
        get; set;
    }
}
