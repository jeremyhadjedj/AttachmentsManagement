using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using McTools.Xrm.Connection;
using Microsoft.Crm.Sdk.Messages;
using XrmToolBox.Extensibility.Args;
using System.Drawing.Drawing2D;

namespace AttachmentsManagement
{
    public partial class MyPluginControl : PluginControlBase, IStatusBarMessenger
    {
        #region parameters
        private Settings mySettings;
        private bool isAppInstalled;
        private AzureSettings settings;
        private SortedList<string, string> activeEntities;
        public event EventHandler<StatusBarMessageEventArgs> SendMessageToStatusBar;
        #endregion

        #region constructor
        public MyPluginControl()
        {
            InitializeComponent();
        }
        #endregion

        #region plugin loading method
        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            // Loads or creates the settings for the plugin
            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();

                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                LogInfo("Settings found and loaded");
            }
            ExecuteMethod(IsAppInstalled);
        }
        #endregion

        #region click event methods
        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }
        private void Btn_RefreshCRM_Click(object sender, EventArgs e)
        {
            if (isAppInstalled)
                ExecuteMethod(GetAttachments);
        }
        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), mySettings);
        }
        private void Btn_MoveEmailsFromCrm_Click(object sender, EventArgs e)
        {
            if (isAppInstalled)
                ExecuteMethod(MoveEmailsAttachmentsToAzure);
        }
        private void Btn_MoveNotesFromCrm_Click(object sender, EventArgs e)
        {
            if (isAppInstalled)
                ExecuteMethod(MoveNotesAttachmentsToAzure);
        }
        private void Btn_MoveEmailsToCrm_Click(object sender, EventArgs e)
        {
            if (isAppInstalled)
                ExecuteMethod(RollBackEmailsAttachmentToCrm);
        }
        private void Btn_MoveNotesToCrm_Click(object sender, EventArgs e)
        {
            if (isAppInstalled)
                ExecuteMethod(RollBackNotesAttachmentToCrm);
        }
        #endregion

        #region logic methods
        private void IsAppInstalled()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting App Informations",
                Work = (worker, args) =>
                {
                    LogInfo("Getting App Informations");
                    isAppInstalled = AttachmentHelper.IsAttachmentManagementAppInstalled(Service);
                    args.Result = isAppInstalled;
                    string info1 = isAppInstalled ? "installed" : "not installed";
                    LogInfo($"Attachment Management App is {info1} !");
                    if (isAppInstalled)
                    {
                        settings = new AzureSettings(Service);
                        activeEntities = NotesAttachmentSettings.GetAvailableNotesAttachmentContainerAndEntityName(Service);
                        string info2 = $"AccountName : {settings.AccountName}, OrganizationID : {settings.OrgGuid}, Sas Token is empty : {string.IsNullOrWhiteSpace(settings.SASToken)}";
                        LogInfo(info2);
                    }
                },
                PostWorkCallBack = (args) =>
                {
                    if (!isAppInstalled)
                        MessageBox.Show("Attachment Management App is not installed !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        ShowInfoNotification("Make sure PostUpdateAnnotation and PreUpdateAnnotation are disabled on documentbody and filesize fields when moving annotation !", null, 32);
                        string entitiesList = "Annotation will be manage for : ";
                        foreach (KeyValuePair<string, string> entity in activeEntities)
                        {
                            entitiesList += entity.Key + ", ";
                        }
                        CheckedEntities.Text = entitiesList.Substring(0, entitiesList.Length - 2);
                    }
                }
            });
        }
        private void GetAttachments()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting attachments",
                Work = (worker, args) =>
                {
                    args.Result = AttachmentHelper.CountEmailsAttachmentInCrm(Service);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (args.Result != null)
                        EmailsInCrmCount.Text = args.Result.ToString();
                }
            });
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting attachments",
                Work = (worker, args) =>
                {
                    args.Result = AttachmentHelper.CountNotesAttachmentInCrm(Service);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (args.Result != null)
                        NotesInCrmCount.Text = args.Result.ToString();
                }
            });
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting attachments",
                Work = (worker, args) =>
                {
                    args.Result = AttachmentHelper.CountEmailsAttachmentOutOfCrm(Service);
                },
                ProgressChanged = args =>
                {
                    SendMessageToStatusBar?.Invoke(this, new StatusBarMessageEventArgs(args.ProgressPercentage, args.UserState.ToString()));
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (args.Result != null)
                        EmailsOutOfCrmCount.Text = args.Result.ToString();
                }
            });
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting attachments",
                Work = (worker, args) =>
                {
                    args.Result = AttachmentHelper.CountNotesAttachmentOutOfCrm(Service);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if (args.Result != null)
                        NotesOutOfCrmCount.Text = args.Result.ToString();
                }
            });
        }
        private void MoveEmailsAttachmentsToAzure()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Moving emails attachments to Azure",
                Work = (worker, args) =>
                {
                    LogInfo("Moving emails attachments to Azure");
                    if (string.IsNullOrEmpty(settings.AccountName) || string.IsNullOrEmpty(settings.SASToken) || string.IsNullOrEmpty(settings.AttachmentsContainer))
                    {
                        LogWarning("Either Azure AccountName or SAS token or Email Attachments Container is empty");
                        return;
                    }
                    worker.ReportProgress(0, "Getting attachements...");
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
                    EntityCollection attachmentCollection = AttachmentHelper.GetAllEntities(attachmentQuery, Service);
                    worker.ReportProgress(10, "Transfert to Azure...");
                    if (attachmentCollection.Entities.Count > 0)
                    {
                        int i = 0;
                        int total = attachmentCollection.Entities.Count;
                        LogInfo($"Get {total} attachments to move");
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
                                string log = AttachmentHelper.MoveEmailsAttachmentsToAzureBlob(metadataList, settings.AccountName, settings.SASToken, settings.AttachmentsContainer, blobName, body, attachment.Id.ToString().ToLower(), Service);
                                if (!string.IsNullOrEmpty(log))
                                    LogWarning(log);
                            }
                            i++;
                            int progress = 10 + (int)Math.Round((i / (decimal)total) * 90m, 0);
                            worker.ReportProgress(progress, "Transfert to Azure...");
                        }
                    }
                    worker.ReportProgress(100, "Done !");
                },
                ProgressChanged = (args) =>
                {
                    SendMessageToStatusBar?.Invoke(this, new StatusBarMessageEventArgs(args.ProgressPercentage, args.UserState.ToString()));
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LogError($"Error occured : {args.Error.ToString()}");
                    }
                    else
                    {
                        LogInfo("Done !");
                        GetAttachments();
                    }
                    SendMessageToStatusBar?.Invoke(this, new StatusBarMessageEventArgs(string.Empty));
                }
            });
        }
        private void MoveNotesAttachmentsToAzure()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Moving notes attachments to Azure",
                Work = (worker, args) =>
                {
                    LogInfo("Moving notes attachments to Azure");
                    if (string.IsNullOrEmpty(settings.AccountName) || string.IsNullOrEmpty(settings.SASToken) || string.IsNullOrEmpty(settings.AttachmentsContainer))
                    {
                        LogWarning("Either Azure AccountName or SAS token or Email Attachments Container is empty");
                        return;
                    }
                    worker.ReportProgress(0, "Getting entities to get attachements from...");
                    SortedList<string, string> entityNameAndContainerList = NotesAttachmentSettings.GetAvailableNotesAttachmentContainerAndEntityName(Service);
                    int i = 0;
                    int total = entityNameAndContainerList.Count;
                    LogInfo($"Get {total} entity to process");
                    worker.ReportProgress(10, "Transfert to Azure...");
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
                        EntityCollection attachmentCollection = AttachmentHelper.GetAllEntities(attachmentQuery, Service);
                        if (attachmentCollection.Entities.Count > 0)
                        {
                            int actualProgress = 10 + (int)Math.Round((i / (decimal)total) * 90m, 0);
                            worker.ReportProgress(actualProgress, $"Transfert {entityNameAndContainer.Key} notes to Azure...");
                            int nextProgress = 10 + (int)Math.Round(((i + 1) / (decimal)total) * 90m, 0);
                            int delta = nextProgress - actualProgress;
                            int j = 0;
                            int subTotal = attachmentCollection.Entities.Count;
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
                                    string log = AttachmentHelper.MoveNotesAttachmentsToAzureBlob(metadataList, settings.AccountName, settings.SASToken, entityNameAndContainer.Value, blobName, documentbody, attachment.Id.ToString().ToLower(), Service);
                                    if (!string.IsNullOrEmpty(log))
                                        LogWarning(log);
                                }
                                j++;
                                decimal varSubProgress = (j / (decimal)subTotal);
                                int subProgress = (int)Math.Round(delta * varSubProgress, 0);
                                worker.ReportProgress(actualProgress + subProgress, $"Transfert {entityNameAndContainer.Key} notes to Azure...");
                            }
                        }
                        i++;
                        int progress = 10 + (int)Math.Round((i / (decimal)total) * 90m, 0);
                        worker.ReportProgress(progress, $"Transfert {entityNameAndContainer.Key} notes to Azure...");
                    }
                    worker.ReportProgress(100, "Done !");
                },
                ProgressChanged = (args) =>
                {
                    SendMessageToStatusBar?.Invoke(this, new StatusBarMessageEventArgs(args.ProgressPercentage, args.UserState.ToString()));
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LogError($"Error occured : {args.Error.ToString()}");
                    }
                    else
                    {
                        LogInfo("Done !");
                        GetAttachments();
                    }
                    SendMessageToStatusBar?.Invoke(this, new StatusBarMessageEventArgs(string.Empty));
                }
            });
        }
        private void RollBackEmailsAttachmentToCrm()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Moving emails attachments to CRM",
                Work = (worker, args) =>
                {
                    LogInfo("Moving emails attachments to CRM");
                    if (string.IsNullOrEmpty(settings.AccountName) || string.IsNullOrEmpty(settings.SASToken) || string.IsNullOrEmpty(settings.AttachmentsContainer))
                    {
                        LogWarning("Either Azure AccountName or SAS token or Email Attachments Container is empty");
                        return;
                    }
                    worker.ReportProgress(0, "Getting attachements...");
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
                    EntityCollection attachmentCollection = AttachmentHelper.GetAllEntities(attachmentQuery, Service);
                    worker.ReportProgress(10, "Transfert to CRM...");
                    if (attachmentCollection.Entities.Count > 0)
                    {
                        int i = 0;
                        int total = attachmentCollection.Entities.Count;
                        LogInfo($"Get {total} attachments to move");
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
                                string log = AttachmentHelper.MoveEmailsAttachmentsFromAzureBlob(settings.AccountName, settings.SASToken, settings.AttachmentsContainer, settings.OrgGuid, blobName, attachment.Id.ToString().ToLower(), Service, settings.OrganizationID);
                                if (!string.IsNullOrEmpty(log))
                                    LogWarning(log);
                            }
                            i++;
                            int progress = 10 + (int)Math.Round((i / (decimal)total) * 90m, 0);
                            worker.ReportProgress(progress, "Transfert to CRM...");
                        }
                    }
                    worker.ReportProgress(100, "Done !");
                },
                ProgressChanged = (args) =>
                {
                    SendMessageToStatusBar?.Invoke(this, new StatusBarMessageEventArgs(args.ProgressPercentage, args.UserState.ToString()));
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LogError($"Error occured : {args.Error.ToString()}");
                    }
                    else
                    {
                        LogInfo("Done !");
                        GetAttachments();
                    }
                    SendMessageToStatusBar?.Invoke(this, new StatusBarMessageEventArgs(string.Empty));
                }
            });
        }
        private void RollBackNotesAttachmentToCrm()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Moving notes attachments to CRM",
                Work = (worker, args) =>
                {
                    LogInfo("Moving notes attachments to CRM");
                    if (string.IsNullOrEmpty(settings.AccountName) || string.IsNullOrEmpty(settings.SASToken) || string.IsNullOrEmpty(settings.AttachmentsContainer))
                    {
                        LogWarning("Either Azure AccountName or SAS token or Email Attachments Container is empty");
                        return;
                    }
                    worker.ReportProgress(0, "Getting entities to get attachements from...");
                    SortedList<string, string> entityNameAndContainerList = NotesAttachmentSettings.GetAvailableNotesAttachmentContainerAndEntityName(Service);
                    int i = 0;
                    int total = entityNameAndContainerList.Count;
                    LogInfo($"Get {total} entity to process");
                    worker.ReportProgress(10, "Transfert to CRM...");
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
                        EntityCollection attachmentCollection = AttachmentHelper.GetAllEntities(attachmentQuery, Service);
                        if (attachmentCollection.Entities.Count > 0)
                        {
                            int actualProgress = 10 + (int)Math.Round((i / (decimal)total) * 90m, 0);
                            worker.ReportProgress(actualProgress, $"Transfert {entityNameAndContainer.Key} notes to CRM...");
                            int nextProgress = 10 + (int)Math.Round(((i + 1) / (decimal)total) * 90m, 0);
                            int delta = nextProgress - actualProgress;
                            int j = 0;
                            int subTotal = attachmentCollection.Entities.Count;
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
                                    string log = AttachmentHelper.MoveNotesAttachmentsFromAzureBlob(settings.AccountName, settings.SASToken, entityNameAndContainer.Value, settings.OrgGuid, blobName, attachment.Id.ToString().ToLower(), Service, settings.OrganizationID);
                                    if (!string.IsNullOrEmpty(log))
                                        LogWarning(log);
                                }
                                j++;
                                decimal varSubProgress = (j / (decimal)subTotal);
                                int subProgress = (int)Math.Round(delta * varSubProgress, 0);
                                worker.ReportProgress(actualProgress + subProgress, $"Transfert {entityNameAndContainer.Key} notes to CRM...");
                            }
                        }
                        i++;
                        int progress = 10 + (int)Math.Round((i / (decimal)total) * 90m, 0);
                        worker.ReportProgress(progress, $"Transfert {entityNameAndContainer.Key} notes to CRM...");
                    }
                    worker.ReportProgress(100, "Done !");
                },
                ProgressChanged = (args) =>
                {
                    SendMessageToStatusBar?.Invoke(this, new StatusBarMessageEventArgs(args.ProgressPercentage, args.UserState.ToString()));
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        LogError($"Error occured : {args.Error.ToString()}");
                    }
                    else
                    {
                        LogInfo("Done !");
                        GetAttachments();
                    }
                    SendMessageToStatusBar?.Invoke(this, new StatusBarMessageEventArgs(string.Empty));
                }
            });
        }
        #endregion

        #region connection updating method
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            if (mySettings != null && detail != null)
            {
                mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
                LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
            }
        }
        #endregion
    }
}