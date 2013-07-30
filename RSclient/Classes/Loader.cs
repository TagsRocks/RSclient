using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RSclient
{
    public class Loader
    {
        public Dictionary<int, Domain> getDomains(CommandReader cr)
        {
            Dictionary<int, Domain> res = new Dictionary<int,Domain>();
            int domainCount = cr.GetIntValue();
            for (int i = 0; i < domainCount; i++)
            {
                Domain dom = new Domain();
                dom.id = cr.GetIntValue();
                dom.color = cr.GetIntValue();
                dom.description = cr.GetStringValue(cr.GetIntValue());
                dom.x = cr.GetIntValue();
                dom.y = cr.GetIntValue();
                res.Add(dom.id, dom);
            }
            return res;
        }
        public Dictionary<int, Nebula> getNebulas(CommandReader cr)
        {
            Dictionary<int, Nebula> res = new Dictionary<int, Nebula>();
            int nebulasCount = cr.GetIntValue();
            for (int i = 0; i < nebulasCount; i++)
            {
                Nebula neb = new Nebula();
                neb.id = cr.GetIntValue();
                neb.type = cr.GetIntValue();
                neb.x = cr.GetIntValue();
                neb.y = cr.GetIntValue();
                neb.scale = cr.GetIntValue();
                neb.angle = cr.GetIntValue();
                res.Add(neb.id, neb);
            }
            return res;
        }
        public Dictionary<int, Location> getLocations(CommandReader cr, MainData mainData)
        {
            Dictionary<int, Location> res = new Dictionary<int, Location>();
            int locationCount = cr.GetIntValue();
            for (int i = 0; i < locationCount; i++)
            {
                Location loc = new Location();
                loc.id = cr.GetIntValue();
                loc.starName = cr.GetStringValue(cr.GetIntValue());
                loc.starType = cr.GetIntValue();
                loc.x = cr.GetIntValue();
                loc.y = cr.GetIntValue();
                loc.radius = cr.GetIntValue();
                loc.domain = mainData.domains[cr.GetIntValue()];
                res.Add(loc.id, loc);
            }
            return res;
        }
        public Dictionary<int, Planet> getPlanets(CommandReader cr, MainData mainData)
        {
            Dictionary<int, Planet> res = mainData.planets;
            Location parentLocation = mainData.locations[cr.GetIntValue()];
            int planetsCount = cr.GetIntValue();
            for (int i = 0; i < planetsCount; i++)
            {
                Planet pln = new Planet();
                pln.parent = parentLocation;
                pln.id = cr.GetIntValue();
                pln.planetName = cr.GetStringValue(cr.GetIntValue());
                pln.planetType = cr.GetIntValue();
                pln.r_speed = cr.GetIntValue();
                pln.orbit = cr.GetIntValue();
                pln.radius = cr.GetIntValue();
                pln.color = cr.GetIntValue();
                pln.atmosphere = cr.GetIntValue();
                pln.domain = mainData.domains[cr.GetIntValue()];
                pln.atmosphere_speedX = cr.GetIntValue();
                pln.atmosphere_speedY = cr.GetIntValue();
                pln.price_coef = cr.GetIntValue();
                parentLocation.planets.Add(pln.id, pln);
                res.Add(pln.id, pln);
            }
            return res;
        }
        public ItemCollection getItems(CommandReader cr, MainData mainData)
        {
            ItemCollection res = new ItemCollection();
            int itemTypeCount = cr.GetIntValue();
            for (int i = 0; i < itemTypeCount; i++)
            {
                int itemCount = cr.GetIntValue();
                for (int j = 0; j < itemCount; j++)
                {
                    int item_id = cr.GetIntValue();
                    Item.ItemType item_itemType = (Item.ItemType)cr.GetIntValue();
                    String item_description = cr.GetStringValue(cr.GetIntValue());
                    int item_volume = cr.GetIntValue();
                    int item_region_id = cr.GetIntValue();
                    int item_packing = cr.GetIntValue();
                    int item_use_only = cr.GetIntValue();
                    int item_price = cr.GetIntValue();
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
                                item.packing = item_packing;
                                res.consumables.Add(item.id, item);
                                break;
                            }
                        case Item.ItemType.device:
                            {
                                String device_vendorStr = cr.GetStringValue(cr.GetIntValue());
                                Device.DeviceType device_deviceType = (Device.DeviceType)cr.GetIntValue();
                                int device_durability = cr.GetIntValue();
                                switch (device_deviceType)
                                {
                                    case Device.DeviceType.Body:
                                        {
                                            Body dev = new Body();
                                            int body_slot_weapons = cr.GetIntValue();
                                            int body_slot_droids = cr.GetIntValue();
                                            int body_slot_shield = cr.GetIntValue();
                                            int body_slot_hyper = cr.GetIntValue();
                                            dev.id = item_id;
                                            dev.itemType = item_itemType;
                                            dev.description = item_description;
                                            dev.volume = item_volume;
                                            dev.region = item_region_id;
                                            dev.packing = item_packing;
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
                                            int droid_power = cr.GetIntValue();
                                            int droid_time_reload = cr.GetIntValue();
                                            int radius = cr.GetIntValue();
                                            dev.id = item_id;
                                            dev.itemType = item_itemType;
                                            dev.description = item_description;
                                            dev.volume = item_volume;
                                            dev.region = item_region_id;
                                            dev.packing = item_packing;
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
                                            int engine_power = cr.GetIntValue();
                                            int engine_economic = cr.GetIntValue();
                                            dev.id = item_id;
                                            dev.itemType = item_itemType;
                                            dev.description = item_description;
                                            dev.volume = item_volume;
                                            dev.region = item_region_id;
                                            dev.packing = item_packing;
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
                                            int fuelbag_compress = cr.GetIntValue();
                                            dev.id = item_id;
                                            dev.itemType = item_itemType;
                                            dev.description = item_description;
                                            dev.volume = item_volume;
                                            dev.region = item_region_id;
                                            dev.packing = item_packing;
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
                                            int hyper_radius = cr.GetIntValue();
                                            int hyper_time_start = cr.GetIntValue();
                                            int hyper_time_reload = cr.GetIntValue();
                                            dev.id = item_id;
                                            dev.itemType = item_itemType;
                                            dev.description = item_description;
                                            dev.volume = item_volume;
                                            dev.region = item_region_id;
                                            dev.packing = item_packing;
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
                                            int radar_radius = cr.GetIntValue();
                                            int radar_defense = cr.GetIntValue();
                                            int big_radius = cr.GetIntValue();
                                            dev.id = item_id;
                                            dev.itemType = item_itemType;
                                            dev.description = item_description;
                                            dev.volume = item_volume;
                                            dev.region = item_region_id;
                                            dev.packing = item_packing;
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
                                            int shield_power = cr.GetIntValue();
                                            dev.id = item_id;
                                            dev.itemType = item_itemType;
                                            dev.description = item_description;
                                            dev.volume = item_volume;
                                            dev.region = item_region_id;
                                            dev.packing = item_packing;
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
                                            int weapon_weaponType = cr.GetIntValue();
                                            int weapon_radius = cr.GetIntValue();
                                            int weapon_power = cr.GetIntValue();
                                            int weapon_time_start = cr.GetIntValue();
                                            int weapon_time_reload = cr.GetIntValue();
                                            dev.id = item_id;
                                            dev.itemType = item_itemType;
                                            dev.description = item_description;
                                            dev.volume = item_volume;
                                            dev.region = item_region_id;
                                            dev.packing = item_packing;
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
    }
}
