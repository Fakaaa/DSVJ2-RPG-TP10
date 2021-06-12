using UnityEngine;
using GameManagerScript;

namespace EnemyAIFSMScript
{
    public class EnemyAIFSM : MonoBehaviour, IHittable<int>, IAttack
    {
        [SerializeField] public CharacterData enemyData;
        private Rigidbody myBody;

        [Header("Test")]
        public GameObject itemPrefab;
        public Transform container;
        [SerializeField] private GameObject collideMelee;
        private bool attacked = false;

        public enum EnemyState
        {
            Idle,
            GoingToTarget,
            Attacking,
            GoAway,
            Last
        }

        [Header("Enemy states data")]
        [SerializeField] private EnemyState state;
        public float speed = 5;
        public float targetDistance = 10;
        public float distanceToStop = 1;
        public float distanceToRestart = 5;
        public float timeStopped = 1;
        public Transform target;
        private float time;

        private void Awake()
        {
            GameManager.Get().AddEnemyToList(gameObject);
        }

        private void Start()
        {
            myBody = gameObject.GetComponent<Rigidbody>();
            enemyData.characterAlive = true;
        }

        private void Update()
        {
            time += Time.deltaTime;

            switch (state)
            {
                case EnemyState.Idle:
                    if (time > timeStopped)
                    {
                        // Si el player se encuentra cerca del enemigo, este va a atacarlo
                        if (Vector3.Distance(target.transform.position, transform.position) < targetDistance)
                            NextState();
                    }
                    break;
                case EnemyState.GoingToTarget:
                    Vector3 dir = new Vector3(target.transform.position.x - transform.position.x, 0, target.transform.position.z - transform.position.z);
                    transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

                    // Si el player se escapa, el enemigo vuelve a su comportamiento erratico
                    if (Vector3.Distance(target.transform.position, transform.position) > targetDistance)
                        state = EnemyState.Idle;

                    // Si toca al player le saca una vida y se aleja
                    if (Vector3.Distance(transform.position, target.transform.position) < distanceToStop)
                    {
                        Attack();
                        NextState();
                    }
                    break;
                case EnemyState.Attacking:
                    if (attacked)
                    {
                        NextState();
                        collideMelee.SetActive(false);
                        attacked = false;
                    }
                    break;
                case EnemyState.GoAway:
                    Vector3 dir02 = new Vector3(transform.position.x - target.transform.position.x, 0, transform.position.z - target.transform.position.z);
                    transform.Translate(dir02.normalized * speed * Time.deltaTime, Space.World);                    

                    // Cuando llego a cierto punto vuelve a su comportamiento erratico
                    if (Vector3.Distance(transform.position, target.transform.position) > distanceToRestart)
                        NextState();
                    break;
            }
        }

        private void NextState()
        {
            time = 0;
            int intState = (int)state;
            intState++;
            intState = intState % ((int)EnemyState.Last);
            SetState((EnemyState)intState);
        }

        private void SetState(EnemyState enemyState)
        {
            state = enemyState;
        }

        public void Attack()
        {
            collideMelee.SetActive(true);
            // Aca se activaria la animacion
        }

        public void ReceiveDamage(int damageTaken)
        {
            enemyData.characterHp -= damageTaken;

            if (enemyData.characterHp <= 0)
            {
                enemyData.characterHp = 0;
                enemyData.characterAlive = false;

                Debug.Log("Murio");
                GameManager.Get().RemoveEnemyToList(gameObject);
                Instantiate(itemPrefab, transform.position, Quaternion.identity, container);
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("MeleeAttackPlayer"))
            {
                if (myBody != null)
                    myBody.AddExplosionForce(3, other.transform.position, 4, 2, ForceMode.Impulse);

                attacked = true;
                ReceiveDamage(5);
            }
        }
    }
}