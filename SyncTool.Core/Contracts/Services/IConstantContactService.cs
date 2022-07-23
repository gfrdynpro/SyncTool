using System;
using System.Collections.Generic;
using System.Text;

namespace SyncTool.Core.Contracts.Services;
internal interface IConstantContactService
{
    bool Login(string APIkey);
}
