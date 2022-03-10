using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using TicketShop.Domain.DomainModels;
using TicketShop.Domain.Identity;

namespace TicketShop.Repository
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<TicketInShoppingCart> TicketInShoppingCarts { get; set; }
        public virtual DbSet<TicketInOrder> TicketInOrders { get; set; }
        public virtual DbSet<OrderEmailMessage> EmailMessages { get; set; }
 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = Role.STANDARD_USER, NormalizedName = Role.STANDARD_USER, Id = Guid.NewGuid().ToString(), ConcurrencyStamp = Guid.NewGuid().ToString() });
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Name = Role.ADMIN, NormalizedName = Role.ADMIN, Id = Guid.NewGuid().ToString(), ConcurrencyStamp = Guid.NewGuid().ToString() });

            builder.Entity<Ticket>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Cart>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Order>()
                .Property(o => o.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<TicketInShoppingCart>()
               .HasOne(tisc => tisc.Ticket)
               .WithMany(t => t.TicketsInShoppingCart)
               .HasForeignKey(tisc => tisc.TicketId);

            builder.Entity<TicketInShoppingCart>()
                .HasOne(tisc => tisc.UserCart)
                .WithMany(uc => uc.TicketsInShoppingCart)
                .HasForeignKey(tisc => tisc.CartId);

            builder.Entity<TicketInOrder>()
                .HasOne(tio => tio.ticket)
                .WithMany(t => t.TicketsInOrder)
                .HasForeignKey(tio => tio.TicketId);

            builder.Entity<TicketInOrder>()
                .HasOne(tio => tio.order)
                .WithMany(o => o.TicketsInOrder)
                .HasForeignKey(tio => tio.OrderId);

            builder.Entity<Cart>()
                .HasOne<User>(z => z.UserOwner)
                .WithOne(z => z.UserCart)
                .HasForeignKey<Cart>(z => z.UserId);

        }

    }
}
