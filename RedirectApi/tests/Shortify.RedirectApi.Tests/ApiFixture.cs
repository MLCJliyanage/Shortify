﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shortify.Libraries.Testing.Extensions;
using Shortify.RedirectApi.Infrastructure;
using Shortify.RedirectApi.Tests.TestDoubles;
using StackExchange.Redis;
using Testcontainers.Redis;

namespace Shortify.RedirectApi.Tests;

public class ApiFixture : WebApplicationFactory<IRedirectApiAssemblyMarker>, IAsyncLifetime
{
    private readonly RedisContainer _redisContainer = new RedisBuilder().Build();
    public string RedisConnectionString => _redisContainer.GetConnectionString();
    public InMemoryShortenedUrlReader ShortenedUrlReader { get; } = new();
    
    public Task InitializeAsync()
    {
        return _redisContainer.StartAsync();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(
            services =>
            {
                services.Remove<IShortenedUrlReader>();
                services.AddSingleton<IShortenedUrlReader>(
                    s =>
                    new RedisUrlReader(ShortenedUrlReader,
                        ConnectionMultiplexer.Connect(RedisConnectionString),
                        s.GetRequiredService<ILogger<RedisUrlReader>>())
                );

            });
        base.ConfigureWebHost(builder);
    }
    
    public Task DisposeAsync()
    {
        return _redisContainer.StopAsync();
    }
}