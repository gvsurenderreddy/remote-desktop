using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Utils
{
    public class Tools
    {
        ImageConverter _imageConverter;
        RemotingUtils _desktopViewerUtils;
        Cryptography _cryptography;

        static Tools _instance = new Tools();
        static readonly object _syncInstance = new object();

        private Tools() 
        {
            _imageConverter = new ImageConverter();
            _desktopViewerUtils = new RemotingUtils();
            _cryptography = new Cryptography();
        }

        public static Tools Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncInstance)
                    {
                        if (_instance == null)
                        {
                            _instance = new Tools();
                        }
                    }
                }
                return _instance;
            }
        }

        public ImageConverter ImageConverter
        {
            get { return _imageConverter; }
        }

        public RemotingUtils RemotingUtils
        {
            get { return _desktopViewerUtils; }
        }
        public Cryptography Cryptography
        {
            get { return _cryptography; }
        }
    }
}
