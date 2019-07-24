using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoonActivePrint2Concole.Domain.Models;
using MoonActivePrint2Concole.Domain.Providers;

namespace MoonActivePrint2Concole.Controllers
{
    /// <summary>
    /// Controller to get message and echo it in specified time
    /// </summary>
    /// <inheritdoc />

    [Route("api/echoAtTime")]
    [ApiController]
    public class MessageController : Controller
    {
        #region Dependencies

        private readonly IRedisStorageClient _redisStorageClient;
        #endregion
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageController"/> class.
        /// </summary>
        /// <param name="redisStorageClient"><see cref="IRedisStorageClient"/></param>

        #region .ctor
        public MessageController(IRedisStorageClient redisStorageClient)
        {
            this._redisStorageClient = redisStorageClient;
        }
        #endregion
        /// <summary>
        /// echoAtTime receives as parameters time and message
        /// </summary>
        /// <param name="message">Message to echo</param>
        /// <param name="time">string representation of time to print</param>/// 
        /// <returns><see cref="ActionResult"/></returns>
        [HttpGet]
        public async Task<ActionResult> GetMessage(string message, string time)
        {
            if(String.IsNullOrEmpty(message) || String.IsNullOrEmpty(time)) return this.BadRequest();
            DateTime printTime;
            if (!DateTime.TryParse(time, out printTime)) return this.BadRequest();
            var model = new MessageToPrint() { Message = message, PrintTimeStr = time, TimeStamp = printTime };
            await this._redisStorageClient.AddMessageToSortedSet(model);
            return this.Ok();
        }

        /// <summary>
        ///  echoAtTime receives as body model with time and message
        /// </summary>
        /// <param name="model"><see cref="MessageToPrint"/></param>
        /// <returns><see cref="ActionResult"/></returns>
        [HttpPost]
        public ActionResult ReceivePostMessage([FromBody] MessageToPrint model)
        {
            if (model == null)
                return this.BadRequest();
            if (String.IsNullOrEmpty(model.Message) || String.IsNullOrEmpty(model.PrintTimeStr)) return this.BadRequest();
            DateTime printTime;
            if(!DateTime.TryParse(model.PrintTimeStr, out printTime)) return this.BadRequest();

            model.TimeStamp = printTime;
            this._redisStorageClient.AddMessageToSortedSet(model);

            return this.Ok();

        }
    }
}