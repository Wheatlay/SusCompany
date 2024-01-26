# SusCompany: A Lethal Company Mod

This is a mod that introduces a new gameplay mechanic where one player is pitted against the rest of the team.

Impostor Mechanic: One player is randomly chosen as the Impostor at the start of each round. The Impostor's goal is to eliminate all other players without getting caught.

Victory Conditions: The game ends when all players are eliminated, resulting in an Impostor victory, or when the team returns to orbit, resulting in a team victory.

## Compatible mods
It is highly recommended to play with these mods as they provide more interactions among players:
- Midge-PushCompany
- Sophisticasean-KeysLockDoors
- saint_kendrick-Lethal_Doors

## MoreCompany
The mod should work with more than 4 players, but it hasn't been tested for this scenario.

## Configuration
The host of the lobby can configure:

- Spawn rate of the impostor. The number decides what percent of players are supposed to be impostors.
  - `/susmax [0-100]` 
    - default value: 25

- Randomization of impostor number within values of 0 to susmax.
  - `/susrandom [true, false]` 
     -  default value: true

- Ability of impostor to use vents.
  - `/susvent [true, false]` 
     -  default value: true
     
- Help
  - `/sushelp `
     -  Displays avaliable console commands
