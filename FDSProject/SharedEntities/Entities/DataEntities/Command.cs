using SharedEntities.Enums;
using System;

namespace SharedEntities.Entities.DataEntities
{
    public class Command
    {
        public Guid CommandGuid { get; set; }

        public Guid PluginGuid { get; set; }

        public string PluginName { get; set; }

        public string PluginCommand { get; set; }

        public string ArgumentDataJson { get; set; }

        public string ResultDataJson { get; set; }

        public CommandStatusEnum CommandStatus { get; set; }
    }
}
