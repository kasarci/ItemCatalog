using AutoMapper;
using ItemCatalog.API.Dtos;
using ItemCatalog.API.Repositories;
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
    public ActionResult<IEnumerable<ItemDto>> GetItems()
    {
        var items = _repository.GetItems();
        IEnumerable<ItemDto> itemDtos = _mapper.Map<IEnumerable<ItemDto>>(items);

        return Ok(itemDtos);
    }

    [HttpGet("{id}")]
    public ActionResult<ItemDto> GetItem(Guid id)
    {
        var item = _repository.GetItem(id);
        if (item is null)
        {
            return NotFound();
        }
        var itemDto = _mapper.Map<ItemDto>(item);
        return Ok(itemDto);
    }
}