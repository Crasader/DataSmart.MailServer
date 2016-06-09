using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DataSmart.MailServer
{
	internal class SCore
	{
		public static bool IsMatch(string pattern, string text)
		{
			if (pattern.IndexOf("*") > -1)
			{
				if (pattern == "*")
				{
					return true;
				}
				if (pattern.StartsWith("*") && pattern.EndsWith("*") && text.IndexOf(pattern.Substring(1, pattern.Length - 2)) > -1)
				{
					return true;
				}
				if (pattern.IndexOf("*") == -1 && text.ToLower() == pattern.ToLower())
				{
					return true;
				}
				if (pattern.StartsWith("*") && text.ToLower().EndsWith(pattern.Substring(1).ToLower()))
				{
					return true;
				}
				if (pattern.EndsWith("*") && text.ToLower().StartsWith(pattern.Substring(0, pattern.Length - 1).ToLower()))
				{
					return true;
				}
			}
			else if (pattern.ToLower() == text.ToLower())
			{
				return true;
			}
			return false;
		}

		public static bool IsAstericMatch(string pattern, string text)
		{
			pattern = pattern.ToLower();
			text = text.ToLower();
			if (pattern == "")
			{
				pattern = "*";
			}
			while (pattern.Length > 0)
			{
				if (pattern.StartsWith("*"))
				{
					if (pattern.IndexOf("*", 1) <= -1)
					{
						return text.EndsWith(pattern.Substring(1));
					}
					string text2 = pattern.Substring(1, pattern.IndexOf("*", 1) - 1);
					if (text.IndexOf(text2) == -1)
					{
						return false;
					}
					text = text.Substring(text.IndexOf(text2) + text2.Length);
					pattern = pattern.Substring(pattern.IndexOf("*", 1));
				}
				else
				{
					if (pattern.IndexOfAny(new char[]
					{
						'*'
					}) <= -1)
					{
						return text == pattern;
					}
					string text3 = pattern.Substring(0, pattern.IndexOfAny(new char[]
					{
						'*'
					}));
					if (!text.StartsWith(text3))
					{
						return false;
					}
					text = text.Substring(text.IndexOf(text3) + text3.Length);
					pattern = pattern.Substring(pattern.IndexOfAny(new char[]
					{
						'*'
					}));
				}
			}
			return true;
		}

		public static string PathFix(string path)
		{
			return path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
		}

		public static void StreamCopy(Stream source, Stream destination)
		{
			byte[] array = new byte[8000];
			for (int i = source.Read(array, 0, array.Length); i > 0; i = source.Read(array, 0, array.Length))
			{
				destination.Write(array, 0, i);
			}
		}

		public static string RtfToText(string rtfText)
		{
			string result;
			using (RichTextBox richTextBox = new RichTextBox())
			{
				richTextBox.Rtf = rtfText;
				string text = richTextBox.Text.Replace("\r\n", "\n").Replace("\n", "\r\n");
				result = text;
			}
			return result;
		}

		public static string RtfToHtml(string rtfText)
		{
			string result;
			using (RichTextBox richTextBox = new RichTextBox())
			{
				richTextBox.Rtf = rtfText;
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("<html>\r\n");
				richTextBox.SelectionStart = 0;
				richTextBox.SelectionLength = 1;
				Font selectionFont = richTextBox.SelectionFont;
				Color selectionColor = richTextBox.SelectionColor;
				Color selectionBackColor = richTextBox.SelectionBackColor;
				int num = 0;
				while (richTextBox.Text.Length > richTextBox.SelectionStart)
				{
					richTextBox.SelectionStart++;
					richTextBox.SelectionLength = 1;
					if (richTextBox.Text.Length == richTextBox.SelectionStart || selectionFont.Name != richTextBox.SelectionFont.Name || selectionFont.Size != richTextBox.SelectionFont.Size || selectionFont.Style != richTextBox.SelectionFont.Style || selectionColor != richTextBox.SelectionColor || selectionBackColor != richTextBox.SelectionBackColor)
					{
						string text = richTextBox.Text.Substring(num, richTextBox.SelectionStart - num).Replace("\r", "").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "<br/>");
						string text2 = "#" + selectionColor.R.ToString("X2") + selectionColor.G.ToString("X2") + selectionColor.B.ToString("X2");
						string text3 = "#" + selectionBackColor.R.ToString("X2") + selectionBackColor.G.ToString("X2") + selectionBackColor.B.ToString("X2");
						string text4 = "";
						string text5 = "";
						if (selectionFont.Bold)
						{
							text4 += "<b>";
							text5 += "</b>";
						}
						if (selectionFont.Italic)
						{
							text4 += "<i>";
							text5 += "</i>";
						}
						if (selectionFont.Underline)
						{
							text4 += "<u>";
							text5 += "</u>";
						}
						stringBuilder.Append(string.Concat(new object[]
						{
							"<span style=\"color:",
							text2,
							"; font-size:",
							selectionFont.Size,
							"pt; font-family:",
							selectionFont.FontFamily.Name,
							"; background-color:",
							text3,
							";\">",
							text4,
							text,
							text5
						}));
						num = richTextBox.SelectionStart;
						selectionFont = richTextBox.SelectionFont;
						selectionColor = richTextBox.SelectionColor;
						selectionBackColor = richTextBox.SelectionBackColor;
						stringBuilder.Append("</span>\r\n");
					}
				}
				stringBuilder.Append("</html>\r\n");
				result = stringBuilder.ToString();
			}
			return result;
		}
	}
}
