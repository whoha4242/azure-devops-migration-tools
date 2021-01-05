using System.Collections.Generic;

namespace MigrationTools._EngineV1.Configuration.Processing
{
    public class WorkItemMigrationConfig : IWorkItemProcessorConfig
    {
        public bool ReplayRevisions { get; set; }
        public bool PrefixProjectToNodes { get; set; }
        public bool UpdateCreatedDate { get; set; }
        public bool UpdateCreatedBy { get; set; }
        public bool BuildFieldTable { get; set; }
        public bool AppendMigrationToolSignatureFooter { get; set; }
        public string WIQLQueryBitSource { get; set; }

        /// <inheritdoc />
        public string WIQLOrderBitSource { get; set; }
        public string WIQLQueryBitTarget { get; set; }
        public string WIQLOrderBitTarget { get; set; }

        public bool Enabled { get; set; }

        /// <inheritdoc />
        public string Processor => "WorkItemMigrationContext";

        public bool LinkMigration { get; set; }
        public bool AttachmentMigration { get; set; }
        public string AttachmentWorkingPath { get; set; }

        public bool FixHtmlAttachmentLinks { get; set; }

        public bool SkipToFinalRevisedWorkItemType { get; set; }

        public int WorkItemCreateRetryLimit { get; set; }

        public bool FilterWorkItemsThatAlreadyExistInTarget { get; set; }
        public bool PauseAfterEachWorkItem { get; set; }
        public int AttachmentMaxSize { get; set; }
        public bool CollapseRevisions { get; set; }
        public bool LinkMigrationSaveEachAsAdded { get; set; }
        public bool GenerateMigrationComment { get; set; }
        public string[] NodeBasePaths { get; set; }
        public IList<int> WorkItemIDs { get; set; }

        /// <inheritdoc />
        public bool IsProcessorCompatible(IReadOnlyList<IProcessorConfig> otherProcessors)
        {
            return true;
        }

        /// <summary>
        /// Creates a new workitemmigrationconfig with default values
        /// </summary>
        public WorkItemMigrationConfig()
        {
            Enabled = false;
            WorkItemCreateRetryLimit = 5;
            FilterWorkItemsThatAlreadyExistInTarget = true;
            ReplayRevisions = true;
            LinkMigration = true;
            AttachmentMigration = true;
            FixHtmlAttachmentLinks = false;
            AttachmentWorkingPath = "c:\\temp\\WorkItemAttachmentWorkingFolder\\";
            AttachmentMaxSize = 480000000;
            UpdateCreatedBy = true;
            PrefixProjectToNodes = false;
            UpdateCreatedDate = true;
            SkipToFinalRevisedWorkItemType = true;
            LinkMigrationSaveEachAsAdded = false;
            GenerateMigrationComment = true;
            WIQLQueryBitSource = @"AND  [Microsoft.VSTS.Common.ClosedDate] = '' AND [System.WorkItemType] NOT IN ('Test Suite', 'Test Plan')";
            WIQLOrderBitSource = "[System.ChangedDate] desc";
            WIQLQueryBitTarget = @"AND  [Microsoft.VSTS.Common.ClosedDate] = '' AND [System.WorkItemType] NOT IN ('Test Suite', 'Test Plan')";
            WIQLOrderBitTarget = "[System.ChangedDate] desc";
        }
    }
}