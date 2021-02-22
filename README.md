# NServiceBus.Extensions.Diagnostics

![CI](https://github.com/jbogard/NServiceBus.Extensions.Diagnostics/workflows/CI/badge.svg)

## Usage

The `NServiceBus.Extensions.Diagnostics.OpenTelemetry` package provides an extension to [OpenTelemetry](https://opentelemetry.io/).

It is a small wrapper to subscribe to the `ActivitySource` exposed as part of `NServiceBus.Extensions.Diagnostics`, and is not required to use as part of OpenTelemetry.

You can configure OpenTelemetry (typically through the [OpenTelemetry.Extensions.Hosting](https://www.nuget.org/packages/OpenTelemetry.Extensions.Hosting) package).

```csharp
services.AddOpenTelemetry(builder => {
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

This package supports OpenTelemetry version `1.0.1`.
