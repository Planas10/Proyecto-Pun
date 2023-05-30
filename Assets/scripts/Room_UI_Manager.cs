using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Room_UI_Manager : MonoBehaviour
{
    [SerializeField] private Button createButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private Text createText;
    [SerializeField] private Text joinText;
    [SerializeField] private TextMeshProUGUI messageText;
    public GameObject menu;
    public GameObject matchmaking;



    private void Awake()
    {
        createButton.onClick.AddListener(CreateRoom);
        joinButton.onClick.AddListener(JoinRoom);
    }

    private void CreateRoom()
    {

        Photon_Manager._PHOTON_MANAGER.CreateRoom(createText.text.ToString());

    }

    private void JoinRoom()
    {

        Photon_Manager._PHOTON_MANAGER.JoinRoom(joinText.text.ToString());
    }
    public void SetMessageText(string m) 
    {
        messageText.text = m;
    }
}
