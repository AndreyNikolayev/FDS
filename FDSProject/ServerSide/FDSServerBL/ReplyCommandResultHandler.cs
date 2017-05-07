using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Logging;
using SharedEntities.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FdsServerBL
{

    public class ReplyCommandResultHandler : IHandleMessages<ReplyCommandResult>
    {
        static ILog log = LogManager.GetLogger<ReplyCommandResultHandler>();

        public Task Handle(ReplyCommandResult commandResult, IMessageHandlerContext context)
        {
            log.Info($"Received Result = {JsonConvert.DeserializeObject<int>(commandResult.CommandResult.ResultJson)}");

            return Task.CompletedTask;
        }
    }
}
