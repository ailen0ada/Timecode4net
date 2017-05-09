using System;

namespace TimeCode4net
{
    internal static class Extensions
    {
        public static int ToInt(this FrameRate frameRate)
        {
            switch (frameRate)
            {
                case FrameRate.fps23_98:
                case FrameRate.fps24:
                    return 24;
                case FrameRate.fps25:
                    return 25;
                case FrameRate.fps29_97:
                case FrameRate.fps30:
                    return 30;
                case FrameRate.fps50:
                    return 50;
                case FrameRate.fps59_94:
                case FrameRate.fps60:
                    return 60;
                case FrameRate.msec:
                    return 1000;
                default:
                    throw new ArgumentOutOfRangeException(nameof(frameRate), frameRate, null);
            }
        }
    }
}
