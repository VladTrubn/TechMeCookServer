﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace TechMeCookServer.Services
{
    public interface IHttpClientService
    {
        HttpClient GetHttpClient();
    }
}
