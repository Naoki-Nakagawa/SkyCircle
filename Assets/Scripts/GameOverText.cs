/**
 * @file GameOverText
 * @brief ゲームオーバーの文字をカメラに追従する
 * @data 作成日 2015/09/08
 * @author 作成者 Naoki Nakagawa
 */

using UnityEngine;
using System.Collections;

/**
 * @class GameOverText
 * @brief ゲームオーバーの文字をカメラに追従する
 */
public class GameOverText : MonoBehaviour
{
	/**
	 * @fn Update
	 * @biref 更新処理
	 */
	void Update()
	{
		transform.position = Camera.main.transform.position + new Vector3(0f, 10f, 20f);
	}
}
