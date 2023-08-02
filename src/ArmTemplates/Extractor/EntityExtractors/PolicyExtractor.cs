// --------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  Licensed under the MIT License.
// --------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.FileHandlers;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Extractor.EntityExtractors.Abstractions;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.Templates.Abstractions;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.Templates.Policy;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.Constants;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Extractor.Models;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.API.Clients.Abstractions;
using System.IO;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.Templates.Builders.Abstractions;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.Extensions;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.Templates.Products;
using System.Linq;
using System.Reflection;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Extractor.EntityExtractors
{
    public class PolicyExtractor : IPolicyExtractor
    {
        public const string EmptyPolicyFileResourcePath = "Microsoft.Azure.Management.ApiManagement.ArmTemplates.Resources.EmptyPolicyFile.xml";
        public const string PoliciesDirectoryName = "policies";
        public const string OperationPoliciesDirectoryName = "operations";
        public const string ProductPoliciesDirectoryName = "products";
        public const string GlobalServicePolicyFileName = "globalService.xml";

        public const string ProductPolicyFileNameFormat = "{0}.xml";
        public const string ApiPolicyFileNameFormat = "{0}.xml";
        public const string ApiOperationPolicyFileNameFormat =  "{0}-{1}.xml";

        readonly ILogger<PolicyExtractor> logger;
        readonly IPolicyClient policyClient;
        readonly ITemplateBuilder templateBuilder;

        readonly IDictionary<string, string> policyPathToContentCache = new Dictionary<string, string>();

        public PolicyExtractor(
            ILogger<PolicyExtractor> logger,
            IPolicyClient policyApiClient,
            ITemplateBuilder templateBuilder)
        {
            this.logger = logger;
            this.policyClient = policyApiClient;
            this.templateBuilder = templateBuilder;
        }

        public string GetCachedPolicyContent(PolicyTemplateResource policyTemplateResource, string baseFilesGenerationDirectory)
        {
            if (policyTemplateResource?.Properties is null)
            {
                this.logger.LogWarning("Policy was not initialized correctly {0}", policyTemplateResource.Name);
                return string.Empty;
            }

            // easy code flow, if we already have a policy-content-file-full-path to use as a key for policy content caches
            if (!string.IsNullOrEmpty(policyTemplateResource.Properties.PolicyContentFileFullPath))
            {
                if (this.policyPathToContentCache.ContainsKey(policyTemplateResource.Properties.PolicyContentFileFullPath))
                {
                    return this.policyPathToContentCache[policyTemplateResource.Properties.PolicyContentFileFullPath];
                }
                else
                {
                    this.logger.LogWarning("Policy content at '{0}' was initialized, but wasn't cached properly", policyTemplateResource.Properties.PolicyContentFileFullPath);
                }
            }

            // if no path found already, trying to get one from policy content
            // check if this is a file or is it the raw policy content
            var policyContent = policyTemplateResource?.Properties?.PolicyContent ?? string.Empty;
            if (policyContent.Contains(".xml"))
            {
                var fileNameSection = policyContent.Split(',')[1];
                var policyFileName = NamingHelper.GetSubstringBetweenTwoCharacters('\'', '\'', fileNameSection);
                var policyFileFullPath = Path.Combine(baseFilesGenerationDirectory, PoliciesDirectoryName, policyFileName);

                if (File.Exists(policyFileFullPath))
                {
                    var policyContentFromFile = File.ReadAllText(policyFileFullPath);

                    policyTemplateResource.Properties.PolicyContentFileFullPath = policyFileFullPath;
                    this.policyPathToContentCache[policyFileFullPath] = policyContentFromFile;

                    return policyContentFromFile;
                }
            }

            return policyContent;
        }

        public async Task<PolicyTemplateResource> GenerateApiOperationPolicyResourceAsync(
            string apiName,
            string operationName,
            string baseFilesGenerationDirectory,
            ExtractorParameters extractorParameters)
        {
            var apiOperationPolicy = await this.policyClient.GetPolicyLinkedToApiOperationAsync(apiName, operationName, extractorParameters);

            if (apiOperationPolicy is null)
            {
                this.logger.LogWarning("Policy for api '{0}' and operation '{1}' not found", apiName, operationName);

                if (extractorParameters.GenerateEmptyPolicyFiles)
                {
                    apiOperationPolicy = new PolicyTemplateResource()
                    {
                        Properties = new PolicyTemplateProperties()
                        {
                            Format = "xml",
                            PolicyContent = GetEmptyPolicyFileContents()
                        },
                        Type = "Microsoft.ApiManagement/service/apis/operations/policies"
                    };
                }
            }

            apiOperationPolicy.Name = $"[concat(parameters('{ParameterNames.ApimServiceName}'), '/{apiName}/{operationName}/policy')]";
            apiOperationPolicy.ApiVersion = GlobalConstants.ApiVersion;
            apiOperationPolicy.Scale = null;
            apiOperationPolicy.DependsOn = new string[] { $"[resourceId('Microsoft.ApiManagement/service/apis/operations', parameters('{ParameterNames.ApimServiceName}'), '{apiName}', '{operationName}')]" };

            // write policy xml content to file and point to it if policyXMLBaseUrl is provided
            if (extractorParameters.PolicyXMLBaseUrl is not null)
            {
                var policyDirectoryName = Path.Combine(baseFilesGenerationDirectory, PoliciesDirectoryName, OperationPoliciesDirectoryName);
                var policyFileName = string.Format(ApiOperationPolicyFileNameFormat, apiName, operationName);
                await this.SavePolicyXmlAsync(apiOperationPolicy, policyDirectoryName, policyFileName);

                this.SetPolicyTemplateResourcePolicyContentWithArmPresetValues(extractorParameters, apiOperationPolicy, OperationPoliciesDirectoryName + "/" + policyFileName);
            }

            return apiOperationPolicy;
        }

        public async Task<PolicyTemplateResource> GenerateApiPolicyResourceAsync(
            string apiName, 
            string baseFilesGenerationDirectory,
            ExtractorParameters extractorParameters)
        {
            var apiPolicy = await this.policyClient.GetPolicyLinkedToApiAsync(apiName, extractorParameters);

            if (apiPolicy is null)
            {
                this.logger.LogWarning("Policy for api {0} not found", apiName);

                if (extractorParameters.GenerateEmptyPolicyFiles)
                {
                    apiPolicy = new PolicyTemplateResource()
                    {
                        Properties = new PolicyTemplateProperties()
                        {
                            Format = "xml",
                            PolicyContent = GetEmptyPolicyFileContents()
                        },
                        Type = "Microsoft.ApiManagement/service/apis/policies",
                        Name = "policy"
                    };
                }
            }

            var apiPolicyOriginalName = apiPolicy.Name;

            apiPolicy.ApiVersion = GlobalConstants.ApiVersion;
            apiPolicy.Name = $"[concat(parameters('{ParameterNames.ApimServiceName}'), '/{apiName}/{apiPolicyOriginalName}')]";
            apiPolicy.DependsOn = new string[] { NamingHelper.GenerateApisResourceId(apiName) };

            // write policy xml content to file and point to it if policyXMLBaseUrl is provided
            if (extractorParameters.PolicyXMLBaseUrl is not null)
            {
                var policyDirectoryName = Path.Combine(baseFilesGenerationDirectory, PoliciesDirectoryName);
                var policyFileName = string.Format(ApiPolicyFileNameFormat, apiName);
                await this.SavePolicyXmlAsync(apiPolicy, policyDirectoryName, policyFileName);

                this.SetPolicyTemplateResourcePolicyContentWithArmPresetValues(extractorParameters, apiPolicy, policyFileName);
            }

            return apiPolicy;
        }

        public async Task<PolicyTemplateResource> GenerateProductPolicyTemplateAsync(
            ExtractorParameters extractorParameters,
            ProductsTemplateResource productTemplateResource,
            string[] productResourceId,
            string baseFilesGenerationDirectory)
        {
            var productPolicy = await this.policyClient.GetPolicyLinkedToProductAsync(productTemplateResource.OriginalName, extractorParameters);

            if (productPolicy is null)
            {
                this.logger.LogWarning($"Policy not found for product '{productTemplateResource.OriginalName}'");

                if (extractorParameters.GenerateEmptyPolicyFiles)
                {
                    productPolicy = new PolicyTemplateResource()
                    {
                        Properties = new PolicyTemplateProperties()
                        {
                            Format = "xml",
                            PolicyContent = GetEmptyPolicyFileContents()
                        },
                        Type = "Microsoft.ApiManagement/service/products/policies"
                    };
                }
            }

            this.logger.LogDebug($"Policy linked to {productTemplateResource.OriginalName} product found successfuly");

            productPolicy.Name = $"[concat(parameters('{ParameterNames.ApimServiceName}'), '/{productTemplateResource.NewName}/policy')]";
            productPolicy.ApiVersion = GlobalConstants.ApiVersion;
            productPolicy.Scale = null;
            productPolicy.DependsOn = productResourceId;
            
            // due to legacy reasons, providing empty `policyXmlBaseUrl = ""` is recognized as a boolean parameter 
            // telling that it is needed to provide a policy Xml file
            if (extractorParameters.PolicyXMLBaseUrl is not null)
            {
                var policyDirectoryName = Path.Combine(baseFilesGenerationDirectory, PoliciesDirectoryName, ProductPoliciesDirectoryName);
                var policyFileName = string.Format(ProductPolicyFileNameFormat, productTemplateResource.NewName);
                await this.SavePolicyXmlAsync(productPolicy, policyDirectoryName, policyFileName);

                this.SetPolicyTemplateResourcePolicyContentWithArmPresetValues(extractorParameters, productPolicy, ProductPoliciesDirectoryName + "/" + policyFileName);
            }

            return productPolicy;
        }

        public async Task<Template<PolicyTemplateResources>> GenerateGlobalServicePolicyTemplateAsync(ExtractorParameters extractorParameters, string baseFilesDirectory)
        {
            // extract global service policy in both full and single api extraction cases
            var policyTemplate = this.templateBuilder.GenerateTemplateWithApimServiceNameProperty()
                                                       .AddPolicyProperties(extractorParameters)
                                                       .Build<PolicyTemplateResources>();

            // add global service policy resource to template
            try
            {
                var globalServicePolicyResource = await this.policyClient.GetGlobalServicePolicyAsync(extractorParameters);

                Logger.LogInformation($"Global policy found for {extractorParameters.SourceApimName} API Management service");

                // REST API will return format property as rawxml and value property as the xml by default
                globalServicePolicyResource.Name = $"[concat(parameters('{ParameterNames.ApimServiceName}'), '/policy')]";
                globalServicePolicyResource.ApiVersion = GlobalConstants.ApiVersion;
                globalServicePolicyResource.Scale = null;

                // due to legacy reasons, providing empty `policyXmlBaseUrl = ""` is recognized as a boolean parameter 
                // telling that it is needed to provide a policy Xml file
                if (extractorParameters.PolicyXMLBaseUrl is not null)
                {
                    var policyDirectoryName = Path.Combine(baseFilesDirectory, PoliciesDirectoryName);

                    // writing to globalServicePolicy.xml (<files-root>/policies/globalServicePolicy.xml)
                    await this.SavePolicyXmlAsync(globalServicePolicyResource, policyDirectoryName, GlobalServicePolicyFileName);

                    this.SetPolicyTemplateResourcePolicyContentWithArmPresetValues(extractorParameters, globalServicePolicyResource, GlobalServicePolicyFileName);
                }

                policyTemplate.TypedResources.GlobalServicePolicy = globalServicePolicyResource;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Exception occured while global apim service policy generation");
            }

            return policyTemplate;
        }

        static string GetEmptyPolicyFileContents()
        {   var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(EmptyPolicyFileResourcePath))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        void SetPolicyTemplateResourcePolicyContentWithArmPresetValues(
            ExtractorParameters extractorParameters,
            PolicyTemplateResource policyTemplate,
            string policyFileName)
        {
            policyTemplate.Properties.Format = "rawxml-link";
            if (extractorParameters.PolicyXMLSasToken != null)
            {
                policyTemplate.Properties.PolicyContent = $"[concat(parameters('{ParameterNames.PolicyXMLBaseUrl}'),'{policyFileName}', parameters('{ParameterNames.PolicyXMLSasToken}'))]";
            }
            else
            {
                policyTemplate.Properties.PolicyContent = $"[concat(parameters('{ParameterNames.PolicyXMLBaseUrl}'),'{policyFileName}')]";
            }
        }

        async Task SavePolicyXmlAsync(
            PolicyTemplateResource policyTemplateResource,
            string baseFilesDirectory,
            string policyFileName)
        {
            //var policiesDirectory = Path.Combine(baseFilesDirectory, policiesDirectoryName);
            var fullPolicyPathWithName = Path.Combine(baseFilesDirectory, policyFileName);
            FileWriter.CreateFolderIfNotExists(baseFilesDirectory); // creating <files-root>/policies

            var policyXMLContent = policyTemplateResource.Properties.PolicyContent;

            if (this.policyPathToContentCache.ContainsKey(fullPolicyPathWithName))
            {
                this.logger.LogError("Policy content already exists in {0} and will be overwritten!", fullPolicyPathWithName);
            }

            // saving to cache + saving path for easily extraction
            this.policyPathToContentCache[fullPolicyPathWithName] = policyXMLContent;
            policyTemplateResource.Properties.PolicyContentFileFullPath = fullPolicyPathWithName;

            // writing <files-root>/policies/<policyFileName>.xml
            await FileWriter.SaveTextToFileAsync(policyXMLContent, fullPolicyPathWithName);
        }
    }
}
