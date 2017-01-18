using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementspeed = 3.0f;
    public float mouseSensitivity = 3.0f;
    public float verticalMouseRange = 60.0f;
    public float jumpspeed = 4.0f;
    public float maxStamina = 5.0f;
    public float runSpeed = 6.0f;

    float verticalRotation = 0;
    float verticalVelocity = 0;
    float stamina = 5;
    float walkSpeed, vert;

    bool isRunning;

    Rect staminaRect;
    Texture2D staminaTexture;
    public CharacterController cc;
    // Use this for initialization
    void Start()
    {
        cc = GetComponent<CharacterController>();
        walkSpeed = movementspeed;
        staminaRect = new Rect(Screen.width / 10, Screen.height * 9 / 10, Screen.width / 3, Screen.height / 50);
        staminaTexture = new Texture2D(1, 1);
        staminaTexture.SetPixel(0, 0, Color.white);
        staminaTexture.Apply();
    }

    // Update is called once per frame
    void Update()
    {
        #region move
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, mouseX, 0);

        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation = Mathf.Clamp(verticalRotation, -verticalMouseRange, verticalMouseRange);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        float forwardSpeed = Input.GetAxis("Vertical") * movementspeed;
        float sideSpeed = Input.GetAxis("Horizontal") * movementspeed;
        verticalVelocity += Physics.gravity.y * Time.deltaTime;

        Vector3 fspeed = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);

        fspeed = transform.rotation * fspeed;
        cc.Move(fspeed * Time.deltaTime);
        #endregion

        /*jump*/
        if (cc.isGrounded && Input.GetButtonDown("Jump"))
            verticalVelocity = jumpspeed;

        /*run*/
        if (Input.GetButtonDown("Run"))
            SetRunning(true);
        if (Input.GetButtonUp("Run"))
            SetRunning(false);

        if (isRunning)
        {
            stamina -= Time.deltaTime;
            if (stamina < 0)
            {
                stamina = 0;
                SetRunning(false);
            }
        }
        else if (stamina < maxStamina)
        {
            stamina += Time.deltaTime;
        }

    }

    void SetRunning(bool isRunning)
    {
        this.isRunning = isRunning;
        movementspeed = isRunning ? runSpeed : walkSpeed;
    }

    void OnGUI()
    {
        float ratio = stamina / maxStamina;
        float rectWidth = ratio * Screen.width / 3;
        staminaRect.width = rectWidth;
        GUI.DrawTexture(staminaRect, staminaTexture);
    }
}
