using System.Collections.Generic;

namespace Model
{
#pragma warning disable 8618
    public record TextData
    {
        public List<VoiceInfo> VoiceInfos { get; set; }
        public List<ImageInfo> ImageInfos { get; set; }
    }
}