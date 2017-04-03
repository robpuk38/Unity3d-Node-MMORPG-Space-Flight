using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClientsControlManager : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler {

    

    public Image BgHolder;
   
    public Image Padle;

    public Vector3 Direction { set; get; }
   
    

    public virtual void OnDrag(PointerEventData pad)
    {
        
        Vector2 pos = Vector2.zero;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(BgHolder.rectTransform,pad.position,pad.pressEventCamera, out pos))
        {
            //Debug.Log("OnDrag" + BgHolder.name);

            pos.y = (pos.y / BgHolder.rectTransform.sizeDelta.y);
            pos.x = (pos.x / BgHolder.rectTransform.sizeDelta.x);

            float x = (BgHolder.rectTransform.pivot.x == 1) ? pos.x * 2 + 1 : pos.x * 2 - 1;
            float y = (BgHolder.rectTransform.pivot.y == 1) ? pos.y * 2 + 1 : pos.y * 2 - 1;

          //  Debug.Log("OnDrag" + BgHolder.name + " RIGHT LEFT DIRECTION " + x.ToString() + " UP DOWN DIRECTION " + y.ToString());


            Direction = new Vector3(x, 0, y);
            Direction = (Direction.magnitude > 1) ? Direction.normalized : Direction;

            Padle.rectTransform.anchoredPosition = new Vector3(Direction.x * (BgHolder.rectTransform.sizeDelta.x / 3)
                , Direction.z * (BgHolder.rectTransform.sizeDelta.y / 3));
        }
       
    }


    public virtual void OnPointerDown(PointerEventData pad)
    {
        //Debug.Log("OnPointerDown");
        OnDrag(pad);
    }

    public virtual void OnPointerUp(PointerEventData pad)
    {
        //Debug.Log("OnPointerUp");
        Direction = Vector3.zero;
        Padle.rectTransform.anchoredPosition = Vector3.zero;
    }
}
