**This repo is no longer maintained. All of its functionality is now present in [NServiceBus OTel](https://docs.particular.net/nservicebus/operations/opentelemetry) support.**

# NServiceBus.Extensions.Diagnostics

![CI](https://github.com/jbogard/NServiceBus.Extensions.Diagnostics.OpenTelemetry/workflows/CI/badge.svg)
[![NuGet](https://img.shields.io/nuget/dt/NServiceBus.Extensions.Diagnostics.OpenTelemetry.svg)](https://www.nuget.org/packages/NServiceBus.Extensions.Diagnostics.OpenTelemetry) 
[![NuGet](https://img.shields.io/nuget/vpre/NServiceBus.Extensions.Diagnostics.OpenTelemetry.svg)](https://www.nuget.org/packages/NServiceBus.Extensions.Diagnostics.OpenTelemetry)
[![MyGet (dev)](https://img.shields.io/myget/jbogard-ci/v/NServiceBus.Extensions.Diagnostics.OpenTelemetry.svg)](https://myget.org/gallery/jbogard-ci)

## Tracing Usage

The `NServiceBus.Extensions.Diagnostics.OpenTelemetry` package provides an extension to [OpenTelemetry](https://opentelemetry.io/).

It is a small wrapper to subscribe to the `ActivitySource` exposed as part of `NServiceBus.Extensions.Diagnostics`, and is not required to use as part of OpenTelemetry.

You can configure OpenTelemetry (typically through the [OpenTelemetry.Extensions.Hosting](https://www.nuget.org/packages/OpenTelemetry.Extensions.Hosting) package).

```csharp
services.AddOpenTelemetryTracing(builder => {
    builder
        // Configure exporters
        .AddZipkinExporter()
        // Configure adapters
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddNServiceBusInstrumentation(); // Adds NServiceBus OTel support
});
```

This extension method only adds a source with the appropriate name:

```csharp
public static TracerProviderBuilder AddNServiceBusInstrumentation(this TracerProviderBuilder builder)
{
    return builder.AddSource("NServiceBus.Extensions.Diagnostics");
}
```

Since OTel is supported at the NServiceBus level, any transport that NServiceBus supports also supports OTel.

This package supports OpenTelemetry version `1.2.0-rc1` and above.

## Metrics Usage

This package also includes a small extension method to add the `NServiceBus.Extensions.Diagnostics` Meter, for OTel metrics:

```csharp
services.AddOpenTelemetryMetrics(builder => {
    builder
        // Configure adapters
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddNServiceBusInstrumentation() // Adds NServiceBus instrumentation support
        // Configure exporters
        ; 
});
