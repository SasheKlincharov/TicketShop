﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TicketShop.Domain.Identity;
using TicketShop.Repository.Interface;

namespace TicketShop.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<User> entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<User>();
        }
        public IEnumerable<User> GetAll()
        {
            return entities.AsEnumerable();
        }

        public User Get(string id)
        {
            return entities
               .Include(z => z.UserCart)
               .Include("UserCart.TicketsInShoppingCart")
               .Include("UserCart.TicketsInShoppingCart.Ticket")
               .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(User entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Cannot insert null user");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(User entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Cannot update null user");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(User entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Cannot delete null user");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }

    }
}
