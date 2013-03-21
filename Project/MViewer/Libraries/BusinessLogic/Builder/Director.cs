using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericDataLayer;

namespace BusinessLogicLayer
{
    internal class Director
    {
        static readonly object _syncInstance = new object();
        static Director _director;

        private Director() { }

        public static Director Instance
        {
            get
            {
                if (_director == null)
                {
                    lock (_syncInstance)
                    {
                        _director = new Director();
                    }
                }
                return _director;
            }
        }

        public void Construct(Builder builder)
        {
            if (builder.GetType().Equals(typeof(ClientBuilder)))
            {
                builder.BuildBinding();
                builder.BuildContract();
                builder.BuildCertificate();
            }
            else
            {
                builder.BuildUri();
                builder.BuildBehavior();
                builder.BuildBinding();
                builder.BuildCertificate();
            }
        }
    }
}
