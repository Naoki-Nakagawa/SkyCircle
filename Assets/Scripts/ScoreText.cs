/**
 * @file ScoreText
 * @brief スコアの文字クラス
 * @data 作成日 2015/09/08
 * @author 作成者 Naoki Nakagawa
 */

using UnityEngine;
using System.Collections;

/**
 * @class ScoreText
 * @brief スコアの文字クラス
 */
public class ScoreText : MonoBehaviour
{
	/**
	 * @fn Awake
	 * @biref コンストラクタ的な関数
	 */
	void Awake()
	{
		StartCoroutine("Animation");
	}

	/**
	 * @fn Animation
	 * @brief スコアテキストのアニメーション
	 */
	IEnumerator Animation()
	{
		// 1秒かけて上に動かす
		iTween.MoveTo(gameObject, transform.position + new Vector3(0f, 2f, 0f), 1f);
		iTween.ScaleFrom(gameObject, new Vector3(0f, 0f, 0f), 0.3f);

		// 1秒待つ
		yield return new WaitForSeconds(1f);

		// 消す
		Destroy(gameObject);
	}
}
