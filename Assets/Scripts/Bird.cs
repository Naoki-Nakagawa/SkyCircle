/**
 * @file Bird.cs
 * @brief 鳥クラス
 * @data 作成日 2015/09/06
 * @author 作成者 Naoki Nakagawa
 */

using UnityEngine;
using System.Collections;

/**
 * @class Bird
 * @brief 鳥の処理を行うクラス
 */
public class Bird : MonoBehaviour
{
	public float speed = 0.5f;				// 速度
	public float speedVelocity = 0.01f;		// 速度の変わる速度
	public float speedMin = 0.5f;			// 速度の最小値
	public float speedMax = 1f;				// 速度の最大値

	public float angle = 0f;				// 角度
	public float anglerVelocity = 0f;		// 角度の変わる速度
	public float anglerUp = 0.3f;			// 上昇する時の角度の変わる速度
	public float anglerGraty = 0.1f;		// 下降する時の角度の変わる速度
	public float angleMin = -90f;			// 角度の最小値
	public float angleMax = 45f;			// 角度の最大値

	public float animationSpeed = 3f;		// アニメーション速度

	public bool flap = false;				// 上昇フラグ
	public bool freeze = false;				// 動作停止フラグ

	private bool burst = false;				// バーストフラグ
	private float burstTime = 0f;			// バーストしている時間
	public float burstTimeMax = 5f;			// バーストしている時間

	private Animator animator = null;		// アニメーター

	public bool Burst
	{
		set
		{
			this.burst = value;

			if (value)
			{
				// バーストしている時間をリセットする
				burstTime = 0f;
			}
		}
		get
		{
			return burst;
		}
	}

	/**
	 * @fn Awake
	 * @biref コンストラクタ的な関数
	 */
	void Awake()
	{
		// アニメーターを取得
		animator = GetComponent<Animator>();
	}

	/**
	 * @fn Move
	 * @brief 移動の処理
	 */
	public void Move()
	{
		// 動作停止フラグがtrueの時は処理を中断する
		if (freeze)
		{
			// アニメーションを止める
			animator.speed = 0f;

			return;
		}

		// バーストしている時は時間を測って、時間になったらバーストを解除する
		if (burst)
		{
			// バーストしている時間を測る
			burstTime += Time.deltaTime;

			// 時間になったらバーストを解除する
			if (burstTime > burstTimeMax)
			{
				Burst = false;
			}
		}

		// 速度を更新する
		UpdateSpeed();

		// 角度を更新する
		UpdateAngle();

		// アニメーション速度を更新する
		UpdateAnimationSpeed();

		// 角度の方向に進む
		transform.position += transform.right * speed;

		// 画面の上に当たっていたら反射する
		if ((Camera.main) &&
			(Camera.main.WorldToViewportPoint(transform.position).y > 1f))
		{
			// 座標のめり込みを直す
			transform.position = new Vector3(transform.position.x, Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 1f, 0f)).y, transform.position.z);

			// 少し右下を向く
			angle = -30f;
		}
	}

	/**
	 * @fn UpdateSpeed
	 * @brief 速度を更新する
	 */
	private void UpdateSpeed()
	{
		// バーストしていたらスピードを最大にして処理を中断する
		if (burst)
		{
			// スピードを最大にする
			speed = speedMax;

			return;
		}

		// 鳥が上を向いている時はスピードを落とす
		if (angle > 0f)
		{
			// スピードを落とす
			speed -= speedVelocity;

			// スピードが遅すぎる時はスピードを上げる
			if (speed < speedMin)
			{
				speed += speedVelocity;
			}
		}

		// 鳥が下を向いている時はスピードをあげる
		if (angle < 0f)
		{
			// スピードを上げる
			speed += speedVelocity * (Mathf.Abs(angle) / 90f);

			// スピードが速すぎる時はスピードを落とす
			if (speed > speedMax)
			{
				speed -= speedVelocity;
			}
		}
	}

	/**
	 * @fn UpdateAngle
	 * @brief 角度を更新する
	 */
	private void UpdateAngle()
	{
		// 羽ばたいている時は角度を上に向ける
		if (flap)
		{
			// 角度を上に向ける
			anglerVelocity += anglerUp;
		}
		// 羽ばたいていない時は角度を下に向ける
		else
		{
			// 角度を下に向ける
			anglerVelocity -= anglerGraty;
		}

		// 角度の速度を弱める
		anglerVelocity *= 0.9f;

		// 角度に角度の変わる速度を加算する
		angle += anglerVelocity;

		// 角度が上に向きすぎた時は向きの速度を戻す
		if (angle > angleMax)
		{
			anglerVelocity = 0f;
		}

		// 角度が下に向きすぎた時は向きの速度を戻す
		if (angle < angleMin)
		{
			anglerVelocity = 0f;
		}

		// このオブジェクトを回転する
		transform.eulerAngles = new Vector3(0f, 0f, angle);
	}

	/**
	 * @fn UpdateAnimationSpeed
	 * @brief アニメーション速度を更新する
	 */
	private void UpdateAnimationSpeed()
	{
		if (angle < 0f)
		{
			// アニメーションを止める
			animator.speed = 0f;
		}
		else
		{
			// アニメーションの速度を変える
			animator.speed = animationSpeed;
		}
	}

	private float autoMoveAngle = 0f;	// 自動で動かす角度

	/**
	 * @fn AutoMove
	 * @brief 自動で動かす
	 */
	public void AutoMove()
	{
		if (Camera.main)
		{
			// 画面の上の方にいたら上昇を止める
			if (Camera.main.WorldToViewportPoint(transform.position).y > 0.65f)
			{
				autoMoveAngle = Random.Range(angleMin, 0f);
			}

			// 画面の下の方にいたら下降を止める
			if (Camera.main.WorldToViewportPoint(transform.position).y < 0.4f)
			{
				autoMoveAngle = Random.Range(0f, angleMax);
			}

			// ランダムに向きを変える
			if (Random.Range(0, 10) == 0)
			{
				// 1/10の確率で向きを変える
				autoMoveAngle = Random.Range(angleMin, angleMax);
			}
		}

		MoveAngle(autoMoveAngle);
	}

	/**
	 * @fn MoveAngle
	 * @brief 目標の角度に向かって飛ぶ
	 * @param targetAngle 目標の角度
	 */
	public void MoveAngle(float targetAngle)
	{
		if (targetAngle > angle)
		{
			flap = true;
		}
		else
		{
			flap = false;
		}
	}
}
