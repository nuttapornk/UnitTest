using IOR.WebApi.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IOR.WebApi.UnitTest.TestCase
{
    public class MedIncidentReportTest : AppSetting
    {
        [Fact(DisplayName ="Create Medication incident report success")]
        public async Task CreateMedIncidentReportSuccess()
        {
            Services.MedIncidentReport medIncident = new Services.MedIncidentReport(_context, _appSetting.Value, _medIncidentDefaultValue.Value);
            var user = await AppFunc.GetUserAsync(_context, 1, "014419");
            var model = new Models.MedIncidentCreateModel
            {
                LocationId = 217,
                IncidentDate = "2020-08-02",
                IncidentType = "M3",
                IncidentCode = "M3-6",
                Step = "จัดยา (Pre-dispensing Error)",
                Severity = "A",
                UserId = "014419",
                MedEffType = 1,
                IncidentDrugCode = "IncidentDrugCode",
                IncidentDrugName = "IncidentDrugName",
                IncidentHn = "0000000",
                CorrectDrugCode = "CorrectDrugCode",
                CorrectDrugName = "CorrectDrugName",
                CorrectHn = "1111111",
                Descriptopn = "รายละเอียดเบื้องต้น",
                Correction = "การแก้ไข",
                Effect = "ผลการแก้ไข"
            };
            string docNo = await medIncident.CreateAsync(model, user);
            Assert.NotEmpty(docNo);
        }

        [Theory(DisplayName = "Create Medication incident report fail")]
        [InlineData(999,"2021-08-02","M3","M3-5","A","014419", 1, "location")]
        [InlineData(217, "2120-08-02", "M3", "M3-5", "A", "014419", 1, "Incident date")]
        [InlineData(217, "2021-08-02", "M3", "", "A", "014419", 1, "incident events")]
        [InlineData(217, "2021-08-02", "S1", "", "A", "014419", 1, "incident events")]
        [InlineData(217, "2021-08-02", "M3", "M1-5", "A", "014419", 1, "incident events")]
        [InlineData(217, "2021-08-02", "M3", "M3-5", "X", "014419",1, "severity")]
        [InlineData(217, "2021-08-02", "M3", "M3-5", "A", "014419", 5, "MedEffType")]
        public async Task CreateMedIncidentReportFial(
            int locationId,
            string incidentDate,
            string incidentType,
            string incidentCode,            
            string severity,
            string userInsert,
            int MedEffType,
            string expectResult)
        {
            try
            {
                Services.MedIncidentReport medIncident = new Services.MedIncidentReport(_context, _appSetting.Value, _medIncidentDefaultValue.Value);
                var user = await AppFunc.GetUserAsync(_context, 1, userInsert);
                var model = new Models.MedIncidentCreateModel
                {
                    LocationId = locationId,
                    IncidentDate = incidentDate,
                    IncidentType = incidentType,
                    IncidentCode = incidentCode,
                    Severity = severity,
                    UserId = user.Id,
                    Step = "จัดยา (Pre-dispensing Error)",
                    MedEffType = MedEffType,
                    IncidentDrugCode = "IncidentDrugCode",
                    IncidentDrugName = "IncidentDrugName",
                    IncidentHn = "0000000",
                    CorrectDrugCode = "CorrectDrugCode",
                    CorrectDrugName = "CorrectDrugName",
                    CorrectHn = "1111111",
                    Descriptopn = "รายละเอียดเบื้องต้น",
                    Correction = "การแก้ไข",
                    Effect = "ผลการแก้ไข"
                };
                string docNo = await medIncident.CreateAsync(model, user);
            }
            catch (Exception ex)
            {
                Assert.Contains(expectResult.ToLower() , ex.Message.ToLower());
            }
        }


    }
}
