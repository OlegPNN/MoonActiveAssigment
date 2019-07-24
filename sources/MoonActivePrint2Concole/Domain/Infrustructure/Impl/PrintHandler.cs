using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MoonActivePrint2Concole.Domain.Models;
using MoonActivePrint2Concole.Domain.Providers;
using MoonActivePrint2Concole.Domain.Providers.Impl;

namespace MoonActivePrint2Concole.Domain.Infrustructure.Impl
{

    public  class PrintHandler : IPrintHandler
    {
        private readonly IRedisStorageClient _redisClient;
        public PrintHandler()
        {
            _redisClient = new RedisStorageClient();
        }
        public  void LoopForMessages()
        {
            var currtime = DateTime.Now;

            while (true)
            {
                currtime = DateTime.Now;
                _redisClient.PublishPendingMessages();

                //using while to keep the sequence 
                var mSecs = (int)(10000 - (DateTime.Now - currtime).TotalMilliseconds);
                if(mSecs > 0)
                Thread.Sleep(mSecs);
            }
        }
    }
}
