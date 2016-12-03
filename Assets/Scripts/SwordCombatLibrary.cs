using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SwordCombatLibrary
{
    public enum EnemyType { insect, wolf, ghost };
    public enum PlayerWeapons { hammer, sword, magic_staff };
    public enum Directions { north, east, south, west };
    public class GameRules
    {
        public static readonly Dictionary<EnemyType, PlayerWeapons> PlayerAttackMatrix = new Dictionary<EnemyType, PlayerWeapons>()
        {
            {EnemyType.insect, PlayerWeapons.hammer },
            {EnemyType.wolf, PlayerWeapons.sword },
            {EnemyType.ghost, PlayerWeapons.magic_staff }
        };
    }

}