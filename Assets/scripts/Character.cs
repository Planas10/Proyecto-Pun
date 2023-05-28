using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Character : MonoBehaviourPun, IPunObservable
{
    [Header("Stats")]
    [SerializeField]
    private float speed;

    [SerializeField]
    private float jumpForce;

    private Rigidbody2D rb;
    private float desiredMovementAxis = 0f;

    public PhotonView pv;
    private Vector3 enemyPosition = Vector3.zero;
    float direction;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pv = GetComponent<PhotonView>();

        PhotonNetwork.SendRate = 20;
        PhotonNetwork.SerializationRate = 20;

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
        rb.velocity = new Vector2(desiredMovementAxis * Time.fixedDeltaTime * speed, rb.velocity.y);

    }

    //Movimiento jugadores
    private void CheckInputs()
    {
        desiredMovementAxis = Input.GetAxisRaw("Horizontal");
        direction = Mathf.Sign(desiredMovementAxis);
        if (Input.GetButtonDown("Jump") && Mathf.Approximately(rb.velocity.y, 0f))
        {
            rb.AddForce(new Vector2(0f, jumpForce));
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
        bala.GetComponent<Bullet>().StartMoving(direction);
    }


    public void Damage() 
    {
        pv.RPC("NetworkDamage", RpcTarget.All);
    }
    [PunRPC]
    public void NetworkDamage()
    {
        Destroy(this.gameObject);
    }

}
