using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TicketShop.Domain.DomainModels;
using TicketShop.Repository.Interface;

namespace TicketShop.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {

        private readonly ApplicationDbContext context;
        private DbSet<Order> orders;

        public OrderRepository(ApplicationDbContext context)
        {
            this.context = context;
            orders = context.Set<Order>();
        }

        public List<Order> getAllOrders()
        {
            return orders
                .Include(o => o.ApplicationUser)
                .Include(o => o.TicketsInOrder)
                .Include("TicketsInOrder.ticket")
                .ToListAsync().Result;
        }

        public Order getOrderDetails(BaseEntity model)
        {
            return orders
               .Include(o => o.ApplicationUser)
               .Include(o => o.TicketsInOrder)
               .Include("TicketsInOrder.ticket")
               .SingleOrDefaultAsync(o => o.Id == model.Id).Result;
        }
    }
}
