using System.Linq;
using System.Threading.Tasks;
using TicketShop.Domain.DomainModels;
using TicketShop.Repository.Interface;
using TicketShop.Services.Interface;

namespace TicketShop.Services.Implementation
{
    public class BackgroundEmailSender : IBackgroundEmailSender
    {
        private readonly IEmailService _emailService;
        private readonly IRepository<OrderEmailMessage> _mailRepository;

        public BackgroundEmailSender(IEmailService emailService, IRepository<OrderEmailMessage> mailRepository)
        {
            _emailService = emailService;
            _mailRepository = mailRepository;
        }

        public async Task DoWork()
        {
           //var emails = await _emailService.SendEmailAsync(_mailRepository.GetAll().Where(z => !z.Status).ToList());
           // foreach (var mail in emails)
           // {
           //     if (mail.Status)
           //     {
           //         _mailRepository.Update(mail);
           //     }
           // }
        }
    }
}
