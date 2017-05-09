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
            this.Update(true);
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

        private void Update(bool fromTotalFrames = false)
        {
            var totalFramesInHour = 3600 * this._frameRate;
            var totalFramesInMinute = 60 * this._frameRate;

            var frames = this.TotalFrames;
            if (this._isDropFrame)
            {
                frames += CalculateDropFrames(fromTotalFrames);
            }

            this.Hours = frames / totalFramesInHour;
            if (this.Hours > 23)
            {
                this.Hours %= 24;
                frames -= 24 * totalFramesInHour;
            }
            this.Minutes = frames % totalFramesInHour / totalFramesInMinute;
            this.Seconds = frames % totalFramesInHour % totalFramesInMinute / this._frameRate;
            this.Frames = frames % totalFramesInHour % totalFramesInMinute % this._frameRate;

            if (this._isDropFrame && this.Frames == 0 && this.Minutes % 10 > 0)
            {
                switch (this._rawFrameRate)
                {
                    case FrameRate.fps59_94:
                        this.Frames = 4;
                        break;
                    case FrameRate.fps29_97:
                        this.Frames = 2;
                        break;
                }
            }
            UpdateTotalFrames();
        }

        private void UpdateTotalFrames()
        {
            var frames = (this.Hours * 3600 + this.Minutes * 60 + this.Seconds) * this._frameRate + this.Frames;
            if (this._isDropFrame)
            {
                frames -= this.CalculateDropFrames(false);
            }
            this.TotalFrames = frames;
        }

        private int CalculateDropFrames(bool fromTotalFrames)
        {
            if (fromTotalFrames)
            {
                var hours =  this.TotalFrames / (3600 * this._frameRate);
                var mins =  this.TotalFrames % (3600 * this._frameRate) / (60 * this._frameRate);
                var extra = this.Minutes % 10 > 0 ? 1 : 0;
                switch (this._rawFrameRate)
                {
                    case FrameRate.fps29_97:
                        return hours * 6 * 36 + mins / 10 * 36 + mins % 10 * 4 + extra * 4;
                    case FrameRate.fps59_94:
                        return hours * 6 * 18 + mins / 10 * 18 + mins % 10 * 2 + extra * 2;
                }
            }

            switch (this._rawFrameRate)
            {
                case FrameRate.fps29_97:
                    return this.Hours * 6 * 36 + this.Minutes / 10 * 36 + this.Minutes % 10 * 4;
                case FrameRate.fps59_94:
                    return this.Hours * 6 * 18 + this.Minutes / 10 * 18 + this.Minutes % 10 * 2;
                default:
                    throw new ArgumentException("Drop frame is only supported with 29.97 or 59.94 fps.");
            }
        }
    }
}
