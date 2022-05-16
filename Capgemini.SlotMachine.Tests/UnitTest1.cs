using GamePadReader;
using Xunit.Abstractions;

namespace Capgemini.SlotMachine.Tests
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper _output;

        public UnitTest1(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Theory]
        [InlineData("09:59:00")]
        [InlineData("10:00:00")]
        [InlineData("10:00:10")]
        [InlineData("10:01:00")]
        [InlineData("10:02:00")]
        [InlineData("10:03:00")]
        [InlineData("10:04:00")]
        [InlineData("10:05:00")]
        [InlineData("10:06:00")]
        [InlineData("10:07:00")]
        [InlineData("10:08:00")]
        [InlineData("10:09:00")]
        [InlineData("10:10:00")]
        [InlineData("10:11:00")]
        [InlineData("10:12:00")]
        [InlineData("10:13:00")]
        [InlineData("10:14:00")]
        [InlineData("10:15:00")]
        [InlineData("10:15:10")]
        public void TestChanceToWinSinglePrize(string dateTime)
        {
            var session = new Session(DateTime.Parse("2022-05-24T10:00:00"), DateTime.Parse("2022-05-24T10:15:00"), 1);
            var dt = DateTime.Parse($"2022-05-24T{dateTime}");
            var chanceToWin = session.GetChanceToWin(dt);
            _output.WriteLine($"Chance to win at {dt}: {chanceToWin}");
        }

        [Theory]
        [InlineData("09:59:00")]
        [InlineData("10:00:00")]
        [InlineData("10:00:10")]
        [InlineData("10:01:00")]
        [InlineData("10:02:00")]
        [InlineData("10:03:00")]
        [InlineData("10:04:00")]
        [InlineData("10:05:00")]
        [InlineData("10:06:00")]
        [InlineData("10:07:00")]
        [InlineData("10:08:00")]
        [InlineData("10:09:00")]
        [InlineData("10:10:00")]
        [InlineData("10:11:00")]
        [InlineData("10:12:00")]
        [InlineData("10:13:00")]
        [InlineData("10:14:00")]
        [InlineData("10:15:00")]
        [InlineData("10:15:10")]
        public void TestChanceToWinTwoPrizes(string dateTime)
        {
            var session = new Session(DateTime.Parse("2022-05-24T10:00:00"), DateTime.Parse("2022-05-24T10:15:00"), 2);
            var dt = DateTime.Parse($"2022-05-24T{dateTime}");
            var chanceToWin = session.GetChanceToWin(dt);
            _output.WriteLine($"Chance to win at {dt}: {chanceToWin}");
        }
    }
}