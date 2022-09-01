using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SyncTool.Core.Models.CC;

namespace SyncTool.ViewModels;
public partial class CCTrackingActivity : ObservableRecipient
{
    [ObservableProperty]
    private string firstName;
    [ObservableProperty]
    private string lastName;
    [ObservableProperty]
    private string emailAddress;
    [ObservableProperty]
    private string trackingActivityType;
    [ObservableProperty]
    private bool sent;

    public TrackingActivity CCObject;
}
