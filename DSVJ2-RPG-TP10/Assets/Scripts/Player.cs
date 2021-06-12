using System.Collections.Generic;
using UnityEngine;

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

        [SerializeField] private GameObject collideMelee;
        [SerializeField] private float rangeRangedAttack;

        //Provisorio------------------------
        [SerializeField] private List<GameObject> itemsOnInventory;
        [SerializeField] private ItemCollector myItemController;

        private void Start()
        {
            playerData.attackReady = true;
            playerData.characterAlive = true;
            isOnAir = false;
            gravityValue = -9.8f;
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
                        break;
                    case CharacterData.AttackType.Ranged:

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
                                Debug.Log("Golpeo Algo");
                                Rigidbody bodyEnemy = hitInfo.collider.GetComponent<Rigidbody>();
                                if (bodyEnemy != null)
                                    bodyEnemy.AddExplosionForce(5, hitInfo.point, 3, 2, ForceMode.Impulse);
                            }
                        }
                        break;
                }
                playerData.attackReady = false;
            }
            else
                Debug.DrawRay(rayMouseCamera.origin, rayMouseCamera.direction * rangeRangedAttack, Color.white);
        }
        public void ReciveDamage(int damageTaken)
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
                //End game
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
                playerVelocity.y = Mathf.Sqrt(jumpPower * -3.0f * gravityValue);

            ApplyGravity();
        }
        void MovePlayer()
        {
            if (playerData.attackReady)
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                movementVec = new Vector3(horizontal, movementVec.y, vertical);
                Vector3 movementRotated = Quaternion.AngleAxis(0, Vector3.up) * movementVec;

                if (movementVec != Vector3.zero)
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementRotated.normalized), rotateSpeed);

                playerMovement.Move(movementVec * playerData.characterSpeed * Time.deltaTime);

                Jump();
            }
        }
    }
}