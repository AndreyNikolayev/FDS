using NServiceBus;
using SharedEntities.Entities.CommandEntities;
using SharedEntities.Enums;
using System;

namespace SharedEntities.Commands
{
    public class ReplyCommandResult : ICommand
    {
        public Guid ExecuteCommandGuid { get; set; }

        public Guid CurrentClientIdenfifier { get; set; }

        public CommandStatusEnum CommandStatus { get; set; }

        public ResultWrapper CommandResult { get; set; }
    }
}
