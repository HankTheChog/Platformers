using UnityEngine;

namespace Assets.Scripts
{
    public class Rope : MonoBehaviour
    {
        [SerializeField] private float elasticity;
        [SerializeField] private float damping_force;
        [SerializeField] private GameObject red;
        [SerializeField] private GameObject blue;

        public static bool is_active = false; // players can check this to see if swinging motion is allowed
        public static float rope_length;

        private LineRenderer line;

        private void Awake()
        {
            line = GetComponent<LineRenderer>();
        }

        private void Update ()
        {
            if (Input.GetButtonDown("ToggleRope"))
            {
                is_active = !is_active;
                line.enabled = is_active;
                blue.GetComponent<SpringJoint2D>().enabled = is_active;
                if (is_active)
                    rope_length = (red.transform.position - blue.transform.position).magnitude;
            }

            if (is_active)
            {
                line.SetPosition(0, red.transform.position);
                line.SetPosition(1, blue.transform.position);
                blue.GetComponent<SpringJoint2D>().distance = rope_length;
            }
        }
    }
}
