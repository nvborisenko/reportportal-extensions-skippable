All .NET agents for Report Portal require investigation of skipped (ignored) tests. This extension sets issue for all skipped tests to `NO_DEFECT` which means that test is no longer required to be investigated.

Install `ReportPortal.Extensions.Skippable` nuget package beside agent is installed (usually to your test project).

# Features

## Removing binary log attachments by mimetype

In configuration file:

```json
{
  "extensions": {
    "skippable": {
      "mimetypes": ["image/png"]
    }
  }
}
```

