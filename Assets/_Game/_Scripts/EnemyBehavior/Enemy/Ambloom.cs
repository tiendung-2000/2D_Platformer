using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambloom : EnemyProperties
{
    private void Update()
    {
        PatrolMovement();
        //CheckFlip();
    }
}
