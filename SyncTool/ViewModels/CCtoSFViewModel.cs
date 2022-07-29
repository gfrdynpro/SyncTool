using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SyncTool.Contracts.Services;
using SyncTool.Core.Models.CC;
using SyncTool.Models;

namespace SyncTool.ViewModels;
public class CCtoSFViewModel : ObservableRecipient
{
    private readonly IConstantContactClientService _ccService;

    public CCtoSFViewModel()
    {
        _ccService = App.GetService<IConstantContactClientService>();
        LoadCampaigns();
    }

    private async void LoadCampaigns()
    {
        var token = await _ccService.RefreshAccessTokenAsync();
        if (token != null)
        {
            var campaigns = await _ccService.GetCampaignsAsync();
            var list = new ObservableCollection<Campaign>();
            foreach (var item in campaigns.Campaigns)
            {
                list.Add(item);
            }
            CampaignList = list;
        }
    }

    public ObservableCollection<Campaign> CampaignList
    {
        get; set;
    }
}
