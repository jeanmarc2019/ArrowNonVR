using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerListItemScript : MonoBehaviourPunCallbacks
{

    [SerializeField] TMP_Text text;
    Player player;
    public bool delete = true;
    public void SetUp(Player _player) {
        player = _player;
        text.text = _player.NickName;
        delete = false;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer) {
        if (player == otherPlayer) {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom() {
        Destroy(gameObject);
    }
}
