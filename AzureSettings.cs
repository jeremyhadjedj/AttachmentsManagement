using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Crm.Sdk.Messages;
using System;
using System.ServiceModel.Description;

namespace AttachmentsManagement
{
    public class AzureSettings
    {
        public string AttachmentsContainer
        {
            get;
            private set;
        }
        public string AnnotationsContainer
        {
            get;
            private set;
        }
        public string AccountName
        {
            get;
            private set;
        }
        public string SASToken
        {
            get;
            private set;
        }
        public string OrgGuid
        {
            get;
            private set;
        }
        public Guid OrganizationID
        {
            get;
            private set;
        }

        public AzureSettings(IOrganizationService service)
        {
            WhoAmIResponse response = (WhoAmIResponse)service.Execute(new WhoAmIRequest());
            OrganizationID = response.OrganizationId;
            QueryExpression queryAzureBlobStorageSettings = new QueryExpression("msdyn_azureblobstoragesetting")
            {
                ColumnSet = new ColumnSet(new string[5]
                {
                    "msdyn_name",
                    "msdyn_sastoken",
                    "msdyn_attachmentscontainer",
                    "msdyn_organizationguid",
                    "msdyn_annotationcontainer"
                })
            };
            EntityCollection azureBlobStorageSettings = service.RetrieveMultiple(queryAzureBlobStorageSettings);
            foreach (Entity item in azureBlobStorageSettings.Entities)
            {
                AccountName = string.Empty;
                SASToken = string.Empty;
                AttachmentsContainer = string.Empty;
                OrgGuid = string.Empty;

                if (item.Attributes.ContainsKey("msdyn_name"))
                    AccountName = item.GetAttributeValue<string>("msdyn_name");

                if (item.Attributes.ContainsKey("msdyn_sastoken"))
                    SASToken = item.GetAttributeValue<string>("msdyn_sastoken");

                if (item.Attributes.ContainsKey("msdyn_attachmentscontainer"))
                    AttachmentsContainer = item.GetAttributeValue<string>("msdyn_attachmentscontainer");

                if (item.Attributes.ContainsKey("msdyn_organizationguid"))
                    OrgGuid = item.GetAttributeValue<string>("msdyn_organizationguid");

                if (item.Attributes.ContainsKey("msdyn_annotationcontainer"))
                    AnnotationsContainer = item.GetAttributeValue<string>("msdyn_annotationcontainer");
            }
        }
    }
}
