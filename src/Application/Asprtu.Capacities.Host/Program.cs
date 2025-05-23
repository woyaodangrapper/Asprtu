﻿using Asprtu.Capacities;
using Asprtu.Capacities.Host.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
builder.AddHostDefaults()
    .AddApiDocumentation()
    .AddAsprtu();
builder.AddEndpoints();

bool inAspire =
    !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")?.ToLower());

// —— 只有在 dev&cli 环境时才去加载 launchSettings.json ——
if (!inAspire || (builder.Environment.IsDevelopment() && Debugger.IsAttached))
{
    _ = builder.UseLaunchSettings();
    _ = builder.WebHost.UseKestrel();
}

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    _ = app.UseApiDocumentation();
}

app.MapGet("/", () =>
    "与 gRPC 端点的通信必须通过 gRPC 客户端进行。要了解如何创建客户端，请访问：https://go.microsoft.com/fwlink/?linkid=2086909\n" +
    "HTTP/1.1 OpenAPI 文档（跳转）：/docs"
);

app.MapGrpcService<TcpService>();

app.MapGrpcHealthChecksService();

app.MapDefaultEndpoints();

app.UseExceptionHandler();

app.MapEndpoints();

app.Run();