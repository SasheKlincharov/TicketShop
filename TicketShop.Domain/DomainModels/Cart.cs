using System;
using System.Collections.Generic;
using System.Text;
using TicketShop.Domain.Identity;

namespace TicketShop.Domain.DomainModels
{
    public class Cart : BaseEntity
    {
        public string UserId { get; set; }
        public User UserOwner { get; set; }
        public virtual ICollection<TicketInShoppingCart> TicketsInShoppingCart { get; set; }
    }
}
