// --------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  Licensed under the MIT License.
// --------------------------------------------------------------------------

using System;
using System.IO;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Extractor.Models;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.DirectoryHandlers
{
    public class DirectoryNameGenerator : IDirectoryNameGenerator
    {
        readonly string apiBaseDirectory;
        readonly string versionSetMasterFolder;
        readonly string revisionMasterFolder;
        readonly string multipleApisMasterFolder;

        public DirectoryNameGenerator(
            string apiBaseDirectory,
            string versionSetMasterFolder,
            string revisionMasterFolder,
            string multipleApisMasterFolder)
        {
            this.apiBaseDirectory = apiBaseDirectory ?? throw new ArgumentNullException(nameof(apiBaseDirectory));
            this.versionSetMasterFolder = versionSetMasterFolder ?? throw new ArgumentNullException(nameof(versionSetMasterFolder));
            this.revisionMasterFolder = revisionMasterFolder ?? throw new ArgumentNullException(nameof(revisionMasterFolder));
            this.multipleApisMasterFolder = multipleApisMasterFolder ?? throw new ArgumentNullException(nameof(multipleApisMasterFolder));
        }

        public string GetApiVersionAndRevisionFolder(string apiName, string versionOrRevisionName)
        {
            return Path.Combine(this.apiBaseDirectory, apiName, versionOrRevisionName);
        }

        public string GetApiVersionSetMasterFolder(string apiName)
        {
            return Path.Combine(this.apiBaseDirectory, apiName, this.versionSetMasterFolder);
        }

        public string GetApiRevisionMasterFolder(string apiName)
        {
            return Path.Combine(this.apiBaseDirectory, apiName, this.revisionMasterFolder);
        }

        public string GetMultipleApisMasterFolder()
        {
            return Path.Combine(this.apiBaseDirectory, this.multipleApisMasterFolder);
        }
    }
}
