using System;
using System.Collections.Generic;
using System.Text;

namespace Benis
{
    public class BOCCust
    {
        #region Global Members
        #endregion

        #region Constructors
        public BOCCust()
        { 
        }
        #endregion

        #region Methods
        #endregion

        #region Properties
        public string FName { set; get; }
        public string LName { set; get; }
        public string FullName { get { return FName + " " + LName; } }
        public string Address { set; get; }
        public string CounterNo { set; get; }
        #endregion
    }
}
