using BepInEx;
using HarmonyLib;

namespace KingdomRightClickFix
{
    [BepInPlugin(Id, "Right Click Fix", Version)]
    public sealed class RightClickFixPlugin : BaseUnityPlugin
    {
        public const string Id = "urakka.kingdominc.rightclickfix";
        public const string Version = "1.0.0";

        private Harmony harmony;

        private void Awake()
        {
            harmony = new Harmony(Id);
            harmony.PatchAll(typeof(RightClickFixPlugin).Assembly);
            Logger.LogInfo("Right Click Fix ready.");
        }

        private void OnDestroy()
        {
            harmony?.UnpatchSelf();
        }
    }
}

