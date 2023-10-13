namespace SimpleCrm
{
    public interface ICustomerData
    {
        IEnumerable<Customer> GetAll();
        /// <summary>
        /// Gets paged and sorted records for a given CRM account and status.
        /// </summary>
        /// <param name="pageIndex">The zero based page number</param>
        /// <param name="take">The max number of records to take (page size)</param>
        /// <param name="orderBy">The property name to order by and optional direction. (null for default order)</param>
        /// <returns></returns>
        List<Customer> GetAll(int pageIndex, int take, string orderBy);
        Customer Get(int id);

        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(Customer item);
        void Delete(int id);
        /// <summary>
        /// Saves changes to new or modified customers.
        /// </summary>
        void Commit();
    }
}