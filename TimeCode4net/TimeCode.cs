using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TimeCode4net
{
    public class TimeCode
    {
        public static TimeCode FromFrames(int totalFrames, FrameRate frameRate, bool isDropFrame)
        {
            FrameRateSanityCheck(frameRate, isDropFrame);

            var tc = new TimeCode(frameRate, isDropFrame) {TotalFrames = totalFrames};
            tc.UpdateByTotalFrames();
            return tc;
        }

        private const string TimeCodePattern = @"^(?<hours>[0-2][0-9]):(?<minutes>[0-5][0-9]):(?<seconds>[0-5][0-9])[:|;|\.](?<frames>[0-9]{2,3})$";

        public static TimeCode FromString(string input, FrameRate frameRate, bool isDropFrame)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException(nameof(input));
            }
            FrameRateSanityCheck(frameRate, isDropFrame);

            var tcRegex = new Regex(TimeCodePattern);
            var match = tcRegex.Match(input);
            if (!match.Success)
            {
                throw new ArgumentException("Input text was not in valid timecode format.", nameof(input));
            }

            var tc = new TimeCode(frameRate, isDropFrame)
            {
                Hours = int.Parse(match.Groups["hours"].Value),
                Minutes = int.Parse(match.Groups["minutes"].Value),
                Seconds = int.Parse(match.Groups["seconds"].Value),
                Frames = int.Parse(match.Groups["frames"].Value)
            };
            tc.UpdateTotalFrames();

            return tc;
        }

        private static void FrameRateSanityCheck(FrameRate frameRate, bool isDropFrame)
        {
            if (isDropFrame && frameRate != FrameRate.fps29_97 && frameRate != FrameRate.fps59_94)
            {
                throw new ArgumentException("Dropframe is supported with 29.97 or 59.94 fps.", nameof(isDropFrame));
            }
        }

        private TimeCode(FrameRate frameRate, bool isDropFrame)
        {
            this._isDropFrame = isDropFrame;
            this._rawFrameRate = frameRate;
            this._frameRate = frameRate.ToInt();
        }

        private readonly bool _isDropFrame;

        private readonly FrameRate _rawFrameRate;

        private readonly int _frameRate;

        public int TotalFrames { get; private set; }
    
        public int Hours { get; private set; }

        public int Minutes { get; private set; }

        public int Seconds { get; private set; }

        public int Frames { get; private set; }

        public TimeCode AddHours(double hours)
        {
            throw new NotImplementedException();
        }

        public TimeCode AddMinutes(double minutes)
        {
            throw new NotImplementedException();
        }

        public TimeCode AddSeconds(double seconds)
        {
            throw new NotImplementedException();
        }

        public TimeCode AddFrames(uint frames)
        {
            throw new NotImplementedException();
        }

        public TimeSpan ToTimeSpan()
        {
            var tc = new TimeCode(FrameRate.msec, false) {TotalFrames = this.TotalFrames};
            return new TimeSpan(0, tc.Hours, tc.Minutes, tc.Seconds, tc.Frames);
        }

        public override string ToString()
        {
            var frameSeparator = this._isDropFrame ? ";" : ":";
            return $"{this.Hours:D2}:{this.Minutes:D2}:{this.Seconds:D2}{frameSeparator}{this.Frames:D2}";
        }

        private void UpdateTotalFrames()
        {
            var frames = this.Hours * 3600;
            frames += this.Minutes * 60;
            frames += this.Seconds;
            frames *= this._frameRate;
            frames += this.Frames;
            if (this._isDropFrame)
            {
                var totalMinutes = this.Hours * 60 + this.Minutes;
                var dropFrames = this._rawFrameRate == FrameRate.fps29_97 ? 2 : 4;
                frames -= dropFrames * (totalMinutes - totalMinutes / 10);
            }
            this.TotalFrames = frames;
        }

        private void UpdateByTotalFrames()
        {
            var frameCount = this.TotalFrames;
            if (this._isDropFrame)
            {
                // 29.97 - 2, 59.94 - 4
                var dropFrames = this._rawFrameRate == FrameRate.fps29_97 ? 2 : 4;
                var dropInHours = 17982 * dropFrames / 2d;
                var dropInMinutes = 1798 * dropFrames / 2d;
                var h = (int)Math.Floor(this.TotalFrames / dropInHours);
                var m = this.TotalFrames % dropInHours;
                frameCount += 9 * dropFrames * h + dropFrames * (int)Math.Floor((m - dropFrames) / dropInMinutes);
            }
            this.Frames = frameCount % this._frameRate;
            this.Seconds = (int) Math.Floor(frameCount / (double) this._frameRate) % 60;
            this.Minutes = (int) Math.Floor(frameCount / (this._frameRate * 60d)) % 60;
            this.Hours = (int)Math.Floor(frameCount / (this._frameRate * 60 * 60d)) % 24;
        }
    }
}
