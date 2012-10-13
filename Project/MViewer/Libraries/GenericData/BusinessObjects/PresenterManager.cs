using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDataLayer
{
    public class PresenterManager : IPresenterManager
    {
        #region private members

        Dictionary<string, IPresenter> _presenters;

        #endregion

        #region c-tor

        public PresenterManager()
        {
            _presenters = new Dictionary<string, IPresenter>();
        }

        #endregion

        #region public methods

        public void AddPresenter(string identity, IPresenter presenter)
        {
            if (_presenters == null)
            {
                _presenters = new Dictionary<string, IPresenter>();
            }
            if (!_presenters.ContainsKey(identity))
            {
                _presenters.Add(identity, presenter);
            }
        }

        public void RemovePresenter(string identity)
        {
            if (_presenters != null && _presenters.ContainsKey(identity))
            {
                _presenters[identity].StopPresentation();
                _presenters.Remove(identity);
            }
        }

        public void StartPresentation(string identity)
        {
            if (_presenters != null && _presenters.ContainsKey(identity))
            {
                IPresenter presenter = _presenters[identity];
                presenter.StartPresentation();
            }
        }

        public void StopPresentation(string identity)
        {
            if (_presenters != null && _presenters.ContainsKey(identity))
            {
                _presenters[identity].StopPresentation();
            }
        }

        #endregion
    }
}
