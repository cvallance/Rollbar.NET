﻿#if NETFX

namespace Rollbar.AspNet.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Hosting;
    using System.Web.Mvc;
    using Rollbar.Common;
    using Rollbar.Diagnostics;
    using Rollbar.DTOs;

    /// <summary>
    /// Class ExceptionContextPackagingStrategy.
    /// Implements the <see cref="Rollbar.RollbarPackagingStrategyBase" /></summary>
    /// <seealso cref="Rollbar.RollbarPackagingStrategyBase" />
    public class ExceptionContextPackagingStrategy
            : RollbarPackagingStrategyBase
    {
        /// <summary>
        /// The exception context
        /// </summary>
        private readonly ExceptionContext _exceptionContext;

        private readonly string _message;

        /// <summary>
        /// Prevents a default instance of the <see cref="ExceptionContextPackagingStrategy" /> class from being created.
        /// </summary>
        private ExceptionContextPackagingStrategy()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionContextPackagingStrategy" /> class.
        /// </summary>
        /// <param name="exceptionContext">The exception context.</param>
        public ExceptionContextPackagingStrategy(ExceptionContext exceptionContext)
            : this(exceptionContext, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionContextPackagingStrategy" /> class.
        /// </summary>
        /// <param name="exceptionContext">The exception context.</param>
        /// <param name="message">The message.</param>
        public ExceptionContextPackagingStrategy(ExceptionContext exceptionContext, string message)
        {
            this._exceptionContext = exceptionContext;
            this._message = message;
        }

        /// <summary>
        /// Packages as rollbar data.
        /// </summary>
        /// <returns>Rollbar.DTOs.Data.</returns>
        public override Data PackageAsRollbarData()
        {
            if (this._exceptionContext == null)
            {
                return null;
            }

            // let's use composition of available strategies:    
            
            IRollbarPackagingStrategy packagingStrategy = new ExceptionPackagingStrategy(this._exceptionContext.Exception, this._message);

            var httpRequest = this._exceptionContext?.RequestContext?.HttpContext?.Request;
            if (httpRequest != null)
            {
                packagingStrategy = new HttpRequestPackagingStrategyDecorator(packagingStrategy, httpRequest);
            }

            Data rollbarData = packagingStrategy.PackageAsRollbarData();
            return rollbarData;
        }
    }
}

#endif
