using System.Collections.Generic;
using UnityEngine;

public class TextParser
{
	public enum Anchor
	{
		Top = 0,
		Middle = 1,
		Bottom = 2
	}

	public enum Alignment
	{
		Left = 0,
		Center = 1,
		Right = 2
	}

	public class Page
	{
		public float lineOffset;

		public List<Line> lines { get; private set; }

		public float height { get; private set; }

		public List<MeshChar> characters
		{
			get
			{
				List<MeshChar> list = new List<MeshChar>();
				foreach (Line line in lines)
				{
					foreach (Word word in line.words)
					{
						list.AddRange(word.chars);
					}
				}
				return list;
			}
		}

		public Page(float lineOffset)
		{
			lines = new List<Line>();
			this.lineOffset = lineOffset;
		}

		public void AddLine(Line line)
		{
			if (lines.Count > 0)
			{
				line.offset = Vector3.down * height;
			}
			height += lineOffset;
			lines.Add(line);
		}

		public void SetAnchor(Anchor anchor)
		{
			float num = 0f;
			switch (anchor)
			{
			case Anchor.Middle:
				num = (float)lines.Count * lineOffset / 2f - lineOffset / 2f;
				{
					foreach (Line line in lines)
					{
						line.offset = Vector3.up * num;
					}
					break;
				}
			case Anchor.Bottom:
				num = (float)lines.Count * lineOffset - lineOffset / 2f;
				{
					foreach (Line line2 in lines)
					{
						line2.offset = Vector3.up * num;
					}
					break;
				}
			case Anchor.Top:
				num = lineOffset / 2f;
				{
					foreach (Line line3 in lines)
					{
						line3.offset = Vector3.up * num;
					}
					break;
				}
			}
		}

		public void SetAlignment(Alignment alignment)
		{
			switch (alignment)
			{
			case Alignment.Center:
			{
				foreach (Line line in lines)
				{
					line.offset = Vector3.left * line.width / 2f;
				}
				break;
			}
			case Alignment.Right:
			{
				foreach (Line line2 in lines)
				{
					line2.offset = Vector3.left * line2.width;
				}
				break;
			}
			}
		}
	}

	public class Line
	{
		public float wordOffset;

		public List<Word> words { get; private set; }

		public Vector3 offset
		{
			set
			{
				foreach (Word word in words)
				{
					word.offset = value;
				}
			}
		}

		public float width { get; private set; }

		public Line(float wordOffset)
		{
			words = new List<Word>();
			this.wordOffset = wordOffset;
		}

		public void AddWord(Word word)
		{
			if (words.Count > 0)
			{
				word.offset = Vector3.right * (width + wordOffset);
				width += word.width + wordOffset;
			}
			else
			{
				width += word.width;
			}
			words.Add(word);
		}
	}

	public class Word
	{
		public List<MeshChar> chars { get; private set; }

		public float width { get; private set; }

		public Vector3 offset
		{
			set
			{
				foreach (MeshChar @char in chars)
				{
					@char.offset = value;
				}
			}
		}

		public float height
		{
			get
			{
				return (chars.Count <= 0) ? 0f : chars[0].height;
			}
		}

		public Word()
		{
			chars = new List<MeshChar>();
		}

		public void AddChar(MeshChar arg)
		{
			arg.offset = Vector3.right * (width + arg.width / 2f);
			width += arg.width;
			chars.Add(arg);
		}
	}

	private const char SPACE = ' ';

	public UIFont font;

	public Vector2 rect;

	public bool wrapWords;

	public float fontSize;

	public float lineSpace;

	public float spaceWidth;

	public TextParser(UIFont font, bool wrapWords, Vector2 rect, float lineSpace, float spaceWidth, float fontSize)
	{
		this.font = font;
		this.wrapWords = wrapWords;
		this.rect = rect;
		this.lineSpace = lineSpace;
		this.spaceWidth = spaceWidth;
		this.fontSize = fontSize;
	}

	public Page ParseText(string arg)
	{
		Page page = new Page(lineSpace);
		if (arg == null)
		{
			return page;
		}
		Line line = new Line(spaceWidth);
		Word word = new Word();
		foreach (char c in arg)
		{
			if (c == ' ')
			{
				if (wrapWords && line.words.Count > 0 && line.width + word.width > rect.x)
				{
					page.AddLine(line);
					line = new Line(spaceWidth);
					line.AddWord(word);
				}
				else
				{
					line.AddWord(word);
				}
				word = new Word();
			}
			else
			{
				int index = c - font.offset;
				MeshChar arg2 = CreateMeshChar(index);
				word.AddChar(arg2);
			}
		}
		if (word.width > 0f)
		{
			if (wrapWords && line.words.Count > 0 && line.width + word.width > rect.x)
			{
				page.AddLine(line);
				line = new Line(spaceWidth);
				line.AddWord(word);
			}
			else
			{
				line.AddWord(word);
			}
		}
		if (line.words.Count > 0)
		{
			page.AddLine(line);
		}
		return page;
	}

	private MeshChar CreateMeshChar(int index)
	{
		int num = index / font.yCount;
		int num2 = index % font.xCount;
		Rect uv = default(Rect);
		uv.width = 1f / (float)font.xCount;
		uv.height = 1f / (float)font.yCount;
		uv.x = (float)num2 * uv.width;
		uv.y = 1f - (float)num * uv.height;
		return new MeshChar(fontSize, font.GetKerning(index), uv);
	}
}
