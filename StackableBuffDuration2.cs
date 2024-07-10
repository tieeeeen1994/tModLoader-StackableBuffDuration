using System;
using System.Collections.Generic;
using System.ComponentModel;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace StackableBuffDuration2
{
    public class StackableBuffDuration2 : Mod
	{
        public override void Load()
        {
            On_Player.AddBuff_TryUpdatingExistingBuffTime += TryUpdatingExistingBuffTime;
        }

        public override void Unload()
        {
            On_Player.AddBuff_TryUpdatingExistingBuffTime -= TryUpdatingExistingBuffTime;
        }

        private bool TryUpdatingExistingBuffTime(On_Player.orig_AddBuff_TryUpdatingExistingBuffTime orig,
                                                 Player self, int type, int time)
        {
            if (time <= Config.I.buffTimeThreshold || Config.I.blacklist.Exists(buffDefinition => buffDefinition.Type == type))
            {
                return orig(self, type, time);
            }
            else
            {
                bool flag = false;
                for (int i = 0; i < Player.MaxBuffs; i++)
                {
                    if (self.buffType[i] == type)
                    {
                        if (type == 94)
                        {
                            self.buffTime[i] = Math.Min(self.buffTime[i] + time, Player.manaSickTimeMax);
                        }
                        else
                        {
                            self.buffTime[i] += time;   
                        }
                        flag = true;
                        break;
                    }
                }
                return flag;
            }
        }
    }

    public class Config : ModConfig
    {
        public static Config I => ModContent.GetInstance<Config>();

        public override ConfigScope Mode => ConfigScope.ServerSide;

        public List<BuffDefinition> blacklist = [];
        [DefaultValue(15)]
        public int buffTimeThreshold;
    }
}
