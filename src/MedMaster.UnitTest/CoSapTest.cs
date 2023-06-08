using CoreRama.Common.Helper;
using MedMaster.WebUi.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MedMaster.UnitTest
{
    
    public class CoSapTest : AppConfig
    {
        [Fact(DisplayName = "Test get sap environment")]
        public void GetEnvironment()
        {
            var actualResult = CoSap.GetEnvironment(_coSapConfig.Value);
            Assert.True(actualResult != null);
        }

        [Fact(DisplayName = "Test SAP Connection")]
        public void TestConnection()
        {
            var actualResult = CoSap.TestConnection(_coSapConfig.Value);
            if (actualResult.ToString().ToLower() == "connection successful")
                Assert.True(true);
            else
                Assert.True(false);
        }

        [Theory(DisplayName ="Get Material Tran")]
        [InlineData("23.05.2021", "23.05.2021")]
        [InlineData("24.03.2021", "24.03.2021")]
        public void GetMatTran(string fdate,string tdate)
        {
            var actualResult = CoSap.GetNewMats(_coSapConfig.Value, fdate, tdate);
            Assert.True(actualResult.Data.Result != null);
        }

        [Theory(DisplayName="Inbound data to SAP")]
        [InlineData("BRIC-3-E", "AA","AA","AA","AA")]
        [InlineData("AMOX", "BB", "BB", "BB", "BB")]        
        public async Task Inbound(string matId,string tmtCode,string specprep,string strength,string contain)
        {
            var chas = await _context.Characteristics.Where(a => a.Status == 1).ToListAsync();
            var actualResult = CoSap.PostMatData(_coSapConfig.Value, chas, new WebUi.Models.HisMedIn
            {
                SapCode = matId,
                TmtCode = tmtCode ,
                SpecPrep = specprep ,
                Strength = strength,
                Contain = contain ,
            });

            if (actualResult != null)
            {
                if (actualResult.Any(a => a.Type.ToLower() == "e"))
                    Assert.True(false);
                else
                    Assert.True(false);
            }
            else
            {
                Assert.True(true);
            }
            
        }

        [Theory(DisplayName = "Get Material data for HIS Outbound")]
        [InlineData("MONOLINE","1200", "MONOLINE")]
        [InlineData("AMOX-T-", "1200", "AMOX-T-")]
        [InlineData("BRIC-3-E", "1200", "BRIC-3-E")]
        [InlineData("BUVE-3-E", "1200", "BUVE-3-E")]
        public void Outbound(string matId,string plant,string expectResult)
        {
            var actualResult = CoSap.GetMatData(_coSapConfig.Value, matId, plant);
            Assert.Equal(expectResult, actualResult.MatDetail.Id);
        }
    }
}
