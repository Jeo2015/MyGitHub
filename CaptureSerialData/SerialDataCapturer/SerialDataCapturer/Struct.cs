using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialDataCapturer
{
    class DataTypeAndDescrip
    {
        public string name = string.Empty;
        public string descrip = string.Empty;
    };
    enum SegmentType
    {
        No,
        Cycle,
        Byte,
    }
    enum DataWidth
    {
        _int16,
        _int32,
    }
    class SegmentMethod
    {
        public SegmentType type;
        public int SegmentPartNum;
        public string[] filenames = new string[5];
        public DataWidth[] dataWidth = new DataWidth[5];
        //public int[] segmentParts = new int[5];
        public int[] cycle = new int[5];
        public string[] writeStr = new string[5];
        public int[] reserved = new int[5];
    }
}
