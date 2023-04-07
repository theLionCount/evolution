using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : CellBase
{
    public float lightConversionRate = 0.03f;
    World world;

    // Start is called before the first frame update
    void Start()
    {
        world = GameObject.Find("World").GetComponent<World>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        host.feed(world.lightLevel * lightConversionRate);
    }
}
