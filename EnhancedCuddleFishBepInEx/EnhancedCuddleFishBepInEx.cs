using System.Reflection;
using HarmonyLib;
using UnityEngine;
using BepInEx;

namespace EnhancedCuddleFishBepInEx
{
    [BepInPlugin("com.C1oudS.EnhancedCuddleFishBepInEx", "EnhancedCuddleFishBepInEx", "1.0.1.0")]
    public class MyPlugin : BaseUnityPlugin
    {
        private void Start()
        {
            var harmony = new Harmony("com.C1oudS.EnhancedCuddleFishBepInEx");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    internal class EnhancedCuddlefish : MonoBehaviour
    {
        private PingInstance indicator;

        public void Start()
        {
            if (!base.gameObject.TryGetComponent<PingInstance>(out indicator))
            {
                indicator = base.gameObject.AddComponent<PingInstance>();
            }
            indicator.SetLabel(Language.main.Get("Cutefish"));
            indicator.pingType = PingType.Signal;
            indicator.origin = base.gameObject.transform;
        }

        public void Update()
        {
            LiveMixin component = base.gameObject.GetComponent<LiveMixin>();
            string label = Language.main.Get("Cutefish") + " [" + component.health + "/" + component.maxHealth + "]";
            indicator.SetLabel(label);
            indicator.SetType(PingType.Signal);
        }
    }
    internal class EnhancedCuddlefishPatch
    {
        [HarmonyPatch(typeof(CuteFish))]
        [HarmonyPatch("Start")]
        internal class PatchCFishStart
        {
            [HarmonyPostfix]
            public static void Postfix(CuteFish __instance)
            {
                __instance.gameObject.AddComponent<EnhancedCuddlefish>();
            }
        }

        [HarmonyPatch(typeof(CuteFishHandTarget))]
        [HarmonyPatch("PrepareCinematicMode")]
        internal class HealFish
        {
            [HarmonyPostfix]
            public static void Postfix(CuteFishHandTarget __instance)
            {
                LiveMixin component = __instance.cuteFish.gameObject.GetComponent<LiveMixin>();
                component.health = component.maxHealth;
            }
        }
    }
}
