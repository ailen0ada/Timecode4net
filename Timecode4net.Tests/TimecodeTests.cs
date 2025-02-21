using System;
using Xunit;

namespace Timecode4net.Tests
{
    public class TimecodeTests
    {
        [Fact]
        public void CreateFailingTest()
        {
            Assert.Throws<ArgumentException>(
                "isDropFrame",
                () => Timecode.FromFrames(0, FrameRate.fps23_98, true));
            Assert.Throws<ArgumentException>(
                "isDropFrame",
                () => Timecode.FromString("DROPFRAME", FrameRate.fps23_98, true));
            Assert.Throws<ArgumentNullException>(
                "input",
                () => Timecode.FromString(string.Empty, FrameRate.fps23_98, false));
            Assert.Throws<ArgumentException>(
                "input",
                () => Timecode.FromString("NOTVALID", FrameRate.fps23_98, false));
        }

        [Theory, MemberData(nameof(TestData))]
        public void CreateByFrameTest(int frames, FrameRate frameRate, bool isDropFrame, string expected)
        {
            var actual = Timecode.FromFrames(frames, frameRate, isDropFrame);
            Assert.Equal(expected, actual.ToString());
        }

        [Theory, MemberData(nameof(TestData))]
        public void CreateByStringTest(int expected, FrameRate frameRate, bool isDropFrame, string input)
        {
            var actual = Timecode.FromString(input, frameRate, isDropFrame);
            Assert.Equal(expected, actual.TotalFrames);
        }

        [Theory, MemberData(nameof(TestData))]
        public void CreateByTimeSpanTest(int expected, FrameRate frameRate, bool isDropFrame, string input)
        {
            var attempt = Timecode.FromString(input, frameRate, isDropFrame).ToTimeSpan();
            var actual = Timecode.FromTimeSpan(attempt, frameRate, isDropFrame);
            Assert.Equal(expected, actual.TotalFrames);
        }

        public static object[][] TestData =
        [
            [1800, FrameRate.fps29_97, true, "00:01:00;02"],
            [3600, FrameRate.fps59_94, true, "00:01:00;04"],
            [1387252, FrameRate.fps29_97, true, "12:51:28;00"],
            [1078920, FrameRate.fps29_97, true, "10:00:00;00"],
            [215999, FrameRate.fps59_94, true, "01:00:03;35"],
            [215999, FrameRate.fps29_97, false, "01:59:59:29"],
            [215999, FrameRate.fps23_98, false, "02:29:59:23"],
            [1800, FrameRate.fps23_98, false, "00:01:15:00"],
            [15920, FrameRate.fps23_98, false, "00:11:03:08"],
            [1800, FrameRate.fps24, false, "00:01:15:00"],
            [1800, FrameRate.fps25, false, "00:01:12:00"],
            [1800, FrameRate.fps30, false, "00:01:00:00"],
            [1800, FrameRate.fps48, false, "00:00:37:24"],
            [1800, FrameRate.fps50, false, "00:00:36:00"],
            [1800, FrameRate.fps60, false, "00:00:30:00"]
        ];
    }
}
