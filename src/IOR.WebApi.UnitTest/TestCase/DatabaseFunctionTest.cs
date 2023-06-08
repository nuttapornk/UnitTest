using IOR.WebApi.Helper;
using System.Threading.Tasks;
using Xunit;

namespace IOR.WebApi.UnitTest.TestCase
{
    public class DatabaseFunctionTest : AppSetting
    {
        [Theory(DisplayName = "Get risk team id ")]
        [InlineData(1, "50000054", 1)]
        [InlineData(1, "50010857", 2)]
        [InlineData(1, "50010852", 3)]
        [InlineData(1, "50011555", 4)]
        [InlineData(1, "50011412", 4)]
        [InlineData(2, "92000030", 4)]
        public async Task GetRmTeamId(int comId, string orgId, int expectResult)
        {
            var actualResult = await AppFunc.GetRiskTeamId(_context, comId, orgId);
            Assert.Equal(expectResult, actualResult);
        }

        [Theory(DisplayName = "Get full location text")]
        [InlineData(178, "โรงพยาบาลรามาธิบดี/เภสัชกรรม/ห้องจ่ายยาตา")]
        [InlineData(8, "โรงพยาบาลรามาธิบดี/เภสัชกรรม")]
        [InlineData(318, "ศูนย์การแพทย์สมเด็จพระเทพรัตน์/ส่วนสนับสนุนบริการ/คลังยา ชั้น B1")]
        public async Task GetAllLocation(int locationId, string expectResult)
        {
            var actualResult = await AppFunc.GetAllLocationAsync(_context, locationId);
            Assert.Equal(expectResult, actualResult);
        }

        [Theory(DisplayName = "Get event effect name")]
        [InlineData(1, "M", "M1", "M1 แพทย์สั่งยาคลาดเคลื่อน : Prescribing Error (PE)")]
        [InlineData(1, "M", "M3", "M3 ห้องยาจัด/เตรียมยาคลาดเคลื่อน : Predispensing Error")]
        public async Task GetEventEffectName(int clinicTypeId, string eventCode, string code, string expectResult)
        {
            var actualResult = await AppFunc.GetEventDetailName(_context, clinicTypeId, eventCode, code);
            Assert.Equal(expectResult, actualResult);
        }

        [Theory(DisplayName = "Get org full id")]
        [InlineData(1, "50000064", "50000000/50010307/50000061/50000064")]
        [InlineData(1, "50000054", "50000000/50000001/50000054")]
        public async Task GetFullOrgId(int comId, string orgId, string expectResult)
        {
            var actualResult = await AppFunc.GetFullOrgAsync(_context, comId, orgId);
            Assert.Equal(expectResult, actualResult.Id);
        }

        [Theory(DisplayName = "Get org full name")]
        [InlineData(1, "50000432", "คณะแพทยศาสตร์โรงพยาบาลรามาธิบดี/โรงพยาบาลรามาธิบดี/ฝ่ายการพยาบาล/งานการพยาบาลจักษุ โสต ศอ นาสิกวิทยา/หน่วยตรวจโรคหู-คอ-จมูก")]
        [InlineData(1, "50000054", "คณะแพทยศาสตร์โรงพยาบาลรามาธิบดี/สำนักงานคณบดี/ฝ่ายสารสนเทศ")]
        public async Task GetFullOrgName(int comId, string orgId, string expectResult)
        {
            var actualResult = await AppFunc.GetFullOrgAsync(_context, comId, orgId);
            Assert.Equal(expectResult, actualResult.Name);
        }

        [Theory(DisplayName = "Check user name")]
        [InlineData(1, "014419", "นาย ณัฐพร คันศิลป์")]
        [InlineData(1, "002070", "นุชฏา  นุชนนท์")]
        [InlineData(2, "RFS001", "คุณประเสริฐ พัชรบุษราคัมกุล")]
        public async Task CheckUserName(int comId, string staffId, string expectResult)
        {
            var actualResult = await AppFunc.GetUserAsync(_context, comId, staffId);
            Assert.Equal(expectResult, actualResult.Name);
        }

    }
}
