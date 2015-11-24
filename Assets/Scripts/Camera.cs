using UnityEngine;

namespace Assets.Scripts
{
    public class Camera : MonoBehaviour
    {
        [SerializeField] private Transform redPlayer;
        [SerializeField] private Transform bluePlayer;

        [SerializeField] private float minX;
        [SerializeField] private float maxX;

        private void LateUpdate()
        {
            Vector3 midPoint = (redPlayer.position + bluePlayer.position) * 0.5f;

            transform.position = Vector3.Lerp(transform.position, 
                new Vector3(Mathf.Clamp(midPoint.x, minX, maxX), transform.position.y, -1), 
                5 * Time.deltaTime);
        }
    }
}