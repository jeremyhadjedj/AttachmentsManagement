using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttachmentsManagement
{
    public static class AttachmentHelper
    {
        public static EntityCollection GetAllEntities(QueryExpression query, IOrganizationService service)
        {
            EntityCollection collection = new EntityCollection();
            if (query.EntityName == null)
                return collection;

            query.PageInfo.Count = 5000;
            query.PageInfo.PageNumber = 1;
            while (true)
            {
                EntityCollection myEntities = service.RetrieveMultiple(query);
                if (myEntities.Entities != null)
                {
                    collection.Entities.AddRange(myEntities.Entities);
                }
                if (myEntities.MoreRecords)
                {
                    query.PageInfo.PageNumber++;
                    query.PageInfo.PagingCookie = myEntities.PagingCookie;
                }
                else
                {
                    break;
                }
            }
            return collection;
        }
        private static SortedList<string, string> GetAvailableNotesAttachmentContainerAndEntityName(IOrganizationService service)
        {
            SortedList<string, string> list = new SortedList<string, string>();
            QueryExpression noteAttachementSettingQuery = new QueryExpression("msdyn_notesattachmententitysetting")
            {
                ColumnSet = new ColumnSet(new string[2]
                {
                    "msdyn_containername",
                    "msdyn_entityname"
                })
            };
            EntityCollection noteAttachementSettingsCollection = service.RetrieveMultiple(noteAttachementSettingQuery);
            if (noteAttachementSettingsCollection.Entities.Count > 0)
            {
                foreach (Entity noteAttachementSetting in noteAttachementSettingsCollection.Entities)
                {
                    if (noteAttachementSetting.Attributes.Contains("msdyn_entityname"))
                    {
                        list.Add(noteAttachementSetting.GetAttributeValue<string>("msdyn_entityname"), noteAttachementSetting.GetAttributeValue<string>("msdyn_containername"));
                    }
                }
            }
            return list;
        }
        public static int CountEmailsAttachmentInCrm(IOrganizationService service)
        {
            int count = 0;
            FilterExpression filter = new FilterExpression(LogicalOperator.And);
            filter.AddCondition(new ConditionExpression("body", ConditionOperator.NotNull));
            filter.AddCondition(new ConditionExpression("objecttypecode", ConditionOperator.Equal, "email"));
            QueryExpression attachmentQuery = new QueryExpression("activitymimeattachment")
            {
                ColumnSet = new ColumnSet("filename"),
                Criteria = filter
            };
            EntityCollection attachements = GetAllEntities(attachmentQuery, service);
            count = attachements.Entities.Count;
            return count;
        }
        public static int CountEmailsAttachmentOutOfCrm(IOrganizationService service)
        {
            int count = 0;
            FilterExpression filter = new FilterExpression(LogicalOperator.And);
            filter.AddCondition(new ConditionExpression("body", ConditionOperator.Null));
            filter.AddCondition(new ConditionExpression("objecttypecode", ConditionOperator.Equal, "email"));
            QueryExpression attachmentQuery = new QueryExpression("activitymimeattachment")
            {
                ColumnSet = new ColumnSet("filename"),
                Criteria = filter
            };
            EntityCollection attachements = GetAllEntities(attachmentQuery, service);
            count = attachements.Entities.Count;
            return count;
        }
        public static int CountNotesAttachmentInCrm(IOrganizationService service)
        {
            int count = 0;
            SortedList<string, string> entityNameAndContainerList = GetAvailableNotesAttachmentContainerAndEntityName(service);
            foreach (KeyValuePair<string, string> entityNameAndContainer in entityNameAndContainerList)
            {
                FilterExpression filter = new FilterExpression(LogicalOperator.And);
                filter.AddCondition(new ConditionExpression("documentbody", ConditionOperator.NotNull));
                filter.AddCondition(new ConditionExpression("objecttypecode", ConditionOperator.Equal, entityNameAndContainer.Key));
                QueryExpression attachmentQuery = new QueryExpression("annotation")
                {
                    ColumnSet = new ColumnSet("filename"),
                    Criteria = filter
                };
                EntityCollection attachements = GetAllEntities(attachmentQuery, service);
                count += attachements.Entities.Count;
            }
            return count;
        }
        public static int CountNotesAttachmentOutOfCrm(IOrganizationService service)
        {
            int count = 0;
            SortedList<string, string> entityNameAndContainerList = GetAvailableNotesAttachmentContainerAndEntityName(service);
            foreach (KeyValuePair<string, string> entityNameAndContainer in entityNameAndContainerList)
            {
                FilterExpression filter = new FilterExpression(LogicalOperator.And);
                filter.AddCondition(new ConditionExpression("documentbody", ConditionOperator.Null));
                filter.AddCondition(new ConditionExpression("objecttypecode", ConditionOperator.Equal, entityNameAndContainer.Key));
                QueryExpression attachmentQuery = new QueryExpression("annotation")
                {
                    ColumnSet = new ColumnSet("filename"),
                    Criteria = filter
                };
                EntityCollection attachements = GetAllEntities(attachmentQuery, service);
                count += attachements.Entities.Count;
            }
            return count;
        }
        public static bool IsAttachmentManagementAppInstalled(IOrganizationService service)
        {
            FilterExpression filter = new FilterExpression(LogicalOperator.And);
            filter.AddCondition(new ConditionExpression("uniquename", ConditionOperator.Equal, "MicrosoftLabsAzuereBlobStorage"));
            QueryExpression query = new QueryExpression
            {
                EntityName = "solution",
                ColumnSet = new ColumnSet("solutionid"),
                Criteria = filter,
            };
            EntityCollection result = service.RetrieveMultiple(query);
            if (result.Entities.Count > 0)
                return true;

            return false;
        }
        public static string MoveEmailsAttachmentsToAzureBlob(SortedList<string, string> metadataList, string accountName, string sasToken, string containerName, string blobName, string body, string primaryRecordsGuids, IOrganizationService service)
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
            return log;
        }
        public static string MoveNotesAttachmentsToAzureBlob(SortedList<string, string> metadataList, string accountName, string sasToken, string containerName, string blobName, string documentbody, string primaryRecordsGuids, IOrganizationService service)
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
            return log;
        }
        public static string MoveEmailsAttachmentsFromAzureBlob(string accountName, string sasToken, string containerName, string orgGuid, string blobName, string primaryRecordsGuids, IOrganizationService service, Guid organizationId)
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
            return log;
        }
        public static string MoveNotesAttachmentsFromAzureBlob(string accountName, string sasToken, string containerName, string orgGuid, string blobName, string primaryRecordsGuids, IOrganizationService service, Guid organizationId)
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
            return log;
        }
    }
}
