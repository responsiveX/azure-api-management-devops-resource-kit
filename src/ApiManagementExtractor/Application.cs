// --------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  Licensed under the MIT License.
// --------------------------------------------------------------------------

using System.Reflection;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates
{
    class Application
    {
        public const string Name = "API Management Extractor";

        public static readonly string BuildVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
    }
}
