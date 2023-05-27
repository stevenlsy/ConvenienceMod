﻿using System;
using ConvenienceFrontend.CombatStrategy.config;
using DG.Tweening;
using FrameWork;
using GameData.GameDataBridge;
using HarmonyLib;
using Spine.Unity;
using TaiwuModdingLib.Core.Utils;
using TMPro;
using UnityEngine;
using static UnityEngine.UI.Slider;
using Object = UnityEngine.Object;

namespace ConvenienceFrontend.CombatStrategy
{
    internal class UI_CombatPatch
    {
        // Token: 0x06000044 RID: 68 RVA: 0x00005084 File Offset: 0x00003284
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UI_Combat), "Update")]
        public static void UI_Combat_Update_Postfix()
        {
            if (!CombatStrategyMod.ReplaceAI) return;

            bool keyDown = Input.GetKeyDown(CombatStrategyMod.Settings.SwitchAutoMoveKey);
            if (keyDown)
            {
                UI_CombatPatch.SwitchAutoMove();
            }
            else
            {
                bool keyDown2 = Input.GetKeyDown(CombatStrategyMod.Settings.SwitchAutoAttackKey);
                if (keyDown2)
                {
                    UI_CombatPatch.SwitchAutoAttack();
                }
                else
                {
                    bool keyDown3 = Input.GetKeyDown(CombatStrategyMod.Settings.SwitchTargetDistanceKey);
                    if (keyDown3)
                    {
                        UI_CombatPatch.SwitchTargetDistance();
                    }
                    else
                    {
                        bool flag = CombatStrategyMod.Settings.TargetDistance < 120;
                        if (flag)
                        {
                            UI_CombatPatch.CheckKey(CombatStrategyMod.Settings.IncreaseDistanceKey, 1, ref UI_CombatPatch.pressKeyCounterInc);
                        }
                        bool flag2 = CombatStrategyMod.Settings.TargetDistance > 20;
                        if (flag2)
                        {
                            UI_CombatPatch.CheckKey(CombatStrategyMod.Settings.DecreaseDistanceKey, -1, ref UI_CombatPatch.pressKeyCounterDec);
                        }
                    }
                }
            }
        }

        // Token: 0x06000045 RID: 69 RVA: 0x00005140 File Offset: 0x00003340
        private static void CheckKey(KeyCode key, int addFactor, ref int keyCounter)
        {
            bool flag;
            if (!Input.GetKeyDown(key))
            {
                if (Input.GetKey(key))
                {
                    int num = keyCounter;
                    keyCounter = num + 1;
                    flag = (num == CombatStrategyMod.Settings.DistanceChangeSpeed);
                }
                else
                {
                    flag = false;
                }
            }
            else
            {
                flag = true;
            }
            bool flag2 = flag;
            if (flag2)
            {
                UI_CombatPatch.ModifyTargetDistance(addFactor);
                keyCounter = -2;
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(UI_Combat), "OnClick")]
        public static bool UI_Combat_OnClick_Prefix(UI_Combat __instance, CButton btn, sbyte ____showMercyOption)
        {
            if (!CombatStrategyMod.ReplaceAI) return true;
            if (!CombatStrategyMod.ShowUIInCombat) return true;
            if (____showMercyOption >= 0) return true;

            string btnName = btn.name;
            if (btnName == "AiOptionBtn")
            {
                // 点击了ai配置
                OnClickSettings(__instance);
                return false;
            }
            else if (btnName == "AutoFight")
            {
                return false;
            }

            return true;
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(UI_Combat), "OnInit")]
        public static void UI_Combat_OnInit_Postfix(UI_Combat __instance)
        {
            if (!CombatStrategyMod.ReplaceAI) return;

            GameDataBridge.AddMethodCall<ushort, string>(-1, 8, CombatStrategyMod.MethodId, 3, ConfigManager.GetBackendSettingsJson());
            GameDataBridge.AddMethodCall<ushort, string>(-1, 8, CombatStrategyMod.MethodId, 5, ConfigManager.GetStrategiesJson());
            if (CombatStrategyMod.Settings.ShowAutoAttackTips && UI_CombatPatch.autoAttackTips == null)
            {
                Debug.Log("CombatStrategyMod.Settings.ShowAutoAttackTips && UI_CombatPatch.autoAttackTips == null");
                Transform transform = __instance.CGet<Refers>("SelfInfoChar").gameObject.transform;
                UI_CombatPatch.autoAttackTips = Object.Instantiate<GameObject>(__instance.CGet<GameObject>("PauseTips"), transform, false);
                UI_CombatPatch.autoAttackTips.name = "AutoAttckTip";
                UI_CombatPatch.autoAttackTips.GetComponent<RectTransform>().anchoredPosition = new Vector2(-145f, -76f);
                Object.DestroyImmediate(UI_CombatPatch.autoAttackTips.transform.GetComponentInChildren<TextLanguage>());
                UI_CombatPatch.autoAttackTips.transform.GetComponentInChildren<TextMeshProUGUI>().text = "自动攻击";
                UI_CombatPatch.autoAttackTips.GetComponent<DOTweenAnimation>().DOPause();
                UI_CombatPatch.autoAttackTips.GetComponent<CanvasGroup>().alpha = 1f;
                UI_CombatPatch.autoAttackTips.GetComponent<DOTweenAnimation>().DOPlay();
            }
            UI_CombatPatch.UpdateAutoAttackTips(CombatStrategyMod.Settings.AutoAttack);
            if (!CombatStrategyMod.ShowUIInCombat)
            {
                Debug.Log("!CombatStrategyMod.ShowUIInCombat = true");
                GameObject gameObject = UI_CombatPatch.targetDistanceBack;
                if (gameObject != null)
                {
                    gameObject.SetActive(false);
                }
                GameObject gameObject2 = UI_CombatPatch.targetDistance2Back;
                if (gameObject2 != null)
                {
                    gameObject2.SetActive(false);
                }
                GameObject gameObject3 = UI_CombatPatch.distanceSliderBack;
                if (gameObject3 != null)
                {
                    gameObject3.SetActive(false);
                }
            }
            else
            {
                Debug.Log("!CombatStrategyMod.ShowUIInCombat = false");

                GameObject gameObject4 = __instance.transform.Find("Bottom/DistanceBack").gameObject;
                if (UI_CombatPatch.targetDistanceText == null && gameObject4!=null)
                {
                    Debug.Log("UI_CombatPatch.targetDistanceText == null");

                    UI_CombatPatch.targetDistanceBack = Object.Instantiate<GameObject>(gameObject4, gameObject4.transform.parent, false);
                    UI_CombatPatch.targetDistanceBack.name = "TargetDistanceBack";
                    UI_CombatPatch.targetDistanceBack.transform.localPosition = new Vector3(0f, 390f, 0f);
                    UI_CombatPatch.targetDistanceBack.GetComponent<CImage>().SetAlpha(0f);

                    MouseTipDisplayer component = UI_CombatPatch.targetDistanceBack.GetComponentInChildren<MouseTipDisplayer>();
                    if (component != null)
                    {
                        component.IsLanguageKey = false;
                        component.PresetParam = UI_CombatPatch.tipsTargetDistance;
                    }
                    UI_CombatPatch.targetDistanceText = UI_CombatPatch.targetDistanceBack.transform.GetComponentInChildren<TextMeshProUGUI>();
                }
                if (UI_CombatPatch.targetDistance2Text == null && gameObject4 != null)
                {
                    Debug.Log("UI_CombatPatch.targetDistance2Text == null");

                    UI_CombatPatch.targetDistance2Back = Object.Instantiate<GameObject>(gameObject4, gameObject4.transform.parent, false);
                    UI_CombatPatch.targetDistance2Back.name = "TargetDistance2Back";
                    UI_CombatPatch.targetDistance2Back.transform.localPosition = new Vector3(60f, 390f, 0f);
                    UI_CombatPatch.targetDistance2Back.GetComponent<CImage>().SetAlpha(0f);
                    RectTransform component2 = UI_CombatPatch.targetDistance2Back.GetComponent<RectTransform>();
                    component2.sizeDelta = component2.rect.size / 2f;
                    MouseTipDisplayer component3 = UI_CombatPatch.targetDistance2Back.GetComponentInChildren<MouseTipDisplayer>();
                    if (component3 != null) 
                    {
                        component3.IsLanguageKey = false;
                        component3.PresetParam = UI_CombatPatch.tipsTargetDistance2;
                    }

                    UI_CombatPatch.targetDistance2Text = UI_CombatPatch.targetDistance2Back.transform.GetComponentInChildren<TextMeshProUGUI>();
                    UI_CombatPatch.targetDistance2Text.color = Color.grey;
                    UI_CombatPatch.targetDistance2Text.fontSize -= 6f;
                    PointClickBridge pointClickBridge = UI_CombatPatch.targetDistance2Back.AddComponent<PointClickBridge>();
                    PointClickBridge pointClickBridge2 = pointClickBridge;
                    Action onLeftClick;
                    if ((onLeftClick = __SwitchAutoAttack) == null)
                    {
                        onLeftClick = (__SwitchAutoAttack = new Action(UI_CombatPatch.SwitchAutoAttack));
                    }
                    pointClickBridge2.OnLeftClick = onLeftClick;
                }
                if (UI_CombatPatch.distanceSlider == null && gameObject4 != null)
                {
                    Debug.Log("UI_CombatPatch.distanceSlider == null");

                    GameObject gameObject5 = __instance.CGet<Refers>("SelfInfoBottom").CGet<Refers>("WeaponInnerRatio").CGet<CSlider>("InnerRatioSlider").gameObject;
                    UI_CombatPatch.distanceSliderBack = Object.Instantiate<GameObject>(gameObject5, gameObject4.transform.parent, false);
                    UI_CombatPatch.distanceSliderBack.name = "distanceSliderBack";
                    UI_CombatPatch.distanceSliderBack.transform.localPosition = new Vector3(0f, 300f, 0f);
                    UI_CombatPatch.imageLeft = UI_CombatPatch.distanceSliderBack.transform.Find("RangeLeft").GetComponent<CImage>();
                    UI_CombatPatch.imageRight = UI_CombatPatch.distanceSliderBack.transform.Find("RangeRight").GetComponent<CImage>();
                    UI_CombatPatch.distanceSliderBack.transform.Find("BarLeft").gameObject.SetActive(false);
                    UI_CombatPatch.distanceSliderBack.transform.Find("BarRight").gameObject.SetActive(false);
                    UI_CombatPatch.distanceSlider = UI_CombatPatch.distanceSliderBack.GetComponent<CSlider>();
                    UI_CombatPatch.distanceSlider.onValueChanged.RemoveAllListeners();
                    UI_CombatPatch.distanceSlider.wholeNumbers = true;
                    UI_CombatPatch.distanceSlider.maxValue = 120f;
                    UI_CombatPatch.distanceSlider.minValue = 20f;
                    UI_CombatPatch.distanceSlider.SetDirection(Direction.RightToLeft, false); // 1
                    RectTransform component4 = UI_CombatPatch.distanceSliderBack.GetComponent<RectTransform>();
                    component4.sizeDelta = component4.rect.size - new Vector2(40f, 0f);
                    RectTransform component5 = UI_CombatPatch.distanceSliderBack.transform.Find("CurrRatio").gameObject.GetComponent<RectTransform>();
                    UI_CombatPatch.distanceSlider.handleRect = component5;
                    component5.anchoredPosition = new Vector2(0f, 0f);
                    component5.sizeDelta = new Vector2(24f, -23f);
                    UI_CombatPatch.distanceSlider.onValueChanged.AddListener(delegate (float val)
                    {
                        if (val != CombatStrategyMod.Settings.TargetDistance)
                        {
                            UI_CombatPatch.UpdateTargetDistance((int)val);
                        }
                        UI_CombatPatch.imageLeft.fillAmount = (120f - val) / 100f;
                        UI_CombatPatch.imageRight.fillAmount = (val - 20f) / 100f;
                    });
                }
                UI_CombatPatch.UpdateTargetDistance(CombatStrategyMod.Settings.TargetDistance);
                UI_CombatPatch.UpdateTargetDistance2Text(CombatStrategyMod.Settings.TargetDistance2);
                UI_CombatPatch.UpdateAutoMoveText(CombatStrategyMod.Settings.AutoMove);
            }
        }

        // Token: 0x06000047 RID: 71 RVA: 0x00005868 File Offset: 0x00003A68
        private static void OnClickSettings(UI_Combat instance)
        {
            ConfigManager.SaveJsons();
            SkeletonGraphic speedAni = instance.CGet<SkeletonGraphic>("SpeedAni");
            ReflectionExtensions.ModifyField<UI_Combat>(instance, "_selectingUseItem", true);
            bool needResume;
            bool flag = needResume = !((CToggle)ReflectionExtensions.GetFieldValue<UI_Combat>(instance, "_pauseToggle")).isOn;
            if (flag)
            {
                instance.Pause();
            }
            else
            {
                Time.timeScale = 1f;
            }
            ArgumentBox argumentBox = EasyPool.Get<ArgumentBox>();
            argumentBox.Set("InCombat", true);
            UIElement.SelectItemInCombat.SetOnInitArgs(argumentBox);
            UI_CombatStrategySetting.GetUI().SetOnInitArgs(argumentBox);
            UIManager.Instance.ShowUI(UI_CombatStrategySetting.GetUI());
            ReflectionExtensions.CallMethod(instance, "SetSkeletonAndVfxTimePause", new object[]
            {
                true
            });
            Camera particleCamera = (Camera)ReflectionExtensions.GetFieldValue<UI_Combat>(instance, "_particleCamera");
            UI_CombatStrategySetting.GetUI().OnShowed = delegate ()
            {
                particleCamera.enabled = false;
            };
            UI_CombatStrategySetting.GetUI().OnHide = delegate ()
            {
                speedAni.timeScale = (float)ReflectionExtensions.GetFieldValue<UI_Combat>(instance, "_displayTimeScale");
                ReflectionExtensions.CallMethod(instance, "SetSkeletonAndVfxTimePause", new object[]
                {
                    false
                });
                particleCamera.enabled = true;
                ReflectionExtensions.ModifyField<UI_Combat>(instance, "_selectingUseItem", false);
                UI_CombatPatch.UpdateTargetDistance(CombatStrategyMod.Settings.TargetDistance);
                UI_CombatPatch.UpdateTargetDistance2Text(CombatStrategyMod.Settings.TargetDistance2);
                UI_CombatPatch.UpdateAutoMoveText(CombatStrategyMod.Settings.AutoMove);
                UI_CombatPatch.UpdateAutoAttackTips(CombatStrategyMod.Settings.AutoAttack);
                if (needResume)
                {
                    instance.Resume();
                }
                else
                {
                    Time.timeScale = 0f;
                }
            };
            speedAni.timeScale = 0f;
        }

        // Token: 0x06000048 RID: 72 RVA: 0x000059AF File Offset: 0x00003BAF
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UI_Combat), "OnDisable")]
        public static void UI_Combat_OnDisable_Postfix()
        {
            ConfigManager.SaveJsons();
        }

        // Token: 0x06000049 RID: 73 RVA: 0x000059B8 File Offset: 0x00003BB8
        private static void UpdateTargetDistanceText(int val)
        {
            if (UI_CombatPatch.targetDistanceText != null)
            {
                UI_CombatPatch.targetDistanceText.text = ((float)val / 10f).ToString("f1");
            }
        }

        // Token: 0x0600004A RID: 74 RVA: 0x000059F8 File Offset: 0x00003BF8
        private static void UpdateTargetDistance2Text(int val)
        {
            bool flag = UI_CombatPatch.targetDistance2Text == null;
            if (!flag)
            {
                UI_CombatPatch.targetDistance2Text.text = ((float)val / 10f).ToString("f1");
            }
        }

        // Token: 0x0600004B RID: 75 RVA: 0x00005A38 File Offset: 0x00003C38
        private static void UpdateAutoMoveText(bool flag)
        {
            if (UI_CombatPatch.targetDistanceText != null)
            {
                UI_CombatPatch.targetDistanceText.color = (flag ? new Color(0.973f, 0.902f, 0.757f) : Color.grey);
            }
        }

        // Token: 0x0600004C RID: 76 RVA: 0x00005A80 File Offset: 0x00003C80
        public static void UpdateAutoAttackTips(bool flag)
        {
            bool flag2 = UI_CombatPatch.autoAttackTips == null;
            if (!flag2)
            {
                bool flag3 = !CombatStrategyMod.Settings.ShowAutoAttackTips;
                if (flag3)
                {
                    UI_CombatPatch.autoAttackTips.SetActive(false);
                }
                else
                {
                    UI_CombatPatch.autoAttackTips.SetActive(flag);
                }
            }
        }

        // Token: 0x0600004D RID: 77 RVA: 0x00005ACC File Offset: 0x00003CCC
        public static void UpdateTargetDistance(int val)
        {
            if (CombatStrategyMod.Settings.TargetDistance != val)
            {
                CombatStrategyMod.Settings.TargetDistance = val;
            }
            GameDataBridge.AddMethodCall<ushort, int>(-1, 8, CombatStrategyMod.MethodId, 2, val);
            UI_CombatPatch.distanceSlider.value = (float)val;
            UI_CombatPatch.UpdateTargetDistanceText(val);
        }

        // Token: 0x0600004E RID: 78 RVA: 0x00005B1E File Offset: 0x00003D1E
        public static void UpdateTargetDistance2(int val)
        {
            CombatStrategyMod.Settings.TargetDistance2 = val;
            UI_CombatPatch.UpdateTargetDistance2Text(val);
        }

        // Token: 0x0600004F RID: 79 RVA: 0x00005B34 File Offset: 0x00003D34
        public static void SwitchAutoMove()
        {
            CombatStrategyMod.Settings.AutoMove = !CombatStrategyMod.Settings.AutoMove;
            GameDataBridge.AddMethodCall<ushort, bool>(-1, 8, CombatStrategyMod.MethodId, 0, CombatStrategyMod.Settings.AutoMove);
            UI_CombatPatch.UpdateAutoMoveText(CombatStrategyMod.Settings.AutoMove);
        }

        // Token: 0x06000050 RID: 80 RVA: 0x00005B84 File Offset: 0x00003D84
        public static void SwitchAutoAttack()
        {
            CombatStrategyMod.Settings.AutoAttack = !CombatStrategyMod.Settings.AutoAttack;
            GameDataBridge.AddMethodCall<ushort, bool>(-1, 8, CombatStrategyMod.MethodId, 1, CombatStrategyMod.Settings.AutoAttack);
            UI_CombatPatch.UpdateAutoAttackTips(CombatStrategyMod.Settings.AutoAttack);
        }

        // Token: 0x06000051 RID: 81 RVA: 0x00005BD1 File Offset: 0x00003DD1
        public static void ModifyTargetDistance(int addFactor)
        {
            UI_CombatPatch.UpdateTargetDistance(CombatStrategyMod.Settings.TargetDistance + addFactor);
        }

        // Token: 0x06000052 RID: 82 RVA: 0x00005BE8 File Offset: 0x00003DE8
        public static void SwitchTargetDistance()
        {
            int targetDistance = CombatStrategyMod.Settings.TargetDistance;
            UI_CombatPatch.UpdateTargetDistance(CombatStrategyMod.Settings.TargetDistance2);
            UI_CombatPatch.UpdateTargetDistance2(targetDistance);
        }

        // Token: 0x04000062 RID: 98
        private static int pressKeyCounterInc = -2;

        // Token: 0x04000063 RID: 99
        private static int pressKeyCounterDec = -2;

        // Token: 0x04000064 RID: 100
        private static GameObject targetDistanceBack;

        // Token: 0x04000065 RID: 101
        private static GameObject targetDistance2Back;

        // Token: 0x04000066 RID: 102
        private static GameObject distanceSliderBack;

        // Token: 0x04000067 RID: 103
        private static TextMeshProUGUI targetDistanceText;

        // Token: 0x04000068 RID: 104
        private static TextMeshProUGUI targetDistance2Text;

        // Token: 0x04000069 RID: 105
        private static GameObject autoAttackTips;

        // Token: 0x0400006A RID: 106
        private static CSlider distanceSlider;

        // Token: 0x0400006B RID: 107
        private static CImage imageLeft;

        // Token: 0x0400006C RID: 108
        private static CImage imageRight;

        // Token: 0x0400006E RID: 110
        private static readonly string[] tipsTargetDistance = new string[]
        {
            "当前目标距离",
            "角色会自动向该距离移动，颜色为灰色表示不启用自动移动"
        };

        // Token: 0x0400006F RID: 111
        private static readonly string[] tipsTargetDistance2 = new string[]
        {
            "备用目标距离",
            "点击鼠标左键或按下快捷键与当前目标距离调换"
        };

        public static Action __SwitchAutoAttack;
    }
}