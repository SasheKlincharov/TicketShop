﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TicketShop.Domain.DomainModels
{
    public class OrderEmailMessage : BaseEntity
    {
        public string MailTo { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Boolean Status { get; set; }
    }
}
