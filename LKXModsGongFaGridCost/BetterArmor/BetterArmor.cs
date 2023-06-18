﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Config.ConfigCells.Character;
using Config;
using GameData.Domains.Character;
using GameData.Utilities;
using HarmonyLib;

namespace ConvenienceFrontend.BetterArmor
{
    internal class BetterArmor
    {
        // Token: 0x06000006 RID: 6 RVA: 0x000023EC File Offset: 0x000005EC
        private void setDefensePersentage(int type, int ho, int hi, int so, int si)
        {
            this.outPercentage.Add(type, ho);
            this.outPercentage.Add(type + 4, so);
            this.inPercentage.Add(type, hi);
            this.inPercentage.Add(type + 4, si);
        }

        // Token: 0x06000007 RID: 7 RVA: 0x00002438 File Offset: 0x00000638
        private int calcReduction(int val, short subtype)
        {
            int num = this.typeDic[subtype];
            bool flag = num == 4 || num == 8;
            if (flag)
            {
                bool flag2 = subtype != 124 && subtype != 131 && subtype != 138 && subtype != 145 && subtype != 153 && subtype != 161 && subtype != 168 && subtype != 175;
                if (flag2)
                {
                    return 0;
                }
            }
            bool flag3 = val >= 400;
            int result;
            if (flag3)
            {
                result = 3;
            }
            else
            {
                bool flag4 = val >= 200;
                if (flag4)
                {
                    result = 2;
                }
                else
                {
                    bool flag5 = val >= 50;
                    if (flag5)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
            }
            return result;
        }

        // Token: 0x06000008 RID: 8 RVA: 0x000024F0 File Offset: 0x000006F0
        private void setDefense(int type, int hbd, int hdd, int sbd, int sdd)
        {
            this.baseDefense.Add(type, hbd);
            this.deltaDefense.Add(type, hdd);
            this.baseDefense.Add(type + 4, sbd);
            this.deltaDefense.Add(type + 4, sdd);
        }

        // Token: 0x06000009 RID: 9 RVA: 0x0000253C File Offset: 0x0000073C
        private void setAttack(int type, int hba, int hda, int sba, int sda)
        {
            this.baseAttack.Add(type, hba);
            this.deltaAttack.Add(type, hda);
            this.baseAttack.Add(type + 4, sba);
            this.deltaAttack.Add(type + 4, sda);
        }

        // Token: 0x0600000A RID: 10 RVA: 0x00002588 File Offset: 0x00000788
        private void setWeaponDefense(int type, int hbd, int hdd, int sbd, int sdd)
        {
            this.baseWeaponDefense.Add(type, hbd);
            this.deltaWeaponDefense.Add(type, hdd);
            this.baseWeaponDefense.Add(type + 4, sbd);
            this.deltaWeaponDefense.Add(type + 4, sdd);
        }

        // Token: 0x0600000B RID: 11 RVA: 0x000025D4 File Offset: 0x000007D4
        private void setWeaponAttack(int type, int hba, int hda, int sba, int sda)
        {
            this.baseWeaponAttack.Add(type, hba);
            this.deltaWeaponAttack.Add(type, hda);
            this.baseWeaponAttack.Add(type + 4, sba);
            this.deltaWeaponAttack.Add(type + 4, sda);
        }

        // Token: 0x0600000C RID: 12 RVA: 0x00002620 File Offset: 0x00000820
        private void setWeight(int type, int hbw, int hdw, int sbw, int sdw)
        {
            this.baseWeight.Add(type, hbw);
            this.deltaWeight.Add(type, hdw);
            this.baseWeight.Add(type + 4, sbw);
            this.deltaWeight.Add(type + 4, sdw);
        }

        // Token: 0x0600000D RID: 13 RVA: 0x0000266C File Offset: 0x0000086C
        private void setChestWeight()
        {
            this.chestWeight.Add(1, 110);
            this.chestWeight.Add(2, 240);
            this.chestWeight.Add(3, 170);
            this.chestWeight.Add(4, 50);
            this.chestWeight.Add(5, 100);
            this.chestWeight.Add(6, 220);
            this.chestWeight.Add(7, 150);
            this.chestWeight.Add(8, 40);
        }

        // Token: 0x0600000E RID: 14 RVA: 0x00002700 File Offset: 0x00000900
        private void setTrick()
        {
            sbyte b = 0;
            while ((int)b < TrickType.Instance.Count)
            {
                TrickTypeItem item = TrickType.Instance.GetItem(b);
                this.hitType.Add(item.TemplateId, item.AvoidType);
                this.trickName.Add(item.TemplateId, item.Name);
                b += 1;
            }
        }

        // Token: 0x0600000F RID: 15 RVA: 0x00002768 File Offset: 0x00000968
        private void setHits()
        {
            this.bonusLidao.Add(2, 15);
            this.bonusLidao.Add(6, 5);
            this.bonusLidao.Add(1, 5);
            this.bonusLidao.Add(5, -5);
            this.bonusLidao.Add(3, 0);
            this.bonusLidao.Add(7, -5);
            this.bonusLidao.Add(4, 0);
            this.bonusLidao.Add(8, -5);
            this.bonusJingmiao.Add(2, -5);
            this.bonusJingmiao.Add(6, 0);
            this.bonusJingmiao.Add(1, -5);
            this.bonusJingmiao.Add(5, 5);
            this.bonusJingmiao.Add(3, 0);
            this.bonusJingmiao.Add(7, 0);
            this.bonusJingmiao.Add(4, 5);
            this.bonusJingmiao.Add(8, 10);
            this.bonusXunji.Add(2, -5);
            this.bonusXunji.Add(6, 0);
            this.bonusXunji.Add(1, 5);
            this.bonusXunji.Add(5, 5);
            this.bonusXunji.Add(3, 5);
            this.bonusXunji.Add(7, 10);
            this.bonusXunji.Add(4, 0);
            this.bonusXunji.Add(8, 0);
            this.bonusDongxin.Add(2, 0);
            this.bonusDongxin.Add(6, 0);
            this.bonusDongxin.Add(1, 0);
            this.bonusDongxin.Add(5, 0);
            this.bonusDongxin.Add(3, 0);
            this.bonusDongxin.Add(7, 0);
            this.bonusDongxin.Add(4, 0);
            this.bonusDongxin.Add(8, 0);
        }

        // Token: 0x06000010 RID: 16 RVA: 0x00002940 File Offset: 0x00000B40
        private string descGenerator(string name, int old, int cur, bool ret = true)
        {
            bool flag = !this.showModification;
            string result;
            if (flag)
            {
                result = "";
            }
            else
            {
                bool flag2 = old == cur;
                if (flag2)
                {
                    result = "";
                }
                else
                {
                    string text = string.Concat(new string[]
                    {
                        name,
                        ": ",
                        old.ToString(),
                        " -> ",
                        cur.ToString()
                    });
                    if (ret)
                    {
                        text += "\n";
                    }
                    result = text;
                }
            }
            return result;
        }

        // Token: 0x06000011 RID: 17 RVA: 0x000029C0 File Offset: 0x00000BC0
        private bool changeArmor(ArmorItem item)
        {
            short makeItemSubType = item.MakeItemSubType;
            bool flag = makeItemSubType == -1;
            bool result;
            if (flag)
            {
                result = true;
            }
            else
            {
                int num = this.typeDic[makeItemSubType];
                int grade = (int)item.Grade;
                string text = "（更平衡的护甲，Q群478125847）\n";
                int num2 = 50 + 50 * grade;
                bool flag2 = num == 2 || num == 6;
                if (flag2)
                {
                    num2 += 10 + 10 * grade;
                }
                int num3 = this.outPercentage[num];
                int num4 = this.inPercentage[num];
                if (makeItemSubType == 124 || makeItemSubType == 138 || makeItemSubType == 153 || makeItemSubType == 168)
                {
                    num3 += 50;
                }
                if (makeItemSubType == 131 || makeItemSubType == 145 || makeItemSubType == 161 || makeItemSubType == 175)
                {
                    num4 += 50;
                }
                if (makeItemSubType == 147 || makeItemSubType == 155)
                {
                    num3 -= 25;
                    num4 += 25;
                }
                int yutiValue = num2 * num3 / 100;
                int yuqiValue = num2 * num4 / 100;
                int waishangjianmianValue = this.calcReduction(yutiValue, makeItemSubType);
                int neishangjianmianValue = this.calcReduction(yuqiValue, makeItemSubType);
                if (item.ItemSubType == 101)
                {
                    yutiValue += yutiValue / 2;
                    yuqiValue += yuqiValue / 2;
                }
                OuterAndInnerShorts outerAndInnerShorts = new OuterAndInnerShorts((short)yutiValue, (short)yuqiValue);
                text += this.descGenerator("御体", (int)item.BasePenetrationResistFactors.Outer, yutiValue, true);
                text += this.descGenerator("御气", (int)item.BasePenetrationResistFactors.Inner, yuqiValue, true);
                Traverse.Create(item).Field("BasePenetrationResistFactors").SetValue(outerAndInnerShorts);
                OuterAndInnerShorts outerAndInnerShorts2 = new OuterAndInnerShorts((short)waishangjianmianValue, (short)neishangjianmianValue);
                text += this.descGenerator("外伤减免", (int)item.InjuryEffectLevelReduction.Outer, waishangjianmianValue, true);
                text += this.descGenerator("内伤减免", (int)item.InjuryEffectLevelReduction.Inner, neishangjianmianValue, true);
                Traverse.Create(item).Field("InjuryEffectLevelReduction").SetValue(outerAndInnerShorts2);

                int jianrenValue = this.baseDefense[num] + this.deltaDefense[num] * grade;
                if (item.ItemSubType == 101)
                {
                    jianrenValue += jianrenValue / 2;
                }
                text += this.descGenerator("坚韧", (int)item.BaseEquipmentDefense, jianrenValue, true);
                Traverse.Create(item).Field("BaseEquipmentDefense").SetValue((short)jianrenValue);
                int porenValue = this.baseAttack[num] + this.deltaAttack[num] * grade;
                if (item.ItemSubType == 101)
                {
                    porenValue += porenValue / 2;
                }
                text += this.descGenerator("破刃", (int)item.BaseEquipmentAttack, porenValue, true);
                Traverse.Create(item).Field("BaseEquipmentAttack").SetValue((short)porenValue);
                int weightValue = this.baseWeight[num] + this.deltaWeight[num] * grade;
                if (item.ItemSubType == 101)
                {
                    weightValue += this.chestWeight[num];
                }
                text += this.descGenerator("重量", item.BaseWeight, weightValue, true);
                Traverse.Create(item).Field("BaseWeight").SetValue(weightValue);

                int num12 = 20 + grade * 5;
                if (num % 4 == 1)
                {
                    num12 += 5;
                }
                else
                {
                    if (num % 4 == 3)
                    {
                        num12 += 10;
                    }
                    else
                    {
                        if (num % 4 == 2)
                        {
                            num12 += 15;
                        }
                    }
                }
                if (item.ItemSubType == 101)
                {
                    num12 += 10;
                }
                text += this.descGenerator("使用需求", (int)item.RequiredCharacterProperties[0].Value, num12, true);
                List<PropertyAndValue> list = new List<PropertyAndValue>();
                for (int i = 0; i < item.RequiredCharacterProperties.Count; i++)
                {
                    list.Add(new PropertyAndValue(item.RequiredCharacterProperties[i].PropertyId, (short)num12));
                }
                Traverse.Create(item).Field("RequiredCharacterProperties").SetValue(list);
                if (!this.showModification)
                {
                    text += item.Desc;
                }
                Traverse.Create(item).Field("Desc").SetValue(text.TrimEnd(new char[]
                {
                    '\n'
                }));
                result = true;
            }
            return result;
        }

        // Token: 0x06000012 RID: 18 RVA: 0x00002E6C File Offset: 0x0000106C
        private bool changeWeapon(WeaponItem item)
        {
            short makeItemSubType = item.MakeItemSubType;
            bool flag = !this.typeDic.ContainsKey(makeItemSubType);
            bool result;
            if (flag)
            {
                result = true;
            }
            else
            {
                int num = this.typeDic[makeItemSubType];
                int grade = (int)item.Grade;
                string text = "（更平衡的武器，Q群478125847）\n";
                int num2 = 0;
                bool flag2 = num2 < 6;
                if (flag2)
                {
                    int num3 = 40 + 20 * grade;
                    bool flag3 = num == 2 || num == 6;
                    if (flag3)
                    {
                        num3 += 10 + 5 * grade;
                    }
                    num3 *= (int)(1 + item.AttackPreparePointCost);
                    num3 = num3 * this.bonusAtk[makeItemSubType] / 100;
                    text += this.descGenerator("穿透", (int)item.BasePenetrationFactor, num3, true);
                    Traverse.Create(item).Field("BasePenetrationFactor").SetValue((short)num3);
                }
                int num4 = this.baseWeaponDefense[num] + this.deltaWeaponDefense[num] * grade;
                num4 += num4 * (int)item.AttackPreparePointCost / 2;
                num4 = num4 * this.bonusDefense[makeItemSubType] / 100;
                text += this.descGenerator("坚韧", (int)item.BaseEquipmentDefense, num4, true);
                Traverse.Create(item).Field("BaseEquipmentDefense").SetValue((short)num4);
                int num5 = this.baseWeaponAttack[num] + this.deltaWeaponAttack[num] * grade;
                num5 += num5 * (int)item.AttackPreparePointCost / 2;
                num5 = num5 * this.bonusAttack[makeItemSubType] / 100;
                text += this.descGenerator("破甲", (int)item.BaseEquipmentAttack, num5, true);
                Traverse.Create(item).Field("BaseEquipmentAttack").SetValue((short)num5);
                int num6 = this.baseWeight[num] + this.deltaWeight[num] * grade;
                num6 += num6 * (int)item.AttackPreparePointCost / 2;
                num6 = num6 * this.bonusWeight[makeItemSubType] / 100;
                text += this.descGenerator("重量", item.BaseWeight, num6, true);
                Traverse.Create(item).Field("BaseWeight").SetValue(num6);
                text += this.descGenerator("内伤调节", (int)item.InnerRatioAdjustRange, (int)this.innerAdjust[num], true);
                Traverse.Create(item).Field("InnerRatioAdjustRange").SetValue(this.innerAdjust[num]);
                bool flag4 = !this.showModification;
                if (flag4)
                {
                    text += item.Desc;
                }
                Traverse.Create(item).Field("Desc").SetValue(text.TrimEnd(new char[]
                {
                    '\n'
                }));
                result = true;
            }
            return result;
        }

        // Token: 0x06000013 RID: 19 RVA: 0x00003150 File Offset: 0x00001350
        private unsafe bool logWeapon(WeaponItem item)
        {
            short makeItemSubType = item.MakeItemSubType;
            bool flag = item.Grade != 8;
            bool result;
            if (flag)
            {
                result = true;
            }
            else
            {
                bool flag2 = !this.typeDic.ContainsKey(makeItemSubType);
                if (flag2)
                {
                    result = true;
                }
                else
                {
                    string text = "";
                    text = text + item.Name + ",";
                    text = text + this.materialName[this.typeDic[makeItemSubType]] + ",";
                    text = text + this.typeName[makeItemSubType] + ",";
                    text = text + item.MaxDurability.ToString() + ",";
                    text = text + item.BaseWeight.ToString() + ",";
                    text = text + item.BaseEquipmentAttack.ToString() + ",";
                    text = text + item.BaseEquipmentDefense.ToString() + ",";
                    text = text + item.BasePenetrationFactor.ToString() + ",";
                    text = text + ((int)(item.AttackPreparePointCost + 1)).ToString() + ",";
                    text = string.Concat(new string[]
                    {
                        text,
                        item.MinDistance.ToString(),
                        "-",
                        item.MaxDistance.ToString(),
                        ","
                    });
                    for (int i = 0; i < 6; i++)
                    {
                        text = text + this.trickName[item.Tricks[i]] + this.hitType[item.Tricks[i]].ToString();
                    }
                    text = text + "," + item.BaseHitFactors.Items[0].ToString() + ",";
                    text = text + (item.BaseHitFactors.Items[2]).ToString() + ",";
                    text = text + (item.BaseHitFactors.Items[2*2]).ToString() + ",";
                    text = text + (item.BaseHitFactors.Items[3 * 2]).ToString() + ",";
                    text = text + item.DefaultInnerRatio.ToString() + ",";
                    text = text + item.InnerRatioAdjustRange.ToString() + ",";
                    text = text + item.PursueAttackFactor.ToString() + ",";
                    text = text + item.ChangeTrickPercent.ToString() + ",";
                    text = text + item.BlockInjury.ToString() + ",";
                    text = text + item.BlockMind.ToString() + ",";
                    text = text + item.BlockFatalDamage.ToString() + ",";
                    text += item.StanceIncrement.ToString();
                    AdaptableLog.Info(text);
                    result = true;
                }
            }
            return result;
        }

        // Token: 0x06000014 RID: 20 RVA: 0x000034A0 File Offset: 0x000016A0
        private unsafe bool logArmor(ArmorItem item)
        {
            short makeItemSubType = item.MakeItemSubType;
            bool flag = item.Grade != 8;
            bool result;
            if (flag)
            {
                result = true;
            }
            else
            {
                bool flag2 = !this.typeDic.ContainsKey(makeItemSubType);
                if (flag2)
                {
                    result = true;
                }
                else
                {
                    string text = "";
                    text = text + item.Name + ",";
                    text = text + this.materialName[this.typeDic[makeItemSubType]] + ",";
                    text = text + this.typeName[makeItemSubType] + ",";
                    text = text + item.MaxDurability.ToString() + ",";
                    text = text + item.BaseWeight.ToString() + ",";
                    text = text + item.BaseEquipmentAttack.ToString() + ",";
                    text = text + item.BaseEquipmentDefense.ToString() + ",";
                    string str = text;
                    short num = item.BasePenetrationResistFactors.Outer;
                    text = str + num.ToString() + ",";
                    string str2 = text;
                    num = item.InjuryEffectLevelReduction.Outer;
                    text = str2 + num.ToString() + ",";
                    string str3 = text;
                    num = item.BasePenetrationResistFactors.Inner;
                    text = str3 + num.ToString() + ",";
                    string str4 = text;
                    num = item.InjuryEffectLevelReduction.Inner;
                    text = str4 + num.ToString() + ",";
                    text = text + item.BaseAvoidFactors.Items[0].ToString() + ",";
                    text = text + (item.BaseAvoidFactors.Items[2]).ToString() + ",";
                    text = text + (item.BaseAvoidFactors.Items[2 * 2]).ToString() + ",";
                    text += (item.BaseAvoidFactors.Items[3 * 2]).ToString();
                    AdaptableLog.Info(text);
                    result = true;
                }
            }
            return result;
        }

        // Token: 0x06000015 RID: 21 RVA: 0x000036C0 File Offset: 0x000018C0
        private void setCraftToType(short materialid, int materialType)
        {
            List<short> craftableItemTypes = Material.Instance.GetItem(materialid).CraftableItemTypes;
            for (int i = 0; i < craftableItemTypes.Count; i++)
            {
                MakeItemTypeItem item = MakeItemType.Instance.GetItem(craftableItemTypes[i]);
                List<short> makeItemSubTypes = item.MakeItemSubTypes;
                for (int j = 0; j < makeItemSubTypes.Count; j++)
                {
                    bool flag = makeItemSubTypes[j] >= 176;
                    if (!flag)
                    {
                        this.typeDic.Add(makeItemSubTypes[j], materialType);
                        this.bonusHit0.Add(makeItemSubTypes[j], this.bonusLidao[materialType]);
                        this.bonusHit1.Add(makeItemSubTypes[j], this.bonusJingmiao[materialType]);
                        this.bonusHit2.Add(makeItemSubTypes[j], this.bonusXunji[materialType]);
                        this.bonusHit3.Add(makeItemSubTypes[j], this.bonusDongxin[materialType]);
                        this.bonusAtk.Add(makeItemSubTypes[j], 100);
                        this.bonusAttack.Add(makeItemSubTypes[j], 100);
                        this.bonusDefense.Add(makeItemSubTypes[j], 100);
                        this.bonusWeight.Add(makeItemSubTypes[j], 100);
                        this.typeName.Add(makeItemSubTypes[j], item.TypeName);
                    }
                }
            }
        }

        // Token: 0x06000016 RID: 22 RVA: 0x00003860 File Offset: 0x00001A60
        private void setSubtypeHitBonus(short subtype, short lidao, short jingmiao, short xunji, short dongxin = 0)
        {
            Dictionary<short, short> dictionary = this.bonusHit0;
            dictionary[subtype] += lidao;
            dictionary = this.bonusHit1;
            dictionary[subtype] += jingmiao;
            dictionary = this.bonusHit2;
            dictionary[subtype] += xunji;
            dictionary = this.bonusHit3;
            dictionary[subtype] += dongxin;
        }

        // Token: 0x06000017 RID: 23 RVA: 0x000038DC File Offset: 0x00001ADC
        private void setSubtypeHit()
        {
            this.setSubtypeHitBonus(4, -40, 20, 20, 0);
            this.setSubtypeHitBonus(5, -40, 20, 20, 0);
            this.setSubtypeHitBonus(10, 5, 0, -5, 0);
            this.setSubtypeHitBonus(13, 10, -5, -5, 0);
            this.setSubtypeHitBonus(12, 10, 0, -10, 0);
            this.setSubtypeHitBonus(14, 35, -15, -20, 0);
            this.setSubtypeHitBonus(17, 30, -15, -15, 0);
            this.setSubtypeHitBonus(16, 25, -10, -15, 0);
            this.setSubtypeHitBonus(19, 20, -5, -15, 0);
            this.setSubtypeHitBonus(15, 15, -10, -5, 0);
            this.setSubtypeHitBonus(18, 10, -10, 0, 0);
            this.setSubtypeHitBonus(20, -10, 5, 5, 0);
            this.setSubtypeHitBonus(25, 5, 0, -5, 0);
            this.setSubtypeHitBonus(33, -10, 0, 10, 0);
            this.setSubtypeHitBonus(43, 0, -5, 5, 0);
            this.setSubtypeHitBonus(44, -10, 0, 10, 0);
            this.setSubtypeHitBonus(51, -10, 10, 0, 0);
            this.setSubtypeHitBonus(61, -10, 5, 5, 0);
            this.setSubtypeHitBonus(62, -10, 10, 0, 0);
            this.setSubtypeHitBonus(72, 10, -40, 30, 0);
            this.setSubtypeHitBonus(73, 0, -10, 10, 0);
            this.setSubtypeHitBonus(77, 0, -60, 60, 0);
            this.setSubtypeHitBonus(86, -20, -20, 40, 0);
            this.setSubtypeHitBonus(88, -10, -20, 30, 0);
            this.setSubtypeHitBonus(87, -15, -10, 25, 0);
            this.setSubtypeHitBonus(89, -10, -5, 15, 0);
            this.setSubtypeHitBonus(90, -20, 20, 0, 0);
            this.setSubtypeHitBonus(92, -10, 20, -10, 0);
            this.setSubtypeHitBonus(91, -15, 30, -15, 0);
            this.setSubtypeHitBonus(93, -10, 35, -25, 0);
        }

        // Token: 0x06000018 RID: 24 RVA: 0x00003AB8 File Offset: 0x00001CB8
        private void setSubtypeBonus()
        {
            Dictionary<short, int> dictionary = this.bonusWeight;
            dictionary[61] = dictionary[61] + 50;
        }

        // Token: 0x06000019 RID: 25 RVA: 0x00003AE4 File Offset: 0x00001CE4
        private bool changeRefine(RefiningEffectItem item)
        {
            bool flag = item.WeaponType == ERefiningEffectWeaponType.HitRateMind || item.WeaponType == ERefiningEffectWeaponType.HitRateSpeed || item.WeaponType == ERefiningEffectWeaponType.HitRateStrength || item.WeaponType == ERefiningEffectWeaponType.HitRateTechnique;
            if (flag)
            {
                sbyte[] value = new sbyte[]
                {
                    10,
                    15,
                    20,
                    25,
                    30,
                    35,
                    40
                };
                Traverse.Create(item).Field("WeaponBonusValues").SetValue(value);
            }
            bool flag2 = item.ArmorType == ERefiningEffectArmorType.AvoidRateMind || item.ArmorType == ERefiningEffectArmorType.AvoidRateSpeed || item.ArmorType == ERefiningEffectArmorType.AvoidRateStrength || item.ArmorType == ERefiningEffectArmorType.AvoidRateTechnique;
            if (flag2)
            {
                sbyte[] value2 = new sbyte[]
                {
                    12,
                    18,
                    24,
                    30,
                    36,
                    42,
                    48
                };
                Traverse.Create(item).Field("ArmorBonusValues").SetValue(value2);
            }
            bool flag3 = item.WeaponType == ERefiningEffectWeaponType.EquipmentAttack || item.WeaponType == ERefiningEffectWeaponType.EquipmentDefense;
            if (flag3)
            {
                sbyte[] value3 = new sbyte[]
                {
                    2,
                    5,
                    8,
                    11,
                    14,
                    17,
                    20
                };
                Traverse.Create(item).Field("WeaponBonusValues").SetValue(value3);
            }
            bool flag4 = item.ArmorType == ERefiningEffectArmorType.EquipmentAttack || item.ArmorType == ERefiningEffectArmorType.EquipmentDefense;
            if (flag4)
            {
                sbyte[] value4 = new sbyte[]
                {
                    2,
                    5,
                    8,
                    11,
                    14,
                    17,
                    20
                };
                Traverse.Create(item).Field("ArmorBonusValues").SetValue(value4);
            }
            return true;
        }

        // Token: 0x0600001A RID: 26 RVA: 0x00003C34 File Offset: 0x00001E34
        public BetterArmor(bool show)
        {
            this.showModification = show;
            this.setDefensePersentage(2, 100, 0, 50, 50);
            this.setDefensePersentage(3, 60, 40, 40, 60);
            this.setDefensePersentage(1, 80, 20, 20, 80);
            this.setDefensePersentage(4, 50, 0, 0, 50);
            this.setDefense(2, 390, 150, 320, 140);
            this.setDefense(3, 260, 130, 200, 120);
            this.setDefense(1, 540, 170, 460, 160);
            this.setDefense(4, 150, 110, 100, 100);
            this.setWeaponDefense(2, 390, 150, 320, 140);
            this.setWeaponDefense(3, 260, 130, 200, 120);
            this.setWeaponDefense(1, 540, 170, 460, 160);
            this.setWeaponDefense(4, 150, 110, 100, 100);
            this.setAttack(2, 120, 100, 70, 100);
            this.setAttack(3, 240, 150, 170, 150);
            this.setAttack(1, 50, 50, 30, 50);
            this.setAttack(4, 20, 10, 10, 10);
            this.setWeaponAttack(2, 230, 150, 160, 150);
            this.setWeaponAttack(3, 400, 200, 300, 200);
            this.setWeaponAttack(1, 100, 100, 60, 100);
            this.setWeaponAttack(4, 40, 50, 20, 50);
            this.setWeight(2, 170, 40, 130, 40);
            this.setWeight(3, 100, 30, 70, 30);
            this.setWeight(1, 50, 20, 30, 20);
            this.setWeight(4, 20, 10, 10, 10);
            this.setChestWeight();
            this.setHits();
            this.setCraftToType(0, 1);
            this.setCraftToType(7, 5);
            this.setCraftToType(14, 2);
            this.setCraftToType(21, 6);
            this.setCraftToType(28, 3);
            this.setCraftToType(35, 7);
            this.setCraftToType(42, 4);
            this.setCraftToType(49, 8);
            this.setTrick();
            this.setSubtypeHit();
            this.setSubtypeBonus();
        }

        // Token: 0x0600001B RID: 27 RVA: 0x000040C4 File Offset: 0x000022C4
        public void LogInfo()
        {
            AdaptableLog.Info("名字,材质,类型,耐久,重量,破甲,坚韧,穿透,攻势,距离,招式,力道,精妙,迅疾,动心,初始内伤,调节范围,追击系数,变招系数,伤害格挡,心神格挡,重创格挡,架势获取");
            Weapon.Instance.Iterate(new Func<WeaponItem, bool>(this.logWeapon));
            AdaptableLog.Info("名字,材质,类型,耐久,重量,破刃,坚韧,御体,外伤减免,御气,内伤减免,卸力,拆招,闪避,守心");
            Armor.Instance.Iterate(new Func<ArmorItem, bool>(this.logArmor));
        }

        // Token: 0x0600001C RID: 28 RVA: 0x00004118 File Offset: 0x00002318
        public void Modify()
        {
            bool flag = this.showModification;
            if (flag)
            {
                AdaptableLog.Info("------原始数据------");
                this.LogInfo();
            }
            Armor.Instance.Iterate(new Func<ArmorItem, bool>(this.changeArmor));
            Weapon.Instance.Iterate(new Func<WeaponItem, bool>(this.changeWeapon));
            RefiningEffect.Instance.Iterate(new Func<RefiningEffectItem, bool>(this.changeRefine));
            bool flag2 = this.showModification;
            if (flag2)
            {
                AdaptableLog.Info("------修改数据------");
                this.LogInfo();
            }
        }

        // Token: 0x04000005 RID: 5
        private Dictionary<short, int> typeDic = new Dictionary<short, int>();

        // Token: 0x04000006 RID: 6
        private List<string> materialName = new List<string>
        {
            "",
            "硬木",
            "硬铁",
            "硬玉",
            "硬布",
            "软木",
            "软铁",
            "软玉",
            "软布"
        };

        // Token: 0x04000007 RID: 7
        private List<sbyte> innerAdjust = new List<sbyte>
        {
            0,
            30,
            10,
            20,
            40,
            30,
            10,
            20,
            40
        };

        // Token: 0x04000008 RID: 8
        private Dictionary<short, string> typeName = new Dictionary<short, string>();

        // Token: 0x04000009 RID: 9
        private Dictionary<sbyte, sbyte> hitType = new Dictionary<sbyte, sbyte>();

        // Token: 0x0400000A RID: 10
        private Dictionary<sbyte, string> trickName = new Dictionary<sbyte, string>();

        // Token: 0x0400000B RID: 11
        private Dictionary<int, int> outPercentage = new Dictionary<int, int>();

        // Token: 0x0400000C RID: 12
        private Dictionary<int, int> inPercentage = new Dictionary<int, int>();

        // Token: 0x0400000D RID: 13
        private Dictionary<int, int> baseDefense = new Dictionary<int, int>();

        // Token: 0x0400000E RID: 14
        private Dictionary<int, int> baseAttack = new Dictionary<int, int>();

        // Token: 0x0400000F RID: 15
        private Dictionary<int, int> baseWeight = new Dictionary<int, int>();

        // Token: 0x04000010 RID: 16
        private Dictionary<int, int> deltaDefense = new Dictionary<int, int>();

        // Token: 0x04000011 RID: 17
        private Dictionary<int, int> deltaAttack = new Dictionary<int, int>();

        // Token: 0x04000012 RID: 18
        private Dictionary<int, int> deltaWeight = new Dictionary<int, int>();

        // Token: 0x04000013 RID: 19
        private Dictionary<int, int> chestWeight = new Dictionary<int, int>();

        // Token: 0x04000014 RID: 20
        private Dictionary<int, int> baseWeaponDefense = new Dictionary<int, int>();

        // Token: 0x04000015 RID: 21
        private Dictionary<int, int> baseWeaponAttack = new Dictionary<int, int>();

        // Token: 0x04000016 RID: 22
        private Dictionary<int, int> deltaWeaponDefense = new Dictionary<int, int>();

        // Token: 0x04000017 RID: 23
        private Dictionary<int, int> deltaWeaponAttack = new Dictionary<int, int>();

        // Token: 0x04000018 RID: 24
        private Dictionary<short, short> bonusHit0 = new Dictionary<short, short>();

        // Token: 0x04000019 RID: 25
        private Dictionary<short, short> bonusHit1 = new Dictionary<short, short>();

        // Token: 0x0400001A RID: 26
        private Dictionary<short, short> bonusHit2 = new Dictionary<short, short>();

        // Token: 0x0400001B RID: 27
        private Dictionary<short, short> bonusHit3 = new Dictionary<short, short>();

        // Token: 0x0400001C RID: 28
        private Dictionary<int, short> bonusLidao = new Dictionary<int, short>();

        // Token: 0x0400001D RID: 29
        private Dictionary<int, short> bonusJingmiao = new Dictionary<int, short>();

        // Token: 0x0400001E RID: 30
        private Dictionary<int, short> bonusXunji = new Dictionary<int, short>();

        // Token: 0x0400001F RID: 31
        private Dictionary<int, short> bonusDongxin = new Dictionary<int, short>();

        // Token: 0x04000020 RID: 32
        private Dictionary<short, int> bonusAtk = new Dictionary<short, int>();

        // Token: 0x04000021 RID: 33
        private Dictionary<short, int> bonusAttack = new Dictionary<short, int>();

        // Token: 0x04000022 RID: 34
        private Dictionary<short, int> bonusDefense = new Dictionary<short, int>();

        // Token: 0x04000023 RID: 35
        private Dictionary<short, int> bonusWeight = new Dictionary<short, int>();

        // Token: 0x04000024 RID: 36
        private bool showModification;
    }
}