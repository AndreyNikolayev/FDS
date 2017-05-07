using NServiceBus;
using SharedEntities.Commands;
using SharedEntities.Constants;
using SharedEntities.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FdsServerBL
{
    public static class EndpointConfigurationLogic
    {
        public static EndpointConfiguration GetServerEndPointConfiguration()
        {
            var endpointConfiguration = new EndpointConfiguration(EndPointTypes.SERVER_END_POINT);

            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
            transport.ConnectionString(ConfigUtils.GetConnectionString(DatabaseNames.NSERVICE_BUS_DB));

            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo(ServiceBusConstants.FAILED_DESTINATION);
            endpointConfiguration.EnableInstallers();

            return endpointConfiguration;
        }
    }
}
