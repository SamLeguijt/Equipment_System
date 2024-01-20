using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo Clip", menuName = "Equipment/Ammunition/Ammo clip")]
public class AmmoClipObject : AmmunitionEquipmentObject
{


    /* AmmoObject: 
     * We need:
     * 
     * Ammo clip for each weapon, base values to initiate a weapon with 
     * Ammo clip references a type of bullet and its data
     * Ammo clip have types of clips: Shotgun, RPG, AR, pistol etc. 
     * Each type of clip refernces a certain bullet tht matches (Shotgunclip ref to ShotgunBullet prefab, with ShotgunBullet scriptObject bullet)
     * 
     * Also ammo object: 
     * Crates, much more ammo capacit, referncing ammo clips of all? 
     * 
     */
}
