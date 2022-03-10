using System;
using System.Collections.Generic;
using System.Text;

namespace TicketShop.Domain.DomainModels
{
    public class TicketInOrder : BaseEntity
    {
        public Guid TicketId { get; set; }
        public Ticket ticket { get; set; }
        public Guid OrderId { get; set; }
        public Order order { get; set; }
        public int Quantity { get; set; }
    }
}
