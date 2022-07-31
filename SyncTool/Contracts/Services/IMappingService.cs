using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncTool.Core.Models;

namespace SyncTool.Contracts.Services;
public interface IMappingService
{
    List<CCtoSFFieldMapping> GetCCtoSFMappings();
}
