// --------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  Licensed under the MIT License.
// --------------------------------------------------------------------------

using System;
using System.IO;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Extractor.Models;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.FileHandlers
{
    public class DirectoryNameGenerator
    {
        readonly string fileGenerationRootDirectory;
        readonly string versionSetMasterFolder;
        readonly string revisionMasterFolder;
        readonly string multipleApisMasterFolder;

        public DirectoryNameGenerator(ExtractorParameters extractorParameters)
        {
            if (extractorParameters == null)
            {
                throw new ArgumentNullException(nameof(extractorParameters));
            }
            this.fileGenerationRootDirectory = extractorParameters.FilesGenerationRootDirectory;
            this.versionSetMasterFolder = RemoveLeadingSlash(extractorParameters.FileNames.VersionSetMasterFolder);
            this.revisionMasterFolder = RemoveLeadingSlash(extractorParameters.FileNames.RevisionMasterFolder);
            this.multipleApisMasterFolder = RemoveLeadingSlash(extractorParameters.FileNames.GroupAPIsMasterFolder);
        }

        public string GetApiVersionAndRevisionFolder(string apiName, string versionOrRevisionName)
        {
            return Path.Combine(this.fileGenerationRootDirectory, apiName, versionOrRevisionName);
        }

        public string GetApiVersionSetMasterFolder(string apiName)
        {
            return Path.Combine(this.fileGenerationRootDirectory, apiName, this.versionSetMasterFolder);
        }

        public string GetApiRevisionMasterFolder(string apiName)
        {
            return Path.Combine(this.fileGenerationRootDirectory, apiName, this.revisionMasterFolder);
        }

        public string GetMultipleApisMasterFolder(string apiName)
        {
            return Path.Combine(this.fileGenerationRootDirectory, apiName, this.multipleApisMasterFolder);
        }

        static string RemoveLeadingSlash(string value)
        {
            return value.StartsWith('/') ? value[1..] : value;
        }
    }
}
