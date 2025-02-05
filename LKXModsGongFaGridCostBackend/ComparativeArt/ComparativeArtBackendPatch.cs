﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameData.Domains;
using GameData.Domains.Character.Ai;
using GameData.Domains.Taiwu;
using GameData.Domains.Taiwu.LifeSkillCombat;
using HarmonyLib;
using TaiwuModdingLib.Core.Utils;

namespace ConvenienceBackend.ComparativeArt
{
    internal class ComparativeArtBackendPatch : BaseBackendPatch
    {
        private static bool _artAlwaysWin = false;

        public override void OnModSettingUpdate(string modIdStr)
        {
            DomainManager.Mod.GetSetting(modIdStr, "Toggle_ArtAlwaysWin", ref _artAlwaysWin);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Match), "CheckResult")]
        public static void Match_CheckResult_Postfix(Match __instance)
        {
            if (!_artAlwaysWin) return;
            ReflectionExtensions.ModifyField(__instance, "_suiciderPlayerId", (sbyte?)1);
        }
    }
}
