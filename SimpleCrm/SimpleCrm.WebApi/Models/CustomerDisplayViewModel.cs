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
        public string PreferredContactMethod { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string LastContactDate { get; set; }

        public CustomerDisplayViewModel() { }
        public CustomerDisplayViewModel(Customer source)
        {
            if (source == null) return;
            CustomerId = source.Id;
            FirstName = source.FirstName;
            LastName = source.LastName;
            PhoneNumber = source.PhoneNumber;
            EmailAddress = source.EmailAddress;
            PreferredContactMethod = Enum.GetName(typeof(InteractionMethod), source.PreferredContactMethod);
            Type = Enum.GetName(typeof(CustomerType), source.Type);
            Status = Enum.GetName(typeof(CustomerStatus), source.Status);
            LastContactDate = source.LastContactDate.Year > 1 ? source.LastContactDate.ToString("s", CultureInfo.InstalledUICulture) : "";
        }
    }
}
