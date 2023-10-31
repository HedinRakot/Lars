using LarsProjekt.Database.Repositories;

namespace LarsProjekt.Database
{
    internal class SqlUnitOfWork : ISqlUnitOfWork
    {
        private ApplicationDbContext _context;
        private IAddressRepository _addressRepository;
        private IOrderRepository _orderRepository;
        private IOrderDetailRepository _orderDetailRepository;
        private IProductRepository _productRepository;
        private IUserRepository _userRepository;
        public SqlUnitOfWork(ApplicationDbContext context) 
        {
            _context = context;
            _addressRepository = new AddressRepository(context);
            _orderRepository = new OrderRepository(context);
            _orderDetailRepository = new OrderDetailRepository(context);
            _productRepository = new ProductRepository(context);
            _userRepository = new UserRepository(context);
        }

        public IAddressRepository AddressRepository => _addressRepository;
        public IOrderRepository OrderRepository => _orderRepository;
        public IOrderDetailRepository OrderDetailRepository => _orderDetailRepository;
        public IProductRepository ProductRepository => _productRepository;
        public IUserRepository UserRepository => _userRepository;

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
