using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviourPunCallbacks
{

    #region Public Fields

    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;
    public Transform spawnPoint;


    //public static Manager Instance;

    #endregion

    #region Public Methods

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
           Debug.Log("return to lobby");
           UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
            return;
        }
        //Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
        // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate



        PhotonNetwork.Instantiate(this.playerPrefab.name, spawnPoint.position, Quaternion.identity, 0);


        //else
        //{

        //Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);


    }

    public void LeaveRoom()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

    #endregion
}



