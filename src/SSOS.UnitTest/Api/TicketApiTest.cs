using Microsoft.AspNetCore.Mvc;
using SSOS.WebUI.Controllers.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SSOS.UnitTest.Api
{
    public class TicketApiTest : AppConfig
    {
        private readonly TicketController _controller;
        public TicketApiTest()
        {
            _controller = new TicketController(_context);
        }


        [Fact(DisplayName ="ทดสอบดึงข้อมูลวันที่การลงตารางเวลา")]
        public async Task GetFreeBookingDate()
        {
            var result = await _controller.GetFreeBookingDate();
            var actualResult = result as OkObjectResult;
            Assert.Equal(200, actualResult.StatusCode);
        }

        [Theory(DisplayName = "ทดสอบดึงข้อมูลเวลาการลงตารางเวลา")]
        [InlineData("20/04/2564")]
        [InlineData("30/04/2564")]
        public async Task GetFreeBookingTime(string date)
        {
            var result = await _controller.GetFreeBookingTime(date);
            var actualResult = result as OkObjectResult;
            Assert.Equal(200, actualResult.StatusCode);
        }

    }

}
