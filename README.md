# NServiceBus.Extensions.Diagnostics

![CI](https://github.com/jbogard/NServiceBus.Extensions.Diagnostics/workflows/CI/badge.svg)

## Usage

The `NServiceBus.Extensions.Diagnostics.OpenTelemetry` package provides adapters to [OpenTelemetry](https://opentelemetry.io/).

You can configure OpenTelemetry (typically through the [OpenTelemetry.Extensions.Hosting](https://www.nuget.org/packages/OpenTelemetry.Extensions.Hosting) package).

```csharp
services.AddOpenTelemetry(builder => {
    builder
        // Configure exporters
        .UseZipkinExporter()
        // Configure adapters
        .AddAspNetCoreInstrumentation()
        .AddSqlClientDependencyInstrumentation()
        .AddNServiceBusInstrumentation(); // Adds NServiceBus OTel support
});
```

Since OTel is supported at the NServiceBus level, any transport that NServiceBus supports also supports OTel.
This package supports the latest released alpha package on NuGet.

By default, the message body is not logged to OTel. To change this, configure the options:

```csharp
services.AddOpenTelemetry(builder => {
    builder
        // Configure exporters
        .UseZipkinExporter()
        // Configure adapters
        .AddAspNetCoreInstrumentation()
        .AddSqlClientDependencyInstrumentation()
        .AddNServiceBusInstrumentation(opt => opt.CaptureMessageBody = true); // Adds NServiceBus OTel support
});
```
