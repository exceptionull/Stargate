using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using StargateAPI.Business.Commands;
using StargateAPI.Business.Data;
using StargateAPI.Business.Dtos;
using StargateAPI.Business.Queries;
using StargateAPI.Controllers;
using Xunit;

namespace StargateAPI.Tests.Controllers;

[TestSubject(typeof(AstronautDutyController))]
public class AstronautDutyControllerTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly AstronautDutyController _controller;

    public AstronautDutyControllerTests()
    {
        _mockMediator = new Mock<IMediator>();
        var nullLogger = NullLogger<AstronautDutyController>.Instance;
        _controller = new AstronautDutyController(_mockMediator.Object, nullLogger);
    }
    
    #region GetAstronautDutiesByName

    [Fact]
    public async Task GetAstronautDutiesByName_ReturnsResult()
    {
        const string name = "Jon Doe";
        var person = new PersonAstronaut { PersonId = 1, Name = "Jon Doe" };
        List<AstronautDuty> astronautDuties = [
            new() { PersonId = person.PersonId, Id = 1, Rank = "R1", DutyTitle = "C1", DutyStartDate = DateTime.UtcNow }
        ];
        _mockMediator.Setup(x => x.Send(It.IsAny<GetAstronautDutiesByName>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetAstronautDutiesByNameResult { Person = person, AstronautDuties = astronautDuties });
        
        var actionResult = await _controller.GetAstronautDutiesByName(name);
        
        var objectResult = actionResult as ObjectResult;
        Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
        
        var getAstronautDutiesByNameResult = objectResult.Value as GetAstronautDutiesByNameResult;
        Assert.True(getAstronautDutiesByNameResult.Success);
        Assert.Equal(Constants.ResponseSuccessMessage, getAstronautDutiesByNameResult.Message);
        Assert.Equal((int)HttpStatusCode.OK, getAstronautDutiesByNameResult.ResponseCode);
        Assert.Equal(person, getAstronautDutiesByNameResult.Person);
        Assert.Equal(astronautDuties, getAstronautDutiesByNameResult.AstronautDuties);
    }

    [Fact]
    public async Task GetAstronautDutiesByName_ReturnsInternalError()
    {
        const string name = "Jon Doe";
        _mockMediator.Setup(x => x.Send(It.IsAny<GetAstronautDutiesByName>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Error!"));
        
        var actionResult = await _controller.GetAstronautDutiesByName(name);
        
        var objectResult = actionResult as ObjectResult;
        Assert.Equal((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        
        var baseResponse = objectResult.Value as BaseResponse;
        Assert.False(baseResponse.Success);
        Assert.Equal(Constants.InternalErrorMessage, baseResponse.Message);
        Assert.Equal((int)HttpStatusCode.InternalServerError, baseResponse.ResponseCode);
    }
    
    #endregion
    
    #region CreateAstronautDuty

    [Fact]
    public async Task CreateAstronautDuty_ReturnsResult()
    {
        var createAstronautDuty = new CreateAstronautDuty { Name = "Jon Doe", Rank = "R1", DutyTitle = "C1", DutyStartDate =  DateTime.UtcNow };
        var createAstronautDutyResult = new CreateAstronautDutyResult {  Id = 1 };
        _mockMediator.Setup(x => x.Send(It.IsAny<CreateAstronautDuty>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createAstronautDutyResult);
        
        var actionResult = await _controller.CreateAstronautDuty(createAstronautDuty);
        
        var objectResult = actionResult as ObjectResult;
        Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
        
        var createAstronautDutyResult2 = objectResult.Value as CreateAstronautDutyResult;
        Assert.True(createAstronautDutyResult2.Success);
        Assert.Equal(Constants.ResponseSuccessMessage, createAstronautDutyResult2.Message);
        Assert.Equal((int)HttpStatusCode.OK, createAstronautDutyResult2.ResponseCode);
        Assert.Equal(createAstronautDutyResult.Id, createAstronautDutyResult2.Id);
    }

    [Fact]
    public async Task CreateAstronautDuty_ReturnsInternalError()
    {
        var createAstronautDuty = new CreateAstronautDuty { Name = "Jon Doe", Rank = "R1", DutyTitle = "C1", DutyStartDate =  DateTime.UtcNow };
        _mockMediator.Setup(x => x.Send(It.IsAny<CreateAstronautDuty>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Error!"));
        
        var actionResult = await _controller.CreateAstronautDuty(createAstronautDuty);
        
        var objectResult = actionResult as ObjectResult;
        Assert.Equal((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        var baseResponse = objectResult.Value as BaseResponse;
        Assert.False(baseResponse.Success);
        Assert.Equal(Constants.InternalErrorMessage, baseResponse.Message);
        Assert.Equal((int)HttpStatusCode.InternalServerError, baseResponse.ResponseCode);
    }

    [Fact]
    public async Task CreateAstronautDuty_ReturnsBadRequestError()
    {
        var createAstronautDuty = new CreateAstronautDuty { Name = "Jon Doe", Rank = "R1", DutyTitle = "C1", DutyStartDate =  DateTime.UtcNow };
        var badRequestErrorMessage = "Bad Request Error!";
        _mockMediator.Setup(x => x.Send(It.IsAny<CreateAstronautDuty>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new BadHttpRequestException(badRequestErrorMessage));
        
        var actionResult = await _controller.CreateAstronautDuty(createAstronautDuty);
        
        var objectResult = actionResult as ObjectResult;
        Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
        var baseResponse = objectResult.Value as BaseResponse;
        Assert.False(baseResponse.Success);
        Assert.Equal(badRequestErrorMessage, baseResponse.Message);
        Assert.Equal((int)HttpStatusCode.BadRequest, baseResponse.ResponseCode);
    }

    #endregion
}