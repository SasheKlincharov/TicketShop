using System;
using System.Collections.Generic;
using System.Text;
using TicketShop.Domain.DomainModels;
using TicketShop.Domain.DTO;

namespace TicketShop.Services.Interface
{
    public interface ITicketService
    {
        List<Ticket> GetAllTickets();
        Ticket GetDetailsForTicket(Guid? id);
        void CreateNewTicket(Ticket ticket);
        void UpdeteExistingTicket(Ticket ticket);
        AddToCartDTO GetShoppingCartInfo(Guid? id);
        void DeleteTicket(Guid id);
        bool AddTicketToShoppingCart(AddToCartDTO item, string userID);
        List<Ticket> FilterTicketsByDate(DateTime date);
        public byte[] ExportAllTickets();
        public byte[] ExportTicketsByGenre(GenreEnum genre);
    }
}
