using UnityEngine;

namespace Assets.Scripts
{
    public class Rope : MonoBehaviour
    {
        [SerializeField] private float elasticity;
        [SerializeField] private GameObject red_player;
        [SerializeField] private GameObject blue_player;

        public static bool is_active = false; // players can check this to see if swinging motion is allowed
        public static float activate_pull_force_distance; // players can change this when they're climbing
        public static float max_allowed_rope_length; // todo: make this read-only global attribute

        private LineRenderer line;

        private void Awake()
        {
            line = GetComponent<LineRenderer>();
            //line.enabled = false;
        }

        private void FixedUpdate()
        {
            if (is_active)
            {
                float d = (red_player.transform.position - blue_player.transform.position).magnitude;

                // the force of elastic bands is F= -k * dx^2
                float delta = d - activate_pull_force_distance;
                float delta_sqr = delta * delta;
                float force = elasticity * delta_sqr * delta_sqr;
                
                Vector2 blue_to_red = (red_player.transform.position - blue_player.transform.position).normalized;

                float experiment = Mathf.Sign(d - activate_pull_force_distance); // now rope is like spring - pushing when players get too close
                //experiment = 1; // activate this line to make the "pull" players only

                //apply force on both players.
                blue_player.GetComponent<Rigidbody2D>().AddForce(experiment * force * blue_to_red);
                red_player.GetComponent<Rigidbody2D>().AddForce(-experiment * force * blue_to_red);

                //todo: max_allowed_rope_length should be a hard-limit on players' distance from one another
            }
        }
        private void Update ()
        {
            if (Input.GetButtonDown("ToggleRope"))
            {
                is_active = !is_active;
                line.enabled = is_active;
                if (is_active)
                    max_allowed_rope_length = activate_pull_force_distance = (red_player.transform.position - blue_player.transform.position).magnitude;
            }

            if (is_active)
            {
                line.SetPosition(0, red_player.transform.position);
                line.SetPosition(1, blue_player.transform.position);
            }
        }
    }
}
