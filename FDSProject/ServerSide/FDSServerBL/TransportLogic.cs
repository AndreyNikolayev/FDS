using NServiceBus;
using SharedEntities.Commands;
using SharedEntities.Entities.CommandEntities;
using SharedEntities.Entities.DataEntities;
using SharedEntities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FdsServerBL
{
    public static class TransportLogic
    {
        public static async Task SendCommand(IEndpointInstance endpointInstance, Plugin plugin, string pluginCommand, string argumentsJson)
        {
            var command = new ExecuteCommandEvent
            {
                CommandGuid = Guid.NewGuid(),
                CommandSpreadType = CommandSpreadTypeEnum.All,
                CommandArgument = new ArgumentWrapper
                {
                    PluginGuid = plugin.PluginGuid,
                    PluginName = plugin.PluginName,
                    PluginCommand = pluginCommand,
                    ArgumentDataJson = argumentsJson
                }
            };
            await endpointInstance.Publish(command);
        }
    }
}
