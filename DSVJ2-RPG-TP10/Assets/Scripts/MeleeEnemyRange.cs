using UnityEngine;
using EnemyAIFSMScript;

public class MeleeEnemyRange : MonoBehaviour
{
    [SerializeField] public EnemyAIFSM enemy;
    public int GetDamageEnemey()
    {
        return enemy.enemyData.characterDamage;
    }
}
