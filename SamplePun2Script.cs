using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SamplePun2Script : MonoBehaviourPunCallbacks
{
    GameObject human;
    
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Game Room", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        human = PhotonNetwork.Instantiate("刻晴", Vector3.zero, Quaternion.identity, 0);
        var script = human.GetComponent<HumanWalkScript>();
        script.SetFlag(true);
        script.SetMessage(PhotonNetwork.NetworkClientState.ToString());
        Camera.main.GetComponent<HumanCameraScript>().target = human;
    }
}
