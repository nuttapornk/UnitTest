using MedMaster.Infra;
using MedMaster.WebUi.Models;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedMaster.UnitTest
{
    public class AppConfig
    {
        protected readonly AppDbContext _context;
        //protected readonly CoSapConfig _coSapconfig;

        protected readonly IOptions<WebUi.Models.AppConfig> _appConfig;
        protected readonly IOptions<CoSapConfig> _coSapConfig;
        protected readonly IOptions<HisConfig> _hisConfig;
        protected readonly IOptions<SmsNoti> _smsNoti;

        const string connectionString = "Server=10.6.118.72;Database=MedMaster;user id=Nos12;Password=Info2010;";

        public AppConfig()
        {
            var option = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options;
            _context = new AppDbContext(option);

            _appConfig = Options.Create<WebUi.Models.AppConfig>(new WebUi.Models.AppConfig
            {
                Lang = "EN",
                SapDateFormat = "dd.MM.yyyy",
                PageSize = 20,
                DisplayTimeFormat = "hh\\:mm",
                DisplayDateFormat = "dd.MM.yyyy",
                DisplayDatetimeFormat = "dd.MM.yyyy HH:mm:ss",
            });

            _coSapConfig = Options.Create<CoSapConfig>(new CoSapConfig
            {
                Base = "http://10.6.118.72:8080/medmastercosap/api/",
                Environment = "Environments",
                TestConnection = "TestConnection",
                GetMatTrans = "NewMats",
                GetInitMats = "InitMats",
                GetMatData = "MatDetails/getData",
                UpdateMatData = "MatDetails/updateData",                
                UpdateClassificationMulti= "Classifications/updateMulti"
            });

            _hisConfig = Options.Create<HisConfig>(new HisConfig
            {
                Detail = new List<HisConfigDetail>
               {
                   new HisConfigDetail
                   {
                       Plant = "1200",
                       Base= "http://10.6.118.72:8080/medmastercosap/api/",
                       Outbound="TestHisMedOutbounds",
                        ApiKeyName="",
                        ApiKeyValue=""
                   }
               }
            });

            _smsNoti = Options.Create<SmsNoti>(new SmsNoti
            {
                Enable = false,
                UserId = "appSOS",
                SystemApp = "SSOS",
                SystemPass = "5O5S0S"
            });

        }       

    }
}
