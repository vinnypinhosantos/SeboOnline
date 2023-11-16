using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeboOnline.Data;
using SeboOnline.Extensions;
using SeboOnline.Models;
using SeboOnline.ViewModels;
using SeboOnline.ViewModels.Category;
using SeboOnline.ViewModels.User;
using System.Security.Claims;

namespace SeboOnline.Controllers;

[ApiController]
public class CategoryController : ControllerBase
{
    [Authorize(Roles = "admin")]
    [HttpPost("v1/categories")]
    public async Task<IActionResult> PostAsync(
        [FromBody] CreateCategoryViewModel model,
        [FromServices] SeboDataContext context
        )
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<User>(ModelState.GetErrors()));
        var category = new Category()
        {
            Id = 0,
            Name = model.Name,
            Description = model.Description,
            IsActive = true
        };
        try
        {
            await context.Categories.AddAsync(category);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                category.Name
            }));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(400, new ResultViewModel<Category>("Categoria já havia sido cadastrada"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<Category>("Falha interna do servidor"));
        }
    }
    [HttpGet("v1/categories")]
    public async Task<IActionResult> GetAsync(
        [FromServices] SeboDataContext context
        )
    {
        try
        {
            var categories = await context.Categories.Where(x => x.IsActive == true).ToListAsync();
            return Ok(new ResultViewModel<List<Category>>(categories));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("Falha interna do servidor"));
        }
    }
    [HttpGet("v1/categories/{id:int}")]
    public async Task<IActionResult> GetByIdAsync(
    [FromRoute] int id,
    [FromServices] SeboDataContext context)
    {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
            return NotFound(new ResultViewModel<Category>("Usuário não encontrado"));

        return Ok(new ResultViewModel<Category>(category));
    }
    [Authorize(Roles = "admin")]
    [HttpPut("v1/categories/{id:int}")]
    public async Task<IActionResult> PutAsync(
        [FromRoute] int id,
        [FromBody] EditICategoryViewModel model,
        [FromServices] SeboDataContext context)
    {
        try
        {
            
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound(new ResultViewModel<User>("Categoria não encontrada"));

            if (model.Name != null)
                category.Name = model.Name;
            if (model.Description != null)
                category.Description = model.Description;

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new ResultViewModel<User>("Não foi possível atualizar o usuário"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<User>("Falha interna no servidor"));
        }
    }
    [Authorize(Roles = "admin")]
    [HttpDelete("v1/categories/{id:int}")]
    public async Task<IActionResult> DeleteAsync(
    [FromRoute] int id,
    [FromServices] SeboDataContext context)
    {
        try
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound(new ResultViewModel<Category>("Categoria não encontrada"));

            category.IsActive = false;

            context.Categories.Update(category);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Category>(category));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new ResultViewModel<User>("Não foi possível excluir a categoria"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<User>("Falha interna no servidor"));
        }
    }
}
