using System.Threading.Tasks;

namespace TicketShop.Services.Interface
{
    public interface IBackgroundEmailSender
    {
        Task DoWork();
    }
}
