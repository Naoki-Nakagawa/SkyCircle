/**
 * @file Circle.cs
 * @brief 輪クラス
 * @data 作成日 2015/09/08
 * @author 作成者 Naoki Nakagawa
 */

using UnityEngine;
using System.Collections;

/**
 * @class Circle
 * @brief 輪クラス
 */
public class Circle : MonoBehaviour
{
	private SpriteRenderer spriteRenderer = null;	// スプライトレンダラー
	private SpriteRenderer childSpriteRenderer = null;	// 子のスプライトレンダラー
	private float t = 0f;
	private float colorationScale = 0f;	// 色の変化の値

	/**
	 * @fn Awake
	 * @biref コンストラクタ的な関数
	 */
	void Awake()
	{
		// スプライトレンダラーを取得する
		spriteRenderer = GetComponent<SpriteRenderer>();

		// 子のスプライトレンダラーを取得する
		childSpriteRenderer = transform.FindChild("CircleFront").GetComponent<SpriteRenderer>();

		// このオブジェクトを消さないようにする
		DontDestroyOnLoad(gameObject);
	}
	
	/**
	 * @fn Update
	 * @brief 更新処理
	 */
	void Update()
	{
		Coloration();

		// X座標がカメラ範囲の見えないところに移動したら消す
		if (transform.position.x < Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0f)).x - (spriteRenderer.bounds.size.x / 2f))
		{
			if (tag == "Circle")
			{
				// スコアの倍率をリセットする
				GameObject.Find("Player").GetComponent<Player>().ScoreRate = 0;
			}

			// このオブジェクトを消す
			Destroy(gameObject);
		}
	}

	/**
	 * @fn Coloration
	 * @brief 色を変化させる
	 */
	void Coloration()
	{
		t += Time.deltaTime * 3f;

		// 経過フレームから色の変化の値を計算する
		colorationScale = Mathf.Abs(Mathf.Sin(t));

		// 色を変化させる
		spriteRenderer.color = new Color(1f, 0.5f + (colorationScale / 2f), colorationScale);
		childSpriteRenderer.color = new Color(1f, 0.5f + (colorationScale / 2f), colorationScale);
	}
}
