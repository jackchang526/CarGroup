﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppApi.Controllers
{
    public class CarController : BaseController
    {
        public string Test()
        {
            return "hello world!";
        }
    }
}
