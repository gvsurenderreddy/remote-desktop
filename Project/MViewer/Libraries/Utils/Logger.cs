using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Utils
{
    public class Logger
    {
        #region private members

        ILog _logger;

        #endregion

        #region public methods

        public void LoggerInitialize()
        {
            log4net.Config.XmlConfigurator.Configure();
           _logger = log4net.LogManager.GetLogger("MViewer");
        }

        public void LogInfo(string text)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine(string.Format("{0} : --------------", DateTime.Now.ToString()));
            message.AppendLine(text);
            message.AppendLine("-------------- END INFO ------------");
            _logger.Info(message.ToString());
        }

        public void LogError(string text)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("--------------");
            message.AppendLine(text);
            message.AppendLine("END ERROR -------");
            _logger.Info(message.ToString());
        }

        #endregion
    }
}
