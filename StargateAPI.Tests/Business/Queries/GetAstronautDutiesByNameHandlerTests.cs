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

[TestSubject(typeof(GetAstronautDutiesByNameHandler))]
public class GetAstronautDutiesByNameHandlerTests : IDisposable
{
    private ServiceProvider _serviceProvider;
    
    private GetAstronautDutiesByNameHandler _handler;
    private StargateContext _dbContext;

    public GetAstronautDutiesByNameHandlerTests()
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
        
        _handler = new GetAstronautDutiesByNameHandler(_dbContext);
    }
    
    public void Dispose()
    {
        //_dbContext.Database.EnsureDeleted();
        //_dbContext.Database.EnsureCreated();
    }

    [Fact]
    public async Task Handle_WhenNoPerson_ReturnsResult()
    {
        var getAstronautDutiesByName = new GetAstronautDutiesByName { Name = "Mike Doe" };
        
        var getAstronautDutiesByNameResult = await _handler.Handle(getAstronautDutiesByName, CancellationToken.None);
        
        Assert.Empty(getAstronautDutiesByNameResult.AstronautDuties);
        Assert.Null(getAstronautDutiesByNameResult.Person);
    }
    
    [Fact]
    public async Task Handle_WhenNoDuties_ReturnsResult()
    {
        var getAstronautDutiesByName = new GetAstronautDutiesByName { Name = "Jane Doe" };
        
        var getAstronautDutiesByNameResult = await _handler.Handle(getAstronautDutiesByName, CancellationToken.None);
        
        Assert.Equal(2, getAstronautDutiesByNameResult.Person.PersonId);
        Assert.Equal(getAstronautDutiesByName.Name, getAstronautDutiesByNameResult.Person.Name);
        Assert.Empty(getAstronautDutiesByNameResult.AstronautDuties);
    }
    
    [Fact]
    public async Task Handle_ReturnsResult()
    {
        var getAstronautDutiesByName = new GetAstronautDutiesByName { Name = "John Doe" };
        
        var getAstronautDutiesByNameResult = await _handler.Handle(getAstronautDutiesByName, CancellationToken.None);
        
        Assert.Equal(1, getAstronautDutiesByNameResult.Person.PersonId);
        Assert.Equal(getAstronautDutiesByName.Name, getAstronautDutiesByNameResult.Person.Name);
        Assert.Single(getAstronautDutiesByNameResult.AstronautDuties);
        Assert.Equal(1, getAstronautDutiesByNameResult.AstronautDuties[0].Id);
        Assert.Equal("1LT", getAstronautDutiesByNameResult.AstronautDuties[0].Rank);
        Assert.Equal("Commander", getAstronautDutiesByNameResult.AstronautDuties[0].DutyTitle);
    }
}