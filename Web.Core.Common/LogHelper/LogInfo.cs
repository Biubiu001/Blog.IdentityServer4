﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Core.Common.LogHelper
{
  public   class LogInfo
    {
        public DateTime Datetime { get; set; }
        public string Content { get; set; }
        public string IP { get; set; }
        public string LogColor { get; set; }
        public int Import { get; set; } = 0;
    }
}
