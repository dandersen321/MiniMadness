using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFactory : MonoBehaviour {

    public GameObject archerArcRender;

    public PlayerManager createLevel(int levelNumber)
    {
        Level level = new Level();
        level.initLevel(levelNumber);
        GameObject archerAbilityRender = GameObject.Find("ArcherAbilityArcRender");
        ArcherAbility archerAbility = new ArcherAbility(archerAbilityRender.GetComponent<LineRenderer>());
        PlayerManager playerManager = new PlayerManager(archerAbility);
        return playerManager;
    }

}
