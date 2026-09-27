using UnityEngine;

public class ParticleFollowing : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.5f;
    [SerializeField] private Vector3 velocity = Vector3.zero;

    private void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
    }
}
