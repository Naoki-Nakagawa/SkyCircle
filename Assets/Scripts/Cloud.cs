/**
 * @file Cloud.cs
 * @brief 雲クラス
 * @data 作成日 2015/09/07
 * @author 作成者 Naoki Nakagawa
 */

using UnityEngine;
using System.Collections;

/**
 * @class Cloud
 * @brief 雲クラス
 */
public class Cloud : MonoBehaviour
{
	private SpriteRenderer spriteRenderer = null;	// スプライトレンダラー

	/**
	 * @fn Awake
	 * @biref コンストラクタ的な関数
	 */
	void Awake()
	{
		// スプライトレンダラーを取得
		spriteRenderer = GetComponent<SpriteRenderer>();

		// X座標をカメラ範囲の右の見えないところに移動する
		// Y座標はカメラの範囲内でランダムで変える
		// Z座標は元のままを保持
		transform.position = new Vector3(
			Camera.main.ViewportToWorldPoint(new Vector3(1f, 0.5f, 0f)).x + (spriteRenderer.bounds.size.x / 2f),
			Random.Range(Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).y, Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0f)).y), 
			transform.position.z);

		// このオブジェクトを消さないようにする
		DontDestroyOnLoad(gameObject);
	}

	/**
	 * @fn Update
	 * @brief 更新処理
	 */
	void Update()
	{
		// X座標がカメラ範囲の見えないところに移動したら消す
		if (transform.position.x < Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0f)).x - (spriteRenderer.bounds.size.x / 2f))
		{
			Destroy(gameObject);
		}
	}
}
