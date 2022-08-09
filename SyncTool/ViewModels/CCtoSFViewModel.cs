using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using SyncTool.Contracts.Services;
using SyncTool.Contracts.ViewModels;
using SyncTool.Core.Models.CC;
using SyncTool.Models;

namespace SyncTool.ViewModels;
public partial class CCtoSFViewModel : ObservableRecipient, INavigationAware
{
    private readonly IConstantContactClientService _ccService;
    private readonly INavigationService _navService;

    public CCtoSFViewModel()
    {
        RunConfigureMappingsCommand = new RelayCommand(RunConfigureMappings);
        _ccService = App.GetService<IConstantContactClientService>();
        _navService = App.GetService<INavigationService>();
        CampaignList = new ObservableCollection<Campaign>();
        LoadCampaigns();
    }

    public ICommand RunConfigureMappingsCommand { get; }

    private void RunConfigureMappings()
    {
        _navService.NavigateTo("SyncTool.ViewModels.CCtoSFMappingViewModel");
    }

    private async void LoadCampaigns()
    {
        ShowBusy = true;
        var token = await _ccService.RefreshAccessTokenAsync();
        if (token != null)
        {
            var campaigns = await _ccService.GetCampaignsAsync();
            CampaignList.Clear();
            foreach (var item in campaigns)
            {
                CampaignList.Add(item);
            }
        }
        ShowBusy = false;
    }

    public void CampaignSelected(object sender, ItemClickEventArgs e)
    {
        var clickedItem = (Campaign)e.ClickedItem;
        _navService.NavigateTo("SyncTool.ViewModels.CCtoSFSyncViewModel", clickedItem);
    }

    public void OnNavigatedTo(object parameter)
    {
    }

    public void OnNavigatedFrom()
    {
    }

    [ObservableProperty]
    private ObservableCollection<Campaign> campaignList;

    [ObservableProperty]
    private bool showBusy;
}
