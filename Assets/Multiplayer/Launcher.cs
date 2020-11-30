using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;
    // Start is called before the first frame update

    void Awake(){
        Instance = this;
    }

    void Start()
    {
        // tutorial set the loading screen to open explicitly, but this is probably cleaner
        MenuManager.Instance.OpenMenu("loading");
        bool connected = PhotonConnect();
        if (!connected) {
            Debug.Log("I fear something terrible has happened");
        }
    }

    public bool PhotonConnect()
    {
        Debug.Log("Connecting to Master");
        return PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("title");
        PhotonNetwork.NickName = "Player " + UnityEngine.Random.Range(0,1000).ToString("0000");
        Debug.Log(PhotonNetwork.NickName + " joined lobby");
    }

    public void CreateRoom() {
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("loading");
    }

    public void StartGame() {
        PhotonNetwork.LoadLevel(3);
    }

    public override void OnJoinedRoom() {
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = "Room: " +  PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;
        for(int i = 0; i < players.Length; i++) {
            try
            {
                Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItemScript>().SetUp(players[i]);
            }
            catch (NullReferenceException e) when (players[i].IsInactive) {
                Debug.Log("Player is removed");
            }
            catch (NullReferenceException e) when (!players[i].IsInactive) {
                Debug.Log("Player is added");
            }
        }

        foreach(Transform trans in playerListContent) {
            if (trans.GetComponent<PlayerListItemScript>().delete) {
                Destroy(trans.gameObject);
            }
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient) {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message) {
        MenuManager.Instance.OpenMenu("error");
        errorText.text = "Error " + returnCode + ": " + message;
    }

    public void LeaveRoom() {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    public void JoinRoom(RoomInfo info) {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("loading");
    }
//    //this probably isn't needed as OnConnectedToMaster() is called next by default, but if we wanted to do something here, we can
//    public override void OnLeftRoom() {
//        MenuManager.Instance.OpenMenu("title");
//    }
//
    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        for(int i = 0; i < roomList.Count; i++) {
            try
            {
                Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItemScript>().SetUp(roomList[i]);
            }
            catch (MissingReferenceException e) when (roomList[i].RemovedFromList) {
                Debug.Log("Room is removed");
            }
            catch (MissingReferenceException e) when (!roomList[i].RemovedFromList) {
                Debug.Log("Room is created");
            }
        }
        // deleting the default room name button should be done after everything is loaded
        // another solution: hide the default room name button so that the other room buttons are made
        //      but the default one is not shown
        foreach(Transform trans in roomListContent) {
            if (trans.GetComponent<RoomListItemScript>().delete) {
                Destroy(trans.gameObject);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItemScript>().SetUp(newPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
