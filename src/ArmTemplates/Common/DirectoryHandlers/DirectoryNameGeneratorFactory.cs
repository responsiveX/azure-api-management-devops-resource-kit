// --------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  Licensed under the MIT License.
// --------------------------------------------------------------------------

using System;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Extractor.Models;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.DirectoryHandlers
{
    public static class DirectoryNameGeneratorFactory
    {
        public static IDirectoryNameGenerator GetDirectoryNameGenerator(ExtractorParameters extractorParameters)
        {
            if (extractorParameters == null)
            {
                throw new ArgumentNullException(nameof(extractorParameters));
            }

            return new DirectoryNameGenerator(
                extractorParameters.GlobalFileRootDirectory,
                extractorParameters.ApiFileRootDirectory,
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