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
    public class RollBackNotesAttachments
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
                    ProcessNoteAttachments(service, settings.AccountName, settings.SASToken, settings.OrgGuid, settings.OrganizationID);
                }
                catch (Exception ex)
                {
                    error = "Error at RollBackAttachments : " + ex.Message;
                }
            }
            return error;
        }
        private static void ProcessNoteAttachments(IOrganizationService service, string accountName, string sasToken, string orgGuid, Guid organizationID)
        {
            SortedList<string, string> entityNameAndContainerList = NotesAttachmentSettings.GetAvailableNotesAttachmentContainerAndEntityName(service);
            foreach (KeyValuePair<string, string> entityNameAndContainer in entityNameAndContainerList)
            {
                FilterExpression filter = new FilterExpression(LogicalOperator.And);
                filter.AddCondition(new ConditionExpression("documentbody", ConditionOperator.Null));
                filter.AddCondition(new ConditionExpression("objecttypecode", ConditionOperator.Equal, entityNameAndContainer.Key));
                QueryExpression attachmentQuery = new QueryExpression("annotation")
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
                        MoveFromAzureBlob(accountName, sasToken, entityNameAndContainer.Value, orgGuid, blobName, attachment.Id.ToString().ToLower(), service, organizationID);
                    }
                }
            }
        }
        private static void MoveFromAzureBlob(string accountName, string sasToken, string containerName, string orgGuid, string blobName, string primaryRecordsGuids, IOrganizationService service, Guid organizationId)
        {
            string log = string.Empty;
            BlobHelper blobHelper = new BlobHelper(accountName, sasToken);
            byte[] blob = blobHelper.GetBlob(containerName, blobName);
            string documentbody = Convert.ToBase64String(blob);
            Entity attachement = new Entity("annotation", new Guid(primaryRecordsGuids));
            attachement["documentbody"] = documentbody;
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
