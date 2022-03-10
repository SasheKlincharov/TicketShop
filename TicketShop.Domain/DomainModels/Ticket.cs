using System;
using System.Collections.Generic;
using System.Text;

namespace TicketShop.Domain.DomainModels
{
    public class Ticket : BaseEntity
    {
        public string MovieTitle { get; set; }
        public string MovieImage { get; set; }
        public GenreEnum MovieGenre { get; set; }
        public DateTime Date { get; set; }
        public double TicketPrice { get; set; }
        public int RowNumber { get; set; }
        public int SeatNumber { get; set; }
        public virtual ICollection<TicketInShoppingCart> TicketsInShoppingCart { get; set; }
        public IEnumerable<TicketInOrder> TicketsInOrder { get; set; }
    }
}
