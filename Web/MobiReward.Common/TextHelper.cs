using System;

namespace MobiReward.Common
{
	public class TextHelper
	{
		public static string GenerateRandomText(int length)
		{
			string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
			string pwd = String.Empty;
			Random rnd = new Random();
			for (int i = 0; i < length; i++)
				pwd += chars[rnd.Next(chars.Length)];
			return pwd;
		}

		public static string ClearUnSafeTags(string text)
		{
			System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"<(.|\n)+?>");
			return re.Replace(text, "");
		}

		public static string CutText(string text, int length)
		{
			text = ClearUnSafeTags(text);
			if (text.Length > length)
				return text.Substring(0, length) + " ... ";
			else
				return text;
		}
	}
}
