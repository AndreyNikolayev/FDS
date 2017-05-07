using FdsClientBL;
using NServiceBus;
using NServiceBus.Logging;
using SharedEntities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTestConsoleApp
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
            Console.Title = EndPointTypes.CLIENT_END_POINT;

            var endpointConfiguration = ClientEndpointConfigurationLogic.GetClientEndPointConfiguration();

            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}
