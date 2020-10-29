using System;
using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;
        
        float minFov = 15f;
        float maxFov = 90f;
        float sensitivity = 10f;

        private bool isCameraLocked = true;

        private int theScreenWidth;
        private int theScreenHeight;
        
        public int speed = 50;
        private bool isCamMoving;
        
        void Start() 
        {
            theScreenWidth = Screen.width;
            theScreenHeight = Screen.height;
        }
        
        private void LateUpdate()
        {
            if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
            
            lockUnlockCamera();
            
            if (isCameraLocked)
            {
                transform.position = target.position;
            }
            else
            {
               MoveCam();
            }
            

            float fov = Camera.main.fieldOfView;
            fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
            fov = Mathf.Clamp(fov, minFov, maxFov);
            Camera.main.fieldOfView = fov;


        }
        
        void MoveCam()
        {
            Vector3 camPos = transform.position;
            if (Input.mousePosition.x > theScreenWidth - 30)
            {
                isCamMoving = true;
                camPos.z += speed * Time.deltaTime;
            }
            else if (Input.mousePosition.x < 30)
            {
                isCamMoving = true;
                camPos.z -= speed * Time.deltaTime;
            }
         
            else if (Input.mousePosition.y > theScreenHeight - 30)
            {
                isCamMoving = true;
                camPos.x -= speed * Time.deltaTime;
            }
            else if (Input.mousePosition.y < 30)
            {
                isCamMoving = true;
                camPos.x += speed * Time.deltaTime;
            }
            else
            {
                isCamMoving = false;
            }
            transform.position = camPos ;
        }

        private void lockUnlockCamera()
        {
            if (Input.GetButtonDown("CameraLock"))
            {
                if (isCameraLocked)
                {
                    isCameraLocked = false;
                }
                else
                {
                    isCameraLocked = true;
                }
            }
        }
    }
}

   
