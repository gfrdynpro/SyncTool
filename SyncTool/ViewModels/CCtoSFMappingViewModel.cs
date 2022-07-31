using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SyncTool.Contracts.Services;
using SyncTool.Core.Models;

namespace SyncTool.ViewModels;
public class CCtoSFMappingViewModel : ObservableRecipient
{
    private readonly INavigationService _navService;
    private readonly IMappingService _mappingService;

    public CCtoSFMappingViewModel()
    {
        _navService = App.GetService<INavigationService>();
        _mappingService = App.GetService<IMappingService>();
        BuildFieldMappings();
    }

    private void BuildFieldMappings()
    {
        var obslist = new ObservableCollection<CCtoSFFieldMapping>();
        var list = _mappingService.GetCCtoSFMappings();
        foreach (var mapping in list) { obslist.Add(mapping); }
        CCFieldMappings = obslist;
    }

    public ObservableCollection<CCtoSFFieldMapping> CCFieldMappings
    {
        get;set;
    }
}
