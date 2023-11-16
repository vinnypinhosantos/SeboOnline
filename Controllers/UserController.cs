using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeboOnline.Data;
using SeboOnline.Extensions;
using SeboOnline.Models;
using SeboOnline.Services;
using SeboOnline.ViewModels;
using Exception = System.Exception;
using SecureIdentity.Password;
using static System.Net.WebRequestMethods;
using System.Security.Claims;
using SeboOnline.ViewModels.User;

namespace SeboOnline.Controllers;

[ApiController]
public class UserController : ControllerBase
{
    [HttpPost("v1/users")]
    public async Task<IActionResult> PostAsync(
        [FromBody] CreateUserViewModel model,
        [FromServices] PasswordHashService hashService,
        [FromServices] SeboDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<User>(ModelState.GetErrors()));
        var user = new User()
        {
            Id = 0,
            Name = model.Name,
            Email = model.Email,
            IsActive = true,
            PasswordHash = PasswordHasher.Hash(model.Password),
            Roles = null
        };
        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email,
                model.Password
            }));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(400, new ResultViewModel<User>("Esse e-mail já está cadastrado"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<User>("Falha interna do servidor"));
        }

    }
    [HttpPost("v1/users/login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginViewModel model,
        [FromServices] SeboDataContext context,
        [FromServices] TokenService tokenService)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<User>(ModelState.GetErrors()));
        var user = await context
            .Users
            .AsNoTracking()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Email == model.Email);
        if (user == null)
            return StatusCode(401, new ResultViewModel<string>("Usuário inválido"));

        if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
            return StatusCode(401, new ResultViewModel<string>("Usuário ou senha inválidos"));
        try
        {
            var token = tokenService.GenerateToken(user);
            if (!user.IsActive)
            {
                return StatusCode(401, new ResultViewModel<string>("Usuário inválido"));
            }
            return Ok(new ResultViewModel<string>(token, null));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Falha interna do servidor"));
        }
    }
    [HttpGet("v1/users/{id:int}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] int id,
        [FromServices] SeboDataContext context)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        if (id != userId)
        {
            return Forbid();
        }

        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

        if (user == null)
            return NotFound(new ResultViewModel<User>("Usuário não encontrado"));

        return Ok(new ResultViewModel<User>(user));
    }

    [HttpPut("v1/users/{id:int}")]
    public async Task<IActionResult> PutAsync(
    [FromRoute] int id,
    [FromBody] EditUserViewModel model,
    [FromServices] SeboDataContext context)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (id != userId)
            {
                return Forbid();
            }

            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return NotFound(new ResultViewModel<User>("Usuário não encontrado"));

            if (model.Name != null)
                user.Name = model.Name;
            if (model.Email != null)
                user.Email = model.Email;

            context.Users.Update(user);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<User>(user));
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

    [HttpDelete("v1/users/{id:int}")]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] int id,
        [FromServices] SeboDataContext context)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Verifique se o usuário autenticado está excluindo seu próprio perfil
            if (id != userId)
            {
                return Forbid(); // Usuário não tem permissão para excluir este perfil
            }

            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return NotFound(new ResultViewModel<User>("Usuário não encontrado"));

            user.IsActive = false;

            context.Users.Update(user);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<User>(user));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new ResultViewModel<User>("Não foi possível excluir o usuário"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<User>("Falha interna no servidor"));
        }
    }
}
