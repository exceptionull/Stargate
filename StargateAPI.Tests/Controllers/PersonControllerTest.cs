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
using StargateAPI.Business.Dtos;
using StargateAPI.Business.Queries;
using StargateAPI.Controllers;
using Xunit;

namespace StargateAPI.Tests.Controllers;

[TestSubject(typeof(PersonController))]
public class PersonControllerTest
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly PersonController _controller;

    public PersonControllerTest()
    {
        _mockMediator = new Mock<IMediator>();
        var nullLogger = NullLogger<PersonController>.Instance;
        _controller = new PersonController(_mockMediator.Object, nullLogger);
    }
    
    #region GetPeople

    [Fact]
    public async Task GetPeople_ReturnsResult()
    {
        List<PersonAstronaut> people = [new PersonAstronaut { Name = "Jon Doe" }];
        _mockMediator.Setup(x => x.Send(It.IsAny<GetPeople>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetPeopleResult { People = people });
        
        var actionResult = await _controller.GetPeople();
        
        var objectResult = actionResult as ObjectResult;
        Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
        
        var getPeopleResult = objectResult.Value as GetPeopleResult;
        Assert.True(getPeopleResult.Success);
        Assert.Equal(Constants.ResponseSuccessMessage, getPeopleResult.Message);
        Assert.Equal((int)HttpStatusCode.OK, getPeopleResult.ResponseCode);
        Assert.Equal(people, getPeopleResult.People);
    }

    [Fact]
    public async Task GetPeople_ReturnsInternalError()
    {
        List<PersonAstronaut> people = [new PersonAstronaut { Name = "Jon Doe" }];
        _mockMediator.Setup(x => x.Send(It.IsAny<GetPeople>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Error!"));
        
        var actionResult = await _controller.GetPeople();
        
        var objectResult = actionResult as ObjectResult;
        Assert.Equal((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        
        var baseResponse = objectResult.Value as BaseResponse;
        Assert.False(baseResponse.Success);
        Assert.Equal(Constants.InternalErrorMessage, baseResponse.Message);
        Assert.Equal((int)HttpStatusCode.InternalServerError, baseResponse.ResponseCode);
    }
    
    #endregion

    #region GetPersonByName

    [Fact]
    public async Task GetPersonByName_ReturnsResult()
    {
        var name = "Jon Doe";
        var person = new PersonAstronaut { Name = name };
        _mockMediator.Setup(x => x.Send(It.IsAny<GetPersonByName>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetPersonByNameResult { Person = person });
        
        var actionResult = await _controller.GetPersonByName(name);
        
        var objectResult = actionResult as ObjectResult;
        Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
        
        var getPersonByNameResult = objectResult.Value as GetPersonByNameResult;
        Assert.True(getPersonByNameResult.Success);
        Assert.Equal(Constants.ResponseSuccessMessage, getPersonByNameResult.Message);
        Assert.Equal((int)HttpStatusCode.OK, getPersonByNameResult.ResponseCode);
        Assert.Equal(person, getPersonByNameResult.Person);
    }

    [Fact]
    public async Task GetPersonByName_ReturnsInternalError()
    {
        var name = "Jon Doe";
        var person = new PersonAstronaut { Name = name };
        _mockMediator.Setup(x => x.Send(It.IsAny<GetPersonByName>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Error!"));
        
        var actionResult = await _controller.GetPersonByName(name);
        
        var objectResult = actionResult as ObjectResult;
        Assert.Equal((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        var baseResponse = objectResult.Value as BaseResponse;
        Assert.False(baseResponse.Success);
        Assert.Equal(Constants.InternalErrorMessage, baseResponse.Message);
        Assert.Equal((int)HttpStatusCode.InternalServerError, baseResponse.ResponseCode);
    }

    #endregion
    
    #region CreatePerson

    [Fact]
    public async Task CreatePerson_ReturnsResult()
    {
        var createPerson = new CreatePerson { Name = "Jon Doe" };
        var createPersonResult = new CreatePersonResult {  Id = 1 };
        _mockMediator.Setup(x => x.Send(It.IsAny<CreatePerson>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createPersonResult);
        
        var actionResult = await _controller.CreatePerson(createPerson);
        
        var objectResult = actionResult as ObjectResult;
        Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
        
        var createPersonResult2 = objectResult.Value as CreatePersonResult;
        Assert.True(createPersonResult2.Success);
        Assert.Equal(Constants.ResponseSuccessMessage, createPersonResult2.Message);
        Assert.Equal((int)HttpStatusCode.OK, createPersonResult2.ResponseCode);
        Assert.Equal(createPersonResult.Id, createPersonResult2.Id);
    }

    [Fact]
    public async Task CreatePerson_ReturnsInternalError()
    {
        var createPerson = new CreatePerson { Name = "Jon Doe" };
        _mockMediator.Setup(x => x.Send(It.IsAny<CreatePerson>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Error!"));
        
        var actionResult = await _controller.CreatePerson(createPerson);
        
        var objectResult = actionResult as ObjectResult;
        Assert.Equal((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        var baseResponse = objectResult.Value as BaseResponse;
        Assert.False(baseResponse.Success);
        Assert.Equal(Constants.InternalErrorMessage, baseResponse.Message);
        Assert.Equal((int)HttpStatusCode.InternalServerError, baseResponse.ResponseCode);
    }

    [Fact]
    public async Task CreatePerson_ReturnsBadRequestError()
    {
        var createPerson = new CreatePerson { Name = "Jon Doe" };
        var badRequestErrorMessage = "Bad Request Error!";
        _mockMediator.Setup(x => x.Send(It.IsAny<CreatePerson>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new BadHttpRequestException(badRequestErrorMessage));
        
        var actionResult = await _controller.CreatePerson(createPerson);
        
        var objectResult = actionResult as ObjectResult;
        Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
        var baseResponse = objectResult.Value as BaseResponse;
        Assert.False(baseResponse.Success);
        Assert.Equal(badRequestErrorMessage, baseResponse.Message);
        Assert.Equal((int)HttpStatusCode.BadRequest, baseResponse.ResponseCode);
    }

    #endregion
    
    #region UpdatePerson

    [Fact]
    public async Task UpdatePerson_ReturnsResult()
    {
        var updatePerson = new UpdatePerson { Name = "Jon Doe", NewName = "Jane Doe" };
        var updatePersonResult = new UpdatePersonResult {  Id = 1 };
        _mockMediator.Setup(x => x.Send(It.IsAny<UpdatePerson>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatePersonResult);
        
        var actionResult = await _controller.UpdatePerson(updatePerson);
        
        var objectResult = actionResult as ObjectResult;
        Assert.Equal((int)HttpStatusCode.OK, objectResult.StatusCode);
        
        var updatePersonResult2 = objectResult.Value as UpdatePersonResult;
        Assert.True(updatePersonResult2.Success);
        Assert.Equal(Constants.ResponseSuccessMessage, updatePersonResult2.Message);
        Assert.Equal((int)HttpStatusCode.OK, updatePersonResult2.ResponseCode);
        Assert.Equal(updatePersonResult.Id, updatePersonResult2.Id);
    }

    [Fact]
    public async Task UpdatePerson_ReturnsInternalError()
    {
        var updatePerson = new UpdatePerson { Name = "Jon Doe", NewName = "Jane Doe" };
        _mockMediator.Setup(x => x.Send(It.IsAny<UpdatePerson>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Error!"));
        
        var actionResult = await _controller.UpdatePerson(updatePerson);
        
        var objectResult = actionResult as ObjectResult;
        Assert.Equal((int)HttpStatusCode.InternalServerError, objectResult.StatusCode);
        var baseResponse = objectResult.Value as BaseResponse;
        Assert.False(baseResponse.Success);
        Assert.Equal(Constants.InternalErrorMessage, baseResponse.Message);
        Assert.Equal((int)HttpStatusCode.InternalServerError, baseResponse.ResponseCode);
    }

    [Fact]
    public async Task UpdatePerson_ReturnsBadRequestError()
    {
        var updatePerson = new UpdatePerson { Name = "Jon Doe", NewName = "Jane Doe" };
        var badRequestErrorMessage = "Bad Request Error!";
        _mockMediator.Setup(x => x.Send(It.IsAny<UpdatePerson>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new BadHttpRequestException(badRequestErrorMessage));
        
        var actionResult = await _controller.UpdatePerson(updatePerson);
        
        var objectResult = actionResult as ObjectResult;
        Assert.Equal((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
        var baseResponse = objectResult.Value as BaseResponse;
        Assert.False(baseResponse.Success);
        Assert.Equal(badRequestErrorMessage, baseResponse.Message);
        Assert.Equal((int)HttpStatusCode.BadRequest, baseResponse.ResponseCode);
    }

    #endregion
}