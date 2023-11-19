using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aluba : EnemyProperties
{
    private void Update()
    {
        PatrolMovement();
        //CheckFlip();
    }
}
