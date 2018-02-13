using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラ制御スクリプト
/// </summary>
public class CinderellaCamera : MonoBehaviour
{
    [SerializeField]
    float m_distanceOffset = 2.0f;
    [SerializeField]
    float m_heightOffset = 2.0f;

    Transform m_target;
    // MonoBehaviourのtransformは内部キャッシュしないから実は重い。
    // そのため、メンバ変数にキャッシュしておく
    Transform m_transform;

	void Start ()
    {
        m_target = GameObject.FindGameObjectWithTag(TagName.Player).GetComponent<Transform>();
        m_transform = GetComponent<Transform>();
	}
	
	void LateUpdate ()
    {
        Vector3 pos = m_target.position;
        pos = pos - Vector3.back * m_distanceOffset;

        pos.y = m_heightOffset;

        m_transform.position = pos;
        m_transform.LookAt(m_target);
	}
}
