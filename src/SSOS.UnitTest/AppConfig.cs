using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SSOS.Infra;
using SSOS.WebUI.Models;

namespace SSOS.UnitTest
{
    public class AppConfig
    {
        protected readonly AppDbContext _context;
        protected readonly IOptions<AppVariable> _appVariable;
        protected readonly IOptions<SmsVariable> _smsVariable;
        protected readonly IOptions<EmailVariable> _emailVariable;

        public AppConfig()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=10.6.118.72;Database=SSOS_Test;user id=Nos12;Password=Info2010;")
                .Options;
            _context = new AppDbContext(options);

            _appVariable = Options.Create<AppVariable>(new AppVariable
            {
                TicketHorizontalSize = 5,
                TicketVerticalSize = 4,
                PageSize = 20,
                DisplayTimeFormat = "hh\\:mm",
                DisplayDateFormat = "dd/MM/yyyy",
                DisplayDatetimeFormat = "dd/MM/yyyy HH:mm"
            });

            _smsVariable = Options.Create<SmsVariable>(new SmsVariable
            {
                Enable = false,
                ApiUri = "http://wsback.rama.mahidol.ac.th/SMSapi/api/SendsmsNows/SendNow",
                UserId = "appSOS",
                SystemApp = "SSOS",
                SystemPass = "5O5S0S"
            });

            _emailVariable = Options.Create<EmailVariable>(new EmailVariable
            {
                Enable = true,
                GeneralMailTemplate = "general-mail-template1.html",
                MailName = "โครงการช่วยเหลือด้านจิตสังคมสำหรับบุคลากร : SSOS",
                MailAddress = "nuttaporn.kun@mahidol.ac.th",
                MailPassword = "poolana445",
                EnableSSL = true,
                Host = "mumail.mahidol.ac.th",
                Port = 587
            });
        }
    }
}