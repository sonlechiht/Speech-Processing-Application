using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechProcessing.Models
{
    public class WorkingBase
    {
        public static string WorkingFolder { get; set; }
        public static string STTData = "STTData.txt";
        public static string TTSData = "TTSData.txt";
        public static string CMDSpeechProcessingFoldername = "SpeechProcessing";
        public static string CMDSpeechProcessingFilename = "SpeechProcessing.exe";
    }
}
