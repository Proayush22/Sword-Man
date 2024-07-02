using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;
    public GameObject playerPrefab;
    public GameObject playerPrefab2;
    public PhotonView view;

    public TMP_Text text;
    public GameObject panel;

    public GameObject escape;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        PhotonNetwork.ConnectUsingSettings();

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!escape.activeSelf)
            {
                escape.SetActive(true);
            }
            else
            {
                escape.SetActive(false);
            }
        }
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            gameObject.SetActive(false);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Room Created");
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
        panel.gameObject.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        panel.gameObject.SetActive(false);
        /*if there is one player do this and if there is two players do this*/
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(-5f, 5f, 0f), Quaternion.identity, 0);
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.Instantiate(this.playerPrefab2.name, new Vector3(5f, 5f, 0f), Quaternion.identity, 0);
        }
        PhotonNetwork.NickName = text.text;
    }

    public void Respawn()
    {
        PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
    }

    public void endGame()
    {
        Application.Quit();
    }
}


