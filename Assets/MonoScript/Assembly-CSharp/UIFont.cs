using System;
using System.Collections.Generic;
using UnityEngine;

public class UIFont : MonoBehaviour
{
	[Serializable]
	public class CharacterKerning
	{
		public int character;

		public float kerning;

		public CharacterKerning(int character, float kerning)
		{
			this.character = character;
			this.kerning = kerning;
		}
	}

	public int xCount = 16;

	public int yCount = 16;

	public float kerning = 1f;

	public int offset;

	public List<CharacterKerning> perCharacterKerning;

	public float GetKerning(int character)
	{
		foreach (CharacterKerning item in perCharacterKerning)
		{
			if (item.character - offset == character)
			{
				return item.kerning;
			}
		}
		return kerning;
	}
}
