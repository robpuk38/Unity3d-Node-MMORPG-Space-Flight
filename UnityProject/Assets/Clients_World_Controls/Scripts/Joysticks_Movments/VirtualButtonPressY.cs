using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualButtonPressY : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{

    private Image bgImg;
    private Image joystickImg;

    public Vector3 InputDicection { set; get; }


    private void Start()
    {
        bgImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();
        InputDicection = Vector3.zero;
    }


  

    public virtual void OnPointerDown(PointerEventData ped)
    {
        Debug.Log("Lets Give The Buttons Some Movment As If They Are Being Pressed");
        Debug.Log("WE HAVE PRESSSED THE Y BUTTON");

        Vector2 pos = Vector2.zero;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle
            (bgImg.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            float x = (bgImg.rectTransform.pivot.x == 1) ? pos.x * 2 + 1 : pos.x * 2 - 1;
            float y = (bgImg.rectTransform.pivot.y == 1) ? pos.y * 2 + 1 : pos.y * 2 - 1;

            InputDicection = new Vector3(x, 0, y);
            InputDicection = (InputDicection.magnitude > 1) ? InputDicection.normalized : InputDicection;

            joystickImg.rectTransform.anchoredPosition = new Vector3(InputDicection.x * (bgImg.rectTransform.sizeDelta.x / 3)
                , InputDicection.z * (bgImg.rectTransform.sizeDelta.y / 3));


        }

        if (MoveShip.Instance != null)
        {
            MoveShip.Instance.ButtonY();
        }

    }

    public virtual void OnPointerUp(PointerEventData ped)
    {
        InputDicection = Vector3.zero;
        joystickImg.rectTransform.anchoredPosition = Vector3.zero;

    }

}
