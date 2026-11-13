using KingdomFoundation.Api;

namespace KingdomRightClickFix
{
    // Mod policy stays here. Native access and interception belong to Foundation.
    internal static class ProcessCancelPatch
    {
        private const float PanThreshold = 50f;
        private static readonly GameMenu[] CloseOrder = {
            GameMenu.Build, GameMenu.Resources, GameMenu.Deconstruction, GameMenu.Decorations,
            GameMenu.War, GameMenu.Forest, GameMenu.Farm, GameMenu.Dungeons, GameMenu.Items,
            GameMenu.Clone, GameMenu.AreaMove
        };
        internal static void Before(GameHookCall call)
        {
            if (call.IsCancelled) return;
            var ui = FoundationApi.UiAccess;
            if (ui.IsAlertVisible || ui.IsMenuVisible || ui.HasPopup) return;
            if (ui.IsMenuOpen(GameMenu.Research)) { HandleTech(); call.Cancel(); return; }
            if (ui.CancelDragDrop()) { call.Cancel(); return; }
            if (ui.PointerTraverseDistance >= PanThreshold || IsOverRightClickConsumer()) { call.Cancel(); return; }
            CloseOpenMenu(); call.Cancel();
        }
        private static void HandleTech()
        {
            if (!FoundationApi.UiAccess.IsPointerOverResearchList && FoundationApi.UiAccess.PointerTraverseDistance < PanThreshold)
                FoundationApi.UiAccess.ProcessResearchEscape();
        }
        private static void CloseOpenMenu()
        {
            foreach (var menu in CloseOrder)
                if (FoundationApi.UiAccess.IsMenuOpen(menu)) { FoundationApi.UiAccess.SetMenuVisible(menu, false); return; }
        }
        private static bool IsOverRightClickConsumer() => FoundationApi.UiAccess.PointerHasNativeRightClickAction;
    }
}
