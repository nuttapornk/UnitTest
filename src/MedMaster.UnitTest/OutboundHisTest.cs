using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MedMaster.UnitTest
{
    public class OutboundHisTest : AppConfig
    {
        private readonly WebUi.Controllers.Api.OutboundsController outbounds;
        public OutboundHisTest()
        {
            outbounds = new WebUi.Controllers.Api.OutboundsController(
                   context: _context,
                   appConfig: _appConfig,
                   coSapConfig: _coSapConfig,
                   hisConfig: _hisConfig,
                   smsNoti: _smsNoti,
                   accessor: null);
        }

        [Theory(DisplayName ="outbound material master data to mock HIS web api")]
        [InlineData("04.01.2022", "04.01.2022")]
        [InlineData("05.01.2022", "05.01.2022")]
        public async Task Outbound(string fdate,string tdate)
        {
            var result = await outbounds.Get(fdate, tdate);
            var actualResult = result as OkObjectResult;

            Assert.NotNull(actualResult);
            Assert.Equal(200, actualResult.StatusCode);
        }
    }
}
