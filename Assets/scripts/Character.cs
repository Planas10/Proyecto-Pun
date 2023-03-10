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

    private PhotonView pv;
    private Vector3 enemyPosition = Vector3.zero;

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

        if (Input.GetButtonDown("Jump") && Mathf.Approximately(rb.velocity.y, 0f))
        {
            rb.AddForce(new Vector2(0f, jumpForce));
        }

        //codigo pium pium
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            MuleMule();
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
    private void MuleMule() {
        PhotonNetwork.Instantiate("Bullet", transform.position + new Vector3(1f, 0f, 0f), Quaternion.identity);
    }


    public void Damage() {
        pv.RPC("NetworkDamage", RpcTarget.All);
    }

    private void NetworkDamage() {
        Destroy(this.gameObject);
    }

}
