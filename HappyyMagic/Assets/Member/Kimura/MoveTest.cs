using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    [SerializeField]
    float m_moveSpeed = 2.0f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        float h, v;
        h = GamePadManager.Instance.GetAxis(GamePadAxis.Horizontal);
        v = GamePadManager.Instance.GetAxis(GamePadAxis.Vertical);

        transform.position += new Vector3(h, 0.0f, v) * m_moveSpeed * Time.deltaTime;
	}
}
