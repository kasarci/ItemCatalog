using System.Collections.Generic;
using System;
using System.Collections;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using ItemCatalog.API.Controllers;
using ItemCatalog.API.Models.Dtos;
using ItemCatalog.API.Models.Entities;
using ItemCatalog.API.Mapping;
using ItemCatalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Xunit.Abstractions;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Driver;
using System.Data;
using FluentAssertions.Primitives;

namespace ItemCatalog.UnitTests;

public class ItemsControllerTests
{
    private readonly ITestOutputHelper _outputHelper;
    private readonly IMapper _mapperStub;
    private readonly Mock<IItemsRepository> _repositoryStub = new();
    private readonly Random _random = new();

    public ItemsControllerTests(ITestOutputHelper outputHelper)
    {
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new ItemProfile()));
        _mapperStub = new Mapper(configuration);
        _outputHelper = outputHelper;
    }

    //Naming convention UnitOfWork_StateUnderTest_ExpectedBehavior()
    [Fact]
    public async Task GetItemAsync_WithUnexistingItem_ReturnsNull()
    {
        //Arrange
        _repositoryStub.Setup(repo =>
                            repo.GetAsync(It.IsAny<Guid>()))
                            .ReturnsAsync((Item)null);

        var controller = new ItemsController(_repositoryStub.Object, _mapperStub);

        //Act
        var result = await controller.GetItemAsync(Guid.NewGuid());

        //Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()
    {
        //Arrange
        var expectedItem = CreateRandomItem();
        _repositoryStub.Setup(repo =>
                    repo.GetAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(expectedItem);

        var controller = new ItemsController(_repositoryStub.Object, _mapperStub);

        //Act
        var result = (((await controller.GetItemAsync(Guid.NewGuid())).Result) as OkObjectResult).Value as ItemDto;

        //Assert
        result.Should().BeEquivalentTo(expectedItem,
                                        options => 
                                                options.ComparingByMembers<Item>().Excluding(item => item.CreatedDate));
    }

    [Fact]
    public async Task GetItemsAsync_WithUnexistingItems_ReturnsEmptyList() 
    {
        //Arrange
        _repositoryStub.Setup(repo =>
                    repo.GetAsync()).ReturnsAsync((IEnumerable<Item>) null);

        var controller = new ItemsController(_repositoryStub.Object, _mapperStub);

        //Act
        var result =(((await controller.GetItemsAsync()).Result) as OkObjectResult).Value as IEnumerable<ItemDto>;
        //Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetItemsAsync_WithExistingItems_ReturnsAllItems() 
    {
        //Arrange
        var expectedItems = CreateRandomItems(3);
        _repositoryStub.Setup(repo => repo.GetAllAsync()).ReturnsAsync(expectedItems);

        var controller = new ItemsController(_repositoryStub.Object, _mapperStub);

        //Act
        var result = ((await controller.GetItemsAsync()).Result as OkObjectResult).Value as IEnumerable<ItemDto>;

        //Assert
        result.Should().BeEquivalentTo(expectedItems, opt => opt.Excluding(item => item.CreatedDate));
    }

    [Fact]
    public async Task CreateItemAsync_WithValidItem_ReturnsTheCreatedItemAsItemDto()
    {
        //Arrange
        var expectedItem = CreateRandomItem();

        var controller = new ItemsController(_repositoryStub.Object, _mapperStub);
        
        //Act
        var result = ((await controller.CreateItemAsync(_mapperStub.Map<Item, CreateItemDto>(expectedItem))).Result as CreatedAtActionResult).Value as ItemDto;

        //Assert
        result.Should().BeEquivalentTo(expectedItem, opt => 
            opt.ComparingByMembers<ItemDto>().Excluding(item => item.Id).Excluding(item => item.CreatedDate))
        .And.BeOfType<ItemDto>();

        result.Id.Should().NotBeEmpty();
        result.Price.Should().BeInRange(0,1000);
    }

    [Fact]
    public async Task UpdateItemAsync_WithUnexsitingItem_ReturnsNotFound()
    {
        //Arrange
        var controller = new ItemsController(_repositoryStub.Object, _mapperStub);

        _repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Item) null);
        
        //Act
        var result = (controller.UpdateItemAsync(Guid.NewGuid(), _mapperStub.Map<Item,UpdateItemDto>(CreateRandomItem())).Result as NotFoundResult);

        //Assert
        result.Should().BeOfType(typeof(NotFoundResult));
    }

    [Fact]
    public async Task UpdateItemAsync_WithValidExistingItem_ReturnsNoContent()
    {
        //Arrange
        var controller = new ItemsController(_repositoryStub.Object, _mapperStub);
        var item = CreateRandomItem();

        _repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync(item);
        
        //Act
        var result = controller.UpdateItemAsync(Guid.NewGuid(), _mapperStub.Map<Item,UpdateItemDto>(CreateRandomItem())).Result as NoContentResult;

        //Assert
        result.Should().BeOfType(typeof(NoContentResult));
    }

    [Fact]
    public async Task DeleteItemAsync_WithUnexsitingItem_ReturnsNotFound()
    {
        //Arrange
        var controller = new ItemsController(_repositoryStub.Object, _mapperStub);

        _repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync((Item) null);
        
        //Act
        var result = controller.DeleteItemAsync(Guid.NewGuid()).Result as NotFoundResult;

        //Assert
        result.Should().BeOfType(typeof(NotFoundResult));
    }

    [Fact]
    public async Task DeleteItemAsync_WithValidExistingItem_ReturnsNoContent()
    {
        //Arrange
        var controller = new ItemsController(_repositoryStub.Object, _mapperStub);
        var item = CreateRandomItem();

        _repositoryStub.Setup(repo => repo.GetAsync(It.IsAny<Guid>())).ReturnsAsync(item);
        
        //Act
        var result = controller.DeleteItemAsync(Guid.NewGuid()).Result as NoContentResult;

        //Assert
        result.Should().BeOfType(typeof(NoContentResult));
    }


    private Item CreateRandomItem()
    {
        return new Item()
        {
            Id = Guid.NewGuid(),
            Name = Guid.NewGuid().ToString(),
            Price = _random.Next(1000),
            CreatedDate = DateTimeOffset.UtcNow
        };
    }

    private IEnumerable<Item> CreateRandomItems(int count) 
    {
        if(count < 0) count = 1;

        var items = new List<Item>();
        for (int i = 0; i < count; i++)
        {
            items.Add(CreateRandomItem());
        }
        return items;
    }
}