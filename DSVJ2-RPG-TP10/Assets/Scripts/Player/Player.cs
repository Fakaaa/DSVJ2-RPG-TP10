using System.Collections.Generic;
using UnityEngine;
using ItemCollectorScript;
using Inventory;
using GameManagerScript;
using AudioManagerScript;

namespace PlayerScript
{
    public class Player : MonoBehaviour, IHittable<int>, IAttack
    {
        [SerializeField] private CharacterController playerMovement;

        [Header("PLAYER GENERAL DATA")]
        [SerializeField] Vector3 movementVec;
        [SerializeField] Vector3 playerVelocity;
        [SerializeField] private float rotateSpeed;
        [SerializeField] private float jumpPower;
        [SerializeField] private bool isOnAir;
        [SerializeField] public CharacterAnimator playerAnimator;
        [SerializeField] private float speedMultiplerWhenRun;
        [Space(50)]
        private float gravityValue;

        [Header("PLAYER SPECIFIC DATA")]
        [SerializeField] public CharacterData playerData;
        [SerializeField] private GameObject collideMelee;
        [SerializeField] private GameObject arrowModelPrefab;
        [SerializeField] private float rangeRangedAttack;
        [Header("PLAYER INVENTORY")]
        [SerializeField] private Inventory.Inventory itemsInventory;
        [SerializeField] private Equipment playerEquipment;
        [SerializeField] private ItemCollector myItemCollector;
        private GameObject arrow;
        private Vector3 posToReach;

        //Provisorio------------------------
        [Header("TEMPORAL")]
        [SerializeField] private List<GameObject> itemsOnInventory;

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
            if(GameManager.Get() != null)
                GameManager.Get().InitializeResultScreen();

            RangedAttack.damageArrow += PassDamageToArrow;

            if (playerData.actualAttackType == CharacterData.AttackType.Ranged)
                Cursor.lockState = CursorLockMode.Locked;
        }
        private void OnDisable()
        {
            RangedAttack.damageArrow -= PassDamageToArrow;
        }
        void Update()
        {
            MovePlayer();
            CheckIfIsOnGround();
            PlayerTakeItem();
            Attack();
        }

        public int PassDamageToArrow()
        {
           return playerData.characterDamage;
        }
        public void PlayerTakeItem()
        {
            if (Input.GetKey(KeyCode.E))
            {
                myItemCollector.gameObject.SetActive(true);
            }
            else
                myItemCollector.gameObject.SetActive(false);

            if (myItemCollector.ItemPicked())
            {
                if (!itemsOnInventory.Contains(myItemCollector.ReturnItemToPlayer()))
                    itemsOnInventory.Add(myItemCollector.ReturnItemToPlayer());
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
                        if (AudioManager.Get() != null)
                            AudioManager.Get().Play("Sword");
                        break;
                    case CharacterData.AttackType.Ranged:
                        playerAnimator.PlayRangedAttack();
                        if (AudioManager.Get() != null)
                            AudioManager.Get().Play("Arrow");
                        RaycastHit hitInfo;
                        Debug.DrawRay(rayMouseCamera.origin, rayMouseCamera.direction * rangeRangedAttack, Color.red);
                        if (Physics.Raycast(rayMouseCamera, out hitInfo, rangeRangedAttack))
                        {
                            //Direccion flecha
                            Ray arrowDirection;
                            arrowDirection = new Ray(transform.position + Vector3.up, hitInfo.point - (transform.position + Vector3.up));
                            posToReach = hitInfo.point;
                            Debug.DrawRay(arrowDirection.origin, arrowDirection.direction * rangeRangedAttack, Color.magenta);
                            arrow = Instantiate(arrowModelPrefab, arrowDirection.origin, Quaternion.identity);

                            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(arrowDirection.direction.normalized), 1);
                            arrow.transform.rotation = Quaternion.Slerp(arrow.transform.rotation, Quaternion.LookRotation(transform.forward), 1);
                        }
                        break;
                }
                playerData.attackReady = false;
            }
            else
                Debug.DrawRay(rayMouseCamera.origin, rayMouseCamera.direction * rangeRangedAttack, Color.white);

            if(arrow != null)
            {
                if (arrow.transform.position != posToReach)
                    arrow.transform.position = Vector3.Lerp(arrow.transform.position, posToReach, Time.deltaTime * 2);
            }
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
                if (GameManager.Get() != null)
                    GameManager.Get().GameIsOver();
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
                    if (AudioManager.Get() != null)
                    {
                        AudioManager.Get().Stop("Walk");
                        AudioManager.Get().Play("Run");
                    }

                        if (playerData.characterSpeed < maxSpeedRunning)
                        playerData.characterSpeed += speedMultiplerWhenRun * Time.deltaTime;
                    else
                        playerData.characterSpeed = maxSpeedRunning;
                }
                else
                {
                    if (playerData.characterSpeed > originalSpeed)
                        playerData.characterSpeed -= speedMultiplerWhenRun * Time.deltaTime;
                    else
                    {
                        playerData.characterSpeed = originalSpeed;

                        if (AudioManager.Get() != null && movementVec != Vector3.zero)
                        {
                            AudioManager.Get().Play("Walk");
                            AudioManager.Get().Stop("Run");
                        }
                    }
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
                if (AudioManager.Get() != null)
                    AudioManager.Get().Play("Pushed");

                ReceiveDamage(5);
            }
        }
    }
}