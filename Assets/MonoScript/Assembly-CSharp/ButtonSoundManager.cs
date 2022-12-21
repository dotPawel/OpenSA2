using System;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSoundManager : MonoBehaviour
{
	[Serializable]
	public class ButtonSet
	{
		public UIButton[] buttons;

		public AudioClip clip;
	}

	[Serializable]
	public class TabButtonSet
	{
		public UITabButton[] buttons;

		public AudioClip clip;
	}

	public AudioSource audioSource;

	public ButtonSet[] buttonSet;

	public TabButtonSet[] tabButtonSet;

	private Dictionary<UIButton, AudioClip> buttonMap = new Dictionary<UIButton, AudioClip>();

	private Dictionary<UITabButton, AudioClip> tabButtonMap = new Dictionary<UITabButton, AudioClip>();

	private void Start()
	{
		ButtonSet[] array = this.buttonSet;
		foreach (ButtonSet buttonSet in array)
		{
			UIButton[] buttons = buttonSet.buttons;
			foreach (UIButton uIButton in buttons)
			{
				buttonMap.Add(uIButton, buttonSet.clip);
				uIButton.onFocused += OnButtonFocused;
			}
		}
		TabButtonSet[] array2 = this.tabButtonSet;
		foreach (TabButtonSet tabButtonSet in array2)
		{
			UITabButton[] buttons2 = tabButtonSet.buttons;
			foreach (UITabButton uITabButton in buttons2)
			{
				tabButtonMap.Add(uITabButton, tabButtonSet.clip);
				uITabButton.onClicked += OnButtonClicked;
			}
		}
	}

	private void OnButtonClicked(UITabButton button)
	{
		audioSource.clip = tabButtonMap[button];
		audioSource.Play();
	}

	private void OnButtonFocused(UIButton button)
	{
		audioSource.clip = buttonMap[button];
		audioSource.Play();
	}
}
