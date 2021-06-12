using UnityEngine;

namespace CameraFollowScript
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] public Transform lookAtThat;

        [SerializeField] [Range(0.5f, 9)] public float speedFollow;
        [SerializeField] [Range(3, 10)] public float zoomDistance;
        [SerializeField] [Range(2, 8)] public float zoomUp;
        [SerializeField] public bool lookAtPlayer;
        private Vector3 zoom;

        private Vector3 posToMoveTowards;

        private void Awake()
        {
            lookAtPlayer = true;
        }
        public void LookAtPlayer()
        {
            transform.LookAt(lookAtThat, lookAtThat.up);
        }
        void LateUpdate()
        {
            FocusToTargetAndMove();
        }
        public void FocusToTargetAndMove()
        {
            Vector3 myPos = transform.position;

            zoom = new Vector3(0, zoomUp, -zoomDistance);

            if (lookAtThat != null)
            {
                posToMoveTowards = lookAtThat.position + zoom;

                if (lookAtPlayer)
                    LookAtPlayer();

                transform.position = Vector3.Lerp(myPos, posToMoveTowards, Vector3.Distance(myPos, posToMoveTowards) * Time.deltaTime * speedFollow);
            }
        }
    }
}
