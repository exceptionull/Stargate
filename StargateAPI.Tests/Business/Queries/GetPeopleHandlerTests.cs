using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StargateAPI.Business.Data;
using StargateAPI.Business.Queries;
using Xunit;

namespace StargateAPI.Tests.Business.Queries;

[TestSubject(typeof(GetPeopleHandler))]
public class GetPeopleHandlerTests : IDisposable
{
    private ServiceProvider _serviceProvider;
    
    private GetPeopleHandler _handler;
    private StargateContext _dbContext;

    public GetPeopleHandlerTests()
    {
        var services = new ServiceCollection();
        
        const string db = "Data Source=starbase-unit.db";
        services.AddDbContext<StargateContext>(options =>
        {
            options.UseSqlite(db);
        });

        _serviceProvider = services.BuildServiceProvider();
        
        _dbContext = _serviceProvider.GetRequiredService<StargateContext>();
        _dbContext.Database.EnsureCreated();
        
        _handler = new GetPeopleHandler(_dbContext);

    }
    
    public void Dispose()
    {
        //_dbContext.Database.EnsureDeleted();
    }

    [Fact]
    public async Task Handle_ReturnsResult()
    {
        var getPeople = new GetPeople();
        
        var getPeopleResult = await _handler.Handle(getPeople, CancellationToken.None);
        
        Assert.Equal(2, getPeopleResult.People.Count);
        Assert.Equal(1, getPeopleResult.People[0].PersonId);
        Assert.Equal(2, getPeopleResult.People[1].PersonId);
    }
}