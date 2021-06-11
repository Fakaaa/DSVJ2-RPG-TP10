using UnityEngine;

namespace PlayerScript
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private CharacterController playerMovement;
        [SerializeField] private float speed;
        [SerializeField] Vector3 movementVec;
        [SerializeField] Vector3 playerVelocity;
        [SerializeField] private float rotateSpeed;

        [SerializeField] private bool isGrounded;
        [SerializeField] private float jumpPower;

        [SerializeField] private bool isOnAir;
        private float gravityValue;

        private void Start()
        {
            isOnAir = false;
            isGrounded = playerMovement.isGrounded;
            gravityValue = -9.8f;
        }
        void ApplyGravity()
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
            playerMovement.Move(playerVelocity * Time.deltaTime);
        }
        void Jump()
        {
            isGrounded = playerMovement.isGrounded;

            if (isGrounded && playerVelocity.y < 0 && isOnAir)
            {
                isOnAir = false;
                playerVelocity.y = 0;
            }

            if (Input.GetKey(KeyCode.Space) && !isOnAir)
            {
                playerVelocity.y += Mathf.Sqrt(jumpPower * -3.0f * gravityValue);
            }

            if(playerVelocity.y >= jumpPower)
                isOnAir = true;

            ApplyGravity();
        }
        void MovePlayer()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            movementVec = new Vector3(horizontal, movementVec.y, vertical);
            Vector3 movementRotated = Quaternion.AngleAxis(0, Vector3.up) * movementVec;
            //Debug.Log(Camera.main.transform.eulerAngles.y)

            if (movementVec != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementRotated.normalized), rotateSpeed);

            playerMovement.Move(movementVec * speed * Time.deltaTime);

            Jump();
        }

        void Update()
        {
            MovePlayer();
        }
    }
}