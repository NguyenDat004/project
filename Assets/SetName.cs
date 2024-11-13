using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class SetName : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Get the text from SetCharacter.namePlayerText and set it to the TMP_Text component
        this.GetComponent<TMP_Text>().text = SetCharacter.playerNameText;
    }
}
