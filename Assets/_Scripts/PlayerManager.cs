using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum Ability
//{
//    ArcherFire
//}

public class PlayerManager
{
    public Ability currentAbility = null;

    private Ability archerAbility;

    public PlayerManager(ArcherAbility archerAbility)
    {
        this.archerAbility = archerAbility;
        currentAbility = this.archerAbility;
        currentAbility.startAbility();
    }

    public void updateAbility()
    {
        //Debug.Log("updateAbility");
        currentAbility.update();

        if(!currentAbility.active)
        {
            currentAbility = archerAbility;
            currentAbility.startAbility();
        }
    }

    //public void mouseIsDown()
    //{
    //    //if(currentAbility == Ability.ArcherFire)
    //    //{
    //    //    updateArcherTrajectory();
    //    //}

    //    currentAbility.update(Input.GetMouseButton(0));
    //}

    //public void mouseIsUp()
    //{
    //    if (currentAbility == Ability.ArcherFire)
    //    {
    //        archersFire();
    //    }
    //}

    //public void updateArcherTrajectory()
    //{
    //    Debug.Log("update archer trajectory");
    //}

    //public void archersFire()
    //{
    //    Debug.Log("archers, fire!");
    //}


}
