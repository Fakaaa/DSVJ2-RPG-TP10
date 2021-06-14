using System.Collections.Generic;
using UnityEngine;
using ItemCollectorScript;

namespace PlayerScript
{
    public class Player : MonoBehaviour, IHittable<int>, IAttack
    {
        [SerializeField] private CharacterController playerMovement;
        [SerializeField] Vector3 movementVec;
        [SerializeField] Vector3 playerVelocity;
        [SerializeField] private float rotateSpeed;

        private float gravityValue;
        
        [SerializeField] private float jumpPower;
        [SerializeField] private bool isOnAir;

        [SerializeField] public CharacterData playerData;
        [SerializeField] public CharacterAnimator playerAnimator;
        [SerializeField] private float speedMultiplerWhenRun;

        [SerializeField] private GameObject collideMelee;
        [SerializeField] private float rangeRangedAttack;

        //Provisorio------------------------
        [SerializeField] private List<GameObject> itemsOnInventory;
        [SerializeField] private ItemCollector myItemController;

        private float originalSpeed;
        private float maxSpeedRunning;
        private void Start()
        {
            originalSpeed = playerData.characterSpeed;
            maxSpeedRunning = originalSpeed + speedMultiplerWhenRun;
            playerData.attackReady = true;
            playerData.characterAlive = true;
            isOnAir = false;
            gravityValue = -9.8f;
            playerAnimator = new CharacterAnimator(gameObject.GetComponentInChildren<Animator>());
        }
        void Update()
        {
            MovePlayer();
            CheckIfIsOnGround();
            PlayerTakeItem();
            Attack();
        }
        public void PlayerTakeItem()
        {
            if (Input.GetKey(KeyCode.E))
            {
                myItemController.gameObject.SetActive(true);
            }
            else
                myItemController.gameObject.SetActive(false);

            if (myItemController.ItemPicked())
            {
                if (!itemsOnInventory.Contains(myItemController.ReturnItemToPlayer()))
                    itemsOnInventory.Add(myItemController.ReturnItemToPlayer());
            }
        }
        public void Attack()
        {
            if (!playerData.attackReady)
                playerData.coldownAttack += Time.deltaTime;

            if (playerData.coldownAttack >= playerData.resetColdown && !playerData.attackReady)
            {
                collideMelee.SetActive(false);
                playerData.attackReady = true;
                playerData.coldownAttack = 0;
            }

            Vector3 mousePos = Input.mousePosition;
            Ray rayMouseCamera = Camera.main.ScreenPointToRay(mousePos);

            if (Input.GetButton("Fire1") && playerData.attackReady)
            {
                switch (playerData.actualAttackType)
                {
                    case CharacterData.AttackType.Melee:
                        collideMelee.SetActive(true);
                        playerAnimator.PlayMeleeAttack();
                        break;
                    case CharacterData.AttackType.Ranged:
                        playerAnimator.PlayRangedAttack();
                        RaycastHit hitInfo;
                        Debug.DrawRay(rayMouseCamera.origin, rayMouseCamera.direction * rangeRangedAttack, Color.red);
                        if (Physics.Raycast(rayMouseCamera, out hitInfo, rangeRangedAttack))
                        {
                            //Direccion flecha
                            Ray arrowDirection;
                            arrowDirection = new Ray(transform.position + Vector3.up, hitInfo.point - (transform.position + Vector3.up));
                            Debug.DrawRay(arrowDirection.origin, arrowDirection.direction * rangeRangedAttack, Color.magenta);
                            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(arrowDirection.direction.normalized), rotateSpeed);

                            if (!hitInfo.collider.CompareTag("Player"))
                            {
                                EnemyAIFSMScript.EnemyAIFSM enemyHited = hitInfo.collider.gameObject.GetComponent<EnemyAIFSMScript.EnemyAIFSM>();
                                if(enemyHited != null && playerData.attackReady)
                                {
                                    enemyHited.ReceiveDamage(playerData.characterDamage);
                                }
                            }
                        }
                        break;
                }
                playerData.attackReady = false;
            }
            else
                Debug.DrawRay(rayMouseCamera.origin, rayMouseCamera.direction * rangeRangedAttack, Color.white);
        }
        public void ReceiveDamage(int damageTaken)
        {
            if (playerData.characterDefense > 0)
                playerData.characterDefense -= damageTaken;
            else
            {
                playerData.characterDefense = 0;
                playerData.characterHp -= damageTaken;
            }

            if (playerData.characterHp <= 0)
            {
                playerData.characterHp = 0;
                playerData.characterAlive = false;
                if (GameManagerScript.GameManager.Get() != null)
                    GameManagerScript.GameManager.Get().GameIsOver();
            }
        }
        void CheckIfIsOnGround()
        {
            RaycastHit hit;
            Ray raycast = new Ray(transform.position, -transform.up * 0.1f);
            Debug.DrawRay(raycast.origin, raycast.direction * 0.1f, Color.red);
            if (Physics.Raycast(raycast, out hit, 0.1f))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    isOnAir = false;
                    playerVelocity.y = 0;
                }
            }
            else
                isOnAir = true;
        }
        void ApplyGravity()
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
            playerMovement.Move(playerVelocity * Time.deltaTime);
        }
        void Jump()
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isOnAir)
                playerVelocity.y = Mathf.Sqrt(jumpPower * -3.0f * gravityValue);    //En deshuso (No animaciones , muchos bugs con el enemigo.)

            ApplyGravity();
        }
        void MovePlayer()
        {
            if (playerData.attackReady)
            {
                if(Input.GetKey(KeyCode.LeftShift)) //Run
                {
                    if(playerData.characterSpeed < maxSpeedRunning)
                        playerData.characterSpeed += speedMultiplerWhenRun * Time.deltaTime;
                    else
                        playerData.characterSpeed = maxSpeedRunning;
                }
                else
                {
                    if (playerData.characterSpeed > originalSpeed)
                        playerData.characterSpeed -= speedMultiplerWhenRun * Time.deltaTime;
                    else
                        playerData.characterSpeed = originalSpeed;
                }

                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                movementVec = new Vector3(horizontal, movementVec.y, vertical);
                playerAnimator.UpdateSpeed(movementVec.sqrMagnitude * (playerData.characterSpeed / originalSpeed)); //ANIMATION
                Vector3 movementRotated = Quaternion.AngleAxis(0, Vector3.up) * movementVec;

                if (movementVec != Vector3.zero)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementRotated.normalized), rotateSpeed);

                playerMovement.Move(movementVec * playerData.characterSpeed * Time.deltaTime);

                //Jump();
                ApplyGravity();
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("MeleeAttackEnemy"))
            {
                ReceiveDamage(5);
            }
        }
    }
}