﻿namespace Sample.AspNetCore2.WebApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("api/[controller]")]
    public class ValuesController 
        : Controller
    {
        private readonly ILogger _logger = null;

        private readonly IHttpContextAccessor _httpContextAccessor = null;

        public ValuesController(ILogger<ValuesController> logger, IHttpContextAccessor httpContextAccessor)
        {
            this._logger = logger;
            this._httpContextAccessor = httpContextAccessor;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            this._logger.LogCritical(nameof(ValuesController) + ".Get() is called...");

            try
            {
                int level = 0;
                this.FakeExceptionCallStack(ref level);
            }
            catch(Exception ex)
            {
                this._logger.Log(logLevel: LogLevel.Critical, exception: ex, message: "Got one!");
            }

            //// Let's simulate an unhandled exception:
            throw new Exception("AspNetCore2.WebApi sample: Unhandled exception within the ValueController.Get()");

            return new string[] { "value1", "value2" };
        }

        private void FakeExceptionCallStack(ref int level)
        {
            level++;
            if (level == 5)
            {
                throw new Exception("Outer handled exception", new NullReferenceException("Inner handled exception"));
            }
            this.FakeExceptionCallStack(ref level);
        }

        private void Rollbar_InternalEvent(object sender, Rollbar.RollbarEventArgs e)
        {
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
            throw new Exception("AspNetCore2.WebApi sample: Unhandled exception within the ValueController.Post(...)");
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}