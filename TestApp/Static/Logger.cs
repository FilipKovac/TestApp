using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApp.Static
{
    public static class Logger
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void Debug(object message) => log.Debug(message);
        public static void Debug(object message, Exception e) => log.Debug(message, e);
        public static void Fatal(object message) => log.Fatal(message);
        public static void Fatal(object message, Exception e) => log.Fatal(message, e);
        public static void Info(object message) => log.Info(message);
        public static void Info(object message, Exception e) => log.Info(message, e);
    }
}
