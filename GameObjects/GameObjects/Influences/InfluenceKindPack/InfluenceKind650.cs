﻿namespace GameObjects.Influences.InfluenceKindPack
{
    using GameObjects;
    using GameObjects.Influences;
    using System;

    internal class InfluenceKind650 : InfluenceKind
    {
        private int increment;


        public override void ApplyInfluenceKind(Troop troop)
        {
            troop.ChanceIncrementOfCriticalStrikeInViewArea += this.increment;
        }

        public override void PurifyInfluenceKind(Troop troop)
        {
            troop.ChanceIncrementOfCriticalStrikeInViewArea -= this.increment;
        }

        public override void InitializeParameter(string parameter)
        {
            try
            {
                this.increment = int.Parse(parameter);
            }
            catch
            {
            }
        }

        public override void ApplyInfluenceKind(Architecture architecture)
        {
            foreach (Microsoft.Xna.Framework.Point point in architecture.ViewArea.Area)
            {
                Troop troopByPosition = base.Scenario.GetTroopByPosition(point);
                if ((troopByPosition != null) && architecture.IsFriendly(troopByPosition.BelongedFaction))
                {
                    troopByPosition.ChanceIncrementOfCriticalStrike += this.increment;
                }
            }
        }

        public override void PurifyInfluenceKind(Architecture architecture)
        {
            foreach (Microsoft.Xna.Framework.Point point in architecture.ViewArea.Area)
            {
                Troop troopByPosition = base.Scenario.GetTroopByPosition(point);
                if ((troopByPosition != null) && architecture.IsFriendly(troopByPosition.BelongedFaction))
                {
                    troopByPosition.ChanceIncrementOfCriticalStrike -= this.increment;
                }
            }
        }
    }
}

