using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Vector3 touchStartPos;
    private Vector3 touchEndPos;

    void Update()
    {
        //スマホのタッチ情報を取得
        TouchInfo info = InputSmartPhoneUtil.GetTouch();

        //タッチ開始時
        if (info == TouchInfo.Began)
        {
            //タッチ開始時の位置情報を取得する
            touchStartPos = InputSmartPhoneUtil.GetTouchPosition();
            return;
        }
        //タッチ終了時
        else if (info == TouchInfo.Ended)
        {
            //タッチ終了時の位置情報を取得する
            touchEndPos = InputSmartPhoneUtil.GetTouchPosition();

            //タップかフリックかの判定を行う(X座標のみ)
            float directionX = touchEndPos.x - touchStartPos.x;
            bool isFlick = (Mathf.Abs(directionX) > 200) ? true : false;

            //フリックの場合
            if (isFlick)
            {
                Ray ray = Camera.main.ScreenPointToRay(touchStartPos);
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject clickedGameObject = hit.collider.gameObject;
                    if (!"Door".Equals(clickedGameObject.name))
                    {
                        return;
                    }
                }
                ray = Camera.main.ScreenPointToRay(touchEndPos);
                hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject clickedGameObject = hit.collider.gameObject;
                    if (!"Door".Equals(clickedGameObject.name))
                    {
                        return;
                    }
                }

                //どこでもドアの行先変更(Movie)
                GameObject sciptGameObject = GameObject.Find("360Movie");
                SwitchMovie switchMovie = sciptGameObject.GetComponent<SwitchMovie>();
                switchMovie.changeMovie();
                return;
            }
            else
            { 
                //どこでもドアがタップされた場合
                Ray ray = Camera.main.ScreenPointToRay(touchEndPos);
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(ray, out hit))
                {
                    GameObject clickedGameObject = hit.collider.gameObject;
                    if ("Door".Equals(clickedGameObject.name))
                    {
                        //どこでもドアの行先変更(Layer)
                        GameObject sciptGameObject = GameObject.Find("CameraManager");
                        SwitchCamera switchCamera = sciptGameObject.GetComponent<SwitchCamera>();
                        switchCamera.ChangeDoor();
                    }
                }

            }
        }
    }
}
