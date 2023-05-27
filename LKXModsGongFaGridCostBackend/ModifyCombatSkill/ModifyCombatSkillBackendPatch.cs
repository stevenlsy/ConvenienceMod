﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Config;
using GameData.Common;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;
using GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Blade;
using GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Sword;
using GameData.Utilities;
using HarmonyLib;
using TaiwuModdingLib.Core.Utils;

namespace ConvenienceBackend.ModifyCombatSkill
{
    internal class ModifyCombatSkillBackendPatch : BaseBackendPatch
    {
        private static bool enableBladeAndSwordDoubleJue = false;

        public override void OnModSettingUpdate(string modIdStr)
        {
            DomainManager.Mod.GetSetting(modIdStr, "enable_BladeAndSwordDoubleJue", ref enableBladeAndSwordDoubleJue);

            UpdateSkillEffectDesc();
        }

        private static void UpdateSkillEffectDesc()
        {
            var flag = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.DeclaredOnly;
            if (enableBladeAndSwordDoubleJue)
            {
                AdaptableLog.Info("真刀剑双绝");

                ReflectionExtensions.ModifyField(SpecialEffect.Instance[723], "Desc", new string[]{ "此功法发挥十成威力时，运用者后退2距离，如果敌人仍在运用者的攻击范围内，运用者从施展进度50%开始立即无消耗施展另一个品阶不超过此功法的「剑法」攻击敌人" }, flag);
                ReflectionExtensions.ModifyField(SpecialEffect.Instance[1449], "Desc", new string[] { "此功法发挥十成威力时，运用者前进2距离，如果敌人仍在运用者的攻击范围内，运用者从施展进度50%开始立即无消耗施展另一个品阶不超过此功法的「剑法」攻击敌人" }, flag);

                ReflectionExtensions.ModifyField(SpecialEffect.Instance[715], "Desc", new string[] { "此功法发挥十成威力时，运用者前进2距离，如果敌人仍在运用者的攻击范围内，运用者从施展进度50%开始立即无消耗施展另一个品阶不超过此功法的「刀法」攻击敌人" }, flag);
                ReflectionExtensions.ModifyField(SpecialEffect.Instance[1441], "Desc", new string[] { "此功法发挥十成威力时，运用者后退2距离，如果敌人仍在运用者的攻击范围内，运用者从施展进度50%开始立即无消耗施展另一个品阶不超过此功法的「刀法」攻击敌人" }, flag);
            }
            else 
            {
                ReflectionExtensions.ModifyField(SpecialEffect.Instance[723], "Desc", new string[] { "此功法未发挥十成威力时，运用者后退2距离，如果敌人仍在运用者的攻击范围内，运用者从施展进度50%开始立即无消耗施展另一个品阶不超过此功法的「剑法」攻击敌人" }, flag);
                ReflectionExtensions.ModifyField(SpecialEffect.Instance[1449], "Desc", new string[] { "此功法未发挥十成威力时，运用者前进2距离，如果敌人仍在运用者的攻击范围内，运用者从施展进度50%开始立即无消耗施展另一个品阶不超过此功法的「剑法」攻击敌人" }, flag);

                ReflectionExtensions.ModifyField(SpecialEffect.Instance[715], "Desc", new string[] { "此功法未发挥十成威力时，运用者前进2距离，如果敌人仍在运用者的攻击范围内，运用者从施展进度50%开始立即无消耗施展另一个品阶不超过此功法的「刀法」攻击敌人" }, flag);
                ReflectionExtensions.ModifyField(SpecialEffect.Instance[1441], "Desc", new string[] { "此功法未发挥十成威力时，运用者后退2距离，如果敌人仍在运用者的攻击范围内，运用者从施展进度50%开始立即无消耗施展另一个品阶不超过此功法的「刀法」攻击敌人" }, flag);
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AutoMoveAndCast), "OnSkillAttackEnd")]
        public static bool AutoMoveAndCast_OnSkillAttackEnd_Prefix(AutoMoveAndCast __instance, DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool hit)
        {
            if (!enableBladeAndSwordDoubleJue) return true;

            var instanceWrapper = Traverse.Create(__instance);
            var CombatChar = DomainManager.Combat.GetElement_CombatCharacterDict(__instance.CharacterId);
            var SkillTemplateId = __instance.SkillKey.SkillTemplateId;

            if (index == 3 && attacker == CombatChar && skillId == SkillTemplateId && CombatChar.GetAttackSkillPower() >= 100)
            {
                bool IsDirect = instanceWrapper.Field<bool>("IsDirect").Value;
                bool flag = (instanceWrapper.Field<bool>("DirectMoveForward").Value ? IsDirect : (!IsDirect));
                DomainManager.Combat.ChangeDistance(context, CombatChar, flag ? (-20) : 20);
                DomainManager.Combat.SetDisplayPosition(context, CombatChar.IsAlly, (!flag) ? int.MinValue : DomainManager.Combat.GetDisplayPosition(CombatChar.IsAlly, DomainManager.Combat.GetCurrentDistance()));
                if (flag)
                {
                    CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!CombatChar.IsAlly);
                    CombatChar.SetCurrentPosition(CombatChar.GetDisplayPosition(), context);
                    combatCharacter.SetCurrentPosition(combatCharacter.GetDisplayPosition(), context);
                }
            }
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(AutoMoveAndCast), "OnCastSkillEnd")]
        public static bool AutoMoveAndCast_OnCastSkillEnd_Prefix(
            AutoMoveAndCast __instance, 
            DataContext context, 
            int charId, 
            bool isAlly, 
            short skillId, 
            sbyte power, 
            bool interrupted
        )
        {
            if (!enableBladeAndSwordDoubleJue) return true;
            bool flag = charId != __instance.CharacterId || skillId != __instance.SkillKey.SkillTemplateId;
            if (!flag)
            {
                var instanceWrapper = Traverse.Create(__instance);
                var CombatChar = DomainManager.Combat.GetElement_CombatCharacterDict(__instance.CharacterId);
                bool flag2 = !interrupted && power >= 100 && DomainManager.Combat.InAttackRange(CombatChar);
                if (flag2)
                {
                    var CastSkillType = instanceWrapper.Field<sbyte>("CastSkillType").Value;
                    var _autoCastSkillId = DomainManager.Combat.GetRandomAttackSkill(CombatChar, CastSkillType, Config.CombatSkill.Instance[__instance.SkillKey.SkillTemplateId].Grade, context.Random, true);
                    if (_autoCastSkillId >= 0)
                    {
                        instanceWrapper.Field<bool>("IsSrcSkillPerformed").Value = true;
                        DomainManager.Combat.CastSkillFree(context, CombatChar, _autoCastSkillId);
                        DomainManager.Combat.ShowSpecialEffectTips(context, __instance.CharacterId, __instance.EffectId, 0);
                    }
                    instanceWrapper.Field<short>("_autoCastSkillId").Value = _autoCastSkillId;
                }
                if (instanceWrapper.Field<short>("_autoCastSkillId").Value < 0)
                {
                    instanceWrapper.Method("RemoveSelf", context);
                }
            }

            return false;
        }
    }
}