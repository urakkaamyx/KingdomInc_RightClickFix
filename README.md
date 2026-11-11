# Right Click Fix

Right click now closes any open menu in Kingdom Inc. and takes you back to your workspace.

In the base game, right click only closes popups, steps back in the tech tree and drops a held building. Menus such as the build menu and the tech tree itself ignore it. This mod makes them respond too.

## Features

- **Right click closes menus:** Build menu, tech tree, resources, deco, war effort, forest crew, farm, dungeon and item menus.
- **Cancels tool modes:** Deconstruct, clone and area-move modes end with a right click.
- **Tech tree:** Right click steps back one level, then closes the tree.
- **ESC menu untouched:** The ESC menu and alert windows behave exactly as before.
- **Right-drag panning still works:** Dragging the workspace with the right mouse button never closes a menu.
- **Right-click actions keep working:** Where the game already uses right click (such as currency cells, farm plots, soldiers, pipe nodes and workspace selection), the menu stays open and the normal action happens.

## Usage

Press the right mouse button without dragging. Each right click closes one layer, so you end up back on the workspace.

## Requirements

- Kingdom Inc (Windows x64 Mono)
- [BepInEx 5](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.23.6) (5.4.23.6 or newer)

## Installation

1. Install BepInEx 5 for Kingdom Inc.
2. Extract the release ZIP into your Kingdom Inc game directory.
3. Verify the file exists at `BepInEx/plugins/RightClickFix/RightClickFix.dll`.
4. Launch the game.

## Compatibility

| Class.Method | Hook Type | Impact |
|---|---|---|
| GameUI.ProcessCancel | Harmony Prefix | Lets right click close the build menu, tech tree, resources, deco, war effort, forest crew, farm, dungeon and item menus, and the deconstruct, clone and area-move modes. Popups, the ESC menu and alert windows keep their normal behavior. Another mod that also patches this method may conflict. |

## Removal

Close the game and delete `BepInEx/plugins/RightClickFix/`. Saved kingdoms are not affected.

## Credits

Created by **Urakka**. Game artwork belongs to its respective owners.
