# Mbox

> It took me 4h 40min to do it, including the analysis of the reference game.

> I wasn't used to a rail-based control system so I has to improvise a little bit. Anyway I focused on having a clear level-making system so extending it would be pretty easy.

> I'd really like to improve the level variety but the tech part took me more than I expected. Also the lack of GUI and finishing rewards, animatios etc. More juicy feedback, cam shaking etc.

> I'd like to add ragdolling, FX, gameplay variety, GUI, AI, etc.

> I had a lot of fun doing this but I would have loved to do a "whole day" project to extend myself a bit more.

Instructions
=============
The reference resolution is 9:16. Anyway you can play however you want.
The levels are made in prefabs, and they are composed of Paths and Virtual Cameras.
Each path can be easily modified and every camera is pre-rigged and activated by a trigger. Scaling and making new levels is pretty easy.
Only the current level is stored as persistence on PlayerPrefs.
The GameManager is a singleton that can be accesed from everywhere.
The FxManager (sadly lacking a pooling sys) is meant to take care of every particle.


There are two very similar levels to show the flow.
You can skip/Go back and restart the level with the keys "N", "B" and "R".



This is a quick pre-Analysis I made from the reference before starting.

Fun Race quick analysis
======================

Controls:

- Press to go forward
- Automatic direction
- Slight aceleration
- Almost Instant Deceleration
- Probably railing system 

Camera:
- Pre-Setup Virtual Cameras on scenario with specific angle and zoom.
- Activation by proximity or trigger.
- Soft transition between cams
- Active camera doesn't rotate but moves with the character, smooth damp and slight off-center liberty.

> Aproach: 
- Instantiate Scenario with Cinemachine Triggers enabling pre-setup cameras on the level.
- Each camera will have a fixed angle with hard look aiming and Simple follow with World Up transposing.

Gameplay:

- Reach finish line
- Dodge Obstacles
- Respawn on last checkpoint
- Competition IA

Environment:

- Simple 1-way corridors with traps and timing challenges.

Workflow
=============

1. Setup a simple circuit [Done]
2. Setup Character. Smooth acceleration movement [Done]
3. Railed Movement, smooth multirail chain [Done]
4. Camera system. Trigger driven virtual camera setup. [Done]
5. Character locomotion / animations [Done]
6. Traps and gameplay elements [Done]
7. Win and lose flow [Done]
8. Score by Time [ops]
9. IA / Enemies.[oh no]


Third party usage
=================
- Some "simplePrototype" visual assets.
- DoTweenPro to do some simple movements.
