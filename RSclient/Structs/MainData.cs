﻿using System;
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
        public Dictionary<int, Location> locations = new Dictionary<int,Location>();
        public Dictionary<int, Nebula> nebulas = new Dictionary<int,Nebula>();
        public Dictionary<int, Planet> planets = new Dictionary<int,Planet>();
        public ItemCollection itemCollect = new ItemCollection();
        private bool _isDomains;
        private bool _isLocations;
        private bool _isPlanets;
        private bool _isNebulas;
        private bool _isItems;
        
        public bool isLoaded;
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
        public bool isLocations
        {
            get
            {
                return _isLocations;
            }
            set
            {
                if (value != _isLocations)
                {
                    _isLocations = value;
                    if (_isLocations)
                    {
                        OnIsLocationsLoadEvent();
                    }
                }
            }
        }
        public bool isPlanets
        {
            get
            {
                return _isPlanets;
            }
            set
            {
                if (value != _isPlanets)
                {
                    _isPlanets = value;
                    if (_isPlanets)
                    {
                        OnIsPlanetsLoadEvent();
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
        public event MainDataEventHandler isLocationsLoad;
        public event MainDataEventHandler isPlanetsLoad;
        public event MainDataEventHandler isItemsLoad;
        public event MainDataEventHandler isNebulasLoad;
        protected virtual void OnIsDomainsLoadEvent()
        {
            if (isDomainsLoad != null)
                isDomainsLoad(this, EventArgs.Empty);
        }
        protected virtual void OnIsLocationsLoadEvent()
        {
            if (isLocationsLoad != null)
                isLocationsLoad(this, EventArgs.Empty);
        }
        protected virtual void OnIsPlanetsLoadEvent()
        {
            if (isPlanetsLoad != null)
                isPlanetsLoad(this, EventArgs.Empty);
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