using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using FileIO;
using Microsoft.VisualBasic.FileIO;

namespace MinecraftAutoSave
{
	class Program
	{
		/// <summary>
		/// 別スレッドで取得したキー入力を格納する変数。aが初期値
		/// </summary>
		static string InputKey = new string('a', 1);

		static void Main(string[] args)
		{
			CreateLogFile();
			ConsoleIO.WriteLine("Minecraft Server WorldDataBackup");
			ConsoleIO.WriteLine();
			ReadSettingsFile();
			ConsoleIO.WriteLine();
			var IndexList = StartProgram(out IndexHeader Header, out List<Index> AllIndexList);
			ConsoleIO.WriteLine("\nバックアップを開始します。\n　コマンドは以下の通りです。\n" +
								"　s : 設定変更\n" +
								"　r : アプリケーションを再起動\n" +
								"　x : アプリケーションを終了\n" +
								"　c : 強制的にすべてのバックアップを作成\n");

			Backup.CreateAllBackup(ref IndexList, AllIndexList);
			IndexFile.CreateIndexFile(IndexList, AllIndexList, Header);

			bool Loop = true;
			int LoopNum = 0;
			long LoopTicks = DateTime.Now.Ticks;

			Task.Run(() =>
				GetKey()
			);

			while (Loop)
			{
				if (InputKey != "a")
				{
					ConsoleIO.WriteLine("コマンドが入力されました。バックアップを一時停止します。");
					string s = "";
					switch (InputKey)
					{
						case "S":
							UpdateSettings();
							break;

						case "R":
							ConsoleIO.WriteLine("再起動しますか？\n y/N");
							s = Console.ReadLine();
							if(s == "y")
							{
								ConsoleIO.WriteLine("再起動します、しばらくお待ちください。");
								Application.Restart();
								Environment.Exit(0);
							}
							break;

						case "X":
							ConsoleIO.WriteLine("アプリケーションを終了しますか？\n y/N");
							s = Console.ReadLine();
							if (s == "y")
							{
								ConsoleIO.WriteLine("アプリケーションを終了します、しばらくお待ちください。");
								Environment.Exit(0);
							}
							break;

						case "C":
							ConsoleIO.WriteLine("強制的にすべてのバックアップを作成します。よろしいですか？\n y/N");
							s = Console.ReadLine();
							if (s == "y")
							{
								ConsoleIO.WriteLine("バックアップを作成中です、しばらくお待ちください。");
								Backup.CreateAllBackup(ref IndexList, AllIndexList, true);
								IndexFile.CreateIndexFile(IndexList, AllIndexList, Header);
								ConsoleIO.WriteLine("すべてのバックアップの作成を開始しました。\n");
							}
							break;

						default:
							ConsoleIO.WriteLine("\nコマンドは以下の通りです。\n" +
								"s : 設定変更\n" +
								"r : アプリケーションを再起動\n" +
								"x : アプリケーションを終了\n" +
								"c : 強制的にすべてのバックアップを作成\n");
							break;
					}
					InputKey = "a";
					Task.Run(() =>
						GetKey()
					);
					Console.WriteLine("バックアップを再開します。\n　コマンドは以下の通りです。\n" +
								"　s : 設定変更\n" +
								"　r : アプリケーションを再起動\n" +
								"　x : アプリケーションを終了\n" +
								"　c : 強制的にすべてのバックアップを作成\n");
				}
				if(DateTime.Now.Ticks > LoopTicks + Define.BackupInterval * 10000L)
				{
					LoopTicks = DateTime.Now.Ticks;
					LoopNum++;
					Backup.CreateAllBackup(ref IndexList, AllIndexList);
					IndexFile.CreateIndexFile(IndexList, AllIndexList, Header);
				}
				Thread.Sleep(Define.LoopInterval);
			}


			//System.IO.File.Exists(fileName);

			//FileSystem.CopyDirectory("a", "b", UIOption.AllDialogs, UICancelOption.DoNothing);


		}

		/// <summary>
		/// キー入力を取得し、InputKeyに格納します。別スレッドでの実行を前提にし、ロックをかけて書き込みます。
		/// </summary>
		static void GetKey()
		{
			string s = Console.ReadKey().Key.ToString();
			lock (InputKey)
			{
				InputKey = s;
			}
		}

		/// <summary>
		/// ログファイルを新規作成します。
		/// </summary>
		static void CreateLogFile()
		{
			Define.LogFilePath = Define.LogFilePath1 + DateTime.Now.Ticks + Define.LogFilePath2;
			TextIO.WriteStrings(Define.LogFilePath, "MCAS_Log_" + Define.MCAS_Version);

			// TextIO.WriteStrings(Define.LogFilePath, "");

		}


		// 時間の単位．入力された値にこの数字をかけたものが[ms]になるようにする．
		static int timeUnit = 1;
		static string timeUnit_s = "ms";

		/// <summary>
		/// 設定変更を行います。
		/// </summary>
		public static void UpdateSettings()
		{
			ConsoleIO.WriteLine("\n設定変更をします。");
			int BackupInt = -2;
			while (true)
			{
				ConsoleIO.WriteLine("\nバックアップの間隔を[" + timeUnit_s + "]で指定しください。現在 " + (Define.BackupInterval / timeUnit) + "[" + timeUnit_s + "] に設定されています。\nスキップするには何も入力せずEnterを押してください。\nms,s,m,hを入力することで指定する時間の単位を変更できます。");
				string s = Console.ReadLine();
				if (!int.TryParse(s, out BackupInt))
				{
					if (s == "")
					{
						BackupInt = -1;
						break;
					}
					else if(s == "ms")
					{
						timeUnit = 1;
						timeUnit_s = "ms";
					}
					else if (s == "s")
					{
						timeUnit = 1000;
						timeUnit_s = "s";
					}
					else if (s == "m")
					{
						timeUnit = 60000;
						timeUnit_s = "m";
					}
					else if (s == "h")
					{
						timeUnit = 3600000;
						timeUnit_s = "h";
					}
					else
					{
						ConsoleIO.WriteLine("バックアップ間隔は0以上の整数を入力してください。");
					}
				}
				else
				{
					if (BackupInt < 0)
					{
						ConsoleIO.WriteLine("バックアップ間隔は0以上の整数を入力してください。");
					}
					else
					{
						break;
					}
				}
			}

			if (BackupInt != -1)
			{
				Define.BackupInterval = BackupInt * timeUnit;
				ConsoleIO.WriteLine("バックアップ間隔を " + (Define.BackupInterval / timeUnit) + "[" + timeUnit_s + "]に変更しました。\n");
				if (Define.BackupInterval < 1000)
				{
					Define.LoopInterval = Define.BackupInterval / 2;
				}
				else
				{
					Define.LoopInterval = 500;
				}
			}

			TextIO.WriteStrings(Define.SettingsFileName, "MCAS_Settings_" + Define.MCAS_Version, false);
			TextIO.WriteStrings(Define.SettingsFileName, Define.BackupInterval.ToString());
		}

		/// <summary>
		/// 設定ファイルを読み込みます。
		/// </summary>
		public static void ReadSettingsFile()
		{
			ConsoleIO.WriteLine("設定ファイルを読み込み中…");
			bool b = false;

			if (TextIO.ReadStrings(Define.SettingsFileName, out List<string> SettingsFile))
			{
				if (SettingsFile[0] == "MCAS_Settings_" + Define.MCAS_Version)
				{
					if (int.TryParse(SettingsFile[1], out Define.BackupInterval))
					{
						b = true;
					}
					else
					{
						ConsoleIO.WriteWarning("設定ファイルファイルが異常です。設定ファイルを新規作成します。");
						b = false;
					}
				}
				else
				{
					ConsoleIO.WriteWarning("設定ファイルのバージョンが異なるかファイルが異常です。設定ファイルを新規作成します。");
					b = false;
				}
			}
			else
			{
				ConsoleIO.WriteWarning("設定ファイルが見つかりません。設定ファイルを新規作成します。");
				b = false;
			}

			if (b)
			{
				ConsoleIO.WriteLine("設定ファイルの読み込みに成功しました。");
			}
			else
			{
				SettingsFile = new List<string>();
				SettingsFile.Add("MCAS_Settings_" + Define.MCAS_Version);
				SettingsFile.Add("90000");
				Define.BackupInterval = 90000;
				TextIO.WriteStrings(Define.SettingsFileName, SettingsFile, false);
			}
			ConsoleIO.WriteLine("バックアップ間隔は " + Define.BackupInterval + "[ms] に設定されています。");

			if(Define.BackupInterval < 1000)
			{
				Define.LoopInterval = Define.BackupInterval / 2;
			}
			else
			{
				Define.LoopInterval = 500;
			}
			return;
		}

		/// <summary>
		/// プログラム起動時に呼び出し、有効なIndexを返します。
		/// </summary>
		/// <param name="Header">Indexファイルのヘッダー</param>
		/// <param name="IndexList">無効なIndex</param>
		/// <returns>有効なIndex</returns>
		static List<Index> StartProgram(out IndexHeader Header, out List<Index> IndexList)
		{
			Header = new IndexHeader();
			IndexList = new List<Index>();

			ConsoleIO.WriteLine("FilePathファイルの確認中…");
			if (!File.Exists(Define.FilePathFile_Path))
			{
				ConsoleIO.WriteError("FilePathファイルが存在しません。\n" +
									"FilePath.txt にオートセーブ対象フォルダ名を1行に1つ記述してください。\n" +
									@"例:C:\Users\{ユーザー名}\AppData\Roaming\.minecraft\1.7.10\world");
				TextIO.WriteStrings(Define.FilePathFile_Path, "");
				Console.ReadLine();
				Environment.Exit(0);
			}
			List<string> WorldsPath = FilePathFile.ReadFile_List(Define.FilePathFile_Path);
			ConsoleIO.WriteLine(WorldsPath.Count + " 個のファイルパスを読み取りました。");
			for (int i = 0; i < WorldsPath.Count; i++)
			{
				ConsoleIO.WriteLine("→ " + WorldsPath[i]);
			}

			ConsoleIO.WriteLine("インデックスファイルの読み込み中…");

			// 現時点でバックアップされているファイルのインデックスの最大値を検索
			int MaxIndex = 0;
			//backupフォルダがない場合、新規作成
			if (!Directory.Exists(Define.BackupDirectory))
			{
				Directory.CreateDirectory(Define.BackupDirectory);
			}
			else
			{
				int[] Indexes = Backup.SearchUsedIndex();
				if (Indexes.Length != 0)
				{
					for (int i = 0; i < Indexes.Length; i++)
					{
						if (MaxIndex < Indexes[i])
						{
							MaxIndex = Indexes[i];
						}
					}
				}
			}

			// Indexファイルが存在しない場合、WorldsPathを用いて新規作成
			if (!File.Exists(Define.IndexFile_Path))
			{
				ConsoleIO.WriteLine("インデックスファイルが存在しないので新規作成します。");
				IndexFile.CreateIndexFile(WorldsPath, MaxIndex);
			}

			IndexList = IndexFile.ReadIndexfile(out Header);
			if (IndexList == null)
			{
				ConsoleIO.WriteError("Indexファイルが異常です。初期化しますか？");
				ConsoleIO.WriteLine("y/N");
				if (Console.ReadLine() == "y")
				{
					ConsoleIO.WriteLine("(入力文字:y)");
					IndexFile.CreateIndexFile(WorldsPath, MaxIndex);
				}
				ConsoleIO.WriteLine("(入力文字:y以外)");
				IndexList = IndexFile.ReadIndexfile(out Header);

				if (IndexList == null)
				{
					ConsoleIO.WriteError("Indexファイルの新規作成に失敗しました。");
					Environment.Exit(0);
				}
			}

			ConsoleIO.WriteLine("Indexファイルの内容とFilePathファイルの内容の一致を確認中…");
			var ActiveIndex = IndexFile.ComparisonOfWorldPathFile(ref IndexList, WorldsPath, ref Header.MaxIndex, out int NewIndexNum);

			ConsoleIO.WriteLine("インデックスファイルの読み込み成功");
			ConsoleIO.WriteLine(NewIndexNum + " 個の新しいワールドデータ");
			IndexFile.CreateIndexFile(IndexList, ActiveIndex, Header);

			return ActiveIndex;
		}
	}
}
