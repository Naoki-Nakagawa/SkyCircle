/**
 * @file Timer.cs
 * @brief タイマークラス
 * @data 作成日 2015/09/10
 * @author 作成者 Naoki Nakagawa
 */

using UnityEngine;
using System.Collections;

/**
 * @class Timer
 * @brief タイマークラス
 */
public class Timer
{
	private float startTime = 0f;			// カウントダウンを開始した時間
	private float countDownTime = 0f;		// カウントダウンする時間

	/**
	 * @fn Timer
	 * @biref 受け取った引数分の時間のカウントダウンを開始するコンストラクタ
	 * @param countDownTime カウントダウンする時間
	 */
	public Timer(float countDownTime)
	{
		// 受け取った引数分の時間のカウントダウンを開始する
		StartCountDown(countDownTime);
	}

	/**
	 * @fn StartCountDown
	 * @brief 時間のカウントダウンを開始する
	 * @param countDownTime カウントダウンする時間
	 */
	private void StartCountDown(float countDownTime)
	{
		// カウントダウンする時間を保存する
		this.countDownTime = countDownTime;

		// カウントダウンを開始した時間を保存する
		startTime = Time.time;
	}

	/**
	 * @fn GetTime
	 * @brief カウントダウンしている時間を取得する
	 * @return カウントダウンしている時間を返す
	 */
	public float GetTime()
	{
		// カウントダウンしている時間を返す
		return ((startTime + countDownTime) - Time.time);
	}

	/**
	 * @fn GetTimeOver
	 * @brief カウントダウンが終了したか取得する
	 * @return カウントダウンが終了していたらtrueを、それ以外はfalseを返す
	 */
	public bool GetTimeOver()
	{
		// カウントダウンを開始した時間とカウントダウンする時間を足した時間が
		// 現在の時間以上だったらtrueを返す
		if (startTime + countDownTime <= Time.time)
		{
			return true;
		}

		// それ以外はfalseを返す
		return false;
	}

	/**
	 * @fn operator+
	 * @brief カウントダウンする時間を伸ばす
	 * @param timer タイマークラス
	 * @param addTime 伸ばす時間
	 */
	public static Timer operator + (Timer timer, float addTime)
	{
		// カウントダウンする時間を伸ばす
		timer.countDownTime += addTime;

		// カウントダウンする時間を伸ばしたタイマークラスを返す
		return timer;
	}
}
