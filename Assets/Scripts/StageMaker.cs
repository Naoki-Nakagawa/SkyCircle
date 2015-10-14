/**
 * @file StageMaker.cs
 * @brief ステージを作成する鳥のクラス
 * @data 作成日 2015/09/07
 * @author 作成者 Naoki Nakagawa
 */

using UnityEngine;
using System.Collections;

/**
 * @class StageMaker
 * @brief ステージを作成する鳥のクラス
 */
public class StageMaker : MonoBehaviour
{
	public GameObject coinPrefab = null;		// コインのプレハブ
	public GameObject circlePrefab = null;		// 輪のプレハブ
	public GameObject bonusCirclePrefab = null;	// ボーナス輪のプレハブ
	public float instanceDelay = 0.1f;			// 生成する待ち時間
	public int circleInstanceNum = 5;			// コインを何個生成したら輪を生成するか
	private float instanceTime = 0f;			// 最後に生成した時間
	private int circleInstanceCount = 0;		// 輪を生成した数
	private Bird bird = null;					// 鳥クラス

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
		// 画面の右の見えないところまで動ける
		if (Camera.main.WorldToViewportPoint(transform.position).x < 3f)
		{
			// 生成した時間を保存する
			instanceTime += Time.deltaTime;

			if (instanceTime > instanceDelay)
			{
				// 生成した時間をリセットする
				instanceTime = 0f;

				// 輪を生成した数を増やす
				++circleInstanceCount;

				// 輪を生成しても良いぐらいコインを生成していたら輪を生成する
				if (circleInstanceCount > circleInstanceNum)
				{
					// 輪を生成した数をリセットする
					circleInstanceCount = 0;

					// 1/10の確率でボーナス輪を生成する
					if (Random.Range(0, 10) == 0)
					{
						// ボーナス輪を生成する
						Instantiate(bonusCirclePrefab, transform.position, bonusCirclePrefab.transform.rotation);
					}
					else
					{
						// 輪を生成する
						Instantiate(circlePrefab, transform.position, circlePrefab.transform.rotation);
					}
				}
				else
				{
					// コインを生成する
					Instantiate(coinPrefab, transform.position, coinPrefab.transform.rotation);
				}
			}

			// 3回分動かす
			for (int i = 0; i < 3; ++i)
			{
				bird.AutoMove();
				bird.Move();
			}
		}
	}
}
