using RoR2;
using RoR2.Audio;
using UnityEngine;
using UnityEngine.Networking;

namespace DS1Catacombs
{
    public class PathFollower : MonoBehaviour
    {
        public Transform[] path;
        public float speed = 5f;
        public float reachDist = 1f;

        private int currentPoint = 0;

        void FixedUpdate()
        {
            float dist = Vector3.Distance(path[currentPoint].position, transform.position);

            transform.position = Vector3.MoveTowards(transform.position, path[currentPoint].position, Time.deltaTime * speed);

            if (dist <= reachDist)
            {
                currentPoint++;
            }

            if (currentPoint >= path.Length)
            {
                Destroy(gameObject);
            }
        }
    }
}
