using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : Object {

    public bool active;
    public float cooldown;
    public float nextActiveTime;

    public Ability()
    {
        active = false;
    }

    // used when an ability is interrupted
    public void stopAbility()
    {
        active = false;
    }

    // should be called when an ability is finished
    protected void finishAbility()
    {
        active = false;
        nextActiveTime = Time.time + cooldown;
    }



    public virtual void startAbility()
    {
        active = true;
    }
    
    public abstract void update();
    
}
