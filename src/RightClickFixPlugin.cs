using BepInEx;
using KingdomFoundation.Api;

namespace KingdomRightClickFix
{
    [BepInPlugin(Id, "Right Click Fix", Version)]
    [BepInDependency("urakka.kingdominc.foundation", "0.14.0")]
    public sealed class RightClickFixPlugin : BaseUnityPlugin
    {
        public const string Id = "urakka.kingdominc.rightclickfix";
        public const string Version = "1.0.1";

        private System.IDisposable cancelHook;

        private void Awake()
        {
            cancelHook = FoundationApi.GameHooks.Subscribe(Id, GameHookPoints.Cancel, before: ProcessCancelPatch.Before);
            Logger.LogInfo("Right Click Fix ready.");
        }

        private void OnDestroy()
        {
            cancelHook?.Dispose();
        }
    }
}

