# Monster Evicter

> *Evict every monster before the clock runs out, by exploiting their weaknesses.*

**Monster Evicter** is a fast-paced third-person action game where you play a mage "evicting" monsters. Read each enemy's elemental **weakness**, hit it with the right spell, stun it and perform the eviction, all against the clock.

It is my **Master's Degree final project** (Videogame Programming, U-TAD): a vertical slice with its own GDD, built to showcase clean, event-driven Unity architecture.

| | |
|---|---|
| **Engine** | Unity 6 (`6000.3.2f1`) |
| **Language** | C# |
| **Genre** | Third-person action |
| **Role** | Design and programming |

## Gameplay

- **Elemental affinities** — monsters carry a *weakness* and a *resistance* (`Fire`, `Water`, `Electric`). Hitting the weakness is rewarded with a critical multiplier.
- **Stun and evict** — a monster whose health is depleted is stunned, which opens an interaction to evict it; evictions drop mana orbs.
- **Spells** — elemental ball projectiles, elemental area attacks, a mana-consuming shield and healing.
- **Upgrades and economy** — coins, key items, health potions and tiered upgrades (damage, weakness crit, attack speed, shield efficiency, health, orb drops) driven by `PlayerUpgrades`.
- **Levels** — regular monster levels plus boss levels, handled by dedicated level managers.

## Technical highlights

- **Event-driven components** — health, mana, affinity, collectible and interactable components communicate through `UnityEvent`s, keeping gameplay objects decoupled.
- **State machines from scratch** — enemy AI (`Idle`, `Chase`, `Combat`, `HitReact`, `Stunned`, `Death`) and player state, linked to animation controllers.
- **Patterns** — Singleton managers, **Object Pooling** (`ObjectPoolManager`) and Flyweight-style shared data.
- **ScriptableObject tooling** — custom inspector tools for rapid level design and enemy balancing.
- **Save system** — JSON persistence (`SaveSystem`, serializable vectors) with scene management and a respawn ID per monster.
- **Modular UI/UX** — pause menu, death panel, damage pop-ups, billboarded health bars, input-device-aware prompts.
- **Sound manager** — central audio with per-actor sound relays.
- **Third-party** — Procedural Terrain Painter and asset-store art/VFX packs, each under its own license.

## Project layout

```
Assets/ProyectoFinalFolder/
  Common/     Components, Managers (Game, Level, Pool, Save, Sound), state machine base, UI
  Enemies/    Monster, state machine, states, enemy components
  Player/     Controller, spells/abilities, resources, upgrades
```

## Getting started

1. Install **Unity 6000.3.2f1** via Unity Hub.
2. Clone the repo and add the folder as a project in Unity Hub.
3. Open the project, load the main scene from the Build Settings and press **Play**.

## Author

**Gonzalo Liñán Aldana** — [GitHub](https://github.com/maltrak007) · [LinkedIn](https://www.linkedin.com/in/gonzalo-liñán-aldana-934283221/)
