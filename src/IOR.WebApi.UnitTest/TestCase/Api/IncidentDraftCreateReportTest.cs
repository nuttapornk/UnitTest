using IOR.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IOR.WebApi.UnitTest.TestCase.Api
{
    public class IncidentDraftCreateReportTest : AppSetting
    {
        [Theory(DisplayName = "Create incident report success draft status return Ok(200) Success = true")]
        [InlineData("2022-09-13 13:00",2, "f5UFJVM2HC9JQhnqEAHvfw==", "test-01234-56789", "hIWntAaQNyY62+zMhBd1Bw==")]// Create incident report with real user and existing in the database.
        [InlineData("2022-09-14 14:01", 4, "lkxuI39A+wi4K8jd9vP6wcAHrfBWTMs9xcc9fwgD32nHKfTy/zu6oXx6eDzEdpJH", "test-01234-56789", "hIWntAaQNyY62+zMhBd1Bw==")]
        [InlineData("2022-09-13 13:00", 2, "f5UFJVM2HC9JQhnqEAHvfw==", "ทดสอบภาษาไทย", "odKExkHX8X3NLgFsM+9nUg==")]//Create incident report with real user and Nonexistent in the database (new user).
        [InlineData("2022-09-12 09:45", 2, "f5UFJVM2HC9JQhnqEAHvfw==", "ทดสอบ", "zLEd5xM/638JsHQnN5FVFw==")]
        [InlineData("2022-09-14 14:02", 2, "f5UFJVM2HC9JQhnqEAHvfw==", "test-01234-56789", "yriu8VtwNrYTmGz43d2zyQ==")]//RAMA RFS RFS021
        [InlineData("2022-09-14 14:03", 2, "94fBQYjbQ9NCa5E2K3v8Ew==", "test-01234-56789", "PwOSioIyfwniVu0jFFTItQ==")]//CNMI RFS G00777
        public async Task Success(string incidentDate,int victimsType,string victimsName,string description,string userId)
        {
            var incident = new IOR.WebApi.Controllers.IncidentController(_context, _appSetting, _accessor);
            var response = await incident.Draft(new Models.IncidentDraftCreateModel
            {
                IncidentDate = incidentDate,
                VictimsType = victimsType,
                VictimsName = victimsName,
                Description = description,
                UserId = userId
            });

            var actualResult = response as OkObjectResult;
            var result = (IncidentCreateResult)actualResult.Value;
            Assert.True(result.Success);
        }

        [Theory(DisplayName = "Create incident report Failure incident date not valid")]
        [InlineData("2022-09-01 24:01", 2, "f5UFJVM2HC9JQhnqEAHvfw==", "test-01234-56789", "hIWntAaQNyY62+zMhBd1Bw==")]
        public async Task FailureIncidentDateNotValid(string incidentDate, int victimsType, string victimsName, string description, string userId)
        {
            var incident = new IOR.WebApi.Controllers.IncidentController(_context, _appSetting, _accessor);
            var response = await incident.Draft(new Models.IncidentDraftCreateModel
            {
                IncidentDate = incidentDate,
                VictimsType = victimsType,
                VictimsName = victimsName,
                Description = description,
                UserId = userId
            });

            var actualResult = response as OkObjectResult;
            var result = (IncidentCreateResult)actualResult.Value;
            Assert.False(result.Success);
        }

        [Theory(DisplayName = "Create incident report Failure incident date more than now")]//วันที่มากกว่า วันปัจจุบัน
        [InlineData(2, "f5UFJVM2HC9JQhnqEAHvfw==", "test-01234-56789", "hIWntAaQNyY62+zMhBd1Bw==")] //userId not found.        
        public async Task FailureIncidentDate( int victimsType, string victimsName, string description, string userId)
        {
            var incident = new IOR.WebApi.Controllers.IncidentController(_context, _appSetting, _accessor);
            var response = await incident.Draft(new Models.IncidentDraftCreateModel
            {
                IncidentDate = DateTime.Now.AddHours(1).ToString(_appSetting.Value.ApiFormatDateTime),
                VictimsType = victimsType,
                VictimsName = victimsName,
                Description = description,
                UserId = userId
            });

            var actualResult = response as OkObjectResult;
            var result = (IncidentCreateResult)actualResult.Value;
            Assert.False(result.Success);
        }

        [Theory(DisplayName = "Create incident report Failure hn,user Id not base64")]
        [InlineData("2022-09-13 13:00", 2, "5026729", "test-01234-56789", "014419")]
        public async Task FailureNotBase64(string incidentDate, int victimsType, string victimsName, string description, string userId)
        {
            var incident = new IOR.WebApi.Controllers.IncidentController(_context, _appSetting, _accessor);
            var response = await incident.Draft(new Models.IncidentDraftCreateModel
            {
                IncidentDate = incidentDate,
                VictimsType = victimsType,
                VictimsName = victimsName,
                Description = description,
                UserId = userId
            });

            var actualResult = response as OkObjectResult;
            var result = (IncidentCreateResult)actualResult.Value;
            Assert.False(result.Success);
        }

        [Theory(DisplayName = "Create incident report Failure no hn data")]
        [InlineData("2022-09-13 13:00",2, "GUhbY4U1sdvjFloFtDmZBg==", "test-01234-56789", "hIWntAaQNyY62+zMhBd1Bw==")] //hn 0000002      
        public async Task FailureHn(string incidentDate, int victimsType, string victimsName, string description, string userId)
        {
            var incident = new IOR.WebApi.Controllers.IncidentController(_context, _appSetting, _accessor);
            var response = await incident.Draft(new Models.IncidentDraftCreateModel
            {
                IncidentDate = incidentDate,
                VictimsType = victimsType,
                VictimsName = victimsName,
                Description = description,
                UserId = userId
            });

            var actualResult = response as OkObjectResult;
            var result = (IncidentCreateResult)actualResult.Value;
            Assert.False(result.Success);
        }

        [Theory(DisplayName = "Create incident report Failure no user id")]
        [InlineData("2022-09-13 13:00", 2, "f5UFJVM2HC9JQhnqEAHvfw==", "test-01234-56789", "m06sFXB1bnXLRPvdDJ/aFg==")] //user id 014418
        public async Task FailureUserId(string incidentDate, int victimsType, string victimsName, string description, string userId)
        {
            var incident = new IOR.WebApi.Controllers.IncidentController(_context, _appSetting, _accessor);
            var response = await incident.Draft(new Models.IncidentDraftCreateModel
            {
                IncidentDate = incidentDate,
                VictimsType = victimsType,
                VictimsName = victimsName,
                Description = description,
                UserId = userId
            });

            var actualResult = response as OkObjectResult;
            var result = (IncidentCreateResult)actualResult.Value;
            Assert.False(result.Success);
        }
    }
}
