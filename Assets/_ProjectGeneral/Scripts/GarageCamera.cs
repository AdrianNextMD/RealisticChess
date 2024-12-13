using System.Collections;
using System.Collections.Generic;
using Template.Scripts.Utils;
using UnityEngine;

public class GarageCamera : Singleton<GarageCamera>
{
	[Header("Camera Parameters")]
	[Range(0, 1)] public float zoom;
	[Range(0, 1)] public float minZoom = 0.1f;
	[Range(0, 1)] public float maxZoom = 0.99f;

	public float zoom_direction;
	public bool activateZoom;

	public Vector2 mouse_moves;
	public Vector2 moves_final;
	public float delta_distance;
	public float last_distance;
	public float parts_slider_velocity;
	public Transform cam;
	public Vector3 target_camera_anchor;
	public Quaternion target_rot_temp;
	public bool rotation_transition;
	public float transition_timer;

	public Vector3 delta;
	private Vector3 lastPos;

	[Header("Touch")]
	public float touch_delay = 0.1f;
	public Vector2 touched_down_pos, touched_up_pos;

	public bool controlActivated;
	private RuntimePlatform platform;

    private void Start()
    {
		zoom = minZoom;
		platform = Application.platform;
		StartCoroutine(WaitForActivation());
    }

	public IEnumerator WaitForActivation()
    {
		yield return new WaitForSeconds(1f);
		controlActivated = true;
	}

    private void FixedUpdate()
    {
		//if(controlActivated)
  //      {
		//	//PC INPUT
		//	if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.OSXEditor 
		//		|| platform == RuntimePlatform.OSXPlayer || platform == RuntimePlatform.LinuxEditor || platform == RuntimePlatform.LinuxPlayer)
		//	{
		//		if (Input.GetMouseButton(0))
		//		{
		//			mouse_moves = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

		//			moves_final = Vector2.Lerp(moves_final, mouse_moves / 0.6f, 0.15f);
		//			parts_slider_velocity = Mathf.Lerp(parts_slider_velocity, 0f, 0.1f);
		//		}
		//		else
		//		{
		//			moves_final = Vector2.Lerp(moves_final, Vector2.zero, 0.1f);
		//			parts_slider_velocity = Mathf.Lerp(parts_slider_velocity, 0f, 0.1f);
		//		}
		//	}

		//	//MOBILE TOUCH
		//	if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
		//	{
		//		if (Input.touchCount > 0)
		//		{
		//			if (Input.touchCount == 1)
		//			{
		//				mouse_moves = new Vector2(Input.GetTouch(0).deltaPosition.x / 20f, Input.GetTouch(0).deltaPosition.y / 20f);

		//				moves_final = Vector2.Lerp(moves_final, mouse_moves / 1f, 0.15f);
		//				parts_slider_velocity = Mathf.Lerp(parts_slider_velocity, 0f, 0.1f);
		//			}
		//			else
		//			{
		//				moves_final = Vector2.Lerp(moves_final, Vector2.zero, 0.1f);
		//				parts_slider_velocity = Mathf.Lerp(parts_slider_velocity, 0f, 0.1f);
		//			}

		//		}
		//		else
		//		{
		//			moves_final = Vector2.Lerp(moves_final, Vector2.zero, 0.1f);
		//			parts_slider_velocity = Mathf.Lerp(parts_slider_velocity, 0f, 0.1f);
		//		}
		//	}

			if (zoom_direction > 0)
			{
				if (zoom < 1f)
				{
					zoom += zoom_direction / 100f;
				}
			}
			if (zoom_direction < 0)
			{
				if (zoom > 0.6f)
				{
					zoom += zoom_direction / 100f;
				}
			}

        //if (PlayMenu.Main_Menu_Mode)
        //{
        //if (!rotation_transition)
        //{
        //    if (transform.eulerAngles.x <= 180)
        //    {
        //        if (moves_final.y > 0)
        //        {
        //            if (transform.eulerAngles.x > 5)
        //            {
        //                transform.eulerAngles += new Vector3(-moves_final.y, 0f, 0f);
        //            }
        //        }
        //        else
        //        {
        //            if (transform.eulerAngles.x < 50)
        //            {
        //                transform.eulerAngles += new Vector3(-moves_final.y, 0f, 0f);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (moves_final.y > 0)
        //        {
        //            if (transform.eulerAngles.x < 366)
        //            {
        //                transform.eulerAngles += new Vector3(-moves_final.y, 0f, 0f);
        //            }
        //        }
        //        else
        //        {
        //            transform.eulerAngles += new Vector3(-moves_final.y, 0f, 0f);
        //        }
        //    }
        //    transform.localEulerAngles += new Vector3(0f, moves_final.x * 2f, 0f);
        //}
        //else
        //{
        //    transform.rotation = Quaternion.Slerp(transform.rotation, new Quaternion(target_rot_temp.x, target_rot_temp.y, 0f, target_rot_temp.w), minZoom);
        //    transform.eulerAngles = new Vector3(Mathf.Clamp(target_rot_temp.eulerAngles.x, 5f, 50f), transform.eulerAngles.y, 0f);
        //}

        //float x_offset = 0f;
        //float y_offset = 0f;
        //cam.parent = transform;

        //if (PlayMenu.Main_Menu_Mode)
        //{
        //cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, new Vector3(cam.transform.position.x, cam.transform.position.y, -3f / (zoom)), minZoom);
        //cam.localEulerAngles = Vector3.zero;
        //}
        //else
        //{
        //	cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, new Vector3(x_offset, y_offset, -3f / (zoom)), minZoom);
        //}
        //}

        //if(PlayMenu.Online_Menu_Mode)
        //         {
        //	transform.position = new Vector3(1.70f, 0.76f, 1.5f);
        //	transform.rotation = Quaternion.Euler(7, 176, 0);
        //}
        //else
        //         {
        //	transform.position = new Vector3(0f, 0.76f, 1.5f);
        //}
        //}
    }

    private void Update()
    {
		controlActivated = !UiHelper.IsOnUi();
		if (controlActivated)
        {
			//WINDOWS
			if (platform == RuntimePlatform.WindowsEditor || platform == RuntimePlatform.WindowsPlayer || platform == RuntimePlatform.OSXEditor 
				|| platform == RuntimePlatform.OSXPlayer || platform == RuntimePlatform.LinuxEditor || platform == RuntimePlatform.LinuxPlayer)
			{
				#region Zoom In/Out Camera

				if (activateZoom)
				{
					if (zoom < maxZoom)
					{
						if (Input.GetAxis("Mouse ScrollWheel") > 0.01f)
						{
							zoom += Input.GetAxis("Mouse ScrollWheel");
						}
					}
					if (zoom > minZoom)
					{
						if (Input.GetAxis("Mouse ScrollWheel") < -0.01f)
						{
							zoom += Input.GetAxis("Mouse ScrollWheel");
						}
					}

					zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
					cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(cam.transform.position.x, cam.transform.position.y, -3f / (zoom)), minZoom);
				}

                #endregion
            }

            //MOBILE TOUCH
            if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
			{
				#region Zoom In/Out Camera

				if (activateZoom)
				{
					if (Input.touchCount >= 2)
					{
						Touch touchZero = Input.GetTouch(0);
						Touch touchOne = Input.GetTouch(1);

						Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
						Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

						float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
						float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

						float difference = currentMagnitude - prevMagnitude;

						zoom += (difference * minZoom);
						zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
						cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(cam.transform.position.x, cam.transform.position.y, -3f / (zoom)), minZoom);
					}
				}

                #endregion
            }
		}
	}
}
