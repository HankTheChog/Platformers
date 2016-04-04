using UnityEngine;
using System.Collections;

public class BluePlayerScript : BasicPlayer {

    private float prev_h;
    
    // Blue needs link to red, to pull him
    private GameObject other_player;
    private RedPlayerScript other_player_script;
    private Rigidbody2D other_player_rb;


    //   Magnet related
    private float time_for_full_magnet_power = 4f;
    private bool I_am_pulling_with_magnet;
    private bool in_anti_magnet_field;
    private float start_time_for_magnet;
    private bool on_cooldown = false;
    private float magnet_cooldown_time = 2f;

    // The input buttons for Blue
    private const string horizontal = "BlueHorizontal";
    private const string vertical = "BlueVertical";
    private const string magnet_button = "BlueMagnet";

    // Use this for initialization
    void Start () {
        BasicPlayerStart();
        prev_h = 0f;
        body = transform.GetChild(0);
        anim = body.GetComponent<Animator>();
        other_player = GameObject.Find("Red player");

        transform.GetChild(1).GetComponent<Aura>().SetMagnetButton(magnet_button);
        other_player_script = other_player.GetComponent<RedPlayerScript>();
        other_player_rb = other_player.GetComponent<Rigidbody2D>();

        WhoAmI = PlayerType.BLUE;
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw(horizontal);

        handle_walking(h);

        if (h != 0)
        {
            bool facing_right = h > 0;
            change_sprite_to_match_walk_direction(facing_right);
        }

        if (prev_h == 0 && h != 0)
        {
            anim.Play("Walk");
        }
        else if (h == 0 && prev_h != 0)
        {
            anim.Play("Idle");
        }

        prev_h = h;

        check_magnet_pull();
    }

	// Update is called once per frame
	void Update () {
        if (DungeonMaster.paused)
        {
            return; // we don't process any input if game is paused
        }
        float v = Input.GetAxisRaw(vertical);
        handle_jumping(v);

        bool magnet_button_state = Input.GetButton(magnet_button);
        // If starting to pull
        if (I_am_pulling_with_magnet == false && magnet_button_state && MagnetAllowed())
        {
            I_am_pulling_with_magnet = true;
            other_player_script.NotifyAboutMagnet(true);
            start_time_for_magnet = Time.time;
            anim.Play("Blue activate magnet");
        }

        // If pulling, and letting go of the key (or have to cancel magnet for some reason)
        if (I_am_pulling_with_magnet && (magnet_button_state == false || MagnetAllowed() == false))
        {
            I_am_pulling_with_magnet = false;
            other_player_script.NotifyAboutMagnet(false);
            anim.Play("Blue deactivates magnet");
        }
    }


    //*****************************************************************//
    //                             MAGNET                              //
    //*****************************************************************//
    private void check_magnet_pull()
    {
        if (I_am_pulling_with_magnet && MagnetAllowed())
        {
            Vector3 from_him_to_me = (transform.position - other_player.transform.position);
            //float distance = from_him_to_me.magnitude;

            float start_force = Physics2D.gravity.magnitude * 1.2f;
            float final_force = start_force * 1.4f;

            float time_since_activation = (Time.time - start_time_for_magnet);
            float force_now = Mathf.Lerp(start_force, final_force, time_since_activation / time_for_full_magnet_power);

            other_player_rb.AddForce(from_him_to_me.normalized * force_now);
        }
    }
    public override void EnteredAntiMagnet()
    {
        in_anti_magnet_field = true;
        I_am_pulling_with_magnet = false;
    }
    public override void LeavingAntiMagnet()
    {
        in_anti_magnet_field = false;
        StartCoroutine(CooldownMagnet());
    }
    public bool CanTriggerMagnet()
    {
        return !in_anti_magnet_field && !on_cooldown;
    }
    public bool MagnetAllowed()
    {
        if (other_player)
        {
            float distance = (transform.position - other_player.transform.position).magnitude;
            return (CanTriggerMagnet() && other_player_script.CanBeAffectedByMagnet() && distance < GameParameters.magnet_radius);
        }
        // else, other player is dead... :-(
        return false;
    }
    IEnumerator CooldownMagnet()
    {
        float end_time = Time.time + magnet_cooldown_time;
        on_cooldown = true;
        while (Time.time < end_time)
        {
            yield return null;
        }
        on_cooldown = false;
    }
}
