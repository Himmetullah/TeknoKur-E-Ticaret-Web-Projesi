using Iyzipay.Model;

namespace UI.Customer.Entities
{
    public class TPersonContact : Buyer
    {
        public Address Address { get; set; } = new();
    }
}
