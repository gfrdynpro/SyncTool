using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using SyncTool.Contracts.Services;

namespace SyncTool.ViewModels;
public class CCtoSFMappingViewModel : ObservableRecipient
{
    private readonly INavigationService _navService;

    public CCtoSFMappingViewModel()
    {
        _navService = App.GetService<INavigationService>();
    }
}
