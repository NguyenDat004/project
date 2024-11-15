using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSlot : MonoBehaviour
{
    public GameObject[] spriteRenderer;
    private SpriteRenderer characterSlot;
    // Start is called before the first frame update
    void Start()
    {
        characterSlot=GetComponent<SpriteRenderer>();
        if (SetCharacter.selectedCharacterSprite=="Assassin"){
            characterSlot.sprite=spriteRenderer[0].GetComponent<SpriteRenderer>().sprite;
        }
        else if (SetCharacter.selectedCharacterSprite=="Cowboy"){
            characterSlot.sprite=spriteRenderer[1].GetComponent<SpriteRenderer>().sprite;
        }
        else if (SetCharacter.selectedCharacterSprite=="Madman"){
            characterSlot.sprite=spriteRenderer[2].GetComponent<SpriteRenderer>().sprite;
        }
        else if (SetCharacter.selectedCharacterSprite=="Robot"){
            characterSlot.sprite=spriteRenderer[3].GetComponent<SpriteRenderer>().sprite;
        }
        Debug.Log("Character Sprite set: "+SetCharacter.selectedCharacterSprite+", "+characterSlot.sprite.name);


    }

}
