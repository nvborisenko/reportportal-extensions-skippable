using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Client.Abstractions.Requests;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Extensibility.ReportEvents;
using ReportPortal.Shared.Extensibility.ReportEvents.EventArgs;
using ReportPortal.Shared.Reporter;
using System;
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
            var skippableMimeTypes = args.Configuration.GetValues("Extensions:Skippable:MimeTypes", new string[0]);

            if (!skippableMimeTypes.Any())
            {
                return;
            }

            foreach (var request in args.CreateLogItemRequests.Where(request => request.Attach != null))
            {
                if (skippableMimeTypes.Contains(request.Attach.MimeType, StringComparer.OrdinalIgnoreCase))
                {
                    IgnoreAttachment(request);
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

        private static void IgnoreAttachment(CreateLogItemRequest request)
        {
            var mimetype = request.Attach.MimeType;
            var size = request.Attach.Data?.Length;

            var notice = $"> An attachment with {size} byte(s) and {mimetype} MIME type was removed";

            if (string.IsNullOrEmpty(request.Text))
            {
                request.Text = notice;
            }
            else
            {
                request.Text += $"\n\n{notice}";
            }

            request.Attach = null;
        }
    }
}
