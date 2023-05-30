using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Register : MonoBehaviour
{
    public TextMeshProUGUI user;
    public TextMeshProUGUI password;
    public TMP_Dropdown race;
    void Start()
    {

    }

    void Update()
    {

    }
    public void Click()
    {
        string racestr = (race.value + 1).ToString();
        Network_Manager._NETWORK_MANAGER.RegisterUser(user.text, password.text,racestr);
    }

}