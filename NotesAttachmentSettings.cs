using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Crm.Sdk.Messages;
using System;
using System.ServiceModel.Description;
using System.Collections.Generic;

namespace AttachmentsManagement
{
    public class NotesAttachmentSettings
    {
        public static string GetContainerName(string logicalName, IOrganizationService service)
        {
            string containerName = string.Empty;
            FilterExpression filter = new FilterExpression(LogicalOperator.And);
            filter.AddCondition(new ConditionExpression("msdyn_entityname", ConditionOperator.Equal, logicalName.ToLower()));
            QueryExpression noteAttachementSettingQuery = new QueryExpression("msdyn_notesattachmententitysetting")
            {
                ColumnSet = new ColumnSet(new string[1]
                {
                    "msdyn_containername"
                }),
                Criteria = filter
            };
            EntityCollection noteAttachementSettingsCollection = service.RetrieveMultiple(noteAttachementSettingQuery);
            if (noteAttachementSettingsCollection.Entities.Count > 0)
            {
                containerName = noteAttachementSettingsCollection.Entities[0].GetAttributeValue<string>("msdyn_containername").ToLower();
                Console.WriteLine("Container name : {0} for {1} entity", containerName, logicalName);
            }
            return containerName;
        }
        public static SortedList<string, string> GetAvailableNotesAttachmentContainerAndEntityName(IOrganizationService service)
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
    }
}
