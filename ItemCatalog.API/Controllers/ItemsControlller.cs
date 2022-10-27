using AutoMapper;
using ItemCatalog.API.Models.Dtos;
using ItemCatalog.API.Models.Entities;
using ItemCatalog.API.Repositories.Abstract;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItemCatalog.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IItemsRepository _repository;
    private readonly IMapper _mapper;

    public ItemsController(IItemsRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ItemDto>>> GetItemsAsync()
    {
        var items = await _repository.GetAllAsync();
        IEnumerable<ItemDto> itemDtos = _mapper.Map<IEnumerable<ItemDto>>(items);

        return Ok(itemDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
    {
        var item = await _repository.GetAsync(id);
        if (item is null)
        {
            return NotFound();
        }
        var itemDto = _mapper.Map<ItemDto>(item);
        return Ok(itemDto);
    }

    [HttpPost]
    // [ActionName(nameof(CreateItemAsync))]
    public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto createItemDto)
    {
        var item = _mapper.Map<CreateItemDto, Item>(createItemDto);
        await _repository.CreateAsync(item);
        return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, _mapper.Map<ItemDto>(item));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto updateItemDto)
    {
        var item = await _repository.GetAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        item = item with
        {
            Name = updateItemDto.Name,
            Price = updateItemDto.Price
        };

        await _repository.UpdateAsync(item);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteItemAsync(Guid id)
    {
        var item = await _repository.GetAsync(id);
        if (item is null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(id);
        return NoContent();
    }
}