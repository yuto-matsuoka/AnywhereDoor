using UnityEngine;

public static class InputSmartPhoneUtil
{
    //タッチ座標の内部プロパティ
    private static Vector3 TouchPosition = Vector3.zero;

    //タッチされたかどうかを検出
    public static TouchInfo GetTouch()
    {
        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0)) { return TouchInfo.Began; }
            if (Input.GetMouseButton(0)) { return TouchInfo.Moved; }
            if (Input.GetMouseButtonUp(0)) { return TouchInfo.Ended; }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                return (TouchInfo)((int)Input.GetTouch(0).phase);
            }
        }
        return TouchInfo.None;
    }

    //タッチポジションを取得(エディタと実機を考慮)
    public static Vector3 GetTouchPosition()
    {
        if (Application.isEditor)
        {
            TouchInfo touch = InputSmartPhoneUtil.GetTouch();
            if (touch != TouchInfo.None) { return Input.mousePosition; }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                TouchPosition.x = touch.position.x;
                TouchPosition.y = touch.position.y;
                return TouchPosition;
            }
        }
        return Vector3.zero;
    }

    //タッチワールドポジションを取得(エディタと実機を考慮)
    public static Vector3 GetTouchWorldPosition(Camera camera)
    {
        return camera.ScreenToWorldPoint(GetTouchPosition());
    }
}

//タッチ情報
//UnityEngine.TouchPhaseを拡張
public enum TouchInfo
{
    // タッチなし
    None = 99,
    // タッチ開始
    Began = 0,
    // タッチ移動
    Moved = 1,
    // タッチ静止
    Stationary = 2,
    // タッチ終了
    Ended = 3,
    // タッチキャンセル
    Canceled = 4,
}