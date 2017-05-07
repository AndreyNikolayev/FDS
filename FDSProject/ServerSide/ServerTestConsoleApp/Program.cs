using FdsServerBL;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.Logging;
using SharedEntities.Commands;
using SharedEntities.Constants;
using SharedEntities.Entities.DataEntities;
using SharedEntities.Enums;
using SharedEntities.Handlers;
using SharedEntities.TestEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerTestConsoleApp
{
    class Program
    {
        static ILog log = LogManager.GetLogger<Program>();

        static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = EndPointTypes.SERVER_END_POINT;

            var endpointConfiguration = EndpointConfigurationLogic.GetServerEndPointConfiguration();

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            await RunLoop(endpointInstance);

            await endpointInstance.Stop().ConfigureAwait(false);
        }

        static async Task RunLoop(IEndpointInstance endpointInstance)
        {
            while (true)
            {
                log.Info("Press 'P' to performCommand, or 'Q' to quit.");
                var key = Console.ReadKey();
                Console.WriteLine();

                switch (key.Key)
                {
                    case ConsoleKey.P:
                        var plugin = PluginsHandler.Instance.GetPluginByPluginName("BasicMathPlugin");
                        foreach (var command in PluginsHandler.Instance.GetCommandNamesByPluginName("BasicMathPlugin"))
                        {
                            await TransportLogic.SendCommand(endpointInstance,
                                new Plugin { PluginGuid = plugin.PluginGuid, PluginName = plugin.PluginName, PluginReadableName = plugin.PluginReadableName },
                                command,
                                JsonConvert.SerializeObject(new object[] { 3, 5 }));
                        }
                        break;

                    case ConsoleKey.Q:
                        return;

                    default:
                        log.Info("Unknown input. Please try again.");
                        break;
                }
            }
        }
    }
}
