using System;

namespace WUView
{
    public class EvRec
    {
        #region Properties from Event Log
        public int ELEventID { get; set; }

        public string ELDescription { get; set; }

        public string ELProvider { get; set; }

        public DateTime? ELDate { get; set; }

        public object ElProperty { get; set; }

        #endregion Properties from Event Log

    }
}
