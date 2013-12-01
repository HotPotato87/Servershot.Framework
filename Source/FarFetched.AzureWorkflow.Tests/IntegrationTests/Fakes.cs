﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Workflow.Core;
using Azure.Workflow.Core.Enums;
using Azure.Workflow.Core.Implementation;
using Azure.Workflow.Core.Interfaces;
using Azure.Workflow.Core.Plugins;
using Azure.Workflow.Core.Plugins.Alerts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Azure.Workflow.Tests.IntegrationTests
{
    internal class Fakes
    {
        internal class AddsToQueueProcessingFake : InitialWorkflowModule<object>
        {
            private readonly List<object> _payload;
            private readonly Type _typeToSendTo;

            public AddsToQueueProcessingFake(List<object> payload, Type typeToSendTo)
            {
                _payload = payload;
                _typeToSendTo = typeToSendTo;
            }

            public override async Task OnStart()
            {
                base.Session.AddToQueue(_typeToSendTo, _payload);
                await Task.Delay(10);
            }
        }

        internal class RecievesFromQueueProcessingFake : QueueProcessingWorkflowModule<object>
        {
            public RecievesFromQueueProcessingFake()
            {
                
            }

            public override async Task ProcessAsync(IEnumerable<object> queueCollection)
            {
                this.Recieved = queueCollection;

                this.Recieved.ToList().ForEach(x=>this.CategorizeResult(ProcessingResult.Success));
            }

            public IEnumerable<object> Recieved { get; set; }
        }

        internal class QueueProcessingThowsErrorFake : QueueProcessingWorkflowModule<object>
        {
            private readonly Func<Exception> _exceptionFactory;

            public QueueProcessingThowsErrorFake(Func<Exception> exceptionFactory)
            {
                _exceptionFactory = exceptionFactory;
            }

            public override async Task ProcessAsync(IEnumerable<object> queueCollection)
            {
                this.Recieved = queueCollection;

                this.Recieved.ToList().ForEach(x => this.RaiseError(_exceptionFactory.Invoke()));
            }

            public IEnumerable<object> Recieved { get; set; }
        }

        internal class CategorisesProcessingResultFake : QueueProcessingWorkflowModule<object>
        {
            private readonly List<Tuple<object, string>> _processMessages;

            public CategorisesProcessingResultFake(List<Tuple<object, string>> processMessages)
            {
                _processMessages = processMessages;
            }

            public override async Task ProcessAsync(IEnumerable<object> queueCollection)
            {
                this.Recieved = queueCollection;

                _processMessages.ForEach(x=>base.CategorizeResult(x.Item1, x.Item2));

                await Task.Delay(10);
            }

            public IEnumerable<object> Recieved { get; set; }
        }

        internal class QueueProcessingThowsException : QueueProcessingWorkflowModule<object>
        {
            private readonly Func<Exception> _exceptionFactory;

            public QueueProcessingThowsException(Func<Exception> exceptionFactory)
            {
                _exceptionFactory = exceptionFactory;
            }

            public override async Task ProcessAsync(IEnumerable<object> queueCollection)
            {
                throw new Exception("Should throw");
            }

            public IEnumerable<object> Recieved { get; set; }
        }

        internal class LogsMessageFake : QueueProcessingWorkflowModule<object>
        {
            private readonly string _message;

            public LogsMessageFake(string message)
            {
                _message = message;
            }

            public override async Task ProcessAsync(IEnumerable<object> queueCollection)
            {
                this.LogMessage(_message);
            }
        }

        internal class AlertModuleFake : InitialWorkflowModule<object>
        {
            private readonly string _message;

            public AlertModuleFake(string message)
            {
                _message = message;
            }

            public override async Task OnStart()
            {
                this.RaiseAlert(AlertLevel.Low, _message);
            }
        }

        internal class ReportGenerationFake : ReportGenerationPlugin
        {
            internal WorkflowSession Session { get; private set; }

            public override void SendSessionReport(WorkflowSession workflowSession, IEnumerable<ModuleProcessingSummary> moduleSummaries)
            {
                this.Session = workflowSession;
            }
        }
    }
}
