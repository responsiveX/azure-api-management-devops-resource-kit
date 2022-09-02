// --------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  Licensed under the MIT License.
// --------------------------------------------------------------------------

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.DirectoryHandlers
{
    public interface IDirectoryNameGenerator
    {
        string GetApiRevisionMasterFolder(string apiName);
        string GetApiVersionAndRevisionFolder(string apiName, string versionOrRevisionName);
        string GetApiVersionSetMasterFolder(string apiName);
        string GetMultipleApisMasterFolder();
    }
}