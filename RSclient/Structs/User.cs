using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RSclient
{
    public class User : INotifyPropertyChanged
    {
        public delegate void UserEventHandler(object sender, EventArgs e);

        public enum TargetType : int
        {
            none = 0,
            planet = 1,
            player = 2,
            item = 3,
            star = 4
        }
        public enum ActionList : int
        {
            none = 0,
            attack = 1,
            repair = 2,
        }
        public enum ErrorList : byte
        {
            None,
            Connect,
            Protocol, 
            Password
        }

        private int _id;
        private string _login;
        private string _pilotName;
        private string _shipName;
        private bool _isLogin = false;
        private bool _isPassword = false;
        private bool _loadComplite = false;

        public string password;
        public Socket handler;
        public List<string> log = new List<string>();
        public int x;
        public int y;
        public int targetX;
        public int targetY;
        public Domain domain = new Domain();
        public Planet inPlanet = new Planet();
        public Dictionary<int, Equip> equips = new Dictionary<int, Equip>();
        public int protocolVersion;
        public Dictionary<int, User> usersClose = new Dictionary<int, User>();
        public TargetType targetType;
        public int target;
        public UserShip userShip = new UserShip();
        public double gcd;
        public int serverTime = 0;
        public ErrorList error;
        
        #region ObservableObjects
        public bool isPassword
        {
            get
            {
                return _isPassword;
            }
            set
            {
                if (value != _isPassword)
                {
                    _isPassword = value;
                    if (_isPassword)
                    {
                        OnIsPasswordCompliteEvent();
                    }
                }
            }
        }
        public bool isLogin
        {
            get
            {
                return _isLogin;
            }
            set
            {
                if (value != _isLogin)
                {
                    _isLogin = value;
                    if (_isLogin)
                    {
                        OnIsLoginCompliteEvent();
                    }
                }
            }
        }
        public bool isLoadComplite
        {
            get
            {
                return _loadComplite;
            }
            set
            {
                if (value != _loadComplite)
                {
                    _loadComplite = value;
                    if (_loadComplite)
                    {
                        OnIsLoadCompliteEvent();
                    }
                }
            }
        }
        public string login
        {
            get
            {
                return _login;
            }
            set
            {
                if (value != _login)
                {
                    _login = value;
                    OnPropertyChanged("login");
                }
            }
        }
        public int id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    OnPropertyChanged("id");
                }
            }
        }
        public string pilotName
        {
            get
            {
                return _pilotName;
            }
            set
            {
                if (value != _pilotName)
                {
                    _pilotName = value;
                    OnPropertyChanged("pilotName");
                }
            }
        }
        public string shipName
        {
            get
            {
                return _shipName;
            }
            set
            {
                if (value != _shipName)
                {
                    _shipName = value;
                    OnPropertyChanged("shipName");
                }
            }
        }
        #endregion

        public event UserEventHandler isLoginComplite;
        protected virtual void OnIsLoginCompliteEvent()
        {
            if (isLoginComplite != null)
                isLoginComplite(this, EventArgs.Empty);
        }
        public event UserEventHandler isPasswordComplite;
        protected virtual void OnIsPasswordCompliteEvent()
        {
            if (isPasswordComplite != null)
                isPasswordComplite(this, EventArgs.Empty);
        }
        public event UserEventHandler isLoadingComplite;
        protected virtual void OnIsLoadCompliteEvent()
        {
            if (isLoadingComplite != null)
                isLoadingComplite(this, EventArgs.Empty);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string p_propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(p_propertyName));
            }
        }

        #region Methods
        public void updateUserShip()
        {
            this.userShip.weapons = new List<Equip>();
            this.userShip.droids = new List<Equip>();
            for (int i = 0; i < this.equips.Count; i++)
            {
                Consumable cons = this.equips.ElementAt(i).Value.item as Consumable;
                if ((cons != null) && (cons.id == (int)Consumable.Type.FuelID))
                {
                    this.userShip.fuel = this.equips.ElementAt(i).Value;
                    this.userShip.fuel.num = this.equips.ElementAt(i).Value.num;
                    continue;
                }
                if (this.equips.ElementAt(i).Value.in_use)
                {
                    Body body = this.equips.ElementAt(i).Value.item as Body;
                    if (body != null)
                    {
                        this.userShip.body = this.equips.ElementAt(i).Value;
                        continue;
                    }
                    Engine engine = this.equips.ElementAt(i).Value.item as Engine;
                    if (engine != null)
                    {
                        this.userShip.engine = this.equips.ElementAt(i).Value;
                        continue;
                    }
                    Fuelbag fuelbag = this.equips.ElementAt(i).Value.item as Fuelbag;
                    if (fuelbag != null)
                    {
                        this.userShip.fuelbag = this.equips.ElementAt(i).Value;
                        continue;
                    }
                    Hyper hyper = this.equips.ElementAt(i).Value.item as Hyper;
                    if (hyper != null)
                    {
                        this.userShip.hyper = this.equips.ElementAt(i).Value;
                        continue;
                    }
                    Shield shield = this.equips.ElementAt(i).Value.item as Shield;
                    if (shield != null)
                    {
                        this.userShip.shield = this.equips.ElementAt(i).Value;
                        continue;
                    }
                    Radar radar = this.equips.ElementAt(i).Value.item as Radar;
                    if (radar != null)
                    {
                        this.userShip.radar = this.equips.ElementAt(i).Value;
                        continue;
                    }
                    Weapon weapon = this.equips.ElementAt(i).Value.item as Weapon;
                    if (weapon != null)
                    {
                        this.userShip.weapons.Add(this.equips.ElementAt(i).Value);
                        continue;
                    }
                    Droid droid = this.equips.ElementAt(i).Value.item as Droid;
                    if (droid != null)
                    {
                        this.userShip.droids.Add(this.equips.ElementAt(i).Value);
                        continue;
                    }
                }
            }
            this.userShip.maxFuel = 0;
            this.userShip.maxSpeed = 0;
            if (this.userShip.fuelbag != null)
            {
                Fuelbag fb = this.userShip.fuelbag.item as Fuelbag;
                this.userShip.maxFuel = fb.volume * fb.compress / 100;
            }
            if (this.userShip.engine != null)
            {
                Engine eng = this.userShip.engine.item as Engine;
                this.userShip.maxSpeed = eng.power;
            }
        }
        public void crashUserShip(int percent)
        {
            if (this.userShip.engine != null)
            {
                Device dev = this.userShip.engine.item as Device;
                this.userShip.engine.wear = this.userShip.engine.wear - (int)Math.Round((double)dev.durability * percent / 100, 0);
                if (this.userShip.engine.wear < 0) { this.userShip.engine.wear = 0; }
            }
            if (this.userShip.fuelbag != null)
            {
                Device dev = this.userShip.fuelbag.item as Device;
                this.userShip.fuelbag.wear = this.userShip.fuelbag.wear - (int)Math.Round((double)dev.durability * percent / 100, 0);
                if (this.userShip.fuelbag.wear < 0) { this.userShip.fuelbag.wear = 0; }
            }
            if (this.userShip.hyper != null)
            {
                Device dev = this.userShip.hyper.item as Device;
                this.userShip.hyper.wear = this.userShip.hyper.wear - (int)Math.Round((double)dev.durability * percent / 100, 0);
                if (this.userShip.hyper.wear < 0) { this.userShip.hyper.wear = 0; }
            }
            if (this.userShip.radar != null)
            {
                Device dev = this.userShip.radar.item as Device;
                this.userShip.radar.wear = this.userShip.radar.wear - (int)Math.Round((double)dev.durability * percent / 100, 0);
                if (this.userShip.radar.wear < 0) { this.userShip.radar.wear = 0; }
            }
            if (this.userShip.shield != null)
            {
                Device dev = this.userShip.shield.item as Device;
                this.userShip.shield.wear = this.userShip.shield.wear - (int)Math.Round((double)dev.durability * percent / 100, 0);
                if (this.userShip.shield.wear < 0) { this.userShip.shield.wear = 0; }
            }
            for (int i = 0; i < userShip.droids.Count; i++)
            {
                Device dev = this.userShip.droids[i].item as Device;
                this.userShip.droids[i].wear = this.userShip.droids[i].wear - (int)Math.Round((double)dev.durability * percent / 100, 0);
                if (this.userShip.droids[i].wear < 0) { this.userShip.droids[i].wear = 0; }
            }
            for (int i = 0; i < userShip.weapons.Count; i++)
            {
                Device dev = this.userShip.weapons[i].item as Device;
                this.userShip.weapons[i].wear = this.userShip.weapons[i].wear - (int)Math.Round((double)dev.durability * percent / 100, 0);
                if (this.userShip.weapons[i].wear < 0) { this.userShip.weapons[i].wear = 0; }
            }
        }
        #endregion
    }
}
