using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoonActivePrint2Concole.Domain.Models;

namespace MoonActivePrint2Concole.Domain.Providers
{
    public interface IRedisStorageClient
    {
        Task<bool> AddMessageToSortedSet(MessageToPrint model);
        void PublishPendingMessages();
    }
}
