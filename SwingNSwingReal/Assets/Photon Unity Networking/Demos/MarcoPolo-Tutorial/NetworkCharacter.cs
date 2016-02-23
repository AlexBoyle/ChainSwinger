

/// <summary>
/// ///////////   DUDE ADD VELOCITY
/// </summary>

using UnityEngine;

public class NetworkCharacter : Photon.MonoBehaviour
{
    private Vector3 realPosition = Vector3.zero; // We lerp towards this
    private Quaternion realRotation = Quaternion.identity; // We lerp towards this
    // Update is called once per frame
    void Update()
    {
		
		if (!photonView.isMine) {
            transform.position = Vector3.Lerp(transform.position, this.realPosition, .3f);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.realRotation, .3f);
        }
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);


            //myThirdPersonController myC = GetComponent<myThirdPersonController>();
           // stream.SendNext((int)myC._characterState);
        }
        else
        {
            // Network player, receive data
			realPosition = (Vector3)stream.ReceiveNext();
			realRotation = (Quaternion)stream.ReceiveNext();

           // myThirdPersonController myC = GetComponent<myThirdPersonController>();
         //   myC._characterState = (CharacterState)stream.ReceiveNext();
        }
    }
}
