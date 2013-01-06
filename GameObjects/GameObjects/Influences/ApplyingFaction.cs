﻿namespace GameObjects.Influences
{
    public class ApplyingFaction
    {
        public Faction faction;
        public Applier applier;
        public int applierID;

        public ApplyingFaction(Faction a, Applier p, int i)
        {
            this.faction = a;
            this.applier = p;
            this.applierID = i;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is ApplyingFaction)) return false;
            ApplyingFaction a = (ApplyingFaction)obj;
            return a.applierID == this.applierID && a.applier.Equals(this.applier) && a.faction.Equals(this.faction);
        }

        public override int GetHashCode()
        {
            return 158 * this.faction.GetHashCode() + 37 * this.applier.GetHashCode() + this.applierID;
        }
    }
}