using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MouseController : MonoBehaviour
{
    NavMeshAgent agent;

    //CinderellaControllerを参照
    CinderellaController cinderella;
    string invokeMouseDisabled;
    int tapCount;
    RaycastHit hit;
    //目的地(タップした点)までの距離
    float destinationDistance;
    //進み続けるための変数
    bool IsmoveKeep;

    Button feintButton;

	void Start () {        
        agent = GetComponent<NavMeshAgent>();
        cinderella = GameScene.Instance.Cinderella;
        feintButton = GameScene.Instance.CommandBotton.m_feint;
        tapCount = 0;        

        invokeMouseDisabled = ((System.Action)MouseDisabled).Method.Name;
    }

    public void Initialize(Vector3 position,Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
        gameObject.SetActive(true);       
        tapCount = 0;
    }

    void Update () {
        destinationDistance = Vector3.Distance(hit.point, transform.position);        
        if (cinderella.IsFeintMode && tapCount == 0 && Input.GetMouseButtonDown(0))
        {
            tapCount++;            
            var inputPos = Input.mousePosition;
            //Input.mousePositionをスクリーンサイズで割る！
            var viewportPos = new Vector2(inputPos.x / Screen.width, inputPos.y / Screen.height);
            var ray = Camera.main.ViewportPointToRay(viewportPos);

            //ステージ(但しJoystick以外)をタップした時のみネズミ出撃
            if(Physics.Raycast(ray,out hit) && hit.transform.root.tag == TagName.Stage && hit.transform.root.tag != TagName.Joystick)
            {
                SoundManager.Instance.PlaySE(SEName.Mouse);
                agent.destination = hit.point;
                cinderella.ReleaseFeintMode();
                //ネズミ出撃からn秒後に消滅
                Invoke(invokeMouseDisabled, 5.0f);
                GameScene.Instance.CommandBotton.ButtonActive();
            }
            else
            {
                cinderella.IsFeintMode = true;
                tapCount = 0;
                GameScene.Instance.CommandBotton.ButtonNotActive();
            }            
            //マウスアニメーション
            cinderella.MouseAnim();
        }
        if (this.gameObject.activeInHierarchy)
        {
            feintButton.interactable = false;
        }
        if(destinationDistance < 0.5f)
        {
            IsmoveKeep = true;
        }
        if (IsmoveKeep)
        {
            agent.destination += transform.TransformDirection(Vector3.forward) * agent.speed * Time.deltaTime;
        }
	}

    public void MouseDisabled()
    {
        SoundManager.Instance.PlaySE(SEName.Smoke);
        gameObject.SetActive(false);
        IsmoveKeep = false;
        if(cinderella.tag == TagName.Player && !cinderella.m_fakeMode)
        {
            feintButton.interactable = true;
        }
    }
}
