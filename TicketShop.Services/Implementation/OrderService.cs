using GemBox.Document;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TicketShop.Domain.DomainModels;
using TicketShop.Repository.Interface;
using TicketShop.Services.Interface;

namespace TicketShop.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        public List<Order> getAllOrders()
        {
            return this._orderRepository.getAllOrders();
        }

        public Order getOrderDetails(BaseEntity model)
        {
            return this._orderRepository.getOrderDetails(model);
        }

        public DocumentModel CreateOrderInvoice(Guid orderId)
        {
            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "InvoiceTemplate.docx");
            var document = DocumentModel.Load(templatePath);
            var order = this.getOrderDetails(new BaseEntity { Id = orderId});

            document.Content.Replace("{{OrderNumber}}", order.Id.ToString());
            document.Content.Replace("{{OrderDate}}", order.DateOfOrder.ToString());
            document.Content.Replace("{{ClientEmail}}", order.ApplicationUser.Email);

            StringBuilder sb = new StringBuilder();
            var totalPrice = 0.0;
            var numTickets = 0;
            foreach (var ticket in order.TicketsInOrder)
            {
                totalPrice += ticket.ticket.TicketPrice * ticket.Quantity;
                sb.Append("Ticket for movie: " + ticket.ticket.MovieTitle);
                sb.Append("\tDate of projection: " + ticket.ticket.Date.ToString());
                sb.Append("\tRow number: " + ticket.ticket.RowNumber.ToString() + "\tSeat number: " + ticket.ticket.SeatNumber);
                sb.Append("\tPrice: " + ticket.ticket.TicketPrice.ToString() + "\n");
                numTickets += ticket.Quantity;
            }

            document.Content.Replace("{{TicketList}}", sb.ToString());
            document.Content.Replace("{{NumTicketsOrdered}}", numTickets.ToString());
            document.Content.Replace("{{TotalPrice}}", totalPrice.ToString());

            return document;
        }
    }
}
