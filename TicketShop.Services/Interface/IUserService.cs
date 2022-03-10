using System;
using System.Collections.Generic;
using System.Text;
using TicketShop.Domain.Identity;

namespace TicketShop.Services.Interface
{
    public interface IUserService
    {
        List<string> createUsersFromFile(string filePath);
        public IEnumerable<User> getAllUsers();
    }
}
