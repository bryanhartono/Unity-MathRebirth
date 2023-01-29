using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int level;
    public int currency;
    public float curHP;
    public string trait;
    public string scene;
    public string[] inventory;
    public int[] stageList;
    public string skill1;
    public string skill2;
    public float BGM;
    public float SFX;

    public PlayerData(GlobalControl player)
    {
        level = player.LevelGet();
        trait = player.TraitGet();
        scene = player.SceneGet();
        curHP = player.playerCurrentHP;
        currency = player.playerCurrency;
        inventory = player.InventoryGet();
        stageList = player.stageList;
        skill1 = player.skill1;
        skill2 = player.skill2;
        BGM = player.musicVol;
        SFX = player.soundVol;
    }
}
