using System;
using System.Collections.Generic;
using System.Text;
using TicketShop.Domain.DTO;

namespace TicketShop.Services.Interface
{
    public interface IShoppingCartService
    {
        CartDTO getShoppingCartInfo(string userId);
        bool deleteTicketFromShoppingCart(string userId, Guid ticketId);
        bool orderNow(string userId);
    }
}
