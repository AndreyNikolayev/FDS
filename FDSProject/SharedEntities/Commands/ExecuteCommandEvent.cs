using NServiceBus;
using SharedEntities.Entities.CommandEntities;
using SharedEntities.Enums;
using System;
using System.Collections.Generic;

namespace SharedEntities.Commands
{
    public class ExecuteCommandEvent: IEvent
    {
        public Guid CommandGuid { get; set; }

        public CommandSpreadTypeEnum CommandSpreadType { get; set; }

        public List<Guid> ClientIdentifiers { get; set; }

        public ArgumentWrapper CommandArgument { get; set; }
    }
}
