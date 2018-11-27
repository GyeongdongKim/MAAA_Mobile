using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class VariableJoystick : Joystick
{
    [Header("Variable Joystick Options")]
    public bool isFixed = true;
    //public Vector2 fixedScreenPosition;
    public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
    public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input
    public float sensitive = 1f;
    bool m_Dragging;
    int m_Id = -1;

    CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
    CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input

    Vector2 joystickCenter = Vector2.zero;

    void Start()
    {
        if (isFixed)
            OnFixed();
        else
            OnFloat();

        m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
        CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);

        m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
        CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
    }

    public void ChangeFixed(bool joystickFixed)
    {
        if (joystickFixed)
            OnFixed();
        else
            OnFloat();
        isFixed = joystickFixed;
    }

    /*public override void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Ondragging");
        Vector2 direction = eventData.position - joystickCenter;
        inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
        //ClampJoystick();
        UpdateVirtualAxes();
        handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
    }*/

    private void Update()
    {
        if (!m_Dragging)
        {
            return;
        }
        if (Input.touchCount >= m_Id + 1 && m_Id != -1)
        {
            Vector2 direction = Input.touches[m_Id].position - joystickCenter;
            inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
            UpdateVirtualAxes(inputVector);
            handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
        }
    }

    public void UpdateVirtualAxes(Vector2 value)
    {
        if (joystickMode == JoystickMode.Horizontal)
            m_HorizontalVirtualAxis.Update(value.x*sensitive);
        if (joystickMode == JoystickMode.Vertical)
            m_VerticalVirtualAxis.Update(value.y*sensitive);
        if (joystickMode == JoystickMode.AllAxis)
        {
            m_HorizontalVirtualAxis.Update(value.x*sensitive);
            m_VerticalVirtualAxis.Update(value.y*sensitive);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {

        m_Dragging = true;
        m_Id = eventData.pointerId;
        if (!isFixed)
        {
            background.gameObject.SetActive(true);
            background.position = eventData.position;
            handle.anchoredPosition = Vector2.zero;
            joystickCenter = eventData.position;
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!isFixed)
        {
            background.gameObject.SetActive(false);
        }
        m_Dragging = false;
        m_Id = -1;
        inputVector = Vector2.zero;
        UpdateVirtualAxes(inputVector);
        handle.anchoredPosition = Vector2.zero;
    }

    void OnFixed()
    {
        joystickCenter = background.anchoredPosition;
        background.gameObject.SetActive(true);
        handle.anchoredPosition = Vector2.zero;
        //background.anchoredPosition = fixedScreenPosition;
    }

    void OnFloat()
    {
        handle.anchoredPosition = Vector2.zero;
        background.gameObject.SetActive(false);
    }
}