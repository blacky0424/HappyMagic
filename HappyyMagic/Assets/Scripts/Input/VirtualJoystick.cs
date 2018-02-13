using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VirtualJoystick : Graphic, IPointerDownHandler, IPointerUpHandler, IEndDragHandler, IDragHandler
{
    const string STICK_NAME = "Stick";
    
    [SerializeField, Header("実際に動くスティック部分")]
    Transform m_stick;

    [SerializeField, Header("スティックが動く範囲の半径")]
    float m_radius = 100;

    float m_radiusR;
    RectTransform m_rectTransform;

    public Vector2 Axis { get; private set; }

    Vector3 StickPosition
    {
        set
        {
            m_stick.localPosition = value;
            Axis = new Vector2(value.x * m_radiusR, value.y * m_radiusR);
        }
    }

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    void Init()
    {
        CreateStickIfneeded();

        // 割り算は重たいので先に計算しておく
        m_radiusR = 1.0f / m_radius;

        StickPosition = Vector3.zero;
        m_rectTransform = GetComponent<RectTransform>();

        var image = m_stick.GetComponent<Image>();
        if (image != null)
        {
            image.raycastTarget = false;
        }

        raycastTarget = true;
        color = new Color(0, 0, 0, 0);
    }

    void CreateStickIfneeded()
    {
        // 既にあれば終了
        if(m_stick != null)
        {
            return;
        }

        // スティックが子にあるか検索、取得できれば終了
        m_stick = transform.FindChild(STICK_NAME);
        if (m_stick != null)
        {
            return;
        }

        // スティック生成
        var obj = new GameObject(STICK_NAME);
        m_stick = obj.GetComponent<Transform>();
        m_stick.SetParent(transform);
        m_stick.localRotation = Quaternion.identity;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnEndDrag(eventData);
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        StickPosition = Vector3.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 screenPos = Vector2.zero;
        Vector2 input = new Vector2(eventData.position.x, eventData.position.y);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rectTransform, input, null, out screenPos);

        StickPosition = screenPos;

        // 移動場所が設定した半径を超えている場合は制限内に抑える
        float currentRadius = (Vector3.zero - m_stick.localPosition).magnitude;
        if(currentRadius > m_radius)
        {
            float radian = Mathf.Atan2(m_stick.localPosition.y, m_stick.localPosition.x);
            Vector3 limitedPos = Vector3.zero;
            limitedPos.x = m_radius * Mathf.Cos(radian);
            limitedPos.y = m_radius * Mathf.Sin(radian);

            StickPosition = limitedPos;
        }
    }

#if UNITY_EDITOR

    protected override void OnValidate()
    {
        base.OnValidate();
        Init();
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(transform.position, transform.forward, m_radius);
    }

#endif

}
