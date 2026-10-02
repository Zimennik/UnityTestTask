**Implemented**
- Combo: +1 for each move that clears a line or a 3x3 box, resets after 3 moves without clears. Gives bonus points, shown in the HUD.
- Combo VFX: board glow, sparks, burst and camera shake, stronger on higher combos.
- Refresh power-up with a 30-move cooldown.
- Analytics for figure moves, bonuses and power-ups (console output).

**Architecture**
- Gameplay raises C# events, a separate tracker turns them into analytics events. Gameplay doesn't know about analytics.
- New analytics provider = one class implementing IAnalyticsProvider.
- Power-ups are ScriptableObjects. New power-up = new class + asset.

**Assumptions**
- A "move" is placing a figure.
- Bonuses are combo points and power-up recharge.

**With more time**
- Unit tests, DI, more powerups, better visual effects, particle pooling, cleanup of legacy code.