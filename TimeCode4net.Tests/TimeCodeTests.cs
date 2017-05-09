using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TimeCode4net.Tests
{
    public class TimeCodeTests
    {
        [Theory]
        [InlineData(18000, FrameRate.fps29_97, false, "00:10:00:00")]
        [InlineData(17982, FrameRate.fps29_97, true, "00:10:00;00")]
        [InlineData(15000, FrameRate.fps25, false, "00:10:00:00")]
        [InlineData(3599, FrameRate.fps29_97, false, "00:01:59:29")]
        [InlineData(3004, FrameRate.fps25, false, "00:02:00:04")]
        [InlineData(3597, FrameRate.fps29_97, true, "00:02:00;01")]
        [InlineData(3601, FrameRate.fps29_97, false, "00:02:00:01")]
        [InlineData(3001, FrameRate.fps25, false, "00:02:00:01")]
        public void CreateTest(int frames, FrameRate frameRate, bool isDropFrame, string expected)
        {
            var actual = new TimeCode(frames, frameRate, isDropFrame);
            Assert.Equal(expected, actual.ToString());
        }
    }
}
