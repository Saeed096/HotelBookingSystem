using HotelBookingSystem.Helpers;
using HotelBookingSystem.Interfaces;
using HotelBookingSystem.TemplateMails;
using HotelBookingSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSystem.Controllers
{
  
        public class MailController : Controller
        {
            private readonly IMailService mailService;


            public MailController(IMailService _mailService)
            {
                mailService = _mailService;
            }


            public async Task<IActionResult> SendMail(string emailTo, string callBackUrl, string userName) // should be named send forget pass email  // try recieve MailRequest mailrequest instead // saeed
            {
                MailRequest mailRequest = new MailRequest() { ToEmail = emailTo, Subject = "Reset password" };
                try
                {
                    ResetPasswordTempelate resetPasswordTempelate = new ResetPasswordTempelate();
                    await mailService.SendEmailAsync(mailRequest, resetPasswordTempelate, new MailAdditionalParamsViewModel()
                    { callBackUrl = callBackUrl, userName = userName });
                    return View("confirmSendingForgetPasswordEmail");
                }
                catch (Exception ex)
                {
                    return Content(ex.ToString());
                }
            }


            [HttpGet]
            public async Task<IActionResult> SendForceEmailConfirmationMail(string? toEmail) // try recieve MailRequest mailrequest instead // saeed
            {

                string callBackUrl = Url.Action("mailConfirmed", "account", values: new { email = toEmail },
                         protocol: Request.Scheme);

                MailRequest mailRequest = new MailRequest() { ToEmail = toEmail, Subject = "Email confirmation" };
                try
                {
                    forceEmailConfirmationTemplate forceEmailConfirmationTemplate = new forceEmailConfirmationTemplate();

                    await mailService.SendEmailAsync(mailRequest, forceEmailConfirmationTemplate,
                        new MailAdditionalParamsViewModel()
                        { callBackUrl = callBackUrl });

                    return View("confirmSendingForceConfirmationEmail");
                }
                catch (Exception ex)
                {
                    return Content(ex.ToString());
                }
            }
        }
    }

