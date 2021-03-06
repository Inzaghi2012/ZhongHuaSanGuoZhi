﻿namespace GameObjects.Conditions.ConditionKindPack
{
    using GameObjects;
    using GameObjects.Conditions;
    using System;

    internal class ConditionKind2620 : ConditionKind
    {
        private int val;

        public override bool CheckConditionKind(Architecture a)
        {
            return !a.youzainan || a.zainan.zainanzhonglei.ID != val;
        }

        public override void InitializeParameter(string parameter)
        {
            try
            {
                this.val = int.Parse(parameter);
            }
            catch
            {
            }
        }


    }
}

