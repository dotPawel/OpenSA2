using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StatisticsScreen : UIController
{
	public enum Type
	{
		Victory = 0,
		Defeat = 1
	}

	private const int SCORE_NUMBER_COUNT = 5;

	private const float DELTA_TIME = 1f / 30f;

	private const float ANIMATION_TIME = 3f;

	private const string VICTORY = "VICTORY";

	private const string DEFEAT = "DEFEAT";

	public TargetsStatistics targetsStatistics;

	public UIText titleLabel;

	public UIText scoreLabel;

	public UIButton doneButton;

	public UIButton restartButton;

	public UIButton exitButton;

	public UIIndicator background;

	public UIIndicator ranks;

	public ProgressBar progressBar;

	private Type type;

	private Queue<UIAnimationClip> animations = new Queue<UIAnimationClip>();

	private UIAnimationClip animationClip;

	private bool isFocused;

	public int score
	{
		set
		{
			scoreLabel.text = GetIntField(value, 5);
		}
	}

	[method: MethodImpl(32)]
	public event Action<StatisticsScreen> onDoneButtonClicked;

	[method: MethodImpl(32)]
	public event Action<StatisticsScreen> onRestartButtonClicked;

	[method: MethodImpl(32)]
	public event Action<StatisticsScreen> onExitButtonClicked;

	private void Start()
	{
		doneButton.onClicked += _003CStart_003Em__1D;
		restartButton.onClicked += _003CStart_003Em__1E;
		exitButton.onClicked += _003CStart_003Em__1F;
	}

	private void OnDisable()
	{
		Game.SetPause(false);
	}

	private void Update()
	{
		if (!base.show)
		{
			return;
		}
		if (animationClip == null)
		{
			if (animations.Count > 0)
			{
				animationClip = animations.Dequeue();
			}
		}
		else if (!animationClip.UpdateAnimation(1f / 30f))
		{
			animationClip = null;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			isFocused = true;
		}
		if (!isFocused || !Input.GetKeyUp(KeyCode.Escape))
		{
			return;
		}
		isFocused = false;
		if (type == Type.Victory)
		{
			if (this.onDoneButtonClicked != null)
			{
				this.onDoneButtonClicked(this);
			}
		}
		else if (this.onExitButtonClicked != null)
		{
			this.onExitButtonClicked(this);
		}
	}

	public void Show(Type type)
	{
		this.type = type;
		UIStatesController uIStatesController = rootControler as UIStatesController;
		uIStatesController.Show("StatisticsScreen");
		Game.SetPause(true);
	}

	public override void ViewDidAppear()
	{
		exitButton.show = type == Type.Defeat;
		restartButton.show = type == Type.Defeat;
		doneButton.show = type == Type.Victory;
		titleLabel.text = ((type != 0) ? "DEFEAT" : "VICTORY");
		background.SetState(Player.factionIndex);
	}

	public void SetStatictics(int rank, int score, int infantryCount, int tankCount, int airplaneCount, int structureCount)
	{
		ranks.SetState(rank);
		this.score = score;
		targetsStatistics.infantryCount = infantryCount;
		targetsStatistics.tankCount = tankCount;
		targetsStatistics.airplaneCount = airplaneCount;
		targetsStatistics.structureCount = structureCount;
	}

	public void SetProgress(float progress)
	{
		progressBar.SetProgress(progress);
	}

	public void SetProgress(float from, float to)
	{
		UIProgressAnimation uIProgressAnimation = UIProgressAnimation.Init(base.gameObject, progressBar, 3f * (to - from));
		uIProgressAnimation.Play(from, to);
		AddAnimation(uIProgressAnimation);
	}

	public void SetProgress(float from, float to, int rank)
	{
		UIProgressAnimation uIProgressAnimation = UIProgressAnimation.Init(base.gameObject, progressBar, 3f * (1f - from));
		uIProgressAnimation.Play(from, 1f);
		UISlideAnimation uISlideAnimation = UISlideAnimation.Init(base.gameObject, ranks);
		uISlideAnimation.Play(rank);
		UIProgressAnimation uIProgressAnimation2 = UIProgressAnimation.Init(base.gameObject, progressBar, 3f * to);
		uIProgressAnimation2.Play(0f, to);
		AddAnimation(uIProgressAnimation);
		AddAnimation(uISlideAnimation);
		AddAnimation(uIProgressAnimation2);
	}

	public void AddAnimation(UIAnimationClip animationClip)
	{
		animations.Enqueue(animationClip);
	}

	public static string GetIntField(int value, int numberCount = 3)
	{
		string text = string.Empty;
		if (value > 0)
		{
			for (int num = numberCount - 1; num >= 0; num--)
			{
				if ((float)value >= Mathf.Pow(10f, num))
				{
					text += value;
					break;
				}
				text += "0";
			}
		}
		else
		{
			for (int i = 0; i < numberCount; i++)
			{
				text += "0";
			}
		}
		return text;
	}

	[CompilerGenerated]
	private void _003CStart_003Em__1D(UIButton P_0)
	{
		if (this.onDoneButtonClicked != null)
		{
			this.onDoneButtonClicked(this);
		}
	}

	[CompilerGenerated]
	private void _003CStart_003Em__1E(UIButton P_0)
	{
		if (this.onRestartButtonClicked != null)
		{
			this.onRestartButtonClicked(this);
		}
	}

	[CompilerGenerated]
	private void _003CStart_003Em__1F(UIButton P_0)
	{
		if (this.onExitButtonClicked != null)
		{
			this.onExitButtonClicked(this);
		}
	}
}
