using ReportPortal.Client.Abstractions.Models;
using ReportPortal.Shared.Extensibility;
using ReportPortal.Shared.Extensibility.ReportEvents;

namespace ReportPortal.Extensions.Skippable
{
    public class Skippable : IReportEventsObserver
    {
        public void Initialize(IReportEventsSource reportEventsSource)
        {
            reportEventsSource.OnBeforeTestFinishing += ReportEventsSource_OnBeforeTestFinishing;
        }

        private void ReportEventsSource_OnBeforeTestFinishing(Shared.Reporter.ITestReporter testReporter, Shared.Extensibility.ReportEvents.EventArgs.BeforeTestFinishingEventArgs args)
        {
            if (args.FinishTestItemRequest.Status == Status.Skipped)
            {
                args.FinishTestItemRequest.Issue = new Client.Abstractions.Responses.Issue
                {
                    Type = WellKnownIssueType.NotDefect
                };
            }
        }
    }
}
