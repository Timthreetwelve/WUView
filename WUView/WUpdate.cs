// Copyright(c) Tim Kennedy. All Rights Reserved. Licensed under the MIT License.

using System;
using System.Windows.Documents;

namespace WUView
{
    public class WUpdate
    {
        #region Private backing fields
        private string resultCode;
        private string hResult;
        private string operation;
        private string serversel;
        private string supportURL;
        #endregion Private backing fields

        #region Properties from WUApi
        public string Title { get; set; }
        public string KBNum { get; set; }
        public int Revision { get; set; }
        public string Description { get; set; }
        public string UpdateID { get; set; }
        public DateTime Date { get; set; }
        public string SupportURL
        {
            get
            {
                return supportURL ?? string.Empty;
            }

            set { supportURL = value; }
        }
        public string ResultCode
        {
            get
            {
                if (resultCode != null)
                {
                    return resultCode.Replace("orc", "");
                }
                return string.Empty;
            }
            set
            {
                resultCode = value;
            }
        }
        public string HResult
        {
            get
            {
                if (string.IsNullOrEmpty(hResult))
                {
                    return string.Empty;
                }
                else
                {
                    return string.Format($"0x{int.Parse(hResult):X8}");
                }
            }
            set { hResult = value; }
        }
        public string Operation
        {
            get
            {
                if (operation != null)
                {
                    return operation.Replace("uo", "");
                }
                return string.Empty;
            }
            set
            {
                operation = value;
            }
        }
        public string ServerSelection
        {
            get
            {
                if (serversel != null)
                {
                    return serversel.Replace("ss", "");
                }
                return string.Empty;
            }
            set
            {
                serversel = value;
            }
        }
        #endregion Properties from WUApi

        #region Properties from Event Log
        public int ELEventID { get; set; }

        public string ELDescription { get; set; }

        public string ELProvider { get; set; }

        public DateTime? ELDate { get; set; }

        //public object ElProperty { get; set; }

        #endregion Properties from Event Log

        //
    }
}
