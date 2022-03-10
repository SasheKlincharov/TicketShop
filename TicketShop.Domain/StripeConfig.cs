using System;
using System.Collections.Generic;
using System.Text;

namespace TicketShop.Domain
{
    public class StripeConfig
    {
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }
    }
}
