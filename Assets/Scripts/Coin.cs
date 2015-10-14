/**
 * @file Coin.cs
 * @brief コインクラス
 * @data 作成日 2015/09/08
 * @author 作成者 Naoki Nakagawa
 */

using UnityEngine;
using System.Collections;

/**
 * @class Coin
 * @brief コインクラス
 */
public class Coin : MonoBehaviour
{
	public GameObject timeTextPrefab = null;		// 加算時間の文字のプレハブ
	public bool getFlag = false;					// プレイヤーがゲットしていたらフラグをtrueにする
	private SpriteRenderer spriteRenderer = null;	// スプライトレンダラー

	/**
	 * @fn Awake
	 * @biref コンストラクタ的な関数
	 */
	void Awake()
	{
		// スプライトレンダラーを取得する
		spriteRenderer = GetComponent<SpriteRenderer>();

		// このオブジェクトを消さないようにする
		DontDestroyOnLoad(gameObject);
	}

	/**
	 * @fn Update
	 * @brief 更新処理
	 */
	void Update()
	{
		// プレイヤー
		GameObject player = GameObject.Find("Player");

		// プレイヤーが近くにいて、プレイヤーがバーストしていたらプレイヤーに吸い込まれる
		if (Vector3.Distance(player.transform.position, transform.position) < 7f)
		{
			// プレイヤーがバーストしていたらプレイヤーに吸い込まれる
			if (player.GetComponent<Player>().bird.Burst)
			{
				// プレイヤーとの角度を計算する
				float targetAngle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x);

				// プレイヤーに近づける
				transform.position += new Vector3(Mathf.Cos(targetAngle), Mathf.Sin(targetAngle), 0f);
			}
		}

		// ゲットフラグがtrueだったら、制限時間表示のところに移動する
		if (getFlag)
		{
			// 時間表示
			GameObject time = GameObject.Find("Time");

			// 時間表示に近づける
			transform.position += (time.transform.position - transform.position) * 0.3f;

			// 時間表示の近くだったらこのオブジェクトを消す
			if (Vector3.Distance(time.transform.position, transform.position) < 3f)
			{
				// コインを消す
				Destroy(gameObject);

				// 加算した時間の文字を表示
				Instantiate(timeTextPrefab, time.transform.position + new Vector3(0f, -3f, -1f), timeTextPrefab.transform.rotation);
			}
		}

		// X座標がカメラ範囲の見えないところに移動したら消す
		if (transform.position.x < Camera.main.ViewportToWorldPoint(new Vector3(0f, 0.5f, 0f)).x - (spriteRenderer.bounds.size.x / 2f))
		{
			Destroy(gameObject);
		}
	}
}
