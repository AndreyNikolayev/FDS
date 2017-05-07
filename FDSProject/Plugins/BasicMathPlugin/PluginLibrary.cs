using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicMathPlugin
{
    public static class PluginLibrary
    {
        public static Guid PluginGuid { get { return new Guid("22772EF8-7028-4B73-90C3-6AB17A3E6770"); } }

        public static string PluginName { get { return "BasicMathPlugin"; } }

        public static string PluginReadableName { get { return "Basic Math Plugin"; } }

        public static int Sum(int first, int second)
        {
            return first + second;
        }

        public static int Multi(int first, int second)
        {
            return first * second;
        }

        public static int Divide(int first, int second)
        {
            return first / second;
        }

        public static int Minus(int first, int second)
        {
            return first - second;
        }
    }
}
