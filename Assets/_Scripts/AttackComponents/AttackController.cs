using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackController
{

    private float attackRadius;
    private float health;
    private float attackDamage;
    //private float attackCooldown;
    private float attackCooldownMin = 2;
    private float attackCooldownMax;
    private float timeUntilNextAttack = 0;

    private UnitController attackTarget = null;
    // TODO: set attackTimeLength to length of animation or vice versa
    private float attackTimeLength = 0.7f;
    private float attackingETA = 0;

    abstract public bool enemyInRange();
    abstract public bool attackReady();
    abstract public void startAttack(UnitController target);
    

}
