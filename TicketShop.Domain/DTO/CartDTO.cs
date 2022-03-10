using System;
using System.Collections.Generic;
using System.Text;
using TicketShop.Domain.DomainModels;

namespace TicketShop.Domain.DTO
{
    public class CartDTO
    {
        public List<TicketInShoppingCart> Tickets { get; set; }
        public double TotalPrice { get; set; }

    }
}
