// --------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  Licensed under the MIT License.
// --------------------------------------------------------------------------

using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Extractor.Models;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.DirectoryHandlers
{
    public interface IDirectoryNameGeneratorFactory
    {
        IDirectoryNameGenerator GetDirectoryNameGenerator(ExtractorParameters extractorParameters);
    }
}