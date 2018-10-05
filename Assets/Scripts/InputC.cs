using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.XR;

[Serializable]

public class InputC
{
    public float XSensitivity = 2;
    public float YSensitivity = 2;
    public float MinimumX = -80F;
    public float MaximumX = 80F;
    public bool smooth;
    public float smoothTime = 5f;
    public bool lockCursor = true;
    public int Cycle = 200;
    public float mini = 0.5f;

    private Camera m_Camera;
    private Quaternion previousX;
    private Quaternion previousY;
    private Quaternion m_CameraTargetRot;
    private Quaternion m_CharacterTargetRot;
    private bool m_cursorIsLocked = true;
    private int prey = 0;
    private int asy = 0;
    private int cnty = 0;
    private float preRotx = 0;
    private float preRoty = 0;
    private bool start = false;

    public void Init(Transform character, Transform camera)
    {
        m_CharacterTargetRot = character.localRotation;
        m_Camera = Camera.main;
        m_CameraTargetRot = camera.localRotation;
        previousX = Quaternion.Euler(0f, 0f, 0f);
    }

    public void LookRotation(Transform character, Transform camera)
    {
        float yRot;
        float xRot;
        //VR
        if (XRDevice.isPresent)
        {
            float dirx, diry;
            InputTracking.disablePositionalTracking = true;
            XRDevice.DisableAutoXRCameraTracking(m_Camera, true);
            Quaternion input = InputTracking.GetLocalRotation(XRNode.CenterEye);
            xRot = input.eulerAngles.x;
            yRot = input.eulerAngles.y;
            if (!start && yRot != 0)
            {
                preRotx = xRot;
                preRoty = yRot;
                start = true;
            }
            yRot -= preRoty;
            if (yRot < 0) yRot += 360;
            xRot -= preRotx;
            if (xRot < 0) xRot += 360;
            if (xRot > 15 && xRot < 90)
                dirx = mini * 0.5f;
            else if (xRot > 270 && xRot < 345)
                dirx = 0 - mini * 0.5f;
            else
                dirx = 0;
            if (yRot > 20 && yRot < 180)
                diry = mini * 0.5f;
            else if (yRot > 180 && yRot < 340)
                diry = 0 - mini * 0.5f;
            else
                diry = 0;
            m_CameraTargetRot *= Quaternion.Euler(dirx, 0f, 0f);
            m_CharacterTargetRot *= Quaternion.Euler(0f, diry, 0f);
        }

        // Mouse and Trackpad
        else
        {
            yRot = CrossPlatformInputManager.GetAxis("Mouse X");
            xRot = CrossPlatformInputManager.GetAxis("Mouse Y");

            m_CameraTargetRot *= Quaternion.Euler((0f - xRot) * 2, 0f, 0f);

			//Rotation around axis y
			if (yRot * asy > 0)
				yRot = 0f;
			else
				asy = 0;

			if (yRot < 0.1 && yRot > -0.1)
				yRot = 0;

			if (cnty < Cycle) {
				m_CharacterTargetRot *= previousX;
				cnty++;
			}
			if (cnty == Cycle)
				prey = 0;
			if (yRot > 0 && prey < 1) {
				asy = 1;
				prey++;
			} else if (yRot < 0 && prey > -1) {
				asy = -1;
				prey--;
			}
			if (yRot != 0) {
				previousX = Quaternion.Euler (0f, prey * mini, 0f);
				cnty = 0;
			}
        }

        m_CameraTargetRot = ClampRotationAroundXAxis(m_CameraTargetRot);

        if (smooth)
        {
            camera.localRotation = Quaternion.Slerp(camera.localRotation, m_CameraTargetRot,
                smoothTime * Time.deltaTime);
            character.localRotation = m_CharacterTargetRot;
            character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
             smoothTime * Time.deltaTime);
        }
        else
        {
            camera.localRotation = m_CameraTargetRot;
            character.localRotation = m_CharacterTargetRot;
        }

        UpdateCursorLock();
    }

    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if (!lockCursor)
        {//we force unlock the cursor if the user disable the cursor locking helper
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        //if the user set "lockCursor" we check & properly lock the cursos
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_cursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_cursorIsLocked = true;
        }

        if (m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

}