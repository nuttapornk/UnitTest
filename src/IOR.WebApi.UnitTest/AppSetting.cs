using IOR.Infra;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IOR.WebApi.UnitTest
{
    public class AppSetting
    {
        protected AppDbContext _context;
        protected IOptions<Models.AppSetting> _appSetting;
        protected IOptions<Models.MedIncidentDefaultValue> _medIncidentDefaultValue;
        protected IActionContextAccessor _accessor;

        const string connectionString = "server=10.6.165.60;Database=IOR;user id=;Password=;";
        protected const string urlGetVictim = "";
        public AppSetting()
        {
            var option = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            _context = new AppDbContext(option);

            _appSetting = Options.Create<Models.AppSetting>(new Models.AppSetting
            {
                Lang = "EN",
                ApiFormatDate = "yyyy-MM-dd",
                ApiFormatDateTime = "yyyy-MM-dd HH:mm",
                UrlVictimDetail = urlGetVictim,
                AesIv = "",
                AesKey = ""
            });

            _medIncidentDefaultValue = Options.Create<Models.MedIncidentDefaultValue>(new Models.MedIncidentDefaultValue
            {
                ComId = 1,
                VictimTypeId = 6,
                EventCode = "M",
                ClinicTypeId = 1,
                GeneralClinicId = 1,
                DocStatusClose = 4,
                DocStatusApprovePending = 1
            });
        }

    }
}