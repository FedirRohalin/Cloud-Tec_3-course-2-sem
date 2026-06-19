using Azure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddAzureClients(b =>
        {
            // Підключаємо локальне сховище
            b.AddBlobServiceClient(Environment.GetEnvironmentVariable("blobConn"));

            // Підключаємо клієнт штучного інтелекту
            var endpoint = new Uri(Environment.GetEnvironmentVariable("textAnalyticsEndpoint"));
            var credential = new AzureKeyCredential(Environment.GetEnvironmentVariable("textAnalyticsKey"));
            b.AddTextAnalyticsClient(endpoint, credential);
        });
    });

builder.Build().Run();