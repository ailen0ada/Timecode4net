using System;

namespace Timecode4net
{
    public static class Extensions
    {
        public static int ToInt(this FrameRate frameRate)
        {
            return frameRate switch
            {
                FrameRate.fps23_98 or FrameRate.fps24 => 24,
                FrameRate.fps25 => 25,
                FrameRate.fps29_97 or FrameRate.fps30 => 30,
                FrameRate.fps48 => 48,
                FrameRate.fps50 => 50,
                FrameRate.fps59_94 or FrameRate.fps60 => 60,
                FrameRate.msec => 1000,
                _ => throw new ArgumentOutOfRangeException(nameof(frameRate), frameRate, null)
            };
        }

        public static double ToDouble(this FrameRate frameRate)
        {
            return frameRate switch
            {
                FrameRate.fps23_98 => 24000 / 1001d,
                FrameRate.fps24 => 24,
                FrameRate.fps25 => 25,
                FrameRate.fps29_97 => 30000 / 1001d,
                FrameRate.fps30 => 30,
                FrameRate.fps48 => 48,
                FrameRate.fps50 => 50,
                FrameRate.fps59_94 => 60000 / 1001d,
                FrameRate.fps60 => 60,
                FrameRate.msec => 1,
                _ => throw new ArgumentOutOfRangeException(nameof(frameRate), frameRate, null)
            };
        }
    }
}
