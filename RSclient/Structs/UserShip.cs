using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSclient
{
    public class UserShip
    {
        public Equip body = new Equip();
        public Equip engine = new Equip();
        public Equip fuelbag = new Equip();
        public Equip hyper = new Equip();
        public Equip shield = new Equip();
        public Equip radar = new Equip();
        public List<Equip> weapons = new List<Equip>();
        public List<Equip> droids = new List<Equip>();

        public double energy;
        public int maxFuel;
        public double maxSpeed;

        #region Damage
        public void bodyDamage(int dmg)
        {
            this.body.wear -= dmg;
            if (this.body.wear < 0)
            {
                this.body.wear = 0;
            }
        }
        public void engineDamage(int dmg)
        {
            this.engine.wear -= dmg;
            if (this.engine.wear < 0)
            {
                this.engine.wear = 0;
            }
        }
        public void fuelbagDamage(int dmg)
        {
            this.fuelbag.wear -= dmg;
            if (this.fuelbag.wear < 0)
            {
                this.fuelbag.wear = 0;
            }
        }
        public void hyperDamage(int dmg)
        {
            this.hyper.wear -= dmg;
            if (this.hyper.wear < 0)
            {
                this.hyper.wear = 0;
            }
        }
        public void shieldDamage(int dmg)
        {
            this.shield.wear -= dmg;
            if (this.shield.wear < 0)
            {
                this.shield.wear = 0;
            }
        }
        public void radarDamage(int dmg)
        {
            this.radar.wear -= dmg;
            if (this.radar.wear < 0)
            {
                this.radar.wear = 0;
            }
        }
        public void weaponDamage(int idW, int dmg)
        {
            this.weapons[idW].wear -= dmg;
            if (this.weapons[idW].wear < 0)
            {
                this.weapons[idW].wear = 0;
            }
        }
        public void droidDamage(int idD, int dmg)
        {
            this.droids[idD].wear -= dmg;
            if (this.droids[idD].wear < 0)
            {
                this.droids[idD].wear = 0;
            }
        }
        #endregion

        #region Repair
        public void bodyRepair(int dmg)
        {
            Body body = this.body.item as Body;
            this.body.wear += dmg;
            if (this.body.wear > body.durability)
            {
                this.body.wear = body.durability;
            }
        }
        public void fuelbagRepair(int dmg)
        {
            Fuelbag fuelbag = this.fuelbag.item as Fuelbag;
            this.fuelbag.wear += dmg;
            if (this.fuelbag.wear > fuelbag.durability)
            {
                this.fuelbag.wear = fuelbag.durability;
            }
        }
        public void hyperRepair(int dmg)
        {
            Hyper hyper = this.hyper.item as Hyper;
            this.hyper.wear += dmg;
            if (this.hyper.wear > hyper.durability)
            {
                this.hyper.wear = hyper.durability;
            }
        }
        public void shieldRepair(int dmg)
        {
            Shield shield = this.shield.item as Shield;
            this.shield.wear += dmg;
            if (this.shield.wear > shield.durability)
            {
                this.shield.wear = shield.durability;
            }
        }
        public void radarRepair(int dmg)
        {
            Radar radar = this.radar.item as Radar;
            this.radar.wear += dmg;
            if (this.radar.wear > radar.durability)
            {
                this.radar.wear = radar.durability;
            }
        }
        public void weaponRepair(int idW, int dmg)
        {
            Weapon weapon = this.weapons[idW].item as Weapon;
            this.weapons[idW].wear += dmg;
            if (this.weapons[idW].wear > weapon.durability)
            {
                this.weapons[idW].wear = weapon.durability;
            }
        }
        public void droidRepair(int idD, int dmg)
        {
            Droid droid = this.droids[idD].item as Droid;
            this.droids[idD].wear += dmg;
            if (this.droids[idD].wear > droid.durability)
            {
                this.droids[idD].wear = droid.durability;
            }
        }
        #endregion

        #region Filling
        public void unFill(double f)
        {
            this.energy -= f;
            if (this.energy <= 0)
            {
                this.energy = 0;
                this.maxSpeed = 0.5;
            }
            if (this.energy > maxFuel)
            {
                this.energy = this.maxFuel;
            }
        }
        public void reFill(int f)
        {
            Engine eng = this.engine.item as Engine;
            this.energy += f;
            if (this.energy > maxFuel)
            {
                this.energy = this.maxFuel;
            }
            this.maxSpeed = eng.power;
        }
        #endregion
    }
}
