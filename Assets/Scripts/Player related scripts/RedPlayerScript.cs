using UnityEngine;
using System.Collections;

public class RedPlayerScript : BasicPlayer {

    private float prev_h;
    // Platform mode
    private bool in_platform_mode = false;
    private GameObject human_body;
    private GameObject platform_body;

    // The input buttons for Red
    private const string horizontal = "RedHorizontal";
    private const string vertical = "RedVertical";
    private const string platformize = "RedTransform";

    // Magnet related
    private bool I_am_being_pulled_by_magnet;
    private bool in_anti_magnet_field;

    // Use this for initialization
    void Start () {
        BasicPlayerStart();
        WhoAmI = PlayerType.RED;
        anim = GetComponent<Animator>(); //override what BasicPlayerScript set
        prev_h = 0f;

        foreach (Transform childT in transform)
        {
            GameObject child = childT.gameObject;
            if (child.name == "HumanCollider")
                human_body = child;
            if (child.name == "PlatformCollider")
                platform_body = child;
        }
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw(horizontal);

        if (!in_platform_mode)
        {
            handle_walking(h);

            if (h != 0)
            {
                bool facing_right = h < 0;
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
        }
    }

    // Update is called once per frame
    void Update () {
        if (DungeonMaster.paused)
        {
            return; // we don't process any input if game is paused
        }

        float v = Input.GetAxisRaw(vertical);
        if (!in_platform_mode)
        {
            handle_jumping(v);
        }

        if (Input.GetButtonDown(platformize))
        {
            TransformShape();
        }
    }

    public override void EnteredAntiMagnet()
    {
        in_anti_magnet_field = true;
        if (I_am_being_pulled_by_magnet)
        {
            rb.velocity = Vector2.zero;
        }
        I_am_being_pulled_by_magnet = false;
    }
    public override void LeavingAntiMagnet()
    {
        in_anti_magnet_field = false;
    }
    public bool CanBeAffectedByMagnet()
    {
        return !in_anti_magnet_field && !in_platform_mode;
    }
    public void NotifyAboutMagnet(bool state)
    {
        I_am_being_pulled_by_magnet = state;
    }


    public void TransformShape()
    {
        GetComponent<AudioSource>().Play();
        if (!in_platform_mode)
        {
            in_platform_mode = true;
            rb.isKinematic = true; // gravity doesn't apply if we're kinematic
            anim.Play("Orange turning to platform");
        }
        else
        {
            anim.Play("Orange turning back into player");
            // don't change in_platform_mode and isKinematic, wait for animation to end
        }
    }

    public void TurnedIntoHuman()
    {
        in_platform_mode = false;
        rb.isKinematic = false;
        human_body.SetActive(true);
        platform_body.SetActive(false);

    }
    public void TurnedIntoPlatform()
    {
        human_body.SetActive(false);
        platform_body.SetActive(true);
    }
}
