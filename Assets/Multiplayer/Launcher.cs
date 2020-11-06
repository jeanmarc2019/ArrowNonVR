using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Launcher : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        bool connected = PhotonConnect();
        if (connected) {
            OnConnectToMaster();
        } else {
            Debug.Log("I fear something terrible has happened");
        }
    }

    public bool PhotonConnect()
    {
        Debug.Log("Connecting to Master");
        return PhotonNetwork.ConnectUsingSettings();
    }

    public void OnConnectToMaster()
    {
        Debug.Log("Connected to Master");
        // Might not need this
//        bool lobbyJoined = PhotonNetwork.JoinLobby();
//        Debug.Log(lobbyJoined);
//        if (lobbyJoined) {
//            OnJoinedLobby();
//        }
    }

    public void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
