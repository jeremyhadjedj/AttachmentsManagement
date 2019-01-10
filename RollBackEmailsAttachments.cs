using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Client;

namespace AttachmentsManagement
{
    public class RollBackEmailsAttachments
    {
        public static string RollBackAttachments(AzureSettings settings, IOrganizationService service)
        {
            string error = string.Empty;            
            if (string.IsNullOrEmpty(settings.AccountName) || string.IsNullOrEmpty(settings.SASToken) || string.IsNullOrEmpty(settings.AttachmentsContainer))
            {
                error = "Either Azure AccountName or SAS token or Email Attachments Container is empty";
            }
            else
            {
                try
                {
                    ProcessEmailAttachments(service, settings.AccountName, settings.SASToken, settings.OrgGuid, settings.OrganizationID, settings.AttachmentsContainer);
                }
                catch (Exception ex)
                {
                    error = "Error at RollBackAttachments : " + ex.Message;
                }
            }
            return error;
        }
        private static void ProcessEmailAttachments(IOrganizationService service, string accountName, string sasToken, string orgGuid, Guid organizationID, string emailAttachmentsContainer)
        {
            FilterExpression filter = new FilterExpression(LogicalOperator.And);
            filter.AddCondition(new ConditionExpression("body", ConditionOperator.Null));
            filter.AddCondition(new ConditionExpression("objecttypecode", ConditionOperator.Equal, "email"));
            QueryExpression attachmentQuery = new QueryExpression("activitymimeattachment")
            {
                ColumnSet = new ColumnSet(new string[2]
                {
                    "filename",
                    "objectid"
                }),
                Criteria = filter
            };
            EntityCollection attachmentCollection = AttachmentHelper.GetAllEntities(attachmentQuery, service);
            foreach (Entity attachment in attachmentCollection.Entities)
            {
                string fileName = string.Empty;
                if (attachment.Attributes.Contains("filename"))
                {
                    fileName = attachment.GetAttributeValue<string>("filename");
                }
                if (!string.IsNullOrEmpty(fileName))
                {
                    fileName = Uri.EscapeDataString(fileName);
                    string blobName = $"{attachment.Id.ToString().ToLower()}_{fileName}";
                    MoveFromAzureBlob(accountName, sasToken, emailAttachmentsContainer, orgGuid, blobName, attachment.Id.ToString().ToLower(), service, organizationID);
                }
            }
        }
        private static void MoveFromAzureBlob(string accountName, string sasToken, string containerName, string orgGuid, string blobName, string primaryRecordsGuids, IOrganizationService service, Guid organizationId)
        {
            string log = string.Empty;
            BlobHelper blobHelper = new BlobHelper(accountName, sasToken);
            byte[] blob = blobHelper.GetBlob(containerName, blobName);
            string body = Convert.ToBase64String(blob);
            Entity attachement = new Entity("activitymimeattachment", new Guid(primaryRecordsGuids));
            attachement["body"] = body;
            attachement["filesize"] = blob.Length;
            if (blob.Length == 0)
            {
                log = $"Warning : {blobName} size is {blob.Length}";
            }
            service.Update(attachement);

            if (orgGuid == organizationId.ToString())
            {
                blobHelper.DeleteBlob(containerName, blobName);
                log = $"{blobName} has been moved";
            }
            else
            {
                log = $"{blobName} has been moved but not deleted from Azure. Check organizationID in settings !";
            }
        }
    }
}
