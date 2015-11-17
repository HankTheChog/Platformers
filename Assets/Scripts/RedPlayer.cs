using UnityEngine;
using System.Collections;

public class RedPlayer : Player
{
    // Use this for initialization
    public void Start()
    {
        base.initialize("RedJump", "RedHorizontal");
    }
}
