using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeboOnline.Data;
using SeboOnline.Extensions;
using SeboOnline.Models;
using SeboOnline.ViewModels;
using SeboOnline.ViewModels.Category;
using SeboOnline.ViewModels.Item;
using System.Security.Claims;

namespace SeboOnline.Controllers;

[ApiController]
public class ItemController : ControllerBase
{
    [HttpPost("v1/items")]
    public async Task<IActionResult> PostAsync(
        [FromBody] CreateItemViewModel model,
        [FromServices] SeboDataContext context)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResultViewModel<Item>(ModelState.GetErrors()));
        }

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var item = new Item()
        {
            Id = 0,
            Title = model.Title,
            Author = model.Author,
            CategoryId = 0,
            Category = null,
            Price = model.Price,
            Description = model.Description,
            Status = true,
            Seller = null,
            SellerId = userId,
            IsActive = true,
        };
        try
        {
            item.Category = await context.Categories.FirstOrDefaultAsync(x => x.Name == model.CategoryName);
            Console.WriteLine(item.Category.Name);
            if (item.Category == null)
            {
                return NotFound(new ResultViewModel<Item>("Categoria Inexistente"));
            }
            item.Seller = await context.Users.FirstOrDefaultAsync(x => x.Id == item.SellerId);
            Console.WriteLine(item.Seller.Name);
            if (item.Seller == null)
            {
                return NotFound(new ResultViewModel<Item>("Usuário Inválido"));
            }
            await context.Items.AddAsync(item);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Item>(item));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(400, new ResultViewModel<Item>("Item já havia sido cadastrado"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<Item>("Falha interna do servidor"));
        }
    }
    [HttpGet("v1/items")]
    public async Task<IActionResult> GetAsync(
        [FromServices] SeboDataContext context
        )
    {
        try
        {
            var items = await context.Items.Where(x => x.IsActive == true).ToListAsync();
            return Ok(new ResultViewModel<List<Item>>(items));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Item>>("Falha interna do servidor"));
        }
    }

    [HttpGet("v1/items/{id:int}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] int id,
        [FromServices] SeboDataContext context)
    {
        var item = await context.Items.FirstOrDefaultAsync(x => x.Id == id);

        if (item == null)
            return NotFound(new ResultViewModel<Item>("Item não encontrado"));

        return Ok(new ResultViewModel<Item>(item));
    }
    [HttpPut("v1/items/{id:int}")]
    public async Task<IActionResult> PutAsync(
    [FromRoute] int id,
    [FromQuery] int user,
    [FromBody] EditItemViewModel model,
    [FromServices] SeboDataContext context)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (userId == null)
            {
                return NotFound(new ResultViewModel<User>("Usuário não encontrado"));
            }
            if (user != userId)
            {
                return Forbid();
            }
            var item = await context.Items.FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
                return NotFound(new ResultViewModel<Item>("Item não encontrado"));

            if (model.Title != null)
                item.Title = model.Title;
            if (model.Author != null)
                item.Author = model.Author;
            if (model.CategoryName != null)
                item.Category = await context.Categories.FirstOrDefaultAsync(x => x.Name == model.CategoryName);
            if (model.Price != null)
                item.Price = (decimal)model.Price;
            if (model.Description != null)
                item.Description = model.Description;

            context.Items.Update(item);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Item>(item));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new ResultViewModel<User>("Não foi possível atualizar o item"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<User>("Falha interna no servidor"));
        }
    }

    [HttpDelete("v1/items/{id:int}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] int id,
        [FromQuery] int user,
        [FromServices] SeboDataContext context)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (userId == null)
            {
                return NotFound(new ResultViewModel<User>("Usuário não encontrado"));
            }
            if (user != userId)
            {
                return Forbid();
            }
            var item = await context.Items.FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
                return NotFound(new ResultViewModel<Category>("Item não encontrado"));

            item.IsActive = false;

            context.Items.Update(item);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Item>(item));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new ResultViewModel<User>("Não foi possível excluir o item"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<User>("Falha interna no servidor"));
        }
    }

}
