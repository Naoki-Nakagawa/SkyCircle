/**
 * @file ScoreManager
 * @brief スコアの管理クラス
 * @data 作成日 2015/09/08
 * @author 作成者 Naoki Nakagawa
 */

using UnityEngine;
using System.Collections;

/**
 * @class ScoreManager
 * @brief スコアの管理クラス
 */
public class ScoreManager : MonoBehaviour
{
	public GameObject scoreTextPrefab = null;
	private static int score = 0;
	private static int highScore = 0;
	private GameObject score3dText = null;	// スコアの3DText
	private GameObject highScore3dText = null;	// ハイスコアの3DText

	/**
	 * @fn Awake
	 * @biref コンストラクタ的な関数
	 */
	void Awake()
	{
		// ハイスコアを取得する
		highScore = PlayerPrefs.GetInt("HighScore", 0);

		// スコアの3DTextを取得する
		score3dText = transform.FindChild("Score").gameObject;

		// ハイスコアの3DTextを取得する
		highScore3dText = transform.FindChild("HighScore").gameObject;

		// ハイスコアを3DTextに反映する
		highScore3dText.GetComponent<TextMesh>().text = highScore.ToString();
	}

	/**
	 * @fn AddScore
	 * @brief スコアを加算する
	 * @param addScore 加算するスコア
	 */
	public void AddScore(int addScore)
	{
		// スコアを加算する
		score += addScore;

		// スコアを3DTextに反映する
		score3dText.GetComponent<TextMesh>().text = score.ToString();

		// ハイスコアを更新する
		if (score > highScore)
		{
			// ハイスコアを更新する
			highScore = score;

			// ハイスコアを3DTextに反映する
			highScore3dText.GetComponent<TextMesh>().text = highScore.ToString();
		}

		if (Application.loadedLevelName == "Main")
		{
			// スコアの文字を出す
			GameObject scoreText = (GameObject)Instantiate(scoreTextPrefab, GameObject.Find("Player").transform.position, scoreTextPrefab.transform.rotation);

			// スコアの文字の数字を書き換える
			scoreText.GetComponent<TextMesh>().text = addScore.ToString() + "\n<size=\"10\">POINT</size>";
		}
	}

	/**
	 * @fn SaveHighScore
	 * @brief ハイスコアをセーブする
	 */
	public void SaveHighScore()
	{
		PlayerPrefs.SetInt("HighScore", highScore);
		PlayerPrefs.Save();

		// スコアもリセットする
		score = 0;
	}
}
