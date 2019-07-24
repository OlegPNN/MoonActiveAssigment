using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonActivePrint2Concole.Domain.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MoonActivePrint2Concole.Domain.Providers.Impl
{
    public class RedisStorageClient : IRedisStorageClient
    {
        private readonly RedisKey MessageQueueKey = "MoonActiveMessages";
        public RedisStorageClient()
        { }

        public async Task<bool> AddMessageToSortedSet(MessageToPrint model)
        {
            try
            {
                var redis = RedisStore.RedisCache;
            var rank = model.TimeStamp.ToOADate();
            var result = false;
            do
            {
                var trans = redis.CreateTransaction();
                var members = redis.SortedSetRangeByScore(MessageQueueKey, rank, rank);
                model.Sequence = members.Count() + 1;
                var member = JsonConvert.SerializeObject(model);
                trans.AddCondition(Condition.SortedSetNotContains(MessageQueueKey, member));
                await trans.SortedSetAddAsync(MessageQueueKey, member, rank);
                var exec = trans.ExecuteAsync();
                result = redis.Wait(exec);
            } while (!result);
            return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Error in PublishPendingMessages: exception to work with Redis store is occured. {0}", e.Message));
                return await Task.FromResult(false);
            }

        }
        public void PublishPendingMessages()
        {
            try
            {
                var redis = RedisStore.RedisCache;
                var currtime = DateTime.Now;
                var rank = currtime.ToOADate();
                var result = false;
                do
                {
                    var trans = redis.CreateTransaction();
                    var members = redis.SortedSetRangeByScore(MessageQueueKey, 0, rank);
                    if (members.Count() > 0)
                    {
                        trans.AddCondition(Condition.SortedSetContains(MessageQueueKey, members[0]));
                        trans.AddCondition(Condition.SortedSetContains(MessageQueueKey, members[members.Count() - 1]));
                        IList<MessageToPrint> msgList = new List<MessageToPrint>();
                        foreach (var member in members)
                        {
                            msgList.Add(JsonConvert.DeserializeObject<MessageToPrint>(member));
                            trans.SortedSetRemoveAsync(MessageQueueKey, member);
                        }
                        msgList = msgList.OrderBy(x => x.TimeStamp).ThenBy(y => y.Sequence).ToArray();
                        var s = new StringBuilder();
                        for (int i = 0; i < msgList.Count(); i++)
                        {
                            s.AppendLine(string.Format("Time {0} Message {1}", currtime, msgList[i].Message));
                        }
                        var exec = trans.ExecuteAsync();
                        result = redis.Wait(exec);
                        if (result) Console.Write(s);
                    }
                    else result = true;


                } while (!result);
            }
            catch (Exception e)
            {
                Console.WriteLine(string.Format("Error in PublishPendingMessages: exception to work with Redis store is occured. {0}", e.Message));
            }
        }
    }
}
