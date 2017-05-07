using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedEntities.Entities.DataEntities
{
    public class Plugin
    {
        public Guid PluginGuid { get; set; }

        public string PluginName { get; set; }

        public string PluginReadableName { get; set; }

        public string AssemblyName { get; set; }

        public byte[] AssemblyBytes { get; set; }
    }
}
