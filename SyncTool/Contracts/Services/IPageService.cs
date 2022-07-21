using System;

namespace SyncTool.Contracts.Services;

public interface IPageService
{
    Type GetPageType(string key);
}
