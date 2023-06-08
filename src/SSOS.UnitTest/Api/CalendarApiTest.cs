using Microsoft.AspNetCore.Mvc;
using SSOS.WebUI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SSOS.UnitTest.Api
{
    public class CalendarApiTest : AppConfig
    {
        [Theory(DisplayName = "อ่านข้อมูลการลงตารางเวลาทั้งหมด ")]
        [InlineData(2020, 4, true)]
        [InlineData(2021, 10, false)]
        [InlineData(2020, 6, true)]
        public async Task GetAllBookings(int year, int month, bool expectResult)
        {
            var controller = new SSOS.WebUI.Controllers.Api.CalendarController(_context, _appVariable);
            var result = await controller.AllPeerBooking(year, month);
            var actualResult = result as OkObjectResult;
            var peerBookings = (List<CalendarEvent>)actualResult.Value;
            Assert.Equal(200, actualResult.StatusCode);
            Assert.Equal(expectResult, peerBookings.Any());
        }

        [Theory(DisplayName = "อ่านข้อมูลการลงตารางเวลาของPeer ")]
        [InlineData(2021, 4, "014419", true)]
        [InlineData(2021, 10, "014419", false)]
        public async Task GetPeerBookings(int year, int month, string peerId, bool expectResult)
        {
            var controller = new SSOS.WebUI.Controllers.Api.CalendarController(_context, _appVariable);
            var result = await controller.PeerBooking(year, month, peerId);
            var actualResult = result as OkObjectResult;
            var peerBookings = (List<CalendarEvent>)actualResult.Value;
            Assert.Equal(200, actualResult.StatusCode);
            Assert.Equal(expectResult, peerBookings.Any());
        }

        [Theory(DisplayName = "ตรวจสอบวันหยุด")]
        [InlineData(2020, 4, "2020-04-13", true)]
        [InlineData(2020, 4, "2020-04-01", false)]
        [InlineData(2020, 7, "2020-07-06", true)]
        public async Task IsHoliday(int year, int month, string date, bool expectResult)
        {
            var controller = new SSOS.WebUI.Controllers.Api.CalendarController(_context, _appVariable);
            var result = await controller.Holiday(year, month);
            var actualResult = result as OkObjectResult;
            var holidays = (List<CalendarEvent>)actualResult.Value;

            Assert.Equal(200, actualResult.StatusCode);
            Assert.Equal(expectResult, holidays.Any(a => a.Start == date));
        }
    }
}