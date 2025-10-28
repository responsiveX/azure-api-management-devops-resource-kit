// --------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  Licensed under the MIT License.
// --------------------------------------------------------------------------

using System;
using System.IO;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.DirectoryHandlers
{
    public class DirectoryNameGenerator : IDirectoryNameGenerator
    {
        readonly string rootDirectory;
        readonly string versionSetMasterFolder;
        readonly string revisionMasterFolder;
        readonly string multipleApisMasterFolder;

        public DirectoryNameGenerator(
            string rootDirectory,
            string versionSetMasterFolder,
            string revisionMasterFolder,
            string multipleApisMasterFolder)
        {
            this.rootDirectory = rootDirectory ?? throw new ArgumentNullException(nameof(rootDirectory));
            this.versionSetMasterFolder = versionSetMasterFolder ?? throw new ArgumentNullException(nameof(versionSetMasterFolder));
            this.revisionMasterFolder = revisionMasterFolder ?? throw new ArgumentNullException(nameof(revisionMasterFolder));
            this.multipleApisMasterFolder = multipleApisMasterFolder ?? throw new ArgumentNullException(nameof(multipleApisMasterFolder));
        }

        public string GetApiVersionAndRevisionFolder(string apiName, string versionOrRevisionName)
        {
            return Path.Combine(this.rootDirectory, apiName, versionOrRevisionName);
        }

        public string GetApiVersionSetMasterFolder(string apiName)
        {
            return Path.Combine(this.rootDirectory, apiName, this.versionSetMasterFolder);
        }

        public string GetApiRevisionMasterFolder(string apiName)
        {
            return Path.Combine(this.rootDirectory, apiName, this.revisionMasterFolder);
        }

        public string GetMultipleApisMasterFolder()
        {
            return Path.Combine(this.rootDirectory, this.multipleApisMasterFolder);
        }
    }
}
