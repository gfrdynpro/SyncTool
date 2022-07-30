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

    public CCtoSFMappingViewModel()
    {
        _navService = App.GetService<INavigationService>();
        BuildFieldMappings();
    }

    private void BuildFieldMappings()
    {
        var list = new ObservableCollection<CCtoSFFieldMapping>();
        list.Add(new CCtoSFFieldMapping { CCField = "email_address", SFField = "email" });
        list.Add(new CCtoSFFieldMapping { CCField = "permission_to_send", SFField = "opt_out" });
        CCFieldMappings = list;
    }

    public ObservableCollection<CCtoSFFieldMapping> CCFieldMappings
    {
        get;set;
    }
}
