using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogIn : MonoBehaviour
{
    public TextMeshProUGUI user;
    public TextMeshProUGUI password;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void Click() 
    {
        Network_Manager._NETWORK_MANAGER.ConnectToServer(user.text, password.text);
    }
    
}
