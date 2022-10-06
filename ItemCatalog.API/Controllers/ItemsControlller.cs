using ItemCatalog.API.Entities;
using ItemCatalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ItemCatalog.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IItemsRepository _repository;

    public ItemsController(IItemsRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Item>> GetItems()
    {
        return Ok(_repository.GetItems());
    }

    [HttpGet("{id}")]
    public ActionResult<Item> GetItem(Guid id)
    {
        var result = _repository.GetItem(id);
        if (result is null)
        {
            return NotFound();
        }
        return Ok(result);
    }
}