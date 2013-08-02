using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSclient
{
    public class MainData
    {
        public delegate void MainDataEventHandler(object sender, EventArgs e);
        public Dictionary<int, Domain> domains = new Dictionary<int,Domain>();
        public Dictionary<int, Nebula> nebulas = new Dictionary<int,Nebula>();
        public ItemCollection itemCollect = new ItemCollection();
        private bool _isDomains = false;
        private bool _isNebulas = false;
        private bool _isItems = false;
        public bool loadingDomains = false;
        public bool loadingNebulas = false;
        public bool loadingItems = false;
        public bool isLoaded = false;

        public bool isDomains
        {
            get
            {
                return _isDomains;
            }
            set
            {
                if (value != _isDomains)
                {
                    _isDomains = value;
                    if (_isDomains)
                    {
                        OnIsDomainsLoadEvent();
                    }
                }
            }
        }

        public bool isNebulas
        {
            get
            {
                return _isNebulas;
            }
            set
            {
                if (value != _isNebulas)
                {
                    _isNebulas = value;
                    if (_isNebulas)
                    {
                        OnIsNebulasLoadEvent();
                    }
                }
            }
        }
        public bool isItems
        {
            get
            {
                return _isItems;
            }
            set
            {
                if (value != _isItems)
                {
                    _isItems = value;
                    if (_isItems)
                    {
                        OnIsItemsLoadEvent();
                    }
                }
            }
        }
        public event MainDataEventHandler isDomainsLoad;
        public event MainDataEventHandler isItemsLoad;
        public event MainDataEventHandler isNebulasLoad;
        protected virtual void OnIsDomainsLoadEvent()
        {
            if (isDomainsLoad != null)
                isDomainsLoad(this, EventArgs.Empty);
        }
        protected virtual void OnIsItemsLoadEvent()
        {
            if (isItemsLoad != null)
                isItemsLoad(this, EventArgs.Empty);
        }
        protected virtual void OnIsNebulasLoadEvent()
        {
            if (isNebulasLoad != null)
                isNebulasLoad(this, EventArgs.Empty);
        }
    }
}
