using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Extensions.Skippable.Extensions;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Extensibility.ReportEvents;
using ReportPortal.Shared.Extensibility.ReportEvents.EventArgs;
using ReportPortal.Shared.Reporter;
using System.Linq;

namespace ReportPortal.Extensions.Skippable
{
    public class Skippable : IReportEventsObserver
    {
        public void Initialize(IReportEventsSource reportEventsSource)
        {
            reportEventsSource.OnBeforeTestFinishing += ReportEventsSource_OnBeforeTestFinishing;
            reportEventsSource.OnBeforeLogsSending += ReportEventsSource_OnBeforeLogsSending;
        }

        private void ReportEventsSource_OnBeforeLogsSending(ILogsReporter logsReporter, BeforeLogsSendingEventArgs args)
        {
            var skippableMimeTypes = args.Configuration.GetSkippableMimeTypes();

            if (!skippableMimeTypes.Any())
            {
                return;
            }

            foreach (var request in args.CreateLogItemRequests.Where(request => request.Attach != null))
            {
                if (skippableMimeTypes.Contains(request.Attach.MimeType))
                {
                    request.Attach = null;
                }
            }
        }

        private void ReportEventsSource_OnBeforeTestFinishing(ITestReporter testReporter, BeforeTestFinishingEventArgs args)
        {
            if (args.FinishTestItemRequest?.Status == Status.Skipped)
            {
                if (args.FinishTestItemRequest.Issue == null)
                {
                    args.FinishTestItemRequest.Issue = new Client.Abstractions.Responses.Issue
                    {
                        Type = WellKnownIssueType.NotDefect
                    };
                }
            }
        }
    }
}
