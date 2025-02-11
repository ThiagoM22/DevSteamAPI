using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevSteamAPI.Data;
using DevSteamAPI.Model;

namespace DevSteamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemCarrinhosController : ControllerBase
    {
        private readonly DevSteamAPIContext _context;

        public ItemCarrinhosController(DevSteamAPIContext context)
        {
            _context = context;
        }

        // GET: api/ItemCarrinhos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemCarrinho>>> GetItemCarrinho()
        {
            return await _context.ItemCarrinho.ToListAsync();
        }

        // GET: api/ItemCarrinhos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemCarrinho>> GetItemCarrinho(Guid id)
        {
            var itemCarrinho = await _context.ItemCarrinho.FindAsync(id);

            if (itemCarrinho == null)
            {
                return NotFound();
            }

            return itemCarrinho;
        }

        // PUT: api/ItemCarrinhos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItemCarrinho(Guid id, ItemCarrinho itemCarrinho)
        {
            if (id != itemCarrinho.ItemCarrinhoId)
            {
                return BadRequest();
            }

            _context.Entry(itemCarrinho).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemCarrinhoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ItemCarrinhos
        [HttpPost]
        public async Task<ActionResult<ItemCarrinho>> PostItemCarrinho(ItemCarrinho itemCarrinho)
        {
            itemCarrinho.Total = itemCarrinho.Quantidade * itemCarrinho.Valor;
            _context.ItemCarrinho.Add(itemCarrinho);
            await _context.SaveChangesAsync();

            await UpdateCarrinhoTotal(itemCarrinho.CarrinhoId);

            return CreatedAtAction("GetItemCarrinho", new { id = itemCarrinho.ItemCarrinhoId }, itemCarrinho);
        }

        // DELETE: api/ItemCarrinhos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemCarrinho(Guid id)
        {
            var itemCarrinho = await _context.ItemCarrinho.FindAsync(id);
            if (itemCarrinho == null)
            {
                return NotFound();
            }

            _context.ItemCarrinho.Remove(itemCarrinho);
            await _context.SaveChangesAsync();

            await UpdateCarrinhoTotal(itemCarrinho.CarrinhoId);

            return NoContent();
        }

        // GET: api/ItemCarrinhos/CalculateItemTotal/5
        [HttpGet("CalculateItemTotal/{id}")]
        public async Task<ActionResult<decimal>> CalculateItemTotal(Guid id)
        {
            var itemCarrinho = await _context.ItemCarrinho.FindAsync(id);
            if (itemCarrinho == null)
            {
                return NotFound();
            }

            itemCarrinho.Total = itemCarrinho.Quantidade * itemCarrinho.Valor;
            return itemCarrinho.Total ?? 0;
        }

        // GET: api/ItemCarrinhos/CalculateCarrinhoTotal/5
        [HttpGet("CalculateCarrinhoTotal/{carrinhoId}")]
        public async Task<ActionResult<decimal>> CalculateCarrinhoTotal(Guid carrinhoId)
        {
            var carrinho = await _context.Carrinho.FindAsync(carrinhoId);
            if (carrinho == null)
            {
                return NotFound();
            }

            var itensCarrinho = await _context.ItemCarrinho
                .Where(ic => ic.CarrinhoId == carrinhoId)
                .ToListAsync();

            carrinho.Total = itensCarrinho.Sum(ic => ic.Total ?? 0);
            return carrinho.Total;
        }

        private async Task UpdateCarrinhoTotal(Guid carrinhoId)
        {
            var carrinho = await _context.Carrinho.FindAsync(carrinhoId);
            if (carrinho != null)
            {
                var itensCarrinho = await _context.ItemCarrinho
                    .Where(ic => ic.CarrinhoId == carrinhoId)
                    .ToListAsync();

                carrinho.Total = itensCarrinho.Sum(ic => ic.Total ?? 0);
                await _context.SaveChangesAsync();
            }
        }

        private bool ItemCarrinhoExists(Guid id)
        {
            return _context.ItemCarrinho.Any(e => e.ItemCarrinhoId == id);
        }
    }
}
