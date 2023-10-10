namespace SimpleCrm.SqlDbServices
{
    public class SqlCustomerData : ICustomerData
    {
        private SimpleCrmDbContext _context;

        public SqlCustomerData(SimpleCrmDbContext context)
        {
            _context = context;
        }

        public Customer Get(int id)
        {
            return _context.Customers.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Customer> GetAll()
        {
            // Expensive! TODO: Change to a more efficient implementation. (Use IQueryable?)
            return _context.Customers.ToList();
        }

        public void Add(Customer customer)
        {
            //customer.Id = _customers.Max(x => x.Id) + 1; // DB will auto assign customer.Id
            _context.Customers.Add(customer);
        }

        public void Update(Customer customer)
        {
            _context.Customers.Update(customer);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
