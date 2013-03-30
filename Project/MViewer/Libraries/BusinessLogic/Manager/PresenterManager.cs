using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GenericObjects;

namespace BusinessLogicLayer
{
    public static class PresenterManager 
    {
        #region private members

        static readonly object _syncPresenter = new object();
        static IPresenter _presenter;

        #endregion

        #region c-tor

        static PresenterManager()
        {

        }

        #endregion

        #region public methods

        public static IPresenter Instance(PresenterSettings presenterSettings)
        {
            if (_presenter == null)
            {
                lock (_syncPresenter)
                {
                    if (_presenter == null)
                    {        
                        _presenter = new Presenter(presenterSettings);
                    }
                }
            }
            return _presenter;
        }

        #endregion
    }
}
