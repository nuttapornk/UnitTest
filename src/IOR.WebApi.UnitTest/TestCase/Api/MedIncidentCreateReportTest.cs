using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;

namespace IOR.WebApi.UnitTest.TestCase.Api
{
    public class MedIncidentCreateReportTest : AppSetting
    {        
        [Theory(DisplayName = "Create incident report success result return Ok(200)")]
        [InlineData(217, "2021-10-02", "M1", "M1-4", "A", "000062", "014419", 1)]// Create incident report with real user and existing in the database.
        [InlineData(217, "2021-10-02", "M2", "M2-4", "A","", "014419", 1)]//Create incident report with real user and Nonexistent in the database (new user).
        public async Task Success(int locationId, string incidentDate, string incidentType, string incidentCode, string severity,string doctorStaffId, string userInsert, int MedEffType)
        {
            var medIncident = new IOR.WebApi.Controllers.MedIncidentController(_context, _appSetting, _medIncidentDefaultValue,_accessor);
            var result = await medIncident.Post(new Models.MedIncidentCreateModel
            {
                LocationId = locationId,
                IncidentDate = incidentDate,
                IncidentType = incidentType,
                IncidentCode = incidentCode,
                Step = "test step",
                Severity = severity,
                UserId = userInsert,
                MedEffType = MedEffType,
                IncidentDrugCode = "IncidentDrugCode",
                IncidentDrugName = "IncidentDrugName",
                IncidentHn = "9999999,1234567",
                CorrectDrugCode = "CorrectDrugCode",
                CorrectDrugName = "CorrectDrugName",
                CorrectHn = "8888888,1234567",
                Description = "รายละเอียดเบื้องต้น test",
                Correction = "การแก้ไข test",
                Effect = "ผลการแก้ไข test",
                DoctorStaffId = doctorStaffId,
            });

            var actualResult = result as OkObjectResult;
            Assert.NotNull(actualResult);
            Assert.Equal(200, actualResult.StatusCode);
        }

        [Theory(DisplayName = "Create medication incident report fail result BadRequest(400)")]
        [InlineData(0,"","","","","",0)]/// 1. Data not valid(Not pass all data).
        [InlineData(40, "", "", "", "", "", 0)]/// Data not valid(Not pass location,severity,Med Eff type)
        [InlineData(217, "2021-08-02", "M1", "M1-1", "A", "099999", 1)]/// User does not exist.
        [InlineData(9999, "2021-08-02", "M1", "M1-1", "A", "014419", 1)]/// Location data existing in the database.
        [InlineData(217, "2021-02-30", "M1", "M1-1", "A", "014419", 1)]/// Incident data wrong date.
        [InlineData(217, "2121-08-02", "M1", "M1-1", "A", "014419", 1)]/// Incident date more than current date.
        [InlineData(217, "2021-08-02", "", "M1-1", "A", "014419", 1)]/// Incident type not pass value.
        [InlineData(217, "2021-08-02", "M1", "", "A", "014419", 1)]/// Incident code not pass value.
        [InlineData(217, "2021-08-02", "M3", "M1-1", "A", "014419", 1)]/// Category differrent between incident type and incident code.
        [InlineData(217, "2021-08-02", "M1", "M1-1", "J", "014419", 1)]/// Severity wrong value( not between A and I).
        [InlineData(217, "2021-08-02", "M1", "M1-1", "A", "014419", 6)]/// MedEffType wrong value value not between(1 and 2).
        public async Task Fail(int locationId, string incidentDate, string incidentType, string incidentCode, string severity, string userInsert, int MedEffType)
        {
            var medIncident = new IOR.WebApi.Controllers.MedIncidentController(_context, _appSetting, _medIncidentDefaultValue,_accessor);
            var result = await medIncident.Post(new Models.MedIncidentCreateModel
            {
                LocationId = locationId,
                IncidentDate = incidentDate,
                IncidentType = incidentType,
                IncidentCode = incidentCode,
                Step = "test step",
                Severity = severity,
                UserId = userInsert,
                MedEffType = MedEffType,
                IncidentDrugCode = "IncidentDrugCode",
                IncidentDrugName = "IncidentDrugName",
                IncidentHn = "9999999,1234567",
                CorrectDrugCode = "CorrectDrugCode",
                CorrectDrugName = "CorrectDrugName",
                CorrectHn = "8888888,1234567",
                Description = "รายละเอียดเบื้องต้น test",
                Correction = "การแก้ไข test",
                Effect = "ผลการแก้ไข test"
            });

            var actualResult = result as ObjectResult;
            //Assert.NotNull(actualResult);
            Assert.Equal(400, actualResult.StatusCode);
        }
    }
}
