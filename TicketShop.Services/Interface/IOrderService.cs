using GemBox.Document;
using System;
using System.Collections.Generic;
using TicketShop.Domain.DomainModels;

namespace TicketShop.Services.Interface
{
    public interface IOrderService
    {
        List<Order> getAllOrders();
        Order getOrderDetails(BaseEntity model);
        DocumentModel CreateOrderInvoice(Guid orderId);
    }
}
