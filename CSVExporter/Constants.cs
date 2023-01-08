using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSVExporter
{
    public static class Constants
    {
        public static readonly string[] EXTENSIONS = new string[] { ".srt", ".vtt" };
        public static readonly string VIDEO_TIMESTAMP_URL_FORMAT = @"https://youtu.be/{0}&t={1}";

    }
}
