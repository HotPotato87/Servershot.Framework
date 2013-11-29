﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarFetched.AzureWorkflow.Core.Architecture;
using FarFetched.AzureWorkflow.Core.Implementation;
using FarFetched.AzureWorkflow.Core.Implementation.Reports;
using FarFetched.AzureWorkflow.Core.Interfaces;

namespace FarFetched.AzureWorkflow.Core.Plugins
{
    public abstract class ReportGenerationPlugin : WorkflowSessionPluginBase
    {
        public ReadOnlyCollection<ModuleProcessingSummary> ModuleProcessingSummaries { get; set; }
        private readonly List<ModuleProcessingSummary> _moduleProcessingSummaries = new List<ModuleProcessingSummary>(); 

        public ReportGenerationPlugin()
        {
            ModuleProcessingSummaries = new ReadOnlyCollection<ModuleProcessingSummary>(_moduleProcessingSummaries);
        }

        internal override void OnSessionStarted(WorkflowSession session)
        {
            base.OnSessionStarted(session);

            session.OnSessionFinished += workflowSession => this.SendSessionReport(ModuleProcessingSummaries);
        }

        internal override void OnModuleStarted(IWorkflowModule module)
        {
            _moduleProcessingSummaries.Add(new ModuleProcessingSummary(module));

            if (module is IProcessingWorkflowModule)
            {
                (module as IProcessingWorkflowModule).OnRaiseProcessed += (key, detail) =>
                {
                    var summary = this.ModuleProcessingSummaries.Single(x => x.Module == module);
                    if (!summary.ProcessedList.ContainsKey(key)) summary.ProcessedList[key] = 0;
                    summary.ProcessedList[key]++;
                    if (detail != null)
                    {
                        if (!summary.ProcessedListExtraDetail.ContainsKey(key)) summary.ProcessedListExtraDetail[key] = new List<ProcessedItemDetail>();
                        summary.ProcessedListExtraDetail[key].Add(new ProcessedItemDetail(detail));
                    }
                };    
            }
            
            module.OnError += exception =>
            {
                var moduleSummary = this.ModuleProcessingSummaries.Single(x => x.Module == module);
                moduleSummary.Errors++;
                moduleSummary.ErrorList.Add(exception);
            };
            module.OnFinished += () =>
            {
                var moduleSummary = this.ModuleProcessingSummaries.Single(x => x.Module == module);
                moduleSummary.Duration = module.Ended.Subtract(module.Started);
            };
        }

        public abstract void SendSessionReport(IEnumerable<ModuleProcessingSummary> moduleSummaries);
    }
}
