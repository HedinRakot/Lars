//using LarsProjekt.Domain;

//namespace LarsProjekt.Database.Repositories;

//internal class ShoppingCartRepository : IShoppingCartRepository
//{
//    private readonly ApplicationDbContext _context;

//    public ShoppingCartRepository(ApplicationDbContext context)
//    {
//        _context = context;
//    }

//    public List<ShoppingCart> GetAll()
//    {
//        var carts = _context.ShoppingCarts;
//        return carts.ToList();
//    }

//    public void Add(ShoppingCart cart)
//    {
//        _context.ShoppingCarts.Add(cart);
//        _context.SaveChanges();
//    }

//    public ShoppingCart Get(string id)
//    {
//        return _context.ShoppingCarts.FirstOrDefault(u => u.ShoppingCartId == id);
//    }

//    public void Update(ShoppingCart cart)
//    {
//        _context.ShoppingCarts.Update(cart);
//        _context.SaveChanges();
//    }

//    public void Delete(ShoppingCart cart)
//    {
//        _context.ShoppingCarts.Remove(cart);
//        _context.SaveChanges();
//    }
//}


////private ShoppingCartRepository _repository;
////private ShoppingCartItemRepository _itemRepository;
////public string ShoppingCartId { get; set; }
////public List<ShoppingCartItem> ShoppingCartItems { get; set; }

////public void AddToCart(Product product, int amount)
////{
////    var cartItem = _itemRepository.ShoppingCartItems.SingleOrDefault(
////           c => c.Product.Id == product.Id
////           && c.ShoppingCartId == ShoppingCartId);

////    if (cartItem == null)
////    {
////        cartItem = new ShoppingCartItem
////        {
////            ShoppingCartId = ShoppingCartId,
////            Product = product,
////            Amount = 1
////        };

////        _itemRepository.ShoppingCartItems.Add(cartItem);
////    }
////    else
////    {
////        cartItem.Amount++;
////    }
////}
////public int RemoveFromCart(Product product)
////{
////    var cartItem = _itemRepository.ShoppingCartItems.SingleOrDefault(
////           c => c.Product.Id == product.Id
////           && c.ShoppingCartId == ShoppingCartId);
////    var localAmount = 0;
////    if (cartItem != null)
////    {
////        if (cartItem.Amount > 1)
////        {
////            cartItem.Amount--;
////            localAmount = cartItem.Amount;
////        }
////        else
////        {
////            _itemRepository.ShoppingCartItems.Remove(cartItem);
////        }
////    }
////    return localAmount;
////}
////public void EmptyCart()
////{
////    var cartItems = _itemRepository.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId);
////    foreach (var cartItem in cartItems)
////    {
////        _itemRepository.ShoppingCartItems.Remove(cartItem);
////    }
////}
////public List<ShoppingCartItem> GetCartItems()
////{
////    return _itemRepository.ShoppingCartItems
////        .Where(c => c.ShoppingCartId == ShoppingCartId).ToList();
////}
////public decimal GetTotal()
////{
////    decimal? total = (from ShoppingCartItems in _itemRepository.ShoppingCartItems
////                      where ShoppingCartId == ShoppingCartId
////                      select (int?)ShoppingCartItems.Amount *
////                      ShoppingCartItems.Product.Price).Sum();

////    return total ?? decimal.Zero;
////}