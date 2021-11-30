using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MultiFightPun2Script : MonoBehaviourPunCallbacks
{
    GameObject player;
    GameObject enemy;
    Player enemyPlayer;
    Hashtable properties;
    bool flag;
    public Text message;
    public Text nickname;

    public bool IsFlag()
    {
        return flag;
    }
    public void SetFlag(bool f)
    {
        flag = f;
    }

    public void SetMessage(string s)
    {
        message.text = s;
    }

    public void SetPlayer(GameObject ob)
    {
        player = ob;
    }

    public void SetEnemy(GameObject ob)
    {
        enemy = ob;
    }
    public GameObject GetEnemy()
    {
        return enemy;
    }

    public Player GetEnemyPlayer()
    {
        return enemyPlayer;
    }

    public void ButtonOK()
    {
        flag = true;
        nickname.text = GameObject.Find("InputField").GetComponent<InputField>().text;
        GameObject.Find("GuiCamera").GetComponent<Camera>().depth = -1;
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
        PhotonNetwork.LocalPlayer.NickName = nickname.text;
        PhotonNetwork.ConnectUsingSettings();
        StartCoroutine(TimerEnd());
    }

    IEnumerator TimerEnd()
    {
        yield return new WaitForSeconds(3f);
        message.text = "";
    }

    // Use this for initialization
    void Start ()
    {
        properties = new Hashtable();
    }

    void Update ()
    {
        if (!flag){ return; }
        check();
    }

    //Connected to Master
    public override void OnConnectedToMaster() {
        var opt = new RoomOptions();
        opt.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("Game Room", opt, TypedLobby.Default);
    }

    // Joined Room
    public override void OnJoinedRoom(){
        player = PhotonNetwork.Instantiate("unitychan", Vector3.zero, Quaternion.identity, 0);
        player.GetComponent<MultiFightScript>().SetFlag(true);
        player.GetComponentInChildren<Camera>().depth = 10;
        var prps = PhotonNetwork.LocalPlayer.CustomProperties;
        prps["power"] = 100;
        PhotonNetwork.LocalPlayer.SetCustomProperties(prps);
        foreach (var p in PhotonNetwork.PlayerList)
        {
            if (PhotonNetwork.LocalPlayer.UserId != p.UserId)
            {
                enemyPlayer = p;
                break;
            }
        }
    }

    // ルーム入室時の処理
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        enemyPlayer = newPlayer;
    }

    // カスタムプロパティ更新時の処理
    public void OnPlayerPropertiesUpdate(Player targetPlayer, 
        Hashtable changedProps)
    {
        CheckEnemy();
    }

    // 敵GameObjectを調べる
    public void CheckEnemy()
    {
        foreach(var ob in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (ob != player)
            {
                enemy = ob;
                break;
            }
        }
    }

    // 終了のチェックと処理
    public void check()
    {
        if (PhotonNetwork.LocalPlayer == null){ return; }
        if (enemyPlayer == null){ return; }
        var p = PhotonNetwork.LocalPlayer.CustomProperties["power"] is int value ? value : 1;
        var ep = enemyPlayer.CustomProperties["power"] is int value2 ? value2 : 1;
        if (p <= 0)
        {
            player.GetComponent<Animator>().SetBool("loss", true);
            message.text = enemyPlayer.NickName + " is WIN !";
            flag = false;
            player.GetComponent<MultiFightScript>().SetFlag(false);
        }
        if (ep <= 0)
        {
            player.GetComponent<Animator>().SetBool("win", true);
            message.text = PhotonNetwork.LocalPlayer.NickName + " is WIN !";
            flag = false;
            player.GetComponent<MultiFightScript>().SetFlag(false);
        }
    }
}