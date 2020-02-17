namespace Polaris.Utility
{
    using log4net;
    using log4net.Appender;
    using log4net.Core;
    using log4net.Layout;
    using System;
    using static log4net.Appender.FileAppender;

    public class LogUtil
    {
        static LogUtil()
        {
        }

        public static void AddFileAppender(String fileSavePath = "Log/", Int32 fileMaxSaveCount = 32)
        {
            log4net.Appender.RollingFileAppender appender = new log4net.Appender.RollingFileAppender();
            appender.Name = "fileLog";
            appender.File = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log/");
            appender.AppendToFile = true;
            appender.MaxSizeRollBackups = 1024 * 1024 * 100;
            appender.StaticLogFileName = false;
            appender.DatePattern = "yyy-MM-dd'.log'";
            appender.RollingStyle = RollingFileAppender.RollingMode.Composite;
            appender.LockingModel = new MinimalLock();
            appender.Layout = new PatternLayout("%date{yyyy-MM-dd HH:mm:ss} [%thread] %level - %message%newline");
            appender.MaxSizeRollBackups = 32;
            appender.ActivateOptions();

            //appender.AddFilter(new log4net.Filter.LevelRangeFilter() { LevelMin = Level.Error, LevelMax = Level.Fatal });

            log4net.Config.BasicConfigurator.Configure(appender);
        }

        public static void AddConsoleAppender()
        {
            ConsoleAppender consoleAppender = new ConsoleAppender();
            consoleAppender.Name = "consoleLog";
            consoleAppender.Layout = new PatternLayout("%date{yyyy-MM-dd HH:mm:ss} [%thread] %level - %message%newline");
            consoleAppender.ActivateOptions();
            log4net.Config.BasicConfigurator.Configure(consoleAppender);
        }

        private static ILog GetLog()
        {
            return log4net.LogManager.GetLogger("fileLog");
        }

        public static void Debug(String data, Exception exception)
        {
            if (exception != null)
            {
                GetLog().Debug(data, exception);
            }
            else
            {
                GetLog().Debug(data);
            }
        }

        public static void Debug(String data, params Object[] paramVal)
        {
            if (paramVal != null && paramVal.Length > 0)
            {
                GetLog().DebugFormat(data, paramVal);
            }
            else
            {
                GetLog().Debug(data);
            }
        }

        public static void Error(String data, Exception exception)
        {
            if (exception != null)
            {
                GetLog().Error(data, exception);
            }
            else
            {
                GetLog().Error(data);
            }
        }

        public static void Error(String data, params Object[] paramVal)
        {
            if (paramVal != null && paramVal.Length > 0)
            {
                GetLog().ErrorFormat(data, paramVal);
            }
            else
            {
                GetLog().Error(data);
            }
        }


        public static void Fatal(String data, Exception exception)
        {
            if (exception != null)
            {
                GetLog().Fatal(data, exception);
            }
            else
            {
                GetLog().Fatal(data);
            }
        }

        public static void Fatal(String data, params Object[] paramVal)
        {
            if (paramVal != null && paramVal.Length > 0)
            {
                GetLog().FatalFormat(data, paramVal);
            }
            else
            {
                GetLog().Fatal(data);
            }
        }
        public static void Info(String data, Exception exception)
        {
            if (exception != null)
            {
                GetLog().Info(data, exception);
            }
            else
            {
                GetLog().Info(data);
            }
        }

        public static void Info(String data, params Object[] paramVal)
        {
            if (paramVal != null && paramVal.Length > 0)
            {
                GetLog().InfoFormat(data, paramVal);
            }
            else
            {
                GetLog().Info(data);
            }
        }
        public static void Warn(String data, Exception exception)
        {
            if (exception != null)
            {
                GetLog().Warn(data, exception);
            }
            else
            {
                GetLog().Warn(data);
            }
        }

        public static void Warn(String data, params Object[] paramVal)
        {
            if (paramVal != null && paramVal.Length > 0)
            {
                GetLog().WarnFormat(data, paramVal);
            }
            else
            {
                GetLog().Warn(data);
            }
        }
    }
}
