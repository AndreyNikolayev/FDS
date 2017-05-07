using NServiceBus;
using SharedEntities.Commands;
using SharedEntities.Constants;
using SharedEntities.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FdsClientBL
{
    public class ClientEndpointConfigurationLogic
    {

        public static EndpointConfiguration GetClientEndPointConfiguration()
        {
            var endpointConfiguration = new EndpointConfiguration(EndPointTypes.CLIENT_END_POINT);

            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
            transport.ConnectionString(ConfigUtils.GetConnectionString(DatabaseNames.NSERVICE_BUS_DB));

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(ReplyCommandResult), EndPointTypes.SERVER_END_POINT);
            routing.RegisterPublisher(typeof(ExecuteCommandEvent), EndPointTypes.SERVER_END_POINT);


            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo(ServiceBusConstants.FAILED_DESTINATION);
            endpointConfiguration.EnableInstallers();

            return endpointConfiguration;
        }
    }
}
