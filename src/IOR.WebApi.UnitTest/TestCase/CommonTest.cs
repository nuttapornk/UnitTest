using IOR.WebApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using CoreRama.Common.Cryptography;
using Microsoft.Extensions.Options;

namespace IOR.WebApi.UnitTest.TestCase
{
    public class CommonTest : AppSetting
    {
        private IFormatProvider EnCulture = new System.Globalization.CultureInfo("en-US", false);

        [Theory(DisplayName = "Check input date,do not more than the current date.")]
        [InlineData("2020-08-05", "yyyy-MM-dd", true)]
        [InlineData("2021-08-02", "yyyy-MM-dd", true)]
        [InlineData("2024-08-02", "yyyy-MM-dd", false)]
        [InlineData("2024-04-04", "yyyy-MM-dd", false)]
        public void CheckExceedDate(string date, string format, bool expectResult)
        {
            Assert.Equal(expectResult, (DateTime.ParseExact(date, format, EnCulture) - DateTime.Today).TotalDays <= 0);
        }

        [Fact(DisplayName = "Get document number incident report from import")]
        public async Task GetDocRunningIncidenReportImport()
        {
            DocRunning docRunning = new(DocRunning.DocType.IncidentReportImport, _appSetting.Value.Lang);
            var actualResult = await docRunning.GetDocNumberAsync(_context);
            await _context.SaveChangesAsync();
            Assert.NotNull(actualResult);
        }

        [Theory(DisplayName = "Test get victims success")]
        [InlineData("5026729", 2, "นาย ณัฐพร คันศิลป์")]
        [InlineData("014419", 1, "นาย ณัฐพร คันศิลป์")]
        [InlineData("0000001", 2, "น.ส. สุธีรา อายุวัฒน์")]
        [InlineData("5026728", 2, "นาย อาทิตย์ วัชรานุทัศน์")]
        [InlineData("C1412834", 2, "นาย กิตติพงษ์ จึงจำเริญกุล")]
        [InlineData("C928450", 2, "น.ส. การญ์พิชชา กชกานน")]
        [InlineData("005759", 1, "ดร. สุนทร หลั่นเจริญ")]
        public void SearchVictimPass(string id, int type, string expected)
        {
            List<CoreRama.Utility.Models.KeyValue<string>> header = new()
            {
                 new CoreRama.Utility.Models.KeyValue<string>{ Name="Content-Type",Value="application/json" }
            };

            var data = new { pType = type, pCode = id };

            string dataString = JsonConvert.SerializeObject(data);
            var result = CoreRama.Utility.Helper.WebRequest.Post<Models.VictimSearchResult>(urlGetVictim, dataString, header);
            Assert.Equal(expected, result.d.tName);
        }

        [Theory(DisplayName = "Test get victims failed")]
        [InlineData("hn1", 2)]//
        [InlineData("hn2", 2)]
        [InlineData("StaffId3", 1)]
        [InlineData("StaffId4", 1)]
        [InlineData("StaffId5", 1)]
        public void SearchVictimFail(string hn, int type)
        {
            List<CoreRama.Utility.Models.KeyValue<string>> header = new()
            {
                 new CoreRama.Utility.Models.KeyValue<string>{ Name="Content-Type",Value="application/json" }
            };

            var data = new { pType = type, pCode = hn };
            string dataString = JsonConvert.SerializeObject(data);
            try
            {
                CoreRama.Utility.Helper.WebRequest.Post<Models.VictimSearchResult>(urlGetVictim, dataString, header);
                Assert.True(false);
            }
            catch (Exception)
            {
                Assert.True(true);
            }
        }

        [Theory(DisplayName = "Check string is numberic")]
        [InlineData("1", 1)]
        public void IsNumeric(string txt, int expected)
        {
            int num;
            int.TryParse(txt, out num);
            Assert.Equal(expected, num);
        }

        [Theory(DisplayName = "Test cncrypt plain text with key iv")]
        [InlineData("014419", "hIWntAaQNyY62+zMhBd1Bw==")]
        public void AesEncryptWithKeyIv(string plainText, string expected)
        {
            byte[] iv = Encoding.UTF8.GetBytes(_appSetting.Value.AesIv);
            byte[] key = Encoding.UTF8.GetBytes(_appSetting.Value.AesKey);

            Algorithm1 algorithm = new(key, iv);
            var cipherText = algorithm.Encrypt(plainText);
            Assert.Equal(expected, cipherText);
        }

        [Theory(DisplayName = "Test decrypt plain text with key iv")]
        [InlineData("hIWntAaQNyY62+zMhBd1Bw==", "014419")]
        public void AesDecryptWithKeyIv(string cipherText, string expected)
        {
            byte[] key = Encoding.UTF8.GetBytes(_appSetting.Value.AesKey);
            byte[] iv = Encoding.UTF8.GetBytes(_appSetting.Value.AesIv);

            Algorithm1 algorithm = new(key, iv);
            var plainText = algorithm.Decrypt(cipherText);
            Assert.Equal(expected, plainText);
        }

        [Theory(DisplayName = "Test encrypt plain text with key iv")]
        [InlineData("014419", "f61e0fdcb157c253", "0085f53698db9172", "hIWntAaQNyY62+zMhBd1Bw==")]
        [InlineData("014419", "aeea967e17bd01c8", "ef06b0ef786a8ea1", "xJ0QHOCdvV2f4uR6WCNFmg==")]
        [InlineData("014419", "bec57c9e416e3340", "189d52e3ca43cdca", "FVSA5LZ5zMDOTp6GpIiw0Q==")]
        public void AesEncryptWithCustomKeyIv(string plainText, string plainIv, string plainKey, string expected)
        {
            byte[] iv = Encoding.UTF8.GetBytes(plainIv);
            byte[] key = Encoding.UTF8.GetBytes(plainKey);

            Algorithm1 algorithm = new(key, iv);
            Assert.Equal(expected, algorithm.Encrypt(plainText));
        }

        [Theory(DisplayName = "Test decrypt plain text with key iv")]
        [InlineData("hIWntAaQNyY62+zMhBd1Bw==", "f61e0fdcb157c253", "0085f53698db9172", "014419")]
        [InlineData("xJ0QHOCdvV2f4uR6WCNFmg==", "aeea967e17bd01c8", "ef06b0ef786a8ea1", "014419")]
        [InlineData("FVSA5LZ5zMDOTp6GpIiw0Q==", "bec57c9e416e3340", "189d52e3ca43cdca", "014419")]
        public void AesDecryptWithCustomKeyIv(string cipherText, string plainIv, string plainKey, string expected)
        {
            byte[] iv = Encoding.UTF8.GetBytes(plainIv);
            byte[] key = Encoding.UTF8.GetBytes(plainKey);

            Algorithm1 algorithm = new(key, iv);
            Assert.Equal(expected, algorithm.Decrypt(cipherText));
        }
    }
}
