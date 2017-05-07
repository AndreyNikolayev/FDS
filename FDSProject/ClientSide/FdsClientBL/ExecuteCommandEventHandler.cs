using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Logging;
using SharedEntities.Commands;
using SharedEntities.Entities.CommandEntities;
using SharedEntities.Enums;
using SharedEntities.Handlers;
using SharedEntities.TestEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FdsClientBL
{
    public class ExecuteCommandEventHandler : IHandleMessages<ExecuteCommandEvent>
    {
        static ILog log = LogManager.GetLogger<ExecuteCommandEventHandler>();

        public Task Handle(ExecuteCommandEvent executeCommandEvent, IMessageHandlerContext context)
        {
            var commandArgument = executeCommandEvent.CommandArgument;
            log.Info($"Received Command : Plugin - {commandArgument.PluginName}, Command - {commandArgument.PluginCommand}, ArgumentJson -   {commandArgument.ArgumentDataJson}");

            var command = PluginsHandler.Instance.GetCurrentCommand(commandArgument.PluginName, commandArgument.PluginCommand);


            var argumentObjectArray = JsonConvert.DeserializeObject<object[]>(commandArgument.ArgumentDataJson);
            var commandParameters = command.GetParameters();

            for (var i=0;i< argumentObjectArray.Length;i++)
            {
                if(argumentObjectArray.GetType() != commandParameters[i].ParameterType)
                {
                    argumentObjectArray[i] = Convert.ChangeType(argumentObjectArray[i], commandParameters[i].ParameterType);
                }
            }

            var result = Convert.ChangeType(command.Invoke(null, argumentObjectArray),command.ReturnType);

            var reply = new ReplyCommandResult
            {   
                CommandResult = new ResultWrapper
                {
                    ResultJson = JsonConvert.SerializeObject(result)
                },
                ExecuteCommandGuid = executeCommandEvent.CommandGuid,
                CommandStatus = CommandStatusEnum.Finished
            };

            context.Send(reply);

            return Task.CompletedTask;
        }
    }
}
