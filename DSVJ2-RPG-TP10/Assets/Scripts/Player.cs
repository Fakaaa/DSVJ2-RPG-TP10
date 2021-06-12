﻿using UnityEngine;
using GameManagerScript;

namespace PlayerScript
{
    public class Player : MonoBehaviour, IHittable<int>, IAttack
    {
        [SerializeField] private CharacterController playerMovement;
        [SerializeField] Vector3 movementVec;
        [SerializeField] Vector3 playerVelocity;
        [SerializeField] private float rotateSpeed;

        private float gravityValue;

        [SerializeField] private bool isGrounded;
        [SerializeField] private float jumpPower;
        [SerializeField] private bool isOnAir;

        [SerializeField] public CharacterData playerData;

        [SerializeField] private GameObject collideMelee;

        private void Start()
        {
            playerData.attackReady = true;
            playerData.characterAlive = true;
            isOnAir = false;
            isGrounded = playerMovement.isGrounded;
            gravityValue = -9.8f;
        }
        void Update()
        {
            MovePlayer();
            CheckIfIsOnGround();
            Attack();
        }
        public void Attack()
        {
            if (!playerData.attackReady)
                playerData.coldownAttack += Time.deltaTime;

            if(playerData.coldownAttack >= playerData.resetColdown && !playerData.attackReady)
            {
                collideMelee.SetActive(false);
                playerData.attackReady = true;
                playerData.coldownAttack = 0;
            }

            if (Input.GetButton("Fire1") && playerData.attackReady)
            {
                switch (playerData.actualAttackType)
                {
                    case CharacterData.AttackType.Melee:
                        collideMelee.SetActive(true);
                        break;
                    case CharacterData.AttackType.Ranged:
                        //Raycast
                        break;
                }
                playerData.attackReady = false;
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
                playerVelocity.y = Mathf.Sqrt(jumpPower * -3.0f * gravityValue);

            ApplyGravity();
        }
        void MovePlayer()
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("MeleeAttackEnemy"))
            {             
                ReceiveDamage(5);
            }
        }
    }
}