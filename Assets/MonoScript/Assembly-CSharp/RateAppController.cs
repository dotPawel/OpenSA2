using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RateAppController : MonoBehaviour
{
	private const int MISSION_COUNT = 15;

	private const int EXTA_MISSION_COUNT = 10;

	public bool rateComplete
	{
		get
		{
			return PlayerPrefs.GetInt("rate.complete", 0) > 0;
		}
		set
		{
			PlayerPrefs.SetInt("rate.complete", value ? 1 : 0);
		}
	}

	public int missionCount
	{
		get
		{
			return PlayerPrefs.GetInt("rate.missions", 15);
		}
		set
		{
			PlayerPrefs.SetInt("rate.missions", value);
		}
	}

	[method: MethodImpl(32)]
	public event Action<RateAppController> onDialogClosed;

	public bool Check()
	{
		if (!rateComplete && Player.games >= missionCount && Player.victories > 0)
		{
			RateDialog rateDialog = UIController.Find<RateDialog>();
			rateDialog.onRateClicked += OnRateClicked;
			rateDialog.onLaterClicked += OnLaterClicked;
			rateDialog.onSkipClicked += OnSkipClicked;
			rateDialog.Show();
			return true;
		}
		return false;
	}

	private void OnSkipClicked(RateDialog dialog)
	{
		rateComplete = true;
		UnregisterRateDialog(dialog);
		dialog.Close();
		if (this.onDialogClosed != null)
		{
			this.onDialogClosed(this);
		}
	}

	private void OnLaterClicked(RateDialog dialog)
	{
		missionCount += 10;
		UnregisterRateDialog(dialog);
		dialog.Close();
		if (this.onDialogClosed != null)
		{
			this.onDialogClosed(this);
		}
	}

	private void OnRateClicked(RateDialog dialog)
	{
		rateComplete = true;
		AppStoreController.OpenApp();
		UnregisterRateDialog(dialog);
		dialog.Close();
		if (this.onDialogClosed != null)
		{
			this.onDialogClosed(this);
		}
	}

	private void UnregisterRateDialog(RateDialog dialog)
	{
		dialog.onRateClicked -= OnRateClicked;
		dialog.onLaterClicked -= OnLaterClicked;
		dialog.onSkipClicked -= OnSkipClicked;
	}

	[ContextMenu("Reset")]
	public void Reset()
	{
		rateComplete = false;
		missionCount = 15;
	}
}
