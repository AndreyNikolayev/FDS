using System;

namespace SharedEntities.Entities.CommandEntities
{
    public class ArgumentWrapper
    {
        public Guid PluginGuid { get; set; }

        public string PluginName { get; set; }

        public string PluginCommand { get; set; }

        public string ArgumentDataJson { get; set; }
    }
}
