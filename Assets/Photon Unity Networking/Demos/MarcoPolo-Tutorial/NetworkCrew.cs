using UnityEngine;

public class NetworkCrew : Photon.MonoBehaviour
{
    private Vector3 correctPlayerPos = Vector3.zero; // We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; // We lerp towards this

	string roomName = "autumnvr";

	void Start()
	{
		PhotonNetwork.ConnectUsingSettings("1.0");
	}

    // Update is called once per frame
    void Update()
    {
		return;
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
		return;

        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

         //   myThirdPersonController myC = GetComponent<myThirdPersonController>();
         //   stream.SendNext((int)myC._characterState);
        }
        else
        {
            // Network player, receive data
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();

        //    myThirdPersonController myC = GetComponent<myThirdPersonController>();
        //    myC._characterState = (CharacterState)stream.ReceiveNext();
        }
    }

	[PunRPC]
	void Destroy()
	{
		PhotonNetwork.Destroy(gameObject);
	}
}
