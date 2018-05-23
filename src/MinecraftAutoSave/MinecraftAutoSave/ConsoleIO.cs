using System;
using FileIO;

namespace MinecraftAutoSave
{
	/// <summary>
	/// コンソール関連メソッドです。ログファイルへの書き込みも同時に行います。
	/// </summary>
	class ConsoleIO
	{
		/// <summary>
		/// 警告表示用です。
		/// </summary>
		/// <param name="value">表示する内容</param>
		public static void WriteWarning(string value)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(value);
			Console.ForegroundColor = ConsoleColor.Gray;
			TextIO.WriteStrings(Define.LogFilePath, value);
		}

		/// <summary>
		/// エラー表示用です。
		/// </summary>
		/// <param name="value">表示する内容</param>
		public static void WriteError(string value)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(value);
			Console.ForegroundColor = ConsoleColor.Gray;
			TextIO.WriteStrings(Define.LogFilePath, value);
		}

		/// <summary>
		/// ConsoleIO.WriteLineと同等です。
		/// </summary>
		/// <param name="value"></param>
		public static void WriteLine(string value)
		{
			Console.WriteLine(value);
			TextIO.WriteStrings(Define.LogFilePath, value);
		}

		/// <summary>
		/// ConsoleIO.WriteLineと同等です。
		/// </summary>
		/// <param name="value"></param>
		public static void WriteLine()
		{
			Console.WriteLine();
			TextIO.WriteStrings(Define.LogFilePath, "");
		}
	}
}
