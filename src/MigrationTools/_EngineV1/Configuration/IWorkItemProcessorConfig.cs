using System.Collections.Generic;

namespace MigrationTools._EngineV1.Configuration
{
    internal interface IWorkItemProcessorConfig : IProcessorConfig
    {
        public string WIQLQueryBitSource { get; set; }

        /// <inheritdoc />
        public string WIQLOrderBitSource { get; set; }

        public string WIQLQueryBitTarget { get; set; }

        /// <inheritdoc />
        public string WIQLOrderBitTarget { get; set; }


        public IList<int> WorkItemIDs { get; set; }
        public bool FilterWorkItemsThatAlreadyExistInTarget { get; set; }
        public bool PauseAfterEachWorkItem { get; set; }
        public int WorkItemCreateRetryLimit { get; set; }
    }
}