using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Client;

namespace AttachmentsManagement
{
    public class MoveEmailsAttachments
    {
        public static string MoveAttachements(AzureSettings settings, IOrganizationService service)
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
                    ProcessEmailAttachments(service, settings.AccountName, settings.SASToken, settings.AttachmentsContainer);
                }
                catch (Exception ex)
                {
                    error = "Error at MoveEmailsAttachments : " + ex.Message;
                }
            }
            return error;
        }
        private static void ProcessEmailAttachments(IOrganizationService service, string accountName, string sasToken, string emailAttachmentsContainer)
        {
            FilterExpression filter = new FilterExpression(LogicalOperator.And);
            filter.AddCondition(new ConditionExpression("body", ConditionOperator.NotNull));
            filter.AddCondition(new ConditionExpression("objecttypecode", ConditionOperator.Equal, "email"));
            QueryExpression attachmentQuery = new QueryExpression("activitymimeattachment")
            {
                ColumnSet = new ColumnSet(new string[3]
                {
                    "body",
                    "filename",
                    "objectid"
                }),
                Criteria = filter
            };
            EntityCollection attachmentCollection = AttachmentHelper.GetAllEntities(attachmentQuery, service);
            if (attachmentCollection.Entities.Count > 0)
            {
                foreach (Entity attachment in attachmentCollection.Entities)
                {
                    string fileName = string.Empty;
                    string body = string.Empty;
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
                    if (attachment.Attributes.Contains("body"))
                    {
                        body = attachment.GetAttributeValue<string>("body");
                    }
                    if (!string.IsNullOrEmpty(fileName) && !string.IsNullOrEmpty(body))
                    {
                        fileName = Uri.EscapeDataString(fileName);
                        string blobName = $"{attachment.Id.ToString().ToLower()}_{fileName}";
                        MoveToAzureBlob(metadataList, accountName, sasToken, emailAttachmentsContainer, blobName, body, attachment.Id.ToString().ToLower(), service);
                    }
                }
            }
        }
        private static void MoveToAzureBlob(SortedList<string, string> metadataList, string accountName, string sasToken, string containerName, string blobName, string body, string primaryRecordsGuids, IOrganizationService service)
        {
            string log = string.Empty;
            BlobHelper blobHelper = new BlobHelper(accountName, sasToken);
            if (!blobHelper.PutBlob(containerName, blobName, body, metadataList))
            {
                log = "Attachment is not created to blob.";

            }
            else
            {
                Entity attachment = new Entity("activitymimeattachment", new Guid(primaryRecordsGuids));
                attachment["body"] = null;
                service.Update(attachment);
                log = "body moved from CRM to Azure successfully.";
            }
        }
    }
}
