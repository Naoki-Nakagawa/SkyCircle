/**
 * @file MainCamera.cs
 * @brief カメラクラス
 * @data 作成日 2015/09/07
 * @author 作成者 Naoki Nakagawa
 */

using UnityEngine;
using System.Collections;

/**
 * @class MainCamera
 * @brief カメラクラス
 */
public class MainCamera : MonoBehaviour
{
	public Transform playerTransform = null;	// プレイヤー
	public GameObject cloudPrefab = null;	// 雲

	private float lastCloudInstancePositionX = 0f;

	/**
	 * @fn Awake
	 * @biref コンストラクタ的な関数
	 */
	void Awake()
	{
		// フレームレートを60にする
		Application.targetFrameRate = 60;

		// このオブジェクトを消さないようにする
		DontDestroyOnLoad(gameObject);
	}

	/**
	 * @fn Update
	 * @brief 更新処理
	 */
	void Update()
	{
		if (Application.loadedLevelName == "Clear")
		{
			return;
		}

		if ((Application.loadedLevelName == "Main") &&
			GameObject.Find("Player").GetComponent<Player>().timer.GetTime() < 1.5f)
		{
			return;
		}

		// プレイヤーのX座標に追従する
		transform.position = new Vector3 (playerTransform.position.x + (Camera.main.ViewportToWorldPoint (new Vector3 (0.25f, 0.5f, 0f)).x - Camera.main.ViewportToWorldPoint (new Vector3 (0f, 0.5f, 0f)).x), transform.position.y, transform.position.z);

		// X座標が10f増えるたびに雲を1個生成する
		if (transform.position.x > lastCloudInstancePositionX + 10f)
		{
			// 生成したX座標を保存する
			lastCloudInstancePositionX = transform.position.x;

			// 雲を生成する
			Instantiate(cloudPrefab);
		}
	}
}
