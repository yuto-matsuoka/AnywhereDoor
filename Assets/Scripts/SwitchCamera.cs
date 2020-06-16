using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{

	[Header("Cameras")]
	[SerializeField]
	public Camera mainCamera;
	[SerializeField]
	public List<string> otherLayerList;
	[SerializeField]
	public List<Material> otherRenderMaterialList;
	[SerializeField]
	public GameObject frontSideObj;
	[SerializeField]
	public GameObject backSideObj;

	//行先レイヤー制御用内部プロパティ
	private string doorBackLayerName;
	private int layerIdx = 0;
	private string portalTagName = "portal";

	// どこでもドア用カメラの初期設定
	void Start()
	{
		doorBackLayerName = otherLayerList[0];
		GameObject renderTexturGameObject = GameObject.Find("DoorFront");
		renderTexturGameObject.GetComponent<Renderer>().material = otherRenderMaterialList[0];
		//Debug.Log(Mathf.Abs(frontSideObj.transform.position.z - mainCamera.transform.position.z) < Mathf.Abs(backSideObj.transform.position.z - mainCamera.transform.position.z));
		//Debug.Log(Mathf.Abs(frontSideObj.transform.position.z - mainCamera.transform.position.z));
		//Debug.Log(frontSideObj.transform.position.x);
		//Debug.Log(backSideObj.transform.position.x);
		//Debug.Log(mainCamera.transform.position.x);
	}

	void Update()
	{
        if (mainCamera.transform.position.x > -0.9 && mainCamera.transform.position.x < 0.9 && mainCamera.transform.position.z > 4 && mainCamera.transform.position.z < 4.1)
        {
            mainCamera.LayerCullingShow(doorBackLayerName);
        }

        if (mainCamera.transform.position.x > -0.9 && mainCamera.transform.position.x < 0.9 && mainCamera.transform.position.z > 3.9 && mainCamera.transform.position.z <= 4)
        {
            mainCamera.LayerCullingHide(doorBackLayerName);
        }
    }
	//どこでもドアの行先変更
	public void ChangeDoor() {

		//ドアの先に展開されている情報を変更
		ChangeDoorBackLayer();

		//ドア越しに見える情報を変更
		ChangeDoorFrontTextur();

		//行先が動画の場合、再生制御も行う
		GameObject sciptGameObject = GameObject.Find("360Movie");
		SwitchMovie switchMovie = sciptGameObject.GetComponent<SwitchMovie>();
		if (doorBackLayerName.Equals("VRWorld_MountainMovie"))
		{

			switchMovie.StartMovie();
		}
		else
        {
			switchMovie.StopMovie();
		}
	}

	//ARカメラが保持するColliderの通過判定
	//public void OnTriggerEnter(Collider other)
	//{
	//	//ARカメラをどこでもドアが通過した時
	//	//本来は逆の判定が望ましいがドア側で判定を行うと
	//	//複数のカメラが同時通過するため、可読性を考慮した実装。
	//	if (other.CompareTag(portalTagName))
	//	{
	//		//ドアとカメラが接触した位置を待避させておく
	//		cameraPosition = mainCamera.transform.position;
	//	}
	//}

	//	//ARカメラが保持するColliderの通過判定
	//	public void OnTriggerExit(Collider other)
	//	{
	//        //ARカメラをどこでもドアが通過した時
	//        //本来は逆の判定が望ましいがドア側で判定を行うと
	//        //複数のカメラが同時通過するため、可読性を考慮した実装。
	//        if (other.CompareTag(portalTagName))
	//        {
	//			//X座標を基準にドアの通過判定を行う
	////			if (Mathf.Abs(frontSideObj.transform.position.z - mainCamera.transform.position.z) > Mathf.Abs(backSideObj.transform.position.z - mainCamera.transform.position.z))
	//			if (mainCamera.transform.position.z > 4)
	//				{
	//					//どこでもドアを通過したら行先のレイヤーを表示する
	//					//　→LayerExtensions.csで拡張メソッドをcall
	//					mainCamera.LayerCullingShow(doorBackLayerName);
	//					//mainCamera.LayerCullingToggle(doorBackLayerName);
	//			}
	//            else
	//            {
	//				//mainCamera.LayerCullingHide(doorBackLayerName);
	//				//mainCamera.LayerCullingShow(doorBackLayerName);
	//			}
	//		}
	//	}

	//public void OnTriggerStay(Collider other) {
	//	if (other.CompareTag(portalTagName))
	//	{
	//		if (mainCamera.transform.position.z > 4)
	//		{
	//			mainCamera.LayerCullingShow(doorBackLayerName);
	//		}
	//		else
	//		{
	//			mainCamera.LayerCullingHide(doorBackLayerName);
	//		}
	//	}
 //   }

    //ARカメラが他のレイヤーを含んでいるかの判定
    public bool IsDoorFrontOnly()
	{
		bool isDoorFrontOnly = true;
		foreach (string layerStr in otherLayerList)
		{
			if (mainCamera.LayerCullingIncludes(layerStr))
			{
				isDoorFrontOnly = false;
				break;
			}
		}
		return isDoorFrontOnly;
	}

	//行先レイヤーの変更処理(フリック入力やボタンイベントを想定)
	private void ChangeDoorBackLayer()
	{
		//ARカメラが他のレイヤーを含んでいない場合
		if (IsDoorFrontOnly())
		{
			layerIdx = (layerIdx == (otherLayerList.Count - 1)) ? 0 : (layerIdx + 1);
			doorBackLayerName = otherLayerList[layerIdx];
		}
	}

	//行先RenderTextureの変更処理(フリック入力やボタンイベントを想定)
	private void ChangeDoorFrontTextur()
	{
		//ARカメラが他のレイヤーを含んでいない場合
		if (IsDoorFrontOnly())
		{
			GameObject renderTexturGameObject = GameObject.Find("DoorFront");
			renderTexturGameObject.GetComponent<Renderer>().material = otherRenderMaterialList[layerIdx];
		}
	}

}