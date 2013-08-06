using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RSclient
{
    public static class Loader
    {
        public static Dictionary<int, Domain> getDomains(CommandReader cr)
        {
            Dictionary<int, Domain> res = new Dictionary<int,Domain>();
            int domainCount = cr.getInt();
            for (int i = 0; i < domainCount; i++)
            {
                Domain dom = new Domain();
                dom.id = cr.getInt();
                dom.color = cr.getInt();
                dom.description = cr.getStr();
                dom.x = cr.getInt();
                dom.y = cr.getInt();
                int enemyCount = cr.getInt();
                for (int j = 0; j < enemyCount; j++)
                {
                    dom.enemy.Add(cr.getInt());
                }
                res.Add(dom.id, dom);
            }
            return res;
        }
        public static Dictionary<int, Nebula> getNebulas(CommandReader cr)
        {
            Dictionary<int, Nebula> res = new Dictionary<int, Nebula>();
            int nebulasCount = cr.getInt();
            for (int i = 0; i < nebulasCount; i++)
            {
                Nebula neb = new Nebula();
                neb.id = cr.getInt();
                neb.type = cr.getInt();
                neb.x = cr.getInt();
                neb.y = cr.getInt();
                neb.scale = cr.getInt();
                neb.angle = cr.getInt();
                res.Add(neb.id, neb);
            }
            return res;
        }
        public static void getUseEquip(CommandReader cr, User user)
        {
            int useEquip = cr.getInt();
            int userId = cr.getInt();
            int targetUserId = cr.getInt();
            User.ActionList action = (User.ActionList)cr.getInt();
            int actioonValue = cr.getInt();
            
        }
        public static MoveUser getTouchUser(CommandReader cr, User user)
        {
            MoveUser res = null;
            int userId = cr.getInt();
            int userTX = cr.getInt();
            int userTY = cr.getInt();
            double flyTime = cr.getDbl();
            int fuel = cr.getInt();
            double unic_epox = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
            User usr = null;
            if (user.usersClose.ContainsKey(userId))
            {
                usr = user.usersClose[userId];
            }
            if (user.id == userId)
            {
                usr = user;
            }
            if (usr != null)
            {
                res = new MoveUser();
                res.userId = usr.id;
                res.x = usr.x;
                res.y = usr.y;
                res.targetX = userTX;
                res.targetY = userTY;
                res.startMove = unic_epox;
                res.flyTime = flyTime;
                res.fuel = fuel;
            }
            return res;
        }
        public static void getLocations(CommandReader cr, User user)
        {
            int locationCount = cr.getInt();
            for (int i = 0; i < locationCount; i++)
            {
                Location loc = new Location();
                loc.id = cr.getInt();
                loc.starName = cr.getStr();
                loc.starType = cr.getInt();
                loc.x = cr.getInt();
                loc.y = cr.getInt();
                loc.radius = cr.getInt();
                loc.domain = cr.getInt();
                user.locations.Add(loc.id, loc);
            }
        }
        public static void getPlanets(CommandReader cr, User user)
        {
            Location parentLocation = user.locations[cr.getInt()];
            parentLocation.planets = new Dictionary<int, Planet>();
            parentLocation.isLoadPlanet = true;
            int planetsCount = cr.getInt();
            for (int i = 0; i < planetsCount; i++)
            {
                Planet pln = new Planet();
                pln.parent = parentLocation;
                pln.id = cr.getInt();
                pln.planetName = cr.getStr();
                pln.planetType = cr.getInt();
                pln.r_speed = cr.getInt();
                pln.orbit = cr.getInt();
                pln.radius = cr.getInt();
                pln.color = cr.getInt();
                pln.atmosphere = cr.getInt();
                pln.domain = MainData.domains[cr.getInt()];
                pln.atmosphere_speedX = cr.getInt();
                pln.atmosphere_speedY = cr.getInt();
                pln.price_coef = cr.getInt();
                if (!parentLocation.planets.ContainsKey(pln.id))
                {
                    parentLocation.planets.Add(pln.id, pln);
                }
                if (!user.planets.ContainsKey(pln.id))
                {
                    user.planets.Add(pln.id, pln);
                }
            }
        }
        public static ItemCollection getItems(CommandReader cr, User user)
        {
            ItemCollection res = new ItemCollection();
            int itemTypeCount = cr.getInt();
            for (int i = 0; i < itemTypeCount; i++)
            {
                int itemCount = cr.getInt();
                for (int j = 0; j < itemCount; j++)
                {
                    int item_id = cr.getInt();
                    Item.ItemType item_itemType = (Item.ItemType)cr.getInt();
                    String item_description = cr.getStr();
                    int item_volume = cr.getInt();
                    int item_region_id = cr.getInt();
                    int item_use_only = cr.getInt();
                    int item_price = cr.getInt();
                    switch (item_itemType)
                    {
                        case Item.ItemType.consumable:
                            {
                                Consumable item = new Consumable();
                                item.id = item_id;
                                item.itemType = item_itemType;
                                item.description = item_description;
                                item.volume = item_volume;
                                item.region = item_region_id;
                                res.consumables.Add(item.id, item);
                                break;
                            }
                        case Item.ItemType.device:
                            {
                                String device_vendorStr = cr.getStr();
                                Device.DeviceType device_deviceType = (Device.DeviceType)cr.getInt();
                                int device_durability = cr.getInt();
                                switch (device_deviceType)
                                {
                                    case Device.DeviceType.Body:
                                        {
                                            Body dev = new Body();
                                            int body_slot_weapons = cr.getInt();
                                            int body_slot_droids = cr.getInt();
                                            int body_slot_shield = cr.getInt();
                                            int body_slot_hyper = cr.getInt();
                                            dev.id = item_id;
                                            dev.itemType = item_itemType;
                                            dev.description = item_description;
                                            dev.volume = item_volume;
                                            dev.region = item_region_id;
                                            dev.vendorStr = device_vendorStr;
                                            dev.deviceType = device_deviceType;
                                            dev.durability = device_durability;
                                            dev.use_only = item_use_only;
                                            dev.price = item_price;
                                            dev.slot_weapons = body_slot_weapons;
                                            dev.slot_droids = body_slot_droids;
                                            dev.slot_shield = body_slot_shield;
                                            dev.slot_hyper = body_slot_hyper;
                                            res.bodyes.Add(dev.id, dev);
                                            break;
                                        }
                                    case Device.DeviceType.Droid:
                                        {
                                            Droid dev = new Droid();
                                            int droid_power = cr.getInt();
                                            int droid_time_reload = cr.getInt();
                                            int radius = cr.getInt();
                                            dev.id = item_id;
                                            dev.itemType = item_itemType;
                                            dev.description = item_description;
                                            dev.volume = item_volume;
                                            dev.region = item_region_id;
                                            dev.vendorStr = device_vendorStr;
                                            dev.deviceType = device_deviceType;
                                            dev.durability = device_durability;
                                            dev.use_only = item_use_only;
                                            dev.price = item_price;
                                            dev.power = droid_power;
                                            dev.time_reload = droid_time_reload;
                                            res.droids.Add(dev.id, dev);
                                            break;
                                        }
                                    case Device.DeviceType.Engine:
                                        {
                                            Engine dev = new Engine();
                                            int engine_power = cr.getInt();
                                            int engine_economic = cr.getInt();
                                            dev.id = item_id;
                                            dev.itemType = item_itemType;
                                            dev.description = item_description;
                                            dev.volume = item_volume;
                                            dev.region = item_region_id;
                                            dev.vendorStr = device_vendorStr;
                                            dev.deviceType = device_deviceType;
                                            dev.durability = device_durability;
                                            dev.use_only = item_use_only;
                                            dev.price = item_price;
                                            dev.power = engine_power;
                                            dev.economic = engine_economic;
                                            res.engines.Add(dev.id, dev);
                                            break;
                                        }
                                    case Device.DeviceType.Fuelbag:
                                        {
                                            Fuelbag dev = new Fuelbag();
                                            int fuelbag_compress = cr.getInt();
                                            dev.id = item_id;
                                            dev.itemType = item_itemType;
                                            dev.description = item_description;
                                            dev.volume = item_volume;
                                            dev.region = item_region_id;
                                            dev.vendorStr = device_vendorStr;
                                            dev.deviceType = device_deviceType;
                                            dev.durability = device_durability;
                                            dev.use_only = item_use_only;
                                            dev.price = item_price;
                                            dev.compress = fuelbag_compress;
                                            res.fuelbags.Add(dev.id, dev);
                                            break;
                                        }
                                    case Device.DeviceType.Hyper:
                                        {
                                            Hyper dev = new Hyper();
                                            int hyper_radius = cr.getInt();
                                            int hyper_time_start = cr.getInt();
                                            int hyper_time_reload = cr.getInt();
                                            dev.id = item_id;
                                            dev.itemType = item_itemType;
                                            dev.description = item_description;
                                            dev.volume = item_volume;
                                            dev.region = item_region_id;
                                            dev.vendorStr = device_vendorStr;
                                            dev.deviceType = device_deviceType;
                                            dev.durability = device_durability;
                                            dev.use_only = item_use_only;
                                            dev.price = item_price;
                                            dev.radius = hyper_radius;
                                            dev.time_start = hyper_time_start;
                                            dev.time_reload = hyper_time_reload;
                                            res.hypers.Add(dev.id, dev);
                                            break;
                                        }
                                    case Device.DeviceType.Radar:
                                        {
                                            Radar dev = new Radar();
                                            int radar_radius = cr.getInt();
                                            int radar_defense = cr.getInt();
                                            int big_radius = cr.getInt();
                                            dev.id = item_id;
                                            dev.itemType = item_itemType;
                                            dev.description = item_description;
                                            dev.volume = item_volume;
                                            dev.region = item_region_id;
                                            dev.vendorStr = device_vendorStr;
                                            dev.deviceType = device_deviceType;
                                            dev.durability = device_durability;
                                            dev.use_only = item_use_only;
                                            dev.price = item_price;
                                            dev.radius = radar_radius;
                                            dev.defense = radar_defense;
                                            dev.hyper = big_radius;
                                            res.radars.Add(dev.id, dev);
                                            break;
                                        }
                                    case Device.DeviceType.Shield:
                                        {
                                            Shield dev = new Shield();
                                            int shield_power = cr.getInt();
                                            dev.id = item_id;
                                            dev.itemType = item_itemType;
                                            dev.description = item_description;
                                            dev.volume = item_volume;
                                            dev.region = item_region_id;
                                            dev.vendorStr = device_vendorStr;
                                            dev.deviceType = device_deviceType;
                                            dev.durability = device_durability;
                                            dev.use_only = item_use_only;
                                            dev.price = item_price;
                                            dev.power = shield_power;
                                            res.shields.Add(dev.id, dev);
                                            break;
                                        }
                                    case Device.DeviceType.Weapon:
                                        {
                                            Weapon dev = new Weapon();
                                            int weapon_weaponType = cr.getInt();
                                            int weapon_radius = cr.getInt();
                                            int weapon_power = cr.getInt();
                                            int weapon_time_start = cr.getInt();
                                            int weapon_time_reload = cr.getInt();
                                            dev.id = item_id;
                                            dev.itemType = item_itemType;
                                            dev.description = item_description;
                                            dev.volume = item_volume;
                                            dev.region = item_region_id;
                                            dev.vendorStr = device_vendorStr;
                                            dev.deviceType = device_deviceType;
                                            dev.durability = device_durability;
                                            dev.use_only = item_use_only;
                                            dev.price = item_price;
                                            dev.weaponType = (Weapon.WeaponType)weapon_weaponType;
                                            dev.radius = weapon_radius;
                                            dev.power = weapon_power;
                                            dev.time_start = weapon_time_start;
                                            dev.time_reload = weapon_time_reload;
                                            res.weapons.Add(dev.id, dev);
                                            break;
                                        }
                                }
                                break;
                            }
                    }
                }
            }
            return res;
        }
        public static Dictionary<int, Equip> getEquips(CommandReader cr, User user)
        {
            Dictionary<int, Equip> res = new Dictionary<int, Equip>();
            int equipCount = cr.getInt();
            for (int i = 0; i < equipCount; i++)
            {
                Equip eq = new Equip();
                eq.id = cr.getInt();
                int item_id = cr.getInt();
                Item.ItemType iType = (Item.ItemType)cr.getInt();
                int dType = cr.getInt();
                eq.in_use = cr.getInt()==0 ? false: true;
                eq.wear = cr.getInt();
                int location  = cr.getInt();
                eq.location = location == 0 ? null : user.planets[location];
                eq.num = cr.getInt();
                switch (iType)
                {
                    case Item.ItemType.consumable:
                        {
                            Consumable item = MainData.itemCollect.get<Consumable>(item_id);
                            eq.item = item;
                            break;
                        }
                    case Item.ItemType.device:
                        {
                            Device.DeviceType devT = (Device.DeviceType)dType;
                            switch (devT)
                            {
                                case Device.DeviceType.Body:
                                    {
                                        Body item = MainData.itemCollect.get<Body>(item_id);
                                        eq.item = item;
                                        break;
                                    }
                                case Device.DeviceType.Droid:
                                    {
                                        Droid item = MainData.itemCollect.get<Droid>(item_id);
                                        eq.item = item;
                                        break;
                                    }
                                case Device.DeviceType.Engine:
                                    {
                                        Engine item = MainData.itemCollect.get<Engine>(item_id);
                                        eq.item = item;
                                        break;
                                    }
                                case Device.DeviceType.Fuelbag:
                                    {
                                        Fuelbag item = MainData.itemCollect.get<Fuelbag>(item_id);
                                        eq.item = item;
                                        break;
                                    }
                                case Device.DeviceType.Hyper:
                                    {
                                        Hyper item = MainData.itemCollect.get<Hyper>(item_id);
                                        eq.item = item;
                                        break;
                                    }
                                case Device.DeviceType.Radar:
                                    {
                                        Radar item = MainData.itemCollect.get<Radar>(item_id);
                                        eq.item = item;
                                        break;
                                    }
                                case Device.DeviceType.Shield:
                                    {
                                        Shield item = MainData.itemCollect.get<Shield>(item_id);
                                        eq.item = item;
                                        break;
                                    }
                                case Device.DeviceType.Weapon:
                                    {
                                        Weapon item = MainData.itemCollect.get<Weapon>(item_id);
                                        eq.item = item;
                                        break;
                                    }
                            }
                            break;
                        }
                }
                res.Add(eq.id, eq);
            }
            return res;
        }
        public static User getUserData(CommandReader cr, User user)
        {
            User res = user;
            res.id = cr.getInt();
            res.x = cr.getInt();
            res.y = cr.getInt();
            res.domain = MainData.domains[cr.getInt()];
            int inPlanet = cr.getInt();
            if (inPlanet == 0) { res.inPlanet = null; }
            else { res.inPlanet = user.planets[inPlanet]; }
            res.pilotName = cr.getStr();
            res.shipName = cr.getStr();
            res.equips = getEquips(cr, user);
            return res;
        }
        public static User getAddUser(CommandReader cr, User user)
        {
            User res = new User();
            res.id = cr.getInt();
            res.shipName = cr.getStr();
            res.x = cr.getInt();
            res.y = cr.getInt();
            res.targetX = cr.getInt();
            res.targetY = cr.getInt();
            res.domain = MainData.domains[cr.getInt()];
            res.equips = getEquips(cr, user);
            res.updateUserShip();
            return res;
        }
        public static int getRemoveUser(CommandReader cr)
        {
            int res = cr.getInt();
            return res;
        }
    }
}
