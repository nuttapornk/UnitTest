using Microsoft.AspNetCore.Mvc;
using SSOS.WebUI.Models;
using System.Threading.Tasks;
using Xunit;

namespace SSOS.UnitTest.Api
{
    public class HrStaffProfileTest : AppConfig
    {
        [Theory(DisplayName = "ทดสอบอ่านข้อมูล Personnel Profile มีข้อมูล")]
        [InlineData("014419", "014419")]
        [InlineData("009986", "009986")]
        public async Task GetProfile(string id,string personnelId)
        {
            var controller = new SSOS.WebUI.Controllers.Api.HrStaffProfileController(_context);
            var result = await controller.GetPersonelProfile(id);
            var actualResult = result as OkObjectResult;
            var personelProfile = (PersonelProfile)actualResult.Value;

            Assert.Equal(200, actualResult.StatusCode);
            Assert.Equal(personnelId, personelProfile.Id);
        }

        [Theory(DisplayName = "ทดสอบอ่านข้อมูล Personnel Profile กรณีไม่มีข้อมูล")]
        [InlineData("014417")]
        [InlineData("014418")]
        public async Task GetProfileEmpty(string id)
        {
            var controller = new SSOS.WebUI.Controllers.Api.HrStaffProfileController(_context);
            var result = await controller.GetPersonelProfile(id);
            var actualResult = result as OkObjectResult;
            Assert.True(actualResult==null);
        }
    }
}