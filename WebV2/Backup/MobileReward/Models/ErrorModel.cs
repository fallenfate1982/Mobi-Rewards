﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileReward.Models
{
  public class ErrorModel
  {
    public int HttpStatusCode { get; set; }
    public Exception Exception { get; set; }
  }
}