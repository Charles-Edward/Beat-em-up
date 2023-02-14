using JetBrains.Annotations;
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
    public int damagesToPlayer;
    public int damagesToEnemies;
    public int scoreBasicEnemies;
    public int scoreBoss;

    public float speedGreenEnemy;
    public float speedRedEnemy;
    public float speedBoss;
    public float speedWhiteEnemy;

    public int damageGreenEnemy;
    public int damageRedEnemy;
    public int damageBoss;
    public int damageWhiteEnmy;
}
