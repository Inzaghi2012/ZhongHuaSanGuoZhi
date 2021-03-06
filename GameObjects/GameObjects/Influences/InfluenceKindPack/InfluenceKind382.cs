﻿namespace GameObjects.Influences.InfluenceKindPack
{
    using GameObjects;
    using GameObjects.Influences;
    using System;

    internal class InfluenceKind382 : InfluenceKind
    {
        private int chance;

        public override void ApplyInfluenceKind(Troop troop)
        {
            troop.BaseChanceOfOnFire = this.chance;
        }

        public override void InitializeParameter(string parameter)
        {
            try
            {
                this.chance = int.Parse(parameter);
            }
            catch
            {
            }
        }

        public override void PurifyInfluenceKind(Troop troop)
        {
            if (troop != null)
            {
                troop.ChanceOfOnFire = troop.BaseChanceOfOnFire;
            }
        }
    }
}

