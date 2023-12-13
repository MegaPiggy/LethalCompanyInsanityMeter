using BepInEx;
using BepInEx.Logging;
using GameNetcodeStuff;
using HarmonyLib;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace InsanityMeter
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class InsanityMeterMod : BaseUnityPlugin
    {
        private const string modGUID = "MegaPiggy.InsanityMeter";
        private const string modName = "Insanity Meter";
        private const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static InsanityMeterMod Instance { get; set; }

        private static ManualLogSource LoggerInstance => Instance.Logger;

        public static Sprite CreateSpriteFromTexture(Texture2D texture2D)
        {
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
            sprite.name = texture2D.name;
            return sprite;
        }

        private void Awake()
        {
            if (Instance == null) Instance = this;
            harmony.PatchAll();
            Logger.LogInfo($"Plugin {modName} is loaded with version {modVersion}!");
        }

        [HarmonyPatch(typeof(HUDManager))]
        internal class HUDManagerPatch
        {
            [HarmonyPatch("Awake")]
            [HarmonyPostfix]
            public static void AwakePostfix(HUDManager __instance)
            {
                var parent = __instance.HUDElements[2].canvasGroup.transform.parent;
                GameObject insanityMeterObj = new GameObject("InsanityMeter", typeof(Image), typeof(CanvasGroup));
                var element = new HUDElement { canvasGroup = insanityMeterObj.GetComponent<CanvasGroup>(), targetAlpha = 1 };
                __instance.HUDElements = __instance.HUDElements.AddToArray(element);
                RectTransform rect = insanityMeterObj.GetComponent<RectTransform>();
                rect.pivot = Vector2.one * 0.5f;
                rect.sizeDelta = new Vector2(150, 20);
                rect.anchoredPosition3D = new Vector3(0, 220, -0.075f);
                Image insanityMeter = insanityMeterObj.GetComponent<Image>();
                insanityMeter.sprite = CreateSpriteFromTexture(Texture2D.whiteTexture);
                insanityMeter.color = Color.black;
                insanityMeter.transform.SetParent(parent, false);
                GameObject insanityBarObj = new GameObject("InsanityBar", typeof(Image), typeof(InsanityMeterUI));
                RectTransform rect2 = insanityBarObj.GetComponent<RectTransform>();
                rect2.pivot = Vector2.one * 0.5f;
                rect2.sizeDelta = new Vector2(150, 20);
                Image insanityBar = insanityBarObj.GetComponent<Image>();
                insanityBar.fillMethod = Image.FillMethod.Horizontal;
                insanityBar.type = Image.Type.Filled;
                insanityBar.sprite = CreateSpriteFromTexture(Texture2D.whiteTexture);
                insanityBar.color = new Color32(123, 0, 236, byte.MaxValue);
                insanityBarObj.transform.SetParent(insanityMeter.transform, false);
            }
        }

        [HarmonyPatch(typeof(PlayerControllerB))]
        internal class PlayerControllerPatch
        {
            [HarmonyPatch("LateUpdate")]
            [HarmonyPostfix]
            public static void LateUpdatePostfix(PlayerControllerB __instance)
            {
                if (__instance.IsOwner && (!__instance.IsServer || __instance.isHostPlayerObject))
                {
                    if (__instance.isPlayerControlled && !__instance.isPlayerDead)
                    {
                        InsanityMeterUI.Instance.UpdatePercentage(__instance.GetInsanityPercentage());
                    }
                }
            }
        }
    }
}