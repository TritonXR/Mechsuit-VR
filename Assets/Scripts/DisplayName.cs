using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class DisplayName : MonoBehaviour
{

    // Don't use any Steam functions inside Awake as SteamManager is initialized in Awake

    // Start is called before the first frame update
    void Start()
    {
        if (!SteamManager.Initialized) {
            Debug.Log("SteamManager not initialized");
            return;
        }

        Debug.Log("SteamManager successfully initialized");
        Debug.Log(SteamFriends.GetPersonaName());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
