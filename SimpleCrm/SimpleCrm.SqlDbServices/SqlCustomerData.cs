using System.Linq.Dynamic.Core;

namespace SimpleCrm.SqlDbServices
{
    public class SqlCustomerData : ICustomerData
    {
        private SimpleCrmDbContext _context;

        public SqlCustomerData(SimpleCrmDbContext context)
        {
            _context = context;
        }
        
        public List<Customer> GetAll(CustomerListParameters listParameters)
        {
            var sortFields = new string[] { "FIRSTNAME", "LASTNAME", "TYPE", "STATUS" };
            var sortDirection = new string[] { "ASC", "DESC" };
            var orderBy = listParameters.OrderBy;

            /// <summary>
            /// Validates OrderBy request.
            /// </summary>
            var clauses = (orderBy ??= "").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (var clause in clauses)
            {
                var items = clause.ToUpper().Split(' ');

                if (items.Length > 2) throw new ArgumentException("Invalid search! ", items.ToString()); // catch invalid length
                if (!sortFields.Contains(items[0])) throw new ArgumentException("Invalid search field. ", items[0].ToString());
                if (items.Length == 2 && !sortFields.Contains(items[1])) throw new ArgumentException("Invalid sort direction. ", items[1].ToString());
            }
            if (string.IsNullOrWhiteSpace(orderBy)) { orderBy = "LastName ASC"; }

            //  calls can be chained onto sortedResults before passing to the database
            IQueryable<Customer> sortedResults = _context.Customers
                .OrderBy(orderBy); //validated above

            /// <summary>
            /// Filter based on querystring params
            /// </summary>
            if (!string.IsNullOrWhiteSpace(listParameters.LastName))
            {
                sortedResults = sortedResults
                    .Where(x => x.LastName.Contains(listParameters.LastName.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(listParameters.FirstName))
            {
                sortedResults = sortedResults
                    .Where(x => x.FirstName.Contains(listParameters.FirstName.Trim()));
            }
            if (listParameters.Type != null)
            {
                sortedResults = sortedResults
                    .Where(x => x.Type == listParameters.Type);
            }
            if (listParameters.Status != null)
            {
                sortedResults = sortedResults
                    .Where(x => x.Status == listParameters.Status);
            }

            /// <summary>
            /// Search term that will search all the name and email fields
            /// </summary>
            if (!string.IsNullOrWhiteSpace(listParameters.Term))
            {
                sortedResults = sortedResults
                    .Where(x => (x.FirstName + " " + x.LastName).Contains(listParameters.Term)
                    || x.EmailAddress.Contains(listParameters.Term));
            }

            return sortedResults
                .Skip((listParameters.Page - 1) * listParameters.Take)
                .Take(listParameters.Take)
                .ToList();
            // once an IQueryable is converted into an IList/List, the SQL query is finalized and sent to the database
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
