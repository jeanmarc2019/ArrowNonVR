using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManagerScript : MonoBehaviour
{
    PhotonView PV;
    void Awake() {
        PV = GetComponent<PhotonView>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if (PV.IsMine) {
            CreateController();
        }
    }

    void CreateController()
    {
        Vector3 startPos = new Vector3(0,1,0);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerCharacterController"), startPos, Quaternion.identity);
    }
}
