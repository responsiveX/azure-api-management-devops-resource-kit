// --------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  Licensed under the MIT License.
// --------------------------------------------------------------------------

using System;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Extractor.Models;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.DirectoryHandlers
{
    public class DirectoryNameGeneratorFactory : IDirectoryNameGeneratorFactory
    {
        public IDirectoryNameGenerator GetDirectoryNameGenerator(ExtractorParameters extractorParameters)
        {
            if (extractorParameters == null)
            {
                throw new ArgumentNullException(nameof(extractorParameters));
            }

            return new DirectoryNameGenerator(
                extractorParameters.FilesGenerationRootDirectory,
                RemoveLeadingSlash(extractorParameters.FileNames.VersionSetMasterFolder),
                RemoveLeadingSlash(extractorParameters.FileNames.RevisionMasterFolder),
                RemoveLeadingSlash(extractorParameters.FileNames.GroupAPIsMasterFolder)
                );
        }

        static string RemoveLeadingSlash(string value)
        {
            return value.StartsWith('/') ? value[1..] : value;
        }
    }
}