using System.Collections.Generic;

namespace MigrationTools._EngineV1.Configuration.Processing
{
    public class WorkItemPostProcessingConfig : IWorkItemProcessorConfig
    {
        public IList<int> WorkItemIDs { get; set; }

        /// <inheritdoc />
        public bool Enabled { get; set; }

        /// <inheritdoc />
        public string Processor
        {
            get { return "WorkItemPostProcessingContext"; }
        }

        public string WIQLQueryBitSource { get; set; }
        public string WIQLOrderBitSource { get; set; }
        public string WIQLQueryBitTarget { get; set; }
        public string WIQLOrderBitTarget { get; set; }
        public bool FilterWorkItemsThatAlreadyExistInTarget { get; set; }
        public bool PauseAfterEachWorkItem { get; set; }
        public int WorkItemCreateRetryLimit { get; set; }

        /// <inheritdoc />
        public bool IsProcessorCompatible(IReadOnlyList<IProcessorConfig> otherProcessors)
        {
            return true;
        }

        public WorkItemPostProcessingConfig()
        {
            WIQLQueryBitSource = "AND [TfsMigrationTool.ReflectedWorkItemId] = '' ";
            WIQLQueryBitTarget = "AND [TfsMigrationTool.ReflectedWorkItemId] = '' ";
        }
    }
}