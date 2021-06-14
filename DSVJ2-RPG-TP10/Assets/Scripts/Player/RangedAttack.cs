using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    public delegate int ArrowDamage();
    public static ArrowDamage damageArrow;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            EnemyAIFSMScript.EnemyAIFSM enemyHited = other.gameObject.GetComponent<EnemyAIFSMScript.EnemyAIFSM>();
            if (enemyHited != null)
            {
                if(damageArrow != null)
                    enemyHited.ReceiveDamage(damageArrow.Invoke());
            }
        }
        Destroy(gameObject);
    }
}
