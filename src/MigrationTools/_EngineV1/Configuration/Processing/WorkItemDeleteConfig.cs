using System.Collections.Generic;

namespace MigrationTools._EngineV1.Configuration.Processing
{
    public class WorkItemDeleteConfig : IWorkItemProcessorConfig
    {
        public bool Enabled { get; set; }

        public string Processor
        {
            get { return "WorkItemDelete"; }
        }

        public WorkItemDeleteConfig()
        {
            Enabled = false;
            WIQLQueryBitSource = @"AND  [Microsoft.VSTS.Common.ClosedDate] = '' AND [System.WorkItemType] NOT IN ('Test Suite', 'Test Plan')";
            WIQLOrderBitSource = "[System.ChangedDate] desc";
            WIQLQueryBitTarget = @"AND  [Microsoft.VSTS.Common.ClosedDate] = '' AND [System.WorkItemType] NOT IN ('Test Suite', 'Test Plan')";
            WIQLOrderBitTarget = "[System.ChangedDate] desc";
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
    }
}