namespace SimpleCrm
{
    public class InMemoryCustomerData : ICustomerData
    {
        static IList<Customer> _customers; //not thread safe - only ok for development, single user

        static InMemoryCustomerData()
        {
            _customers = new List<Customer>
                  {
                      new Customer { Id =1, FirstName ="Bob", LastName = "Jones", PhoneNumber = "555-555-2345" },
                      new Customer { Id =2, FirstName ="Jane", LastName = "Smith", PhoneNumber = "555-555-5256" },
                      new Customer { Id =3, FirstName ="Mike", LastName = "Doe", PhoneNumber = "555-555-8547" },
                      new Customer { Id =4, FirstName ="Karen", LastName = "Jamieson", PhoneNumber = "555-555-9134" },
                      new Customer { Id =5, FirstName ="James", LastName = "Dean", PhoneNumber = "555-555-7245" },
                      new Customer { Id =6, FirstName ="Michelle", LastName = "Leary", PhoneNumber = "555-555-3457" }
                  };
        }

        public Customer Get(int id)
        {
            return _customers.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _customers;
        }

        public List<Customer> GetAll(int pageIndex, int take, string orderBy)
        {
            return _customers
                .Skip(pageIndex * take) // bypasses a specified number of elements; take = page size
                .Take(take) // get only this many items
                .ToList(); // convert IEnumerable from prior methods to list
                // TODO : orderBy
        }

        public void Add(Customer customer)
        {
            customer.Id = _customers.Max(x => x.Id) + 1;
            _customers.Add(customer);
        }

        public void Update(Customer customer)
        {
            var saved = _customers.FirstOrDefault(x => x.Id == customer.Id);
            _customers.Remove(saved);
            _customers.Add(customer);
        }

        public void Delete(Customer item)
        {
            _customers.Remove(item);
        }

        public void Delete(int id)
        {
            _customers.Remove(_customers[id]);
        }

        public void Commit()
        {
            // Unused?! Add and update handle all changes, there is no dbcontext requiring a savechanges.
            return;
        }
    }
}