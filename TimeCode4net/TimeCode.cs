using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeCode4net
{
    public class TimeCode
    {
        public TimeCode(int totalFrames, FrameRate frameRate, bool isDropFrame)
        {
            this.TotalFrames = totalFrames;
            this._isDropFrame = isDropFrame;
            this._rawFrameRate = frameRate;
            this._frameRate = frameRate.ToInt();
            this.UpdateByTotalFrames();
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
            var tc = new TimeCode(this.TotalFrames, FrameRate.msec, false);
            return new TimeSpan(0, tc.Hours, tc.Minutes, tc.Seconds, tc.Frames);
        }

        public override string ToString()
        {
            var frameSeparator = this._isDropFrame ? ";" : ":";
            return $"{this.Hours:D2}:{this.Minutes:D2}:{this.Seconds:D2}{frameSeparator}{this.Frames:D2}";
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
