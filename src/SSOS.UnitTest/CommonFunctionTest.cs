using Microsoft.EntityFrameworkCore;
using SSOS.Infra;
using SSOS.WebUI.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SSOS.UnitTest
{
    
    public class CommonFunctionTest:AppConfig
    {

        [Fact(DisplayName ="Get num room")]
        public async Task TestGetNumRoom()
        {
            var actualResult = await Tools.CountRoom(_context);
            Assert.True(actualResult>0);
        }

        [Theory(DisplayName ="ทดสอบการรวมข้อมูลระหว่างวันที่และเวลา")]
        [InlineData("15/01/2564","12:30","2021-01-15 12:30")]
        [InlineData("01/01/2564", "08:00", "2021-01-01 08:00")]
        [InlineData("28/02/2564", "23:59", "2021-02-28 23:59")]
        public void TestCombindDateAndTime(string dateStr,string timeStr,string expectResult)
        {
            var actualResult = Tools.ConvertStringToDate(dateStr, "dd/MM/yyyy", Tools.Lang.Th);
            var time = Tools.ConvertStringToTime(timeStr, "hh\\:mm", Tools.Lang.Th);
            actualResult = actualResult.Add(time);
            Assert.Equal(expectResult, actualResult.ToString("yyyy-MM-dd HH:mm", Tools.EnCulture));
        }

        [Theory(DisplayName = "Check format number")]
        [InlineData(2,"0000","0002")]
        [InlineData(189, "0000", "0189")]
        public void TestFormatDocNo(int num,string format,string expectResult)
        {
            var actualResult = num.ToString(format);
            Assert.Equal(expectResult, actualResult);
        }

        [Theory(DisplayName = "ทดสอบดึงข้อมูลบุคลากร คำนำหน้าชื่อ")]
        [InlineData("014419","นาย")]
        [InlineData("002558", "ศ. พญ.")]
        public async Task TestGetPersonelProfile(string personelId,string expectResult)
        {
            var actualResult = await Tools.GetPersonelProfile(_context, personelId);
            Assert.Equal(actualResult.PreName, expectResult);
        }

        [Theory(DisplayName = "ทดสอบดึงข้อมูลบุคลากร Email")]
        [InlineData("014419", "nuttaporn.kun@mahidol.ac.th")]        
        public async Task TestGetPersonelEmail(string personelId, string expectResult)
        {
            var actualResult = await Tools.GetPersonelProfile(_context, personelId);
            Assert.Equal(actualResult.Email, expectResult);
        }

        [Fact(DisplayName ="ดึงข้อมูลแบบทดสอบ")]
        public async Task GetSurveyScaleSummary()
        {
            DateTime startDate = Tools.ConvertStringToDateTimeFrom("2021-01-01", "yyyy-MM-dd", Tools.Lang.Th);
            DateTime stopDate = DateTime.Now;

            var ticketMeetingSurveys = await _context.TicketMeetingSurveys
                .Include(a => a.Ticket)
                .Where(a => a.Ticket.TimeInsert >= startDate && a.Ticket.TimeInsert <= stopDate)
                .ToListAsync();

            int countPeerSurvey = ticketMeetingSurveys.GroupBy(a => a.TicketId).Count();
            SummaryPeerSurvey summary = new SummaryPeerSurvey(_context, ticketMeetingSurveys, countPeerSurvey);
            var actualResult = await summary.GetScales();
            if (actualResult!=null)
                Assert.True(true);
            else
                Assert.True(false);
        }

    }
}
