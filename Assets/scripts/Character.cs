using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public struct Stats 
{
    public int maxHp;
    public int currenthp;
    public int damage;
    public int bulletSize;
    public int jumpForce;
    public int speed;
}
public class Character : MonoBehaviourPun, IPunObservable
{
    [Header("Stats")]


    private Rigidbody2D rb;
    public TextMeshProUGUI userText;
    private float desiredMovementAxis = 0f;

    public PhotonView pv;
    private Vector3 enemyPosition = Vector3.zero;
    public Stats stats;
    float direction;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();
        stats.speed = 3;
        PhotonNetwork.SendRate = 20;
        PhotonNetwork.SerializationRate = 20;
        if (pv.IsMine)
            UpdateNames(Network_Manager._NETWORK_MANAGER.playerName);
        if (pv.IsMine)
            UpdateStats(Network_Manager._NETWORK_MANAGER.selectedRaza);
        if(stats.speed == 0)
            stats.speed = 1000;
    }

    private void Update()
    {
        if (pv.IsMine)
        {
            CheckInputs();
        }
        else
        {
            SmoothReplicate();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(desiredMovementAxis * Time.fixedDeltaTime * stats.speed, rb.velocity.y);

    }
     
    //Movimiento jugadores
    private void CheckInputs()
    {
        desiredMovementAxis = Input.GetAxisRaw("Horizontal");
        direction = Mathf.Sign(desiredMovementAxis);
        if (Input.GetButtonDown("Jump") && Mathf.Approximately(rb.velocity.y, 0f))
        {
            rb.AddForce(new Vector2(0f, stats.jumpForce * 100));
        }

        //codigo pium pium
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Shoot();
        }
    }

    private void SmoothReplicate()
    {
        transform.position = Vector3.Lerp(transform.position, enemyPosition, Time.deltaTime * 20);
    }

    //
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

            stream.SendNext(transform.position);

        }
        else if (stream.IsReading)
        {

            enemyPosition = (Vector3)stream.ReceiveNext();
        }
    }

    //Disparo pium para todos los jugadores
    private void Shoot()
    {
        GameObject bala = PhotonNetwork.Instantiate("Bullet", transform.position + new Vector3(direction, 0f, 0f), Quaternion.identity);
        bala.GetComponent<Bullet>().playerId = pv.ViewID;
        bala.transform.localScale *= stats.bulletSize;
        bala.GetComponent<Bullet>().StartMoving(direction);
        bala.GetComponent<Bullet>().damage = stats.damage;
    }


    public void Damage() 
    {
        pv.RPC("NetworkDamage", RpcTarget.All);
    }
    public void UpdateNames(string name)
    {
        pv.RPC("SetPlayerName", RpcTarget.All,name);
    }
    public void UpdateStats(Stats newstats)
    {
        pv.RPC("SetPlayerStats", RpcTarget.All, newstats.maxHp,newstats.damage,newstats.bulletSize,newstats.speed,newstats.jumpForce);
    }
    [PunRPC]
    public void NetworkDamage()
    {
        Destroy(this.gameObject);
    }
    [PunRPC]
    public void SetPlayerName(string name) 
    {
        userText.text = name;
    }
    [PunRPC]
    public void SetPlayerStats(int hp, int damage, int bulletsize, int speed, int jumpforce)
    {
        stats.maxHp = hp;
        stats.currenthp = stats.maxHp;
        stats.bulletSize = bulletsize;
        stats.jumpForce = jumpforce;
        stats.speed = speed;
        stats.damage = damage;
    }
}
