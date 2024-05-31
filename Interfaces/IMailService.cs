using HotelBookingSystem.Helpers;
using HotelBookingSystem.TemplateMails;
using HotelBookingSystem.ViewModels;

namespace HotelBookingSystem.Interfaces
{
    public interface IMailService
    {
        public Task SendEmailAsync(MailRequest mailrequest, mailTemplate mailTemplate, MailAdditionalParamsViewModel? additionalParams);
    }
}
