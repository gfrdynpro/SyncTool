using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyncTool.Contracts.Services;
using SyncTool.Contracts.ViewModels;
using SyncTool.Core.Models.CC;
using SyncTool.Core.Models.SF;
using Windows.ApplicationModel.Appointments;

namespace SyncTool.ViewModels;
public partial class CCtoSFSyncViewModel : ObservableRecipient, INavigationAware
{
    private bool _syncInProgress = false;
    private readonly IConstantContactClientService _ccService;
    private readonly ISalesforceClientService _sfService;

    public CCtoSFSyncViewModel()
    {
        Debug.WriteLine("CC to SF Sync View Model Loaded");
        StartSyncCommand = new RelayCommand(StartSync);
        _ccService = App.GetService<IConstantContactClientService>();
        _sfService = App.GetService<ISalesforceClientService>();
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

    public ICommand StartSyncCommand
    {
        get;
    }

    private async void StartSync()
    {
        if (_syncInProgress)
            return;
        _syncInProgress = true;
        for (var i = 0; i < TrackingActivities.Count; i++)
        {
            var activity = TrackingActivities[i];
            CurrentContact = i;
            Debug.WriteLine(activity.EmailAddress);
            var success = await SyncContactToSFAsync(activity);
            if (success)
            {
                activity.Sent = true;
            }
        }
        _syncInProgress = false;
    }

    private async Task<bool> SyncContactToSFAsync(TrackingActivity activity)
    {
        // Find Lead by email address
        var sfLead = await _sfService.GetLeadByEmailAddressAsync(activity.EmailAddress);
        if (sfLead != null && sfLead.Attributes.Type == "Lead") 
        {
            // Map data elements
            var payload = MapCCtoSF(activity, sfLead);
            // Update record
            var success = await _sfService.UpdateLeadRecordAsync(sfLead, payload);
            if (success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private Dictionary<string, object> MapCCtoSF(TrackingActivity ccActivity, Record sfLead)
    {
        var payload = new Dictionary<string, object>();
        var status = "";
        var reason = "";
        var opt_out = false;
        switch (ccActivity.TrackingActivityType)
        {
            case "em_opens":
                status = "Opens";
                break;
            case "em_sends":
                status = "Did not open";
                break;
            case "em_clicks":
                status = "Click";
                break;
            case "em_optouts":
                status = "Unsubscribed";
                reason = ccActivity.OptOutReason;
                opt_out = true;
                break;
            case "em_bounces":
                status = "Bounced";
                switch (ccActivity.BounceCode)
                {
                    case "B":
                        reason = "Non-Existent";
                        break;
                    case "D":
                        reason = "Undeliverable";
                        break;
                    case "F":
                        reason = "Mailbox Full";
                        break;
                    case "S":
                        reason = "Suspended";
                        break;
                    case "V":
                        reason = "";
                        break;
                    case "X":
                        reason = "Other reasons";
                        break;
                    case "Z":
                        reason = "Blocked";
                        break;
                }
                break;
        }
        payload.Add("Constant_Contact_Status__c", status);
        payload.Add("Constant_Contact_Reason__c", reason);
        payload.Add("HasOptedOutOfEmail", opt_out);
        payload.Add("Not_to_Include_in_Any_Campaign__c", opt_out);
        return payload;
    }

    [ObservableProperty]
    private Campaign sourceCampaign;

    [ObservableProperty]
    private ObservableCollection<TrackingActivity> trackingActivities;

    [ObservableProperty]
    private bool showBusy;

    [ObservableProperty]
    private int currentContact;
}
