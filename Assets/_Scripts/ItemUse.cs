using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemUse : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool dragOnSurfaces = true;
    public Canvas frame;

    private GameObject m_DraggingIcon;
    private RectTransform m_DraggingPlane;
    private Image self;
	public GameObject PrefabToSet;
    void Start()
    {
        self = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_DraggingIcon = new GameObject("icon");

        m_DraggingIcon.transform.SetParent(frame.transform, false);
        m_DraggingIcon.transform.SetAsLastSibling();

        var image = m_DraggingIcon.AddComponent<Image>();

        image.sprite = GetComponent<Image>().sprite;
        image.SetNativeSize();

        if (dragOnSurfaces)
            m_DraggingPlane = transform as RectTransform;
        else
            m_DraggingPlane = frame.transform as RectTransform;

        SetDraggedPosition(eventData);
        self.enabled = false;
    }

    public void OnDrag(PointerEventData data)
    {
        if (m_DraggingIcon != null)
            SetDraggedPosition(data);
    }

 

    private void SetDraggedPosition(PointerEventData data)
    {
        if (dragOnSurfaces && data.pointerEnter != null && data.pointerEnter.transform as RectTransform != null)
            m_DraggingPlane = data.pointerEnter.transform as RectTransform;

        var rt = m_DraggingIcon.GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_DraggingPlane, data.position, data.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
            rt.rotation = m_DraggingPlane.rotation;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

		GameObject.Find ("Editor").GetComponent<GameEditor> ().PlaceTile ();



        if (m_DraggingIcon != null)
            Destroy(m_DraggingIcon);



    }

    static public T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null) return null;
        var comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        Transform t = go.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }
}
