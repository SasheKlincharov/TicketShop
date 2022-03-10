using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketShop.Domain.DomainModels;
using TicketShop.Domain.DTO;
using TicketShop.Services.Interface;

namespace TicketShop.Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;

        private readonly List<SelectListItem> Genres = new List<SelectListItem>{
                    new SelectListItem  { Value = GenreEnum.ACTION.ToString(), Text = GenreEnum.ACTION.ToString()},
                    new SelectListItem  { Value = GenreEnum.COMEDY.ToString(), Text = GenreEnum.COMEDY.ToString() },
                    new SelectListItem  { Value = GenreEnum.DRAMA.ToString(), Text = GenreEnum.DRAMA.ToString() },
                    new SelectListItem  { Value = GenreEnum.HORROR.ToString(), Text = GenreEnum.HORROR.ToString() },
                    new SelectListItem  { Value = GenreEnum.KIDS.ToString(), Text = GenreEnum.KIDS.ToString() },
                    new SelectListItem  { Value = GenreEnum.MISTERY.ToString(), Text = GenreEnum.MISTERY.ToString() },
                    new SelectListItem  { Value = GenreEnum.ROMANCE.ToString(), Text = GenreEnum.ROMANCE.ToString() },
                    new SelectListItem  { Value = GenreEnum.SCI_FI.ToString(), Text = GenreEnum.SCI_FI.ToString() },
                    new SelectListItem  { Value = GenreEnum.THRILLER.ToString(), Text = GenreEnum.THRILLER.ToString() },
                };
        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // GET: Tickets
        public IActionResult Index()
        {
            var allTickets = this._ticketService.GetAllTickets();
            ViewData["Genres"] = Genres;
            return View(allTickets);
        }

        [Authorize]
        public IActionResult AddTicketToCart(Guid? id)
        {
            var model = this._ticketService.GetShoppingCartInfo(id);
            ViewData["Genres"] = Genres;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddTicketToCart([Bind("TicketId", "Quantity")] AddToCartDTO item)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._ticketService.AddTicketToShoppingCart(item, userId);

            if (result)
            {
                return RedirectToAction("Index", "Tickets");
            }

            return View(item);
        }

        // GET: Tickets/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = this._ticketService.GetDetailsForTicket(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            ViewData["Genres"] = Genres;
            return View();
        }

        // POST: Tickest/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Create([Bind("Id,MovieTitle,MovieImage,MovieGenre,Date,TicketPrice, RowNumber, SeatNumber")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                this._ticketService.CreateNewTicket(ticket);
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public IActionResult Edit(Guid? ticketId)
        {
            if (ticketId == null)
            {
                return NotFound();
            }

            var ticket = this._ticketService.GetDetailsForTicket(ticketId);

            if (ticket == null)
            {
                return NotFound();
            }
            ViewData["Genres"] = Genres;
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,MovieTitle,MovieImage,MovieGenre,Date,TicketPrice, RowNumber, SeatNumber")] Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this._ticketService.UpdeteExistingTicket(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = this._ticketService.GetDetailsForTicket(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            this._ticketService.DeleteTicket(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("FilterTicketsByDate")]
        public IActionResult FilterTicketsByDate(DateTime date)
        {
            var tickets = _ticketService.FilterTicketsByDate(date);
            if (tickets != null && tickets.Count != 0)
            {
                return View("Index", tickets);
            }
            ViewData["ErrorMessage"] = "No tickets availbale for the specified date";
            return View("Index", _ticketService.GetAllTickets());
        }

        [HttpGet]
        [Authorize(Roles = Role.ADMIN)]
        public FileContentResult ExportAllTickets()
        {
            string fileName = "Tickets.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var content = _ticketService.ExportAllTickets();
            return File(content, contentType, fileName);
        }

        [HttpPost]
        [Authorize(Roles = Role.ADMIN)]
        public FileContentResult FilterTicketsByGenre(GenreEnum genre)
        {
            string fileName = "Tickets-" + genre.ToString() + ".xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var content = _ticketService.ExportTicketsByGenre(genre);
            return File(content, contentType, fileName);
        }

        private bool TicketExists(Guid id)
        {
            return this._ticketService.GetDetailsForTicket(id) != null;
        }
    }
}
