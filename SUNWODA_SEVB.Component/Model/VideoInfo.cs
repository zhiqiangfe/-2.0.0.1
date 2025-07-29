using SUNWODA_SEVB.Component.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.Component.Model
{
    public class VideoInfo : ModelBase
    {
        private bool isPlaying;
        private string? videoState;
        private float fps;
        private DateTime videoLastFrameSystemTime;

        /// <summary>
        /// 是否正在播放
        /// </summary>
        public bool IsPlaying
        {
            get => isPlaying;
            set => SetProperty(ref isPlaying, value);
        }

        /// <summary>
        /// 视频状态
        /// </summary>
        public string? VideoState
        {
            get => videoState;
            set => SetProperty(ref videoState, value);
        }

        /// <summary>
        /// 帧率
        /// </summary>
        public float Fps
        {
            get => fps;
            set => SetProperty(ref fps, value);
        }

        /// <summary>
        /// 视频最后帧的系统时间
        /// </summary>
        public DateTime VideoLastFrameSystemTime
        {
            get => videoLastFrameSystemTime;
            set => SetProperty(ref videoLastFrameSystemTime, value);
        }
    }
}
