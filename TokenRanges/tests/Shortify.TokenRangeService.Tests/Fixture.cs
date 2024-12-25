using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Npgsql;
using Testcontainers.PostgreSql;

namespace Shortify.TokenRangeService.Tests;

public class Fixture : WebApplicationFactory<ITokenRangeAssemblyMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresSqlContainer;
    private string ConnectionString => _postgresSqlContainer.GetConnectionString();

    public Fixture()
    {
        _postgresSqlContainer = new PostgreSqlBuilder()
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _postgresSqlContainer.StartAsync();
        Environment.SetEnvironmentVariable("Postgres__ConnectionString", ConnectionString);
        await InitializeSqlTable();
    }
    
    private async Task InitializeSqlTable()
    {
        var tableSql = await File.ReadAllTextAsync(
            "Table.sql");
        
        await using var connection = new NpgsqlConnection(ConnectionString);
        await connection.OpenAsync();
        
        await using var command = new NpgsqlCommand(tableSql, connection);
        await command.ExecuteNonQueryAsync();
    }

    public async Task DisposeAsync()
    {
        await _postgresSqlContainer.StopAsync();
    }
}