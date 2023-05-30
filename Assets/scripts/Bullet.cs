using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    [SerializeField]
    private float speed = 10f;
    private Rigidbody2D rb;
    public int playerId;
    private PhotonView pv;
    public float damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
    }
    public void StartMoving(float dir) 
    {
        rb.velocity = new Vector2(speed * dir, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character c = collision.gameObject.GetComponent<Character>();
        if (c != null) 
        {
            if (playerId == c.pv.ViewID || playerId == 0)
                return;

            collision.gameObject.GetComponent<Character>().Damage();
            pv.RPC("NetworkDestroy", RpcTarget.All);
        }
    }

    [PunRPC]
    public void NetworkDestroy() 
    {
        Destroy(this.gameObject);
    }

}
