/**
 * @file Player.cs
 * @brief プレイヤークラス
 * @data 作成日 2015/09/07
 * @author 作成者 Naoki Nakagawa
 */

using UnityEngine;
using System.Collections;

/**
 * @class Player
 * @biref プレイヤークラス
 */
public class Player : MonoBehaviour
{
	public float timerLimit = 30f;
	private int scoreRate = 0;			// スコアの倍率
	private bool hitFlag = false;		// 障害物に当たったフラグ
	public Bird bird = null;			// 鳥クラス
	public Timer timer = null;			// タイマークラス
	private float sceneChangeTime = 0f;	// シーンが変わった時の時間

	public int ScoreRate
	{
		set
		{
			this.scoreRate = value;
		}
		get
		{
			return scoreRate;
		}
	}

	/**
	 * @fn Awake
	 * @biref コンストラクタ的な関数
	 */
	void Awake()
	{
		// このオブジェクトを消さないようにする
		DontDestroyOnLoad(gameObject);

		// 鳥クラスを取得する
		bird = GetComponent<Bird>();
	}
	
	/**
	 * @fn Update
	 * @brief 更新処理
	 */
	void Update()
	{
		switch (Application.loadedLevelName)
		{
		case "Title":

			// タッチされたらメインシーンに移行する
			if (Input.GetMouseButtonDown(0))
			{
				// 当たったフラグをfalseにする
				hitFlag = false;

				// タイトルロゴを消す
				Destroy(GameObject.Find("TitleLogo"));

				// メインシーンに移行する
				Application.LoadLevel("Main");

				// タイマークラスを生成する
				timer = new Timer(timerLimit);
			}

			// タイトルでは自動で動かす
			bird.AutoMove();

			break;

		case "Main":

			if (!bird.Burst)
			{
				// バースト時のパーティクルを止める
				transform.FindChild("BurstParticle").GetComponent<ParticleSystem>().Stop();

				// 当たり判定を大きくする
				GetComponent<CircleCollider2D>().radius = 0.18f;
			}

			// 制限時間を画面に表示する
			GameObject.Find("Time").GetComponent<TextMesh>().text = ((int)(timer.GetTime() * 10f)).ToString();

			// 画面の下に落ちたらゲームオーバーに移行する
			if (Camera.main.WorldToViewportPoint(transform.position).y < -0.25f)
			{
				// バースト時のパーティクルを止める
				transform.FindChild("BurstParticle").GetComponent<ParticleSystem>().Stop();

				// ハイスコアを保存する
				GameObject.Find("ScoreManager").GetComponent<ScoreManager>().SaveHighScore();

				// シーンを切り替える時間を保存する
				sceneChangeTime = Time.time;

				// ゲームオーバーに移行する
				Application.LoadLevel("GameOver");
			}
			// 制限時間が終わったらクリアーに移行する
			else if (timer.GetTimeOver())
			{
				// ハイスコアを保存する
				GameObject.Find("ScoreManager").GetComponent<ScoreManager>().SaveHighScore();

				// シーンを切り替える時間を保存する
				sceneChangeTime = Time.time;

				// クリアーに移行する
				Application.LoadLevel("Clear");
			}
			// 障害物に当たっていない時は普通に操作できる
			else if (!hitFlag)
			{
				if (Input.GetMouseButton(0))
				{
					// タッチしていたら羽ばたく
					bird.flap = true;
				}
				else
				{
					// タッチしていなかったら羽ばたかなくする
					bird.flap = false;
				}
			}

			break;

		case "Clear":

			// 動かなくする
			if (Camera.main.WorldToViewportPoint (transform.position).y < -0.25f)
			{
				bird.freeze = true;
			}

			// 画面がタッチされていたらタイトルに戻る
			if ((Input.GetMouseButtonDown(0)) &&
				(Time.time - sceneChangeTime > 1f))
			{
				// ヒエラルキーにあるオブジェクトをすべて消す
				GameObject[] objects = (GameObject[])FindObjectsOfType(typeof(GameObject));

				foreach (GameObject obj in objects)
				{
					if (obj.activeInHierarchy)
					{
						Destroy (obj);
					}
				}

				// タイトルに移行する
				Application.LoadLevel("Title");
			}

			bird.AutoMove();

			break;

		case "GameOver":

			// 動かなくする
			if (Camera.main.WorldToViewportPoint(transform.position).y < -0.25f)
			{
				bird.freeze = true;
			}

			// 画面がタッチされていたらタイトルに戻る
			if ((Input.GetMouseButtonDown(0)) &&
				(Time.time - sceneChangeTime > 1f))
			{
				// ヒエラルキーにあるオブジェクトをすべて消す
				GameObject[] objects = (GameObject[])FindObjectsOfType(typeof(GameObject));

				foreach (GameObject obj in objects)
				{
					if (obj.activeInHierarchy)
					{
						Destroy(obj);
					}
				}

				// タイトルに移行する
				Application.LoadLevel("Title");
			}

			bird.flap = false;

			break;

		default:
			break;
		}

		bird.Move();
	}

	/**
	 * @fn HitObstacle
	 * @brief 障害物に当たった時の処理
	 */
	IEnumerator HitObstacle()
	{
		// 障害物に当たったことにする
		hitFlag = true;

		// 向きを下に向ける
		bird.angle = -90f;

		// 向きの加速度を止める
		bird.anglerVelocity = 0f;

		// スピードを遅くする
		bird.speed = bird.speedMin;

		// ぶつかった時のパーティクルを出す
		transform.FindChild("ObstacleParticle").GetComponent<ParticleSystem>().Play();

		// シーンがメインなら
		if (Application.loadedLevelName == "Main")
		{
			// 動きを止める
			bird.freeze = true;

			// ランダムで揺らす
			iTween.ShakePosition(gameObject, new Vector3(1.05f, 1.05f, 1.05f), 0.75f);

			// 1秒待つ
			yield return new WaitForSeconds(1f);

			// 動きを再開する
			bird.freeze = false;

			// ハイスコアを保存する
			GameObject.Find("ScoreManager").GetComponent<ScoreManager>().SaveHighScore();

			// シーンを切り替える時間を保存する
			sceneChangeTime = Time.time;

			// ゲームオーバーに移行する
			Application.LoadLevel("GameOver");
		}
	}

	/**
	 * @fn OnTriggerEnter2D
	 * @brief オブジェクトと当たった時の処理
	 * @param collider 当たった判定
	 */
	void OnTriggerEnter2D(Collider2D collider)
	{
		// 障害物に当たったらアニメーションしてゲームオーバーに移行する
		if (collider.tag == "Obstacle")
		{
			StartCoroutine("HitObstacle");
		}
		else
		{
			// シーンがメインなら
			if (Application.loadedLevelName == "Main")
			{
				switch (collider.tag)
				{
				case "BonusCircle":

					// スコアの倍率を倍にする
					scoreRate *= 2;

					// スコアを加算する
					GameObject.Find ("ScoreManager").GetComponent<ScoreManager>().AddScore(scoreRate);

					// バーストする
					bird.Burst = true;

					// バースト時のパーティクルを出す
					transform.FindChild("BurstParticle").GetComponent<ParticleSystem>().Play();

					// 当たり判定を小さくする
					GetComponent<CircleCollider2D>().radius = 0.1f;

					// 当たったオブジェクトを削除する
					Destroy(collider.gameObject);

					break;

				case "Circle":

					// スコアの倍率を増やす
					scoreRate += 1000;

					// スコアを加算する
					GameObject.Find("ScoreManager").GetComponent<ScoreManager>().AddScore(scoreRate);

					// 当たったオブジェクトを削除する
					Destroy(collider.gameObject);

					break;

				case "Coin":

					// 制限時間を増やす
					timer += 0.1f;

					// コインのゲットフラグをtrueにする
					collider.GetComponent<Coin>().getFlag = true;

					break;
				}
			}
		}
	}
}
