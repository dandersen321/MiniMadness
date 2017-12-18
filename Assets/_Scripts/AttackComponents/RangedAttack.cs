using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : AttackController
{

    public override bool enemyInRange()
    {
        return true;
    }
    public override bool attackReady()
    {
        return true;
    }
    public override void startAttack(UnitController target)
    {
        fireProjectile(target.transform.position);        
    }

    public void fireProjectile(Vector3 position)
    {

    }

}
