using CoreRama.Common.Helper;
using MedMaster.WebUi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MedMaster.UnitTest
{
    public class InboundHisTest : AppConfig
    {
        private readonly WebUi.Controllers.Api.InboundsController inbounds;
        private readonly WebUi.Controllers.Api.InboundMimsController inboundMims;

        public InboundHisTest()
        {
            inbounds = new WebUi.Controllers.Api.InboundsController(
                context: _context,
                appConfig: _appConfig,
                coSapConfig: _coSapConfig,
                hisConfig: _hisConfig);

            inboundMims = new WebUi.Controllers.Api.InboundMimsController(
                context: _context,
                appConfig: _appConfig,
                coSapConfig: _coSapConfig,
                hisConfig: _hisConfig);
        }

        [Theory(DisplayName = "Inbound material master data to inbound web api")]
        [InlineData("MONOLINE", "AA", "BB", "CC", "DD", false, "TName", "1H,1J,2B,2B", "AA,DD",true)]
        [InlineData("AMOX1", "CC", "cc", "AbbA", "CC",true,"tradeName","1A,1B,2A,2B","AA,NN,CC,DD",true)]
        [InlineData("KALCID", "", "", "", "", false, "", "", "",true)]
        [InlineData("AMOX", "TEST-1234", "", "", "", false, "", "", "",true)]
        [InlineData("SEBM-L1", "", "BB", "AA", "CC", false, "tradeName", "1A", "AA",false)]
        public async Task Inbound(string sapCode, 
            string tmtCode, 
            string specPrep, 
            string strength, 
            string contain,
            bool had,
            string tradeName,
            string mims,
            string genst,bool expectResult)
        {
            var result = await inbounds.Post(new WebUi.Models.HisMedIn
            {
                SapCode = sapCode,
                TmtCode = tmtCode,
                SpecPrep = specPrep,
                Strength = strength,
                Contain = contain,
                highAlertDrug = had,
                TradeName = tradeName,
                Mims = string.IsNullOrEmpty(mims) ? null : mims.Split(",").ToList(),
                Genst = string.IsNullOrEmpty(genst) ? null : genst.Split(",").ToList()
            });

            if (expectResult)
            {
                var actualResult = result as OkObjectResult;
                Assert.NotNull(actualResult);
                Assert.Equal(200, actualResult.StatusCode);
            }
            else
            {
                var actualResult = result as BadRequestObjectResult;
                Assert.NotNull(actualResult);
                Assert.Equal(400, actualResult.StatusCode);
            }
            
        }
        
        [Theory(DisplayName ="Inbound material classification data")]
        [InlineData("AMOX","1A,2A,1B,1C,3C")]
        [InlineData("BBB-TEST", "")]
        [InlineData("XATX-T-","1J,4H")]
        public async Task InboundMims(string sapCode, string mims)
        {
            //inboundMims
            var result = await inboundMims.Post(new WebUi.Models.HisMultiClassification
            {
                SapCode = sapCode,
                Data = string.IsNullOrEmpty(mims) ? null : mims.Split(",").ToList()
            });

            var actualResult = result as OkObjectResult;
            Assert.NotNull(actualResult);
            Assert.Equal(200, actualResult.StatusCode);
        }




    }

}
