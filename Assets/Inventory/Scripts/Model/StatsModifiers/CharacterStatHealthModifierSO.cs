using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStatHealthModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        float maxHP = GameObject.Find("Combat Overlay/Combat_UI/Player").GetComponent<Unit>().maxHP;
        float currentHP = GameObject.Find("Combat Overlay/Combat_UI/Player").GetComponent<Unit>().currentHP;
        if (currentHP + val >= maxHP)
        {
            GameObject.Find("Combat Overlay/Combat_UI/Player").GetComponent<Unit>().currentHP = maxHP;
        }
        else
        {
            GameObject.Find("Combat Overlay/Combat_UI/Player").GetComponent<Unit>().currentHP += val;
        }
    }
}

