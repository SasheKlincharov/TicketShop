﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TicketShop.Domain.Identity.DTO
{
    public class UserAddOrRemoveRole
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }

    }
}
