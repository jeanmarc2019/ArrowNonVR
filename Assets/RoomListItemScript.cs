using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Realtime;
using TMPro;

public class RoomListItemScript : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    RoomInfo info;
    public bool delete = true;

    public void SetUp(RoomInfo _info) {
        info = _info;
        text.text = _info.Name;
        delete = false;
    }

    public void OnClick() {
        Launcher.Instance.JoinRoom(info);
    }
}
