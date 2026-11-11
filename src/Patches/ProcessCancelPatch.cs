using System;
using Assets.Behaviour.Flow;
using Assets.Behaviour.Flow.Elements;
using Assets.Behaviour.Input;
using Assets.Behaviour.UI;
using Assets.Behaviour.UI.Farm;
using Assets.Behaviour.UI.Shared;
using Assets.Behaviour.UI.SwapFlow;
using Assets.Behaviour.UI.TechTree;
using Assets.Behaviour.UI.WarEffort;
using HarmonyLib;
using UnityEngine;

namespace KingdomRightClickFix
{
    /// <summary>
    /// The game's right click (the "Cancel" input action, fired on button release) only closes popups,
    /// the tech tree sub-tree and held ghosts. This patch lets it also close the remaining open menus,
    /// returning to the workspace. The ESC menu is never touched.
    /// </summary>
    [HarmonyPatch(typeof(GameUI), nameof(GameUI.ProcessCancel))]
    internal static class ProcessCancelPatch
    {
        // Same threshold the game uses to tell a right-click from a right-drag pan of the workspace.
        private const float PanThreshold = 50f;

        private static readonly AccessTools.FieldRef<GameUI, UIPopupMenu> PopupField =
            AccessTools.FieldRefAccess<GameUI, UIPopupMenu>("_currentPopup");

        private static readonly AccessTools.FieldRef<GameUI, TechTreeMenu> TechMenuField =
            AccessTools.FieldRefAccess<GameUI, TechTreeMenu>("_techMenu");

        // UI/world elements that already use a right click for their own action.
        private static readonly Type[] RightClickConsumers =
        {
            typeof(CurrencyCell),
            typeof(FarmMenuPlot),
            typeof(WarEffortSoldierSprite),
            typeof(SwapFlowItem),
            typeof(RightClickAction),
            typeof(WorldFlowIO),
            typeof(WorldFlowPipeNode),
            typeof(WorldFlowArenaSlot),
            typeof(WorldFlowTrainingGroundsSlot),
        };

        // Returns false to skip the original method, true to let the vanilla behavior run.
        private static bool Prefix(GameUI __instance)
        {
            if (UIAlertWindow.Active || GameUI.MenuVisible)
            {
                return true;
            }

            if (PopupField(__instance) != null)
            {
                return true;
            }

            if (__instance.IsActive(GameUIType.Tech))
            {
                HandleTech(TechMenuField(__instance));
                return false;
            }

            if (DragDropManager.Instance.CancelDragDrop())
            {
                return false;
            }

            if (PlayerInput.Instance.TraverseDistance >= PanThreshold || IsOverRightClickConsumer())
            {
                return false;
            }

            CloseOpenMenu(__instance);
            return false;
        }

        private static void HandleTech(TechTreeMenu tech)
        {
            if (UIHelper.GetMouseOverUI<TechTreeListItem>() != null ||
                PlayerInput.Instance.TraverseDistance >= PanThreshold)
            {
                return;
            }

            tech.ProcessEscape();
        }

        private static void CloseOpenMenu(GameUI ui)
        {
            if (ui.IsActive(GameUIType.BuyMenu)) ui.ToggleBuyMenu(false);
            else if (ui.IsActive(GameUIType.Resources)) ui.ToggleResourceMenu(false);
            else if (ui.IsActive(GameUIType.Deconstruct)) ui.ToggleDeconstructMode(false);
            else if (ui.IsActive(GameUIType.Deco)) ui.ToggleDecoMenu(false);
            else if (ui.IsActive(GameUIType.WarEffort)) ui.ToggleWarEffortMenu(false);
            else if (ui.IsActive(GameUIType.Forest)) ui.ToggleForestCrewMenu(false);
            else if (ui.IsActive(GameUIType.Farm)) ui.ToggleFarmMenu(false);
            else if (ui.IsActive(GameUIType.Dungeon)) ui.ToggleDungeonMenu(false);
            else if (ui.IsActive(GameUIType.ItemMenu)) ui.ToggleItemMenu(false);
            else if (ui.IsActive(GameUIType.Clone)) ui.ToggleCloneMode(false);
            else if (ui.IsActive(GameUIType.AreaMove)) ui.ToggleAreaMove(false);
        }

        private static bool IsOverRightClickConsumer()
        {
            foreach (GameObject hit in UIHelper.GetAllMouseOverUI())
            {
                foreach (Type consumer in RightClickConsumers)
                {
                    if (hit.GetComponentInParent(consumer) != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

