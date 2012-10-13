using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GenericDataLayer
{
    public interface IView
    {
        void NotifyContactsObserver();
        void NotifyIdentityObserver();
        void NotifyActionsObserver();
        void BindObservers(bool bind);

        void ShowMainForm(bool close);

    }
}
