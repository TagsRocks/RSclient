using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSclient
{
    public static class MainData
    {
        public delegate void MainDataEventHandler(object sender, EventArgs e);
        public static Dictionary<int, Domain> domains = new Dictionary<int, Domain>();
        public static Dictionary<int, Nebula> nebulas = new Dictionary<int, Nebula>();
        public static Dictionary<int, User> users = new Dictionary<int, User>();
        public static ItemCollection itemCollect = new ItemCollection();
        private static bool _isDomains = false;
        private static bool _isNebulas = false;
        private static bool _isItems = false;
        public static bool loadingDomains = false;
        public static bool loadingNebulas = false;
        public static bool loadingItems = false;
        public static bool isLoaded = false;

        public static bool isDomains
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

        public static bool isNebulas
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
        public static bool isItems
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
        public static event MainDataEventHandler isDomainsLoad;
        public static event MainDataEventHandler isItemsLoad;
        public static event MainDataEventHandler isNebulasLoad;
        public static void OnIsDomainsLoadEvent()
        {
            if (isDomainsLoad != null)
                isDomainsLoad(null, EventArgs.Empty);
        }
        public static void OnIsItemsLoadEvent()
        {
            if (isItemsLoad != null)
                isItemsLoad(null, EventArgs.Empty);
        }
        public static void OnIsNebulasLoadEvent()
        {
            if (isNebulasLoad != null)
                isNebulasLoad(null, EventArgs.Empty);
        }
    }
}
