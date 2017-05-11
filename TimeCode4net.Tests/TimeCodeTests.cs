﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TimeCode4net.Tests
{
    public class TimeCodeTests
    {
        [Theory, MemberData(nameof(TestData))]
        public void CreateByFrameTest(int frames, FrameRate frameRate, bool isDropFrame, string expected)
        {
            var actual = new TimeCode(frames, frameRate, isDropFrame);
            Assert.Equal(expected, actual.ToString());
        }

        public static object[][] TestData =
        {
            new object[] {1800, FrameRate.fps29_97, true, "00:01:00;02"},
            new object[] {3600, FrameRate.fps59_94, true, "00:01:00;04"},
            new object[] {1387252, FrameRate.fps29_97, true, "12:51:28;00"},
            new object[] {215999, FrameRate.fps59_94, true, "01:00:03;35"},
            new object[] {215999, FrameRate.fps29_97, false, "01:59:59:29"},
            new object[] {215999, FrameRate.fps23_98, false, "02:29:59:23"},
            new object[] {1800, FrameRate.fps23_98, false, "00:01:15:00"},
            new object[] {1800, FrameRate.fps24, false, "00:01:15:00"},
            new object[] {1800, FrameRate.fps25, false, "00:01:12:00"},
            new object[] {1800, FrameRate.fps30, false, "00:01:00:00"},
            new object[] {1800, FrameRate.fps50, false, "00:00:36:00"},
            new object[] {1800, FrameRate.fps60, false, "00:00:30:00"}
        };
    }
}