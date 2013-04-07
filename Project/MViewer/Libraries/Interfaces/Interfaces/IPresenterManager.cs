using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDataLayer
{
    public interface IPresenterManager
    {
        void AddPresenter(string identity, IPresenter presenter);
        void RemovePresenter(string identity);
        void StartPresentation(string identity);
        void StopPresentation(string identity);
    }
}
