using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Room camera
    [SerializeField] private float speed;
    private float currentPosX;
    private Vector3 velocity = Vector3.zero;

    //Follow player
    /*
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance;
    [SerializeField] private float cameraSpeed;
    private float lookAhead;*/

    private void Update()
    {
        //Room camera
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed);
        //1 current pos ,2 destination 3, velocity , 4 speed

        //Follow player
        //transform.position = new Vector3(player.position.x + lookAhead, transform.position.y, transform.position.z);
        //lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * player.localScale.x), Time.deltaTime * cameraSpeed);
    }

    //change destination of the camera
    public void MoveToNewRoom(Transform _newRoom)
    {
        currentPosX = _newRoom.position.x;
    }
}