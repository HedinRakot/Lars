//using LarsProjekt.Application;
//using LarsProjekt.Domain;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.IdentityModel.Protocols.OpenIdConnect;

//namespace LarsProjekt.Database.Repositories;

//internal class ShoppingCartRepository : IShoppingCartRepository
//{
//    private readonly ApplicationDbContext _context;

//    public ShoppingCartRepository(ApplicationDbContext context)
//    {
//        _context = context;
//    }

    //public string ShoppingCartId { get; set; }
    //public List<ShoppingCartItemRepository> Items { get; set; }

    //public static ShoppingCart GetCart(IServiceProvider services)
    //{
    //    ISession session = services.GetRequiredService<IHttpContextAccessor>()?
    //        .HttpContext.Session;
    //    var context = services.GetService<ApplicationDbContext>();
    //    string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
    //    session.SetString("CartId", cartId);
    //    return new ShoppingCart(context) { ShoppingCartId = cartId };


    //public void AddToCart(Product product, int Amount)
    //{
    //    var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(x => x.Product
    //    .Id == product.Id); // && x.ShoppingCartId == ShoppingCartId)
    //    if (shoppingCartItem == null)
    //    {
    //        shoppingCartItem = new ShoppingCartItem
    //        {
    //            //ShoppingCartId=ShoppingCartId
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
    //}
    //public int RemoveFromCart(Product product)
    //{
    //    var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(x => x.Product
    //    .Id == product.Id); // && x.ShoppingCartId == ShoppingCartId)

    //    var localAmount = 0;
    //    if (shoppingCartItem != null)
    //    {
    //        if (shoppingCartItem.Amount > 1)
    //        {
    //            shoppingCartItem.Amount--;
    //            localAmount = shoppingCartItem.Amount;
    //        }
    //        else
    //        {
    //            _context.ShoppingCartItems.Remove(shoppingCartItem);
    //        }
    //    }
    //    _context.SaveChanges();
    //    return localAmount;

    //}

    //public List<ShoppingCartItem> GetItems()
    //{
    //    var items = _context.ShoppingCartItems;
    //    return items.ToList();
    //}

