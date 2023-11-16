using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeboOnline.Data;
using SeboOnline.Extensions;
using SeboOnline.Models;
using SeboOnline.ViewModels.Item;
using SeboOnline.ViewModels;
using System.Security.Claims;
using SeboOnline.ViewModels.Transaction;

namespace SeboOnline.Controllers;

public class TransactionController : ControllerBase
{
    [HttpPost("v1/transactions")]
    public async Task<IActionResult> PostAsync(
    [FromBody] CreateTransactionViewModel model,
    [FromServices] SeboDataContext context)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ResultViewModel<Transaction>(ModelState.GetErrors()));
        }

        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        var transaction = new Transaction()
        {
            Id = 0,
            IdBuyer = userId,
            IdSeller = 0,
            IdItem = model.IdItem,
            Date = DateTime.Now,
            Value = model.Value,
        };
        try
        {
            transaction.IdSeller = await context.Items
                .Where(x => x.Id == transaction.IdItem)
                .Select(x => x.SellerId)
                .FirstOrDefaultAsync();
            
            await context.Transactions.AddAsync(transaction);
            await context.SaveChangesAsync();

            return Ok(new ResultViewModel<Transaction>(transaction));
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(400, new ResultViewModel<Item>("Transaction já havia sido cadastrada"));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<Item>("Falha interna do servidor"));
        }
    }
    [HttpGet("v1/transactions/{id:int}")]
    public async Task<IActionResult> GetByUserIdAsync(
        [FromRoute] int id,
        [FromServices] SeboDataContext context)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

        if (id != userId)
        {
            return Forbid();
        }

        var transactions = await context.Transactions
            .Where(x => (x.IdSeller == userId || x.IdBuyer == userId))
            .ToListAsync();

        if (transactions == null)
            return NotFound(new ResultViewModel<Category>("Usuário não possui transações"));

        return Ok(new ResultViewModel<List<Transaction>>(transactions));
    }
}
