﻿namespace GameObjects.Influences.InfluenceKindPack
{
    using GameObjects;
    using GameObjects.Influences;
    using System;

    internal class InfluenceKind582 : InfluenceKind
    {

        public override void ApplyInfluenceKind(Troop troop)
        {
            troop.InevitableChaosOnWaylay = true;
        }

        public override void PurifyInfluenceKind(Troop troop)
        {
            troop.InevitableChaosOnWaylay = false;
        }
    }
}

