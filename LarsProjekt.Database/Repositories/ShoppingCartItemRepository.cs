using LarsProjekt.Domain;

namespace LarsProjekt.Database.Repositories;

internal class ShoppingCartItemRepository : IShoppingCartItemRepository
{
    private readonly ApplicationDbContext _context;

    public ShoppingCartItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public List<ShoppingCartItem> GetAll()
    {
        var items = _context.ShoppingCartItems;        
        return items.ToList();
    }

}
