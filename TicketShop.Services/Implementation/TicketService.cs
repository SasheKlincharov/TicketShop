using ClosedXML.Excel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TicketShop.Domain.DomainModels;
using TicketShop.Domain.DTO;
using TicketShop.Repository.Interface;
using TicketShop.Services.Interface;

namespace TicketShop.Services.Implementation
{
    public class TicketService : ITicketService
    {
        private readonly IRepository<Ticket> _ticketRepository;
        private readonly IRepository<TicketInShoppingCart> _ticketInShoppingCartRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<TicketService> _logger;
        public TicketService(IRepository<Ticket> ticketRepository, IRepository<TicketInShoppingCart> ticketInShoppingCartRepository, IUserRepository userRepository, ILogger<TicketService> logger)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _ticketInShoppingCartRepository = ticketInShoppingCartRepository;
            _logger = logger;
        }

        public bool AddTicketToShoppingCart(AddToCartDTO item, string userID)
        {
            var user = this._userRepository.Get(userID);

            var userShoppingCard = user.UserCart;

            if (item.TicketId != null && userShoppingCard != null)
            {
                var ticket = this.GetDetailsForTicket(item.TicketId);

                if (ticket != null)
                {
                    TicketInShoppingCart itemToAdd = new TicketInShoppingCart
                    {
                        Id = Guid.NewGuid(),
                        Ticket = ticket,
                        TicketId = ticket.Id,
                        UserCart = userShoppingCard,
                        CartId = userShoppingCard.Id,
                        Quantity = item.Quantity
                    };

                    this._ticketInShoppingCartRepository.Insert(itemToAdd);
                    _logger.LogInformation("Ticket was successfully added into ShoppingCart");
                    return true;
                }
                return false;
            }
            _logger.LogInformation("Something wnt wrong. TicketId or UserId may be null!");
            return false;
        }

        public void CreateNewTicket(Ticket ticket)
        {
            this._ticketRepository.Insert(ticket);
        }

        public void DeleteTicket(Guid id)
        {
            var ticket = this.GetDetailsForTicket(id);
            this._ticketRepository.Delete(ticket);
        }

        public List<Ticket> GetAllTickets()
        {
            _logger.LogInformation("GetAllTickets was called!");
            return this._ticketRepository.GetAll().ToList();
        }

        public Ticket GetDetailsForTicket(Guid? id)
        {
            return this._ticketRepository.Get(id);
        }

        public AddToCartDTO GetShoppingCartInfo(Guid? id)
        {
            var ticket = this.GetDetailsForTicket(id);
            AddToCartDTO model = new AddToCartDTO
            {
                SelectedTicket = ticket,
                TicketId = ticket.Id,
                Quantity = 1
            };

            return model;
        }
        public List<Ticket> FilterTicketsByDate(DateTime date)
        {
            var tickets = this._ticketRepository.GetAll().Where(t =>
            {
                return t.Date.Date.Equals(date.Date);
            });

            return tickets.ToList();
        }
        public void UpdeteExistingTicket(Ticket ticket)
        {
            this._ticketRepository.Update(ticket);
        }

        public byte[] ExportTicketsByGenre(GenreEnum genre)
        {
            var result = _ticketRepository.GetAll().Where(t=>t.MovieGenre.Equals(genre)).ToList();
            return exportTicketsXlsx(result);
        }
        private byte[] exportTicketsXlsx(List<Ticket> result)
        {
            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Tickets");

                worksheet.Cell(1, 1).Value = "Ticket Id";
                worksheet.Cell(1, 2).Value = "Movie title";
                worksheet.Cell(1, 3).Value = "Movie genre";
                worksheet.Cell(1, 4).Value = "Date and time of projection";
                worksheet.Cell(1, 5).Value = "Ticket price";

                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];

                    worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.MovieTitle;
                    worksheet.Cell(i + 1, 3).Value = item.MovieGenre.ToString();
                    worksheet.Cell(i + 1, 4).Value = item.Date.ToString();
                    worksheet.Cell(i + 1, 5).Value = item.TicketPrice;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return content;
                }

            }
        }
        public byte[] ExportAllTickets()
        {
            var result = _ticketRepository.GetAll().ToList();
            return exportTicketsXlsx(result);
        }

    }
}
