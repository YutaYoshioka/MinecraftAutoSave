using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FileIO;

namespace MinecraftAutoSave
{
	class FilePathFile
	{
		/// <summary>
		/// ファイルパスファイルを読み込みます。
		/// </summary>
		/// <param name="FilePath">ファイルパスファイルへのパス</param>
		/// <returns></returns>
		public static string[] ReadFile_Array(string FilePath)
		{
			if (!(File.Exists("FilePath.txt")))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				ConsoleIO.WriteLine("Error: FilePath.txt が見つかりません。\n       FilePath.txt に、ワールドデータへのパスを入力してください。");
				Console.ForegroundColor = ConsoleColor.Gray;
				Environment.Exit(0);
			}

			List<string> WorldsPath_List = TextIO.ReadStrings("FilePath.txt");
			string[] WorldsPath = WorldsPath_List.ToArray();

			if (WorldsPath.Length == 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				ConsoleIO.WriteLine("Error: FilePath.txt の中身がありません。\n       FilePath.txt に、ワールドデータへのパスを入力してください。");
				Console.ForegroundColor = ConsoleColor.Gray;
				Environment.Exit(0);
			}

			WorldsPath_List.Clear();

			for (int i = 0; i < WorldsPath.Length; i++)
			{
				// 重複確認
				if (Array.IndexOf(WorldsPath, WorldsPath[i]) != i)
				{
					Console.ForegroundColor = ConsoleColor.Green;
					ConsoleIO.WriteLine("Warning: FilePath.txt の " + (i + 1) + " 行目に記述のファイルパスは重複しています。");
					Console.ForegroundColor = ConsoleColor.Gray;
				}
				else
				{
					if (ConfirmationWorldFile(WorldsPath[i], i))
					{
						WorldsPath_List.Add(WorldsPath[i]);
					}
				}
				
			}

			WorldsPath = WorldsPath_List.ToArray();

			if (WorldsPath.Length == 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				ConsoleIO.WriteLine("Error: FilePath.txt の中に有効なファイルパスが存在しません。\n       FilePath.txt に、ワールドデータへのパスを入力してください。");
				Console.ForegroundColor = ConsoleColor.Gray;
				Environment.Exit(0);
			}

			return WorldsPath;
		}


		/// <summary>
		/// ファイルパスファイルを読み込みます。
		/// </summary>
		/// <param name="FilePath">ファイルパスファイルへのパス</param>
		/// <returns></returns>
		public static List<string> ReadFile_List(string FilePath)
		{
			if (!(File.Exists("FilePath.txt")))
			{
				ConsoleIO.WriteError("FilePath.txt が見つかりません。\nFilePath.txt に、ワールドデータへのパスを入力してください。");
				Console.ReadLine();
				Environment.Exit(0);
			}

			List<string> WorldsPath = TextIO.ReadStrings("FilePath.txt");

			if (WorldsPath.Count == 0)
			{
				ConsoleIO.WriteError("FilePath.txt の中身がありません。\nFilePath.txt に、ワールドデータへのパスを入力してください。");
				Console.ReadLine();
				Environment.Exit(0);
			}

			for (int i = WorldsPath.Count - 1; i >= 0; i--)
			{
				// 重複確認
				if (WorldsPath.IndexOf(WorldsPath[i]) != i)
				{
					ConsoleIO.WriteWarning("FilePath.txt の " + (i + 1) + " 行目に記述のファイルパスは重複しています。");
					WorldsPath.RemoveAt(i);
				}
				else
				{
					if (!ConfirmationWorldFile(WorldsPath[i], i))
					{
						WorldsPath.RemoveAt(i);
					}
				}
			}

			if (WorldsPath.Count == 0)
			{
				ConsoleIO.WriteError("FilePath.txt の中に有効なファイルパスが存在しません。\nFilePath.txt に、ワールドデータへのパスを入力してください。");
				Console.ReadLine();
				Environment.Exit(0);
			}

			return WorldsPath;
		}

		/// <summary>
		/// ファイルパスが存在するかどうか、またワールドデータのフォルダかどうかを調べます。存在しなかった場合、警告を表示します。
		/// </summary>
		/// <param name="FilePath">ファイルパス</param>
		/// <param name="Lines">現在の行数(0～n)</param>
		/// <returns>成功したらtrue</returns>
		static bool ConfirmationWorldFile(string FilePath,int Lines)
		{
			bool b = true;
			if (!(Directory.Exists(FilePath))){
				ConsoleIO.WriteWarning("FilePath.txt の " + (Lines + 1) + " 行目に記述のフォルダが見つかりません。");
				b = false;
			}

			if (!(File.Exists(FilePath + "\\level.dat")))
			{
				ConsoleIO.WriteWarning("FilePath.txt の " + (Lines + 1) + " 行目に記述のフォルダはワールドデータのフォルダではありません。");
				b = false;
			}

			return b;
		}
	}
}
