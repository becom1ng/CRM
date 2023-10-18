namespace SimpleCrm
{
    public class CustomerListParameters
    {
        public int Page { get; set; } = 1;
        public int Take { get; set; } = 50;
        public string OrderBy { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public CustomerType? Type { get; set; }
        public CustomerStatus? Status { get; set; }
        /// <summary>
        /// Search term to search amoung all searchable fields.
        /// </summary>
        public string Term { get; set; }
    }
}
