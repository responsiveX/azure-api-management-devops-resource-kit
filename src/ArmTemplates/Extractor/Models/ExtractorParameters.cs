﻿// --------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
//  Licensed under the MIT License.
// --------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Commands.Configurations;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.Extensions;
using Microsoft.Azure.Management.ApiManagement.ArmTemplates.Common.FileHandlers;

namespace Microsoft.Azure.Management.ApiManagement.ArmTemplates.Extractor.Models
{
    public record ExtractorParameters
    {
        
        public string SourceApimName { get; private set; }

        public string DestinationApimName { get; private set; }

        public string ResourceGroup { get; private set; }

        /// <summary>
        /// Naming of all templates from the base folder requested by end-user in extractor-configuration parameters
        /// </summary>
        public FileNames FileNames { get; private set; }

        /// <summary>
        /// The root directory, where templates will be generated.
        /// </summary>
        public string FilesGenerationRootDirectory { get; private set; }

        /// <summary>
        /// Name of a single API that user wants to extract
        /// </summary>
        public string SingleApiName { get; private set; }

        /// <summary>
        /// Names of APIs that user wants to extract
        /// </summary>
        public List<string> MultipleApiNames { get; private set; }

        /// <summary>
        /// Create separate api templates for every API in the source APIM instance
        /// </summary>
        public bool SplitApis { get; private set; }

        /// <summary>
        /// Are revisions included to templates
        /// </summary>
        public bool IncludeAllRevisions { get; private set; }

        public bool GenerateParameterFiles { get; private set; }

        public bool GenerateGlobalTemplates { get; private set; }

        public bool GenerateProductGroupTemplates { get; private set; }

        public bool GenerateApiTemplates { get; private set; }

        public bool GenerateEmptyPolicyFiles { get; private set; }

        public bool GenerateApiManagementServiceTemplate { get; private set; }

        public bool GenerateRevisionMasterTemplates { get; private set; }

        public bool GenerateVersionSetMasterTemplates { get; private set; }

        public bool GenerateReleaseTemplates { get; private set; }

        public bool GenerateLoggerTemplates { get; private set; }

        public bool GenerateNamedValueTemplates { get; private set; }

        public string LinkedTemplatesBaseUrl { get; private set; }

        public string LinkedTemplatesSasToken { get; private set; }

        public string LinkedTemplatesUrlQueryString { get; private set; }

        public string PolicyXMLBaseUrl { get; private set; }

        public string PolicyXMLSasToken { get; private set; }

        public string ApiVersionSetName { get; private set; }

        public bool ParameterizeServiceUrl { get; private set; }

        public bool ParameterizeNamedValue { get; private set; }

        public bool ParameterizeApiLoggerId { get; private set; }

        public bool ParameterizeLogResourceId { get; private set; }

        public bool NotIncludeNamedValue { get; private set; }

        public bool ParamNamedValuesKeyVaultSecrets { get; private set; }

        public int OperationBatchSize { get; private set; }

        public bool ParameterizeBackend { get; private set; }

        public bool ExtractGateways { get; set; }

        public bool OverrideGroupGuids { get; set; }

        public bool OverrideProductGuids { get; set; }

        public bool ParametrizeApiOauth2Scope { get; set; }

        public bool ExtractSecrets { get; set; }

        public bool ExtractIdentityProviders { get; set; }

        public bool ExcludeBuildInGroups { get; set; }

        public string ParametersOutputDirectoryName { get; set; }

        public Dictionary<string, ApiParameterProperty> ApiParameters { get; private set; }

        public ExtractorParameters(ExtractorConsoleAppConfiguration extractorConfig)
        {
            this.SourceApimName = extractorConfig.SourceApimName;
            this.DestinationApimName = extractorConfig.DestinationApimName;
            this.ResourceGroup = extractorConfig.ResourceGroup;
            this.FilesGenerationRootDirectory = extractorConfig.FileFolder;
            this.SingleApiName = extractorConfig.ApiName;
            this.GenerateParameterFiles = string.IsNullOrEmpty(extractorConfig.GenerateParameterFiles) || extractorConfig.GenerateParameterFiles.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.GenerateGlobalTemplates = string.IsNullOrEmpty(extractorConfig.GenerateGlobalTemplates) || extractorConfig.GenerateGlobalTemplates.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.GenerateProductGroupTemplates = string.IsNullOrEmpty(extractorConfig.GenerateProductGroupTemplates) || extractorConfig.GenerateProductGroupTemplates.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.GenerateApiTemplates = string.IsNullOrEmpty(extractorConfig.GenerateApiTemplates) || extractorConfig.GenerateApiTemplates.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.GenerateEmptyPolicyFiles = string.IsNullOrEmpty(extractorConfig.GenerateEmptyPolicyFiles) || extractorConfig.GenerateEmptyPolicyFiles.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.GenerateApiManagementServiceTemplate = string.IsNullOrEmpty(extractorConfig.GenerateApiManagementServiceTemplate) || extractorConfig.GenerateApiManagementServiceTemplate.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.GenerateRevisionMasterTemplates = string.IsNullOrEmpty(extractorConfig.GenerateRevisionMasterTemplates) || extractorConfig.GenerateRevisionMasterTemplates.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.GenerateVersionSetMasterTemplates = string.IsNullOrEmpty(extractorConfig.GenerateVersionSetMasterTemplates) || extractorConfig.GenerateVersionSetMasterTemplates.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.GenerateReleaseTemplates = string.IsNullOrEmpty(extractorConfig.GenerateReleaseTemplates) || extractorConfig.GenerateReleaseTemplates.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.GenerateLoggerTemplates = string.IsNullOrEmpty(extractorConfig.GenerateLoggerTemplates) || extractorConfig.GenerateLoggerTemplates.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.GenerateNamedValueTemplates = string.IsNullOrEmpty(extractorConfig.GenerateNamedValueTemplates) || extractorConfig.GenerateNamedValueTemplates.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.LinkedTemplatesBaseUrl = extractorConfig.LinkedTemplatesBaseUrl;
            this.LinkedTemplatesSasToken = extractorConfig.LinkedTemplatesSasToken;
            this.LinkedTemplatesUrlQueryString = extractorConfig.LinkedTemplatesUrlQueryString;
            this.PolicyXMLBaseUrl = extractorConfig.PolicyXMLBaseUrl;
            this.PolicyXMLSasToken = extractorConfig.PolicyXMLSasToken;
            this.ApiVersionSetName = extractorConfig.ApiVersionSetName;
            this.ParameterizeNamedValue = extractorConfig.ParamNamedValue != null && extractorConfig.ParamNamedValue.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.ParameterizeApiLoggerId = extractorConfig.ParamApiLoggerId != null && extractorConfig.ParamApiLoggerId.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.ParameterizeLogResourceId = extractorConfig.ParamLogResourceId != null && extractorConfig.ParamLogResourceId.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.NotIncludeNamedValue = extractorConfig.NotIncludeNamedValue != null && extractorConfig.NotIncludeNamedValue.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.OperationBatchSize = extractorConfig.OperationBatchSize ?? default;
            this.ParamNamedValuesKeyVaultSecrets = extractorConfig.ParamNamedValuesKeyVaultSecrets != null && extractorConfig.ParamNamedValuesKeyVaultSecrets.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.ParameterizeBackend = extractorConfig.ParamBackend != null && extractorConfig.ParamBackend.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.SplitApis = !string.IsNullOrEmpty(extractorConfig.SplitAPIs) && extractorConfig.SplitAPIs.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.IncludeAllRevisions = !string.IsNullOrEmpty(extractorConfig.IncludeAllRevisions) && extractorConfig.IncludeAllRevisions.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.FileNames = this.GenerateFileNames(extractorConfig.BaseFileName, extractorConfig.SourceApimName);
            this.MultipleApiNames = this.ParseMultipleApiNames(extractorConfig.MultipleAPIs);
            this.ExtractGateways = extractorConfig.ExtractGateways != null && extractorConfig.ExtractGateways.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.OverrideGroupGuids = extractorConfig.OverrideGroupGuids != null && extractorConfig.OverrideGroupGuids.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.OverrideProductGuids = extractorConfig.OverrideProductGuids != null && extractorConfig.OverrideProductGuids.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.ApiParameters = extractorConfig.ApiParameters;
            this.ParameterizeServiceUrl = extractorConfig.ParamServiceUrl != null && extractorConfig.ParamServiceUrl.Equals("true", StringComparison.OrdinalIgnoreCase) || (extractorConfig.ApiParameters != null && extractorConfig.ApiParameters.Any(x => x.Value.ServiceUrl is not null));
            this.ParametrizeApiOauth2Scope = (extractorConfig.ParamApiOauth2Scope != null && extractorConfig.ParamApiOauth2Scope.Equals("true", StringComparison.OrdinalIgnoreCase)) || (extractorConfig.ApiParameters != null && extractorConfig.ApiParameters.Any(x => x.Value.Oauth2Scope is not null));
            this.ExtractSecrets = extractorConfig.ExtractSecrets != null && extractorConfig.ExtractSecrets.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.ExtractIdentityProviders = extractorConfig.ExtractIdentityProviders != null && extractorConfig.ExtractIdentityProviders.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.ExcludeBuildInGroups = extractorConfig.ExcludeBuildInGroups != null && extractorConfig.ExcludeBuildInGroups.Equals("true", StringComparison.OrdinalIgnoreCase);

            this.ParametersOutputDirectoryName = extractorConfig.ParametersOutputDirectoryName;
            if (!string.IsNullOrEmpty(extractorConfig.ParametersOutputDirectoryName))
            {
                this.FileNames.ParametersDirectory = extractorConfig.ParametersOutputDirectoryName;
            }

            if (!this.GenerateLoggerTemplates)
            {
                this.ParameterizeApiLoggerId = false;
            }

            if (!this.GenerateNamedValueTemplates)
            {
                this.ParameterizeNamedValue = false;
                this.ParamNamedValuesKeyVaultSecrets = false;
            }
        }

        public ExtractorParameters OverrideConfiguration(ExtractorConsoleAppConfiguration overridingConfig)
        {
            if (overridingConfig == null) return this;

            this.SourceApimName = overridingConfig.SourceApimName ?? this.SourceApimName;
            this.DestinationApimName = overridingConfig.DestinationApimName ?? this.DestinationApimName;
            this.ResourceGroup = overridingConfig.ResourceGroup ?? this.ResourceGroup;
            this.FilesGenerationRootDirectory = overridingConfig.FileFolder ?? this.FilesGenerationRootDirectory;
            this.SingleApiName = overridingConfig.ApiName ?? this.SingleApiName;
            this.LinkedTemplatesBaseUrl = overridingConfig.LinkedTemplatesBaseUrl ?? this.LinkedTemplatesBaseUrl;
            this.LinkedTemplatesSasToken = overridingConfig.LinkedTemplatesSasToken ?? this.LinkedTemplatesSasToken;
            this.LinkedTemplatesUrlQueryString = overridingConfig.LinkedTemplatesUrlQueryString ?? this.LinkedTemplatesUrlQueryString;
            this.PolicyXMLBaseUrl = overridingConfig.PolicyXMLBaseUrl ?? this.PolicyXMLBaseUrl;
            this.PolicyXMLSasToken = overridingConfig.PolicyXMLSasToken ?? this.PolicyXMLSasToken;
            this.ApiVersionSetName = overridingConfig.ApiVersionSetName ?? this.ApiVersionSetName;
            this.OperationBatchSize = overridingConfig.OperationBatchSize ?? this.OperationBatchSize;

            // there can be no service url parameters in overriding configuration
            // this.ServiceUrlParameters = overridingConfig.ServiceUrlParameters ?? this.ServiceUrlParameters;

            this.ParameterizeServiceUrl = !string.IsNullOrEmpty(overridingConfig.ParamServiceUrl) ? overridingConfig.ParamServiceUrl.Equals("true", StringComparison.OrdinalIgnoreCase) : this.ParameterizeServiceUrl;
            this.ParameterizeNamedValue = !string.IsNullOrEmpty(overridingConfig.ParamNamedValue) ? overridingConfig.ParamNamedValue.Equals("true", StringComparison.OrdinalIgnoreCase) : this.ParameterizeNamedValue;
            this.ParameterizeApiLoggerId = !string.IsNullOrEmpty(overridingConfig.ParamApiLoggerId) ? overridingConfig.ParamApiLoggerId.Equals("true", StringComparison.OrdinalIgnoreCase) : this.ParameterizeApiLoggerId;
            this.ParameterizeLogResourceId = !string.IsNullOrEmpty(overridingConfig.ParamLogResourceId) ? overridingConfig.ParamLogResourceId.Equals("true", StringComparison.OrdinalIgnoreCase) : this.ParameterizeLogResourceId;
            this.NotIncludeNamedValue = !string.IsNullOrEmpty(overridingConfig.NotIncludeNamedValue) ? overridingConfig.NotIncludeNamedValue.Equals("true", StringComparison.OrdinalIgnoreCase) : this.NotIncludeNamedValue;
            this.ParamNamedValuesKeyVaultSecrets = !string.IsNullOrEmpty(overridingConfig.ParamNamedValuesKeyVaultSecrets) ? overridingConfig.ParamNamedValuesKeyVaultSecrets.Equals("true", StringComparison.OrdinalIgnoreCase) : this.ParamNamedValuesKeyVaultSecrets;
            this.ParameterizeBackend = !string.IsNullOrEmpty(overridingConfig.ParamBackend) ? overridingConfig.ParamBackend.Equals("true", StringComparison.OrdinalIgnoreCase) : this.ParameterizeBackend;
            this.SplitApis = !string.IsNullOrEmpty(overridingConfig.SplitAPIs) ? overridingConfig.SplitAPIs.Equals("true", StringComparison.OrdinalIgnoreCase) : this.SplitApis;
            this.IncludeAllRevisions = !string.IsNullOrEmpty(overridingConfig.IncludeAllRevisions) ? overridingConfig.IncludeAllRevisions.Equals("true", StringComparison.OrdinalIgnoreCase) : this.IncludeAllRevisions;
            this.GenerateParameterFiles = !string.IsNullOrEmpty(overridingConfig.GenerateParameterFiles) ? overridingConfig.GenerateParameterFiles.Equals("true", StringComparison.OrdinalIgnoreCase) : this.GenerateParameterFiles;
            this.GenerateGlobalTemplates = !string.IsNullOrEmpty(overridingConfig.GenerateGlobalTemplates) ? overridingConfig.GenerateGlobalTemplates.Equals("true", StringComparison.OrdinalIgnoreCase) : this.GenerateGlobalTemplates;
            this.GenerateApiTemplates = !string.IsNullOrEmpty(overridingConfig.GenerateApiTemplates) ? overridingConfig.GenerateApiTemplates.Equals("true", StringComparison.OrdinalIgnoreCase) : this.GenerateApiTemplates;
            this.GenerateEmptyPolicyFiles = !string.IsNullOrEmpty(overridingConfig.GenerateEmptyPolicyFiles) ? overridingConfig.GenerateEmptyPolicyFiles.Equals("true", StringComparison.OrdinalIgnoreCase) : this.GenerateEmptyPolicyFiles;
            this.GenerateApiManagementServiceTemplate = !string.IsNullOrEmpty(overridingConfig.GenerateApiManagementServiceTemplate) ? overridingConfig.GenerateApiManagementServiceTemplate.Equals("true", StringComparison.OrdinalIgnoreCase) : this.GenerateApiManagementServiceTemplate;
            this.GenerateRevisionMasterTemplates = !string.IsNullOrEmpty(overridingConfig.GenerateRevisionMasterTemplates) ? overridingConfig.GenerateRevisionMasterTemplates.Equals("true", StringComparison.OrdinalIgnoreCase) : this.GenerateRevisionMasterTemplates;
            this.GenerateVersionSetMasterTemplates = !string.IsNullOrEmpty(overridingConfig.GenerateVersionSetMasterTemplates) ? overridingConfig.GenerateVersionSetMasterTemplates.Equals("true", StringComparison.OrdinalIgnoreCase) : this.GenerateVersionSetMasterTemplates;
            this.GenerateReleaseTemplates = !string.IsNullOrEmpty(overridingConfig.GenerateReleaseTemplates) ? overridingConfig.GenerateReleaseTemplates.Equals("true", StringComparison.OrdinalIgnoreCase) : this.GenerateReleaseTemplates;
            this.GenerateLoggerTemplates = !string.IsNullOrEmpty(overridingConfig.GenerateLoggerTemplates) ? overridingConfig.GenerateLoggerTemplates.Equals("true", StringComparison.OrdinalIgnoreCase) : this.GenerateLoggerTemplates;
            this.GenerateNamedValueTemplates = !string.IsNullOrEmpty(overridingConfig.GenerateNamedValueTemplates) ? overridingConfig.GenerateNamedValueTemplates.Equals("true", StringComparison.OrdinalIgnoreCase) : this.GenerateNamedValueTemplates;
            this.ExtractGateways = !string.IsNullOrEmpty(overridingConfig.ExtractGateways) ? overridingConfig.ExtractGateways.Equals("true", StringComparison.OrdinalIgnoreCase) : this.ExtractGateways;
            this.ParametrizeApiOauth2Scope = !string.IsNullOrEmpty(overridingConfig.ParamApiOauth2Scope) ? overridingConfig.ParamApiOauth2Scope.Equals("true", StringComparison.OrdinalIgnoreCase) : this.ParametrizeApiOauth2Scope;
            this.ExtractIdentityProviders = !string.IsNullOrEmpty(overridingConfig.ExtractIdentityProviders) ? overridingConfig.ExtractIdentityProviders.Equals("true", StringComparison.OrdinalIgnoreCase) : this.ExtractIdentityProviders;
            this.ExcludeBuildInGroups = !string.IsNullOrEmpty(overridingConfig.ExcludeBuildInGroups) ? overridingConfig.ExcludeBuildInGroups.Equals("true", StringComparison.OrdinalIgnoreCase) : this.ExcludeBuildInGroups;

            if (!string.IsNullOrEmpty(overridingConfig.BaseFileName))
            {
                this.FileNames = this.GenerateFileNames(overridingConfig.BaseFileName, overridingConfig.SourceApimName);
            }

            if (!string.IsNullOrEmpty(overridingConfig.MultipleAPIs))
            {
                this.MultipleApiNames = this.ParseMultipleApiNames(overridingConfig.MultipleAPIs);
            }

            return this;
        }

        List<string> ParseMultipleApiNames(string multipleApisArgument)
        {
            if (string.IsNullOrEmpty(multipleApisArgument)) return null;            

            var validApiNames = new List<string>();
            foreach (var apiName in multipleApisArgument.Split(','))
            {
                var trimmedApiName = apiName.Trim();
                if (!string.IsNullOrEmpty(trimmedApiName))
                {
                    validApiNames.Add(trimmedApiName);
                }
            }

            return validApiNames;
        }

        FileNames GenerateFileNames(string userRequestedGenerationBaseFolder, string sourceApimInstanceName)
            => string.IsNullOrEmpty(userRequestedGenerationBaseFolder)  
                ? FileNameGenerator.GenerateFileNames(sourceApimInstanceName) 
                : FileNameGenerator.GenerateFileNames(userRequestedGenerationBaseFolder);

        public void Validate()
        {
            if (string.IsNullOrEmpty(this.SourceApimName)) throw new ArgumentException("Missing parameter <sourceApimName>.");
            if (string.IsNullOrEmpty(this.DestinationApimName)) throw new ArgumentException("Missing parameter <destinationApimName>.");
            if (string.IsNullOrEmpty(this.ResourceGroup)) throw new ArgumentException("Missing parameter <resourceGroup>.");
            if (string.IsNullOrEmpty(this.FilesGenerationRootDirectory)) throw new ArgumentException("Missing parameter <globalFileRootDirectory>.");

            bool hasVersionSetName = !string.IsNullOrEmpty(this.ApiVersionSetName);
            bool hasSingleApi = this.SingleApiName != null;
            bool hasMultipleAPIs = !this.MultipleApiNames.IsNullOrEmpty();

            if (this.SplitApis && hasSingleApi)
            {
                throw new NotSupportedException("Can't use splitAPIs and apiName at same time");
            }

            if (this.SplitApis && hasVersionSetName)
            {
                throw new NotSupportedException("Can't use splitAPIs and apiVersionSetName at same time");
            }

            if ((hasVersionSetName || hasSingleApi) && hasMultipleAPIs)
            {
                throw new NotSupportedException("Can't use multipleAPIs with apiName or apiVersionSetName at the same time");
            }

            if (hasSingleApi && hasVersionSetName)
            {
                throw new NotSupportedException("Can't use apiName and apiVersionSetName at same time");
            }

            if (!hasSingleApi && this.IncludeAllRevisions)
            {
                throw new NotSupportedException("\"includeAllRevisions\" can be used when you specify the API you want to extract with \"apiName\"");
            }

            if (!this.GenerateGlobalTemplates && !this.GenerateApiTemplates)
            {
                throw new NotSupportedException("\"generateGlobalTemplates\" and \"generateApiTemplates\" cannot both be false");
            }

            if (!this.GenerateApiTemplates && (hasSingleApi || hasMultipleAPIs || hasVersionSetName || this.SplitApis))
            {
                throw new NotSupportedException("When \"generateApiTemplates\" is false, you cannot specify any of the api options (apiName, multipleAPIs, apiVersionSetName, splitAPIs)");
            }
        }
    }
}