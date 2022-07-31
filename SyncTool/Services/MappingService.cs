using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncTool.Contracts.Services;
using SyncTool.Core.Models;

namespace SyncTool.Services;
public class MappingService : IMappingService
{
    private readonly List<CCtoSFFieldMapping> _cctosfmappings = new();
    private readonly List<SFtoCCFieldMapping> _sftoccmappings = new();

    public MappingService()
    {
        LoadMappingLists();
    }

    private void LoadMappingLists()
    {
        _cctosfmappings.Add(new CCtoSFFieldMapping { CCField = "email_address", SFField = "email" });
        _cctosfmappings.Add(new CCtoSFFieldMapping { CCField = "permission_to_send", SFField = "opt_out" });
    }

    public List<CCtoSFFieldMapping> GetCCtoSFMappings()
    {
        return _cctosfmappings;
    }

    public List<SFtoCCFieldMapping> GetSFtoCCMappings()
    {
        return _sftoccmappings;
    }
}
