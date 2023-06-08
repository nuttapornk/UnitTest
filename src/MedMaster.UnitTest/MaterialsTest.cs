using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MedMaster.UnitTest
{
    public class MaterialsTest : AppConfig
    {
        private readonly WebUi.Controllers.Api.MaterialsController controller;

        public MaterialsTest()
        {
            controller = new WebUi.Controllers.Api.MaterialsController(context: _context,_appConfig);
        }

        [Fact(DisplayName ="Test controller search materials with post")]
        public async Task GetMaterialsData()
        {
            var resule = await controller.GetData(new List<string> { "amox", "amox1", "monoline" });
            var actualResult = resule as OkObjectResult;
            Assert.NotNull(actualResult);
            Assert.Equal(3, ((List<WebUi.Models.SearchMat>)actualResult.Value).Count);
        }

        [Theory(DisplayName = "Test controller search materials with get")]
        [InlineData("amox","","","",true)]
        [InlineData("", "", "201", "", true)]
        [InlineData("", "", "301", "", true)]
        [InlineData("", "", "777", "", false)]
        public async Task SearchMaterialsData(string sapCode,string name ,string matType,string matGroup,bool expectResult)
        {
            var resule = await controller.GetData( sapCode: sapCode, name:name,matType:matType,matGroup:matGroup);
            var actualResult = resule as OkObjectResult;
            Assert.NotNull(actualResult);
            Assert.Equal(expectResult, ((List<WebUi.Models.SearchMat>)actualResult.Value).Count > 0);
        }

        [Theory(DisplayName = "Get Cost price material")]
        [InlineData("AMOX","1200")]
        [InlineData("BBB-TEST","1200")]
        public async Task GetCostPrice(string sapCode,string plant)
        {
            var result = await controller.GetPriceCost(sapCode, plant);
            var actualResult = result as OkObjectResult;
            Assert.NotNull(actualResult);
            Assert.Equal(200, actualResult.StatusCode);
        }


    }
}
