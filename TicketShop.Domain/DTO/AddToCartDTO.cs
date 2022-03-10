using System;
using System.Collections.Generic;
using System.Text;
using TicketShop.Domain.DomainModels;

namespace TicketShop.Domain.DTO
{
    public class AddToCartDTO
    {
        public Ticket SelectedTicket { get; set; }
        public Guid TicketId { get; set; }
        public int Quantity { get; set; }
    }
}
