using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeboOnline.Data;
using SeboOnline.Extensions;
using SeboOnline.Models;
using SeboOnline.Services;
using SeboOnline.ViewModels;
using SeboOnline.ViewModels.Role;
using SeboOnline.ViewModels.User;
using SecureIdentity.Password;

namespace SeboOnline.Controllers;

[ApiController]
public class AdminController : ControllerBase
{
    [HttpPost("v1/admin/login")]
    public async Task<IActionResult> AdminLogin(
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
        Console.WriteLine(user.ToString());
        if (user == null)
            return StatusCode(401, new ResultViewModel<string>("Usuário inválido"));
        if (!user.IsAdministrator())
        {
            return StatusCode(401, new ResultViewModel<string>("Acesso negado. Você não tem permissão de administrador."));
        }
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
    [Authorize(Roles = "admin")]
    [HttpGet("v1/admin/users")]
    public async Task<IActionResult> GetAsync([FromServices] SeboDataContext context)
    {
        try
        {
            var users = await context.Users.Where(x => x.IsActive == true).ToListAsync();
            return Ok(new ResultViewModel<List<User>>(users));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<List<User>>("Falha interna do servidor"));
        }
    }
    [HttpPost("v1/admin/addrole")]
    public async Task<IActionResult> AddRoleToUser([FromBody] AddRoleViewModel model, [FromServices] SeboDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = await context.Users.FindAsync(model.UserId);

        if (user == null)
            return NotFound(new ResultViewModel<string>("Usuário não encontrado"));

        var existingRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == model.RoleName);

        if (existingRole == null)
        {
            user.AddRole(model.RoleName);

            try
            {
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<string>("Papel adicionado com sucesso", null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<string>($"Erro interno: {ex.Message}"));
            }
        }
        else
        {
            user.Roles.Add(existingRole);

            try
            {
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<string>("Papel adicionado com sucesso", null));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<string>($"Erro interno: {ex.Message}"));
            }
        }
    }
}
