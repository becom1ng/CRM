using System.Globalization;

namespace SimpleCrm.WebApi.Models
{
    public class CustomerDisplayViewModel
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public CustomerType Type { get; set; }
        public InteractionMethod PreferredContactMethod { get; set; }
        public CustomerStatus Status { get; set; }
        public DateTimeOffset LastContactDate { get; set; }
        public DateTimeOffset LastModified { get; set; }

        public CustomerDisplayViewModel() { }
        public CustomerDisplayViewModel(Customer source)
        {
            if (source == null) return;
            CustomerId = source.Id;
            FirstName = source.FirstName;
            LastName = source.LastName;
            PhoneNumber = source.PhoneNumber;
            EmailAddress = source.EmailAddress;
            PreferredContactMethod = source.PreferredContactMethod;
            Type = source.Type;
            Status = source.Status;
            LastContactDate = source.LastContactDate;
            LastModified = source.LastModified;
        }
    }
}
