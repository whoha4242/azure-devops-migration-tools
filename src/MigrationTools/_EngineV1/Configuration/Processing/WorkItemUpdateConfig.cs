using System.Collections.Generic;

namespace MigrationTools._EngineV1.Configuration.Processing
{
    public class WorkItemUpdateConfig : IWorkItemProcessorConfig
    {
        public bool WhatIf { get; set; }

        public bool Enabled { get; set; }

        public string Processor
        {
            get { return "WorkItemUpdate"; }
        }

        public string WIQLQueryBitSource { get; set; }
        public string WIQLOrderBitSource { get; set; }
        public string WIQLQueryBitTarget { get; set; }
        public string WIQLOrderBitTarget { get; set; }
        public IList<int> WorkItemIDs { get; set; }
        public bool FilterWorkItemsThatAlreadyExistInTarget { get; set; }
        public bool PauseAfterEachWorkItem { get; set; }
        public int WorkItemCreateRetryLimit { get; set; }

        /// <inheritdoc />
        public bool IsProcessorCompatible(IReadOnlyList<IProcessorConfig> otherProcessors)
        {
            return true;
        }

        public WorkItemUpdateConfig()
        {
            WIQLQueryBitSource = @"AND [TfsMigrationTool.ReflectedWorkItemId] = '' AND  [Microsoft.VSTS.Common.ClosedDate] = '' AND [System.WorkItemType] IN ('Shared Steps', 'Shared Parameter', 'Test Case', 'Requirement', 'Task', 'User Story', 'Bug')";
            WIQLQueryBitTarget = @"AND [TfsMigrationTool.ReflectedWorkItemId] = '' AND  [Microsoft.VSTS.Common.ClosedDate] = '' AND [System.WorkItemType] IN ('Shared Steps', 'Shared Parameter', 'Test Case', 'Requirement', 'Task', 'User Story', 'Bug')";
        }
    }
}