using System;
using System.Collections.Generic;
using System.Text;

namespace TicketShop.Domain.DomainModels
{
    public class TicketInShoppingCart : BaseEntity
    {
        public Guid TicketId { get; set; }
        public Ticket Ticket { get; set; }
        public Guid CartId { get; set; }
        public Cart UserCart { get; set; }
        public int Quantity { get; set; }
    }
}
