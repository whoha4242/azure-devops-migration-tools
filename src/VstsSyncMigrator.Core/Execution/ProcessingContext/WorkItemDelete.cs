﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using MigrationTools;
using MigrationTools._EngineV1.Clients;
using MigrationTools._EngineV1.Configuration;
using MigrationTools._EngineV1.Configuration.Processing;
using MigrationTools.DataContracts;
using VstsSyncMigrator._EngineV1.Processors;

namespace VstsSyncMigrator.Engine
{
    public class WorkItemDelete : StaticProcessorBase
    {
        private WorkItemDeleteConfig _config;

        public WorkItemDelete(IServiceProvider services, IMigrationEngine me, ITelemetryLogger telemetry, ILogger<WorkItemUpdate> logger) : base(services, me, telemetry, logger)
        {
        }

        public override string Name
        {
            get
            {
                return "WorkItemDelete";
            }
        }

        public override void Configure(IProcessorConfig config)
        {
            _config = (WorkItemDeleteConfig)config;
        }

        protected override void InternalExecute()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            //////////////////////////////////////////////////
            string targetQuery =
                string.Format(
                    @"SELECT [System.Id], [System.Tags] FROM WorkItems WHERE [System.TeamProject] = @TeamProject {0} ORDER BY {1}",
                    _config.WIQLQueryBitTarget, _config.WIQLOrderBitTarget);
            var workItems = Engine.Target.WorkItems.GetWorkItems(targetQuery);

            if (workItems.Count > 0)
            {
                Log.LogInformation("We are going to delete {sourceWorkItemsCount} work items?", workItems.Count);

                Console.WriteLine("Enter the number of work Items that we will be deleting! Then hit Enter e.g. 21");
                string result = Console.ReadLine();
                if (int.Parse(result) != workItems.Count)
                {
                    Log.LogWarning("USER ABORTED by selecting a number other than {sourceWorkItemsCount}", workItems.Count);
                    return;
                }

                //////////////////////////////////////////////////
                int current = workItems.Count;

                //int count = 0;
                //long elapsedms = 0;
                var tobegone = (from WorkItemData wi in workItems select int.Parse(wi.Id)).ToList();

                foreach (int begone in tobegone)
                {
                    ICollection<WorkItemOperationError> err = ((TfsWorkItemMigrationClient)Engine.Target.WorkItems).Store.DestroyWorkItems(new List<int>() { begone });
                    if (err.Count > 0)
                    {
                        Log.LogInformation("Delete Failed: {0}", err.First().Exception.ToString());
                    }
                    else
                    {
                        Log.LogInformation("Deleted {0}", begone);
                    }
                }
            }
            else
            {
                Log.LogInformation("Nothing to delete");
            }

            //////////////////////////////////////////////////
            stopwatch.Stop();
            Console.WriteLine(@"DONE in {0:%h} hours {0:%m} minutes {0:s\:fff} seconds", stopwatch.Elapsed);
        }
    }
}