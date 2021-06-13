using UnityEngine;

namespace TestEnemyScript
{
    public class TestEnemy : MonoBehaviour, IHittable<int>, IAttack
    {
        [SerializeField] public CharacterData enemyData;

        private Rigidbody myBody;
        private void Start()
        {
            myBody = gameObject.GetComponent<Rigidbody>();
            enemyData.characterAlive = true;
        }
        public void Attack()
        {
            //Attack
        }

        public void ReceiveDamage(int damageTaken)
        {
            enemyData.characterHp -= damageTaken;

            if (enemyData.characterHp <= 0)
            {
                enemyData.characterHp = 0;
                enemyData.characterAlive = false;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.collider.CompareTag("MeleeAttackPlayer"))
            {
                if (myBody != null)
                    myBody.AddExplosionForce(3, collision.transform.position, 4, 2, ForceMode.Impulse);
                ReceiveDamage(5);
                Debug.Log("Entro?");
            }
        }
    }
}