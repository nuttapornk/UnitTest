using CoreRama.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MedMaster.UnitTest
{
    public class CommonTest
    {
        [Theory(DisplayName = "Concateinate date and time")]
        [InlineData("23.05.2021 00:00:00", "12:05:39","23.05.2021 12:05:39")]
        public void ConcatDateTime(string date,string time, string expectResult)
        {
            DateTime nDate = DateTime.ParseExact(date,"dd.MM.yyyy hh:mm:ss",Tools.EnCulture) ;
            TimeSpan nTime = TimeSpan.ParseExact(time,"hh\\:mm\\:ss",Tools.EnCulture);
            Assert.Equal(expectResult, nDate.Add(nTime).ToString("dd.MM.yyyy HH:mm:ss", Tools.EnCulture));

        }

        [Theory(DisplayName = "Convert decimal to text 2 digit")]
        [InlineData(123454.0000, "123454.00")]
        public void FixDecimal(decimal value,string expectResult)
        {
            Assert.Equal(expectResult, value.ToString("F"));
        }

        [Theory(DisplayName = "Convert decimal to decimal 2 digit places with not round up")]
        [InlineData(12345.99345,12345.99)]
        [InlineData(45.999,45.99)]
        public void Decimal2Digit(decimal value,decimal expectResult)
        {
            decimal decimalVar = Math.Round(value, 2, MidpointRounding.ToZero);
            Assert.Equal(expectResult, decimalVar);
        }

        //[Theory(DisplayName = "Encryt password")]
        //[InlineData("b32a7a1a-234a-4971-87f0-9e9cd107a9b2", "Info2010")]
        ////[InlineData("fb0a7858-747e-4616-8c9b-a2a0413f9093", "P@ssw0rd")]
        //public void EncryptPassword(string key, string password)
        //{
        //    CoreRama.Common.Encrypt.Hash hash = new();
        //    var hashKey = hash.GetMessageDigest(key);
        //    CoreRama.Common.Encrypt.Hash algorithm = new(hashKey,key);

        //    var encrytPassword = algorithm.Encrypt(password);
        //    var decryptPassword = algorithm.Decrypt(encrytPassword);
        //    Assert.Equal(decryptPassword, password);
        //}
    }
}
