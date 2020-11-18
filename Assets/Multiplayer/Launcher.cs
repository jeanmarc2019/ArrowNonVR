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
    [SerializeField] GameObject roomListItemPrefab;
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
    }

    public override void OnJoinedLobby()
    {
        MenuManager.Instance.OpenMenu("title");
        Debug.Log("Joined lobby");
    }

    public void CreateRoom() {
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnJoinedRoom() {
        MenuManager.Instance.OpenMenu("room");
        roomNameText.text = "Room: " +  PhotonNetwork.CurrentRoom.Name;
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
        foreach(Transform trans in roomListContent) {
            Destroy(trans.gameObject);
        }
        for(int i = 0; i < roomList.Count; i++) {
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItemScript>().SetUp(roomList[i]);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
