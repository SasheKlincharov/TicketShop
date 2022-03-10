using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketShop.Domain.DomainModels;
using TicketShop.Domain.DTO;
using TicketShop.Repository.Interface;
using TicketShop.Services.Interface;

namespace TicketShop.Services.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private IRepository<Cart> _shoppingCartRepository;
        private IUserRepository _userRepository;
        private IRepository<Order> _orderRepository;
        private IRepository<TicketInOrder> _ticketsInOrderRepository;
        private IRepository<OrderEmailMessage> _emailRepository;

        public ShoppingCartService (IRepository<Cart> shoppingCartRepository, IRepository<OrderEmailMessage> emailRepository, IUserRepository userRepository, IRepository<Order> orderRepository, IRepository<TicketInOrder> ticketsInOrderRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _ticketsInOrderRepository = ticketsInOrderRepository;
            _emailRepository = emailRepository;
        }
        public bool deleteTicketFromShoppingCart(string userId, Guid ticketId)
        {
            if (!string.IsNullOrEmpty(userId) && ticketId != null)
            {
                var user = this._userRepository.Get(userId);
                var userCart = user.UserCart;
                var ticketToDelete = userCart.TicketsInShoppingCart.Where(t => t.Ticket.Id.Equals(ticketId)).FirstOrDefault();
                userCart.TicketsInShoppingCart.Remove(ticketToDelete);
                this._shoppingCartRepository.Update(userCart);
                return true;
            }
            return false;
        }

        public CartDTO getShoppingCartInfo(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("Invalid user id!");

            var user = this._userRepository.Get(userId);
            var userCart = user.UserCart;
            var tickets = userCart.TicketsInShoppingCart.ToList();

            double totalPrice = 0.0;

            foreach (var ticket in tickets)
            {
                totalPrice += ticket.Quantity * ticket.Ticket.TicketPrice;
            }

            var shoppingCartDto = new CartDTO
            {
                Tickets = tickets,
                TotalPrice = totalPrice
            };

            return shoppingCartDto;
        }

        public bool orderNow(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("Invalid user id!");

            var user = this._userRepository.Get(userId);

            var userShoppingCart = user.UserCart;

            OrderEmailMessage mail = new OrderEmailMessage();
            mail.MailTo = user.Email;
            mail.Subject = "Successfully created order";
            mail.Status = false;

            Order order = new Order
            {
                Id = Guid.NewGuid(),
                ApplicationUser = user,
                UserId = userId,
                DateOfOrder = DateTime.Now
            };

            this._orderRepository.Insert(order);

            List<TicketInOrder> ticketsToBeOrdered = new List<TicketInOrder>();

            var orderedTickets = userShoppingCart.TicketsInShoppingCart.Select(z => new TicketInOrder
            {
                Id = Guid.NewGuid(),
                TicketId = z.Ticket.Id,
                ticket = z.Ticket,
                OrderId = order.Id,
                order = order,
                Quantity = z.Quantity
            }).ToList();

            StringBuilder sb = new StringBuilder();

            var totalPrice = 0.0;

            sb.AppendLine("Your order has been successfully completed. The order conains: ");

            for (int i = 1; i <= orderedTickets.Count(); i++)
            {
                var oTicket = orderedTickets[i - 1];

                totalPrice += oTicket.Quantity * oTicket.ticket.TicketPrice;

                sb.AppendLine(i.ToString() + ". Ticket for: " +
                    oTicket.ticket.MovieTitle + ", Genre: " +
                    oTicket.ticket.MovieGenre +  " with price of: " +
                    oTicket.ticket.TicketPrice + " and quantity of: " 
                    + oTicket.Quantity);
            }

            sb.AppendLine("Total price: " + totalPrice.ToString());


            mail.Content = sb.ToString();

            ticketsToBeOrdered.AddRange(orderedTickets);

            foreach (var item in ticketsToBeOrdered)
            {
                this._ticketsInOrderRepository.Insert(item);
            }

            user.UserCart.TicketsInShoppingCart.Clear();

            this._userRepository.Update(user);
            this._emailRepository.Insert(mail);

            return true;
        }
    }
}
