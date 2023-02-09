using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "newIntVariable", menuName = "Variables/IntVariable")]
public class IntVariable : ScriptableObject
{
    public int player_health;
    public int player_mana;
    public int enemie_health;
    public int boss_health;
    public int damages;
}
