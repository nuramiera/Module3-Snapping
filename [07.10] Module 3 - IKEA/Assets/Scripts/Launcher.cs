using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public GameObject ConnectedScreen;
    public GameObject DisconnectScreen;

    string roomName = "My Room";
    bool ShowGUI = true;




    public void OnClick_ConnectBtn()
    {

        PhotonNetwork.ConnectUsingSettings();

    }

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }



    public override void OnConnectedToMaster()
    {
    

        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        DisconnectScreen.SetActive(true);
    }

    public override void OnJoinedLobby()
    {
        if (DisconnectScreen.activeSelf)
            DisconnectScreen.SetActive(false);

        ConnectedScreen.SetActive(true);


    }
}
