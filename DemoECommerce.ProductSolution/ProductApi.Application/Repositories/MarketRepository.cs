using eCommerce.SharedLibrary.Interface;
using eCommerce.SharedLibrary.Logs;
using eCommerce.SharedLibrary.Response;
using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Interfaces;
using ProductApi.Infrastructure;
using System.Linq.Expressions;

public class MarketRepository : IMarketRepository
{
    private readonly MarketDbContext _context;

    public MarketRepository(MarketDbContext context)
    {
        _context = context;
    }

    public async Task<Response> CreateAsync(Market entity)
    {
        try
        {
            await _context.Markets.AddAsync(entity);
            await _context.SaveChangesAsync(); // Assurez-vous que cette ligne est bien exécutée

            return new Response(true, "Market created successfully.");
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex); // Facultatif : Journalisez les erreurs
            return new Response(false, "An error occurred while creating the market.");
        }
    }



    public async Task<Response> UpdateAsync(Market entity)
    {
        try
        {
            _context.Markets.Update(entity);
            await _context.SaveChangesAsync();

            return new Response(true, "Market updated successfully.");
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "An error occurred while updating the market.");
        }
    }

    public async Task<Response> DeleteAsync(long id)
    {
        try
        {
            Market M= await _context.Markets.FindAsync(id);
            _context.Markets.Remove(M);
            await _context.SaveChangesAsync();

            return new Response(true, "Market deleted successfully.");
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "An error occurred while deleting the market.");
        }
    }

    public async Task<IEnumerable<Market>> GetAllAsync()
    {
        return await _context.Markets.ToListAsync();
    }

    public async Task<Market> FindByIdAsync(long id)
    {
        return await _context.Markets.FindAsync(id);
    }
   

}
