using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace SSOS.UnitTest.Api
{
    public class MeetingRemindApiTest : AppConfig
    {
        [Fact(DisplayName = "ทดสอบ ส่งการแจ้งเตือนการเข้าพบ")]
        public async Task SendNoti()
        {
            var meetingRemind = new SSOS.WebUI.Controllers.Api.MeetingRemindController(_context, _smsVariable, _emailVariable);
            var result = await meetingRemind.Get();

            var actualResult = result as OkResult;
            //Assert.Null(actualResult);
            Assert.Equal(200, actualResult.StatusCode);
        }
    }
}