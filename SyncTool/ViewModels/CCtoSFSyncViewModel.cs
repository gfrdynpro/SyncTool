using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SyncTool.Contracts.Services;
using SyncTool.Contracts.ViewModels;
using SyncTool.Core.Models.CC;
using Windows.ApplicationModel.Appointments;

namespace SyncTool.ViewModels;
public partial class CCtoSFSyncViewModel : ObservableRecipient, INavigationAware
{
    private readonly IConstantContactClientService _ccService;

    public CCtoSFSyncViewModel()
    {
        Debug.WriteLine("CC to SF Sync View Model Loaded");
        _ccService = App.GetService<IConstantContactClientService>();
        TrackingActivities = new ObservableCollection<TrackingActivity>();
    }

    public void OnNavigatedFrom()
    {
    }

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is Campaign) 
        {
            ShowBusy = true;
            Debug.WriteLine("We have a winner.");
            var campaign = (Campaign)parameter;
            SourceCampaign = campaign;
            Debug.WriteLine(SourceCampaign.Name);
            var details = await _ccService.GetCampaignDetailsAsync(campaign.CampaignId);
            var emailActivity = details.CampaignActivities.Where(c => c.Role == "primary_email").FirstOrDefault();
            if (emailActivity != null)
            {
                var activities = await _ccService.GetAllTrackingActivitiesAsync(emailActivity.CampaignActivityId);
                TrackingActivities.Clear();
                foreach (var item in activities)
                {
                    TrackingActivities.Add(item);
                }
            }
            ShowBusy = false;
        }
    }

    [ObservableProperty]
    private Campaign sourceCampaign;

    [ObservableProperty]
    private ObservableCollection<TrackingActivity> trackingActivities;

    [ObservableProperty]
    private bool showBusy;
}
