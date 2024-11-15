using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndSceneCode : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject namePlayer;

    void Start()
    {
        // Find the game object with the "Player" tag
        namePlayer = GameObject.FindGameObjectWithTag("Player");

        if (namePlayer != null)
        {
            // Set the text of the TMP_Text component in the children of this game object
            TMP_Text textComponent = this.GetComponentInChildren<TMP_Text>();
            if (textComponent != null)
            {
                textComponent.text = namePlayer.name + " is the winner";
            }
            else
            {
                Debug.LogError("TMP_Text component not found in children of this game object.");
            }
        }
        else
        {
            Debug.LogError("No GameObject with the tag 'Player' was found.");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            Destroy(gameObject);
        }

    }
}
