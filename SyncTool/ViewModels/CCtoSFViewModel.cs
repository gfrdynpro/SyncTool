using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SyncTool.Contracts.Services;
using SyncTool.Core.Models.CC;
using SyncTool.Models;

namespace SyncTool.ViewModels;
public class CCtoSFViewModel : ObservableRecipient
{
    private readonly IConstantContactClientService _ccService;
    private readonly INavigationService _navService;

    public CCtoSFViewModel()
    {
        RunConfigureMappingsCommand = new RelayCommand(RunConfigureMappings);
        _ccService = App.GetService<IConstantContactClientService>();
        _navService = App.GetService<INavigationService>();
        LoadCampaigns();
    }

    public ICommand RunConfigureMappingsCommand { get; }

    private void RunConfigureMappings()
    {
        _navService.NavigateTo("SyncTool.ViewModels.CCtoSFMappingViewModel");
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
