using UnityEngine;
using UnityEngine.EventSystems;
using UnityStandardAssets.CrossPlatformInput;

public class FixedJoystick : Joystick
{
    Vector2 joystickPosition = Vector2.zero;
    private Camera cam = new Camera();

    public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
    public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input

    public CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
    CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input

    bool m_Dragging;
    int m_Id = -1;

    void Start()
    {
        joystickPosition = RectTransformUtility.WorldToScreenPoint(cam, background.position);

        m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
        CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);

        m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
        CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
    }
    private void Update()
    {
        if (!m_Dragging)
        {
            return;
        }
        if (Input.touchCount >= m_Id + 1 && m_Id != -1)
        {
            Vector2 direction = Input.touches[m_Id].position - joystickPosition;
            inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
            UpdateVirtualAxes(inputVector);
            handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
        }
    }
    public void UpdateVirtualAxes(Vector2 value)
    {
        if (joystickMode == JoystickMode.Horizontal)
            m_HorizontalVirtualAxis.Update(value.x);
        if (joystickMode == JoystickMode.Vertical)
            m_VerticalVirtualAxis.Update(value.y);
        if (joystickMode == JoystickMode.AllAxis)
        {
            m_HorizontalVirtualAxis.Update(value.x);
            m_VerticalVirtualAxis.Update(value.y);
        }
    }
    /*public override void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - joystickPosition;
        inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
        ClampJoystick();
        handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
    }*/

    public override void OnPointerDown(PointerEventData eventData)
    {

        m_Dragging = true;
        m_Id = eventData.pointerId;
        OnDrag(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        m_Dragging = false;
        m_Id = -1;
        UpdateVirtualAxes(Vector2.zero);
        handle.anchoredPosition = Vector2.zero;
    }
}