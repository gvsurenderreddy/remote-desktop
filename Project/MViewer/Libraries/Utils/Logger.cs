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
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                _logger = log4net.LogManager.GetLogger("MViewer");
            }
            catch (Exception)
            {
                //Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void LogInfo(string text)
        {
            try
            {
                StringBuilder message = new StringBuilder();
                message.AppendLine(string.Format("{0} -----------------", DateTime.Now.ToString()));
                message.AppendLine(text);
                message.AppendLine("END INFO -----------------------------------");
                _logger.Info(message.ToString());
            }
            catch (Exception ex)
            {
                Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        public void LogError(string text)
        {
            try
            {
                StringBuilder message = new StringBuilder();
                message.AppendLine(string.Format("{0} -----------------", DateTime.Now.ToString()));
                message.AppendLine(text);
                message.AppendLine("END ERROR -----------------------------------");
                _logger.Error(message.ToString());
            }
            catch (Exception)
            {
                //Tools.Instance.Logger.LogError(ex.ToString());
            }
        }

        #endregion
    }
}
