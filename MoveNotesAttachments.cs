using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Client;

namespace AttachmentsManagement
{
    public class MoveNotesAttachments
    {
        public static string MoveAttachments(AzureSettings settings, IOrganizationService service)
        {
            string error = string.Empty;
            if (string.IsNullOrEmpty(settings.AccountName) || string.IsNullOrEmpty(settings.SASToken) || string.IsNullOrEmpty(settings.AnnotationsContainer))
            {
                error = "Either Azure AccountName or SAS token or Note Attachments Container is empty";
            }
            else
            {
                try
                {
                    ProcessNoteAttachments(service, settings.AccountName, settings.SASToken);
                }
                catch (Exception ex)
                {
                    error = "Error at MoveAttachments : " + ex.Message;
                }
            }
            return error;
        }
        private static void ProcessNoteAttachments(IOrganizationService service, string accountName, string sasToken)
        {
            SortedList<string, string> entityNameAndContainerList = NotesAttachmentSettings.GetAvailableNotesAttachmentContainerAndEntityName(service);
            foreach (KeyValuePair<string, string> entityNameAndContainer in entityNameAndContainerList)
            {
                FilterExpression filter = new FilterExpression(LogicalOperator.And);
                filter.AddCondition(new ConditionExpression("documentbody", ConditionOperator.NotNull));
                filter.AddCondition(new ConditionExpression("objecttypecode", ConditionOperator.Equal, entityNameAndContainer.Key));
                QueryExpression attachmentQuery = new QueryExpression("annotation")
                {
                    ColumnSet = new ColumnSet(new string[3]
                    {
                    "documentbody",
                    "filename",
                    "objectid"
                    }),
                    Criteria = filter
                };
                EntityCollection attachmentCollection = AttachmentHelper.GetAllEntities(attachmentQuery, service);
                foreach (Entity attachment in attachmentCollection.Entities)
                {
                    string fileName = string.Empty;
                    string documentbody = string.Empty;
                    SortedList<string, string> metadataList = null;
                    if (attachment.Attributes.ContainsKey("objectid"))
                    {
                        EntityReference objectReference = attachment.GetAttributeValue<EntityReference>("objectid");
                        metadataList = new SortedList<string, string>
                        {
                            {
                                "RegardingActivity",
                                objectReference.LogicalName
                            },
                            {
                                "RegardingActivityId",
                                objectReference.Id.ToString()
                            }
                        };
                    }
                    if (attachment.Attributes.Contains("filename"))
                    {
                        fileName = attachment.GetAttributeValue<string>("filename");
                    }
                    if (attachment.Attributes.Contains("documentbody"))
                    {
                        documentbody = attachment.GetAttributeValue<string>("documentbody");
                    }
                    if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(documentbody))
                    {
                        fileName = Uri.EscapeDataString(fileName);
                        string blobName = $"{attachment.Id.ToString().ToLower()}_{fileName}";
                        MoveToAzureBlob(metadataList, accountName, sasToken, entityNameAndContainer.Value, blobName, documentbody, attachment.Id.ToString().ToLower(), service);
                    }
                }
            }
        }
        private static void MoveToAzureBlob(SortedList<string, string> metadataList, string accountName, string sasToken, string containerName, string blobName, string documentbody, string primaryRecordsGuids, IOrganizationService service)
        {
            string log = string.Empty;
            BlobHelper blobHelper = new BlobHelper(accountName, sasToken);
            if (!blobHelper.PutBlob(containerName, blobName, documentbody, metadataList))
            {
                log = "Attachment is not created to blob.";
            }
            else
            {
                Entity attachment = new Entity("annotation", new Guid(primaryRecordsGuids));
                attachment["documentbody"] = null;
                service.Update(attachment);
                log = "documentbody moved from CRM to Azure successfully.";
            }
        }
    }
}
