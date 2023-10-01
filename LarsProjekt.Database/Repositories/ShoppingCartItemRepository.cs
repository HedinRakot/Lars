using LarsProjekt.Domain;

namespace LarsProjekt.Database.Repositories;

internal class ShoppingCartItemRepository : IShoppingCartItemRepository
{
    private readonly ApplicationDbContext _context;

    public ShoppingCartItemRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public ShoppingCartItem GetItem(int id)
    {
        return _context.ShoppingCartItems.FirstOrDefault(x => x.Id == id);
    }

    public List<ShoppingCartItem> GetAll()
    {
        return _context.ShoppingCartItems.ToList();
    }

    public ShoppingCartItem AddProduct(ShoppingCartItem product)
    {
        _context.ShoppingCartItems.Add(product);
        _context.SaveChanges();
        return product;
    }

    //public ShoppingCartItem AddToCart(Product product)
    //{
    //    var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(x => x.Product
    //    .Id == product.Id); // && x.ShoppingCartId == ShoppingCartId)
    //    if (shoppingCartItem == null)
    //    {
    //        shoppingCartItem = new ShoppingCartItem
    //        {
    //            ShoppingCartId = ShoppingCartId
    //            Product = product,
    //            Amount = 1
    //        };
    //        _context.ShoppingCartItems.Add(shoppingCartItem);
    //    }
    //    else
    //    {
    //        shoppingCartItem.Amount++;
    //    }
    //    _context.SaveChanges();
    //    return shoppingCartItem;
}

