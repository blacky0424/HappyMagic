using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePadAxis
{
    Horizontal,
    Vertical,
}

/// <summary>
/// 入力管理クラス
/// </summary>
public class GamePadManager : SingletonMonoBehaviour<GamePadManager>
{
    const string HORIZONTAL_NAME = "Horizontal";
    const string VERTICAL_NAME = "Vertical";

    [SerializeField]
    VirtualJoystick m_vJoystick;

    float[] m_axis = new float[] { 0.0f, 0.0f };
    float m_normalizeR;

    private void Start()
    {
        m_normalizeR = 1.0f / 0.7f;
    }

    private void Update()
    {
        float h, v, posX, posY;

        h = Input.GetAxis(HORIZONTAL_NAME);
        v = Input.GetAxis(VERTICAL_NAME);
        posX = m_vJoystick.Axis.x;
        posY = m_vJoystick.Axis.y;

        m_axis[(int)GamePadAxis.Horizontal] = Mathf.Abs(h) > Mathf.Abs(posX) ? h : posX;
        m_axis[(int)GamePadAxis.Vertical]   = Mathf.Abs(v) > Mathf.Abs(posY) ? v : posY;

        m_axis[(int)GamePadAxis.Horizontal] = Mathf.Clamp(m_axis[(int)GamePadAxis.Horizontal] * m_normalizeR, -1.0f, 1.0f);
        m_axis[(int)GamePadAxis.Vertical]   = Mathf.Clamp(m_axis[(int)GamePadAxis.Vertical]   * m_normalizeR, -1.0f, 1.0f);
    }

    public float GetAxis(GamePadAxis axis)
    {
        return m_axis[(int)axis];
    }


}
