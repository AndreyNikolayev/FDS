using NServiceBus;
using SharedEntities.Enums;
using System;

namespace SharedEntities.Commands
{
    public class ReplyCommandStatus : ICommand
    {
        public Guid ExecuteCommandGuid { get; set; }

        public Guid CurrentClientIdenfifier { get; set; }

        public CommandStatusEnum CommandStatus { get; set; }
    }
}
