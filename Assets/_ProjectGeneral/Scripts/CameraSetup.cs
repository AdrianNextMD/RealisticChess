using UnityEngine;
using DG.Tweening;
using System.Collections;

public class CameraSetup : MonoBehaviour
{
    public TeamColor currentTeam;
    
    public Vector3 whitePosition = new Vector3(0f, 17f, 12f);
    public Vector3 blackPosition = new Vector3(0f, 17f, -12f);

    public void Awake()
    {
        StaticActions.shopPositionCamera += ChangeCameraPosition;
        StaticActions.setInitialPositionCamera += SetCameraToInitialPosition;
    }

    private void Start()
    {
        whitePosition = Camera.main.transform.position;
    }

    public void SetupCamera(TeamColor team)
    {
        currentTeam = team;
        if (team == TeamColor.White)
        {
            //WhiteFlipCamera();
        }
        else
        {
            //BlackFlipCamera();
        }
    }

    private void WhiteFlipCamera()
    {
        transform.localPosition = new Vector3(transform.position.x, transform.position.y, -12);
        //mainCamera.transform.Rotate(Vector3.up, 180f, Space.World);
        Vector3 newRotation = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        transform.eulerAngles = newRotation;
    }

    private void BlackFlipCamera()
    {
        transform.localPosition = new Vector3(transform.position.x, transform.position.y, 12);
        //mainCamera.transform.Rotate(Vector3.up, -180f, Space.World);
        Vector3 newRotation = new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z);
        transform.eulerAngles = newRotation;
    }

    private void ChangeCameraPosition()
    {
        transform.DOKill();
        if(currentTeam == TeamColor.White)
            transform.DOMove(new Vector3(0f, 17f, -16f), 3f);
        
        if(currentTeam == TeamColor.Black)
            transform.DOMove(new Vector3(0f, 17f, 16f), 3f);
    }

    private void SetCameraToInitialPosition()
    {
        transform.DOKill();
        if(currentTeam == TeamColor.White && Camera.main.transform.position != whitePosition)
            transform.DOMove(whitePosition, 1.5f); 
        
        if(currentTeam == TeamColor.Black && Camera.main.transform.position != blackPosition)
            transform.DOMove(blackPosition, 1.5f);
    }
    
    public void OnDestroy()
    {
        StaticActions.shopPositionCamera -= ChangeCameraPosition;
        StaticActions.setInitialPositionCamera -= SetCameraToInitialPosition;
    }
}
