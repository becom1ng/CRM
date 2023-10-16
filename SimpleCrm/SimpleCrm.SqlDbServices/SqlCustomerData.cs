using System.Linq.Dynamic.Core;
using System.Runtime.CompilerServices;

namespace SimpleCrm.SqlDbServices
{
    public class SqlCustomerData : ICustomerData
    {
        private SimpleCrmDbContext _context;

        public SqlCustomerData(SimpleCrmDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Customer> GetAll()
        {
            // Expensive! TODO: Change to a more efficient implementation. (Use IQueryable?)
            return _context.Customers.ToList();
        }
        
        public List<Customer> GetAll(int pageIndex, int take, string orderBy)
        {
            var sortFields = new string[] { "FIRSTNAME", "LASTNAME", "TYPE", "STATUS", "LASTCONTACTDATE" };
            var sortDirection = new string[] { "ASC", "DESC" };

            var clauses = (orderBy ??= "").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var clause in clauses)
            {
                var items = clause.ToUpper().Split(' ');

                if (items.Length > 2) throw new ArgumentException("Invalid search! ", items.ToString()); // catch invalid length
                if (!sortFields.Contains(items[0])) throw new ArgumentException("Invalid search field. ", items[0].ToString());
                if (items.Length == 2 && !sortFields.Contains(items[1])) throw new ArgumentException("Invalid sort direction. ", items[1].ToString());
            }
            if (String.IsNullOrWhiteSpace(orderBy)) { orderBy = "LastName ASC"; }
            return _context.Customers
                .OrderBy(orderBy)
                .Skip(pageIndex * take)
                .Take(take)
                .ToList();
        }

        public Customer Get(int id)
        {
            return _context.Customers.FirstOrDefault(x => x.Id == id);
        }


        public void Add(Customer customer)
        {
            _context.Customers.Add(customer); // DB will auto assign customer.Id
        }

        public void Update(Customer customer)
        {
            //No need since changes are tracked by Entity Framework
            _context.Customers.Update(customer);
        }

        public void Delete(Customer item)
        {
            _context.Remove(item); // Hard delete
        }

        public void Delete(int id) // unused, but added for future flexibility/support
        {
            var cust = new Customer { Id = id };
            _context.Attach(cust);
            _context.Remove(cust);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

    }
}
