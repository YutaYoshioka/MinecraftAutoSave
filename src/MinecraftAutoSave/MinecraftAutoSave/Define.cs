using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftAutoSave
{
	class Define
	{
		/// <summary>
		/// ログファイル名(この名前の後に日時を追加)
		/// </summary>
		public static string LogFilePath1 = "MCASLog_";

		/// <summary>
		/// ログファイル名(この名前の前に日時を追加)
		/// </summary>
		public static string LogFilePath2 = ".log";

		/// <summary>
		/// ログファイル名(フル)
		/// </summary>
		public static string LogFilePath = "";

		/// <summary>
		/// Indexファイルの相対パス
		/// </summary>
		public static string IndexFile_Path = "Index.save";

		/// <summary>
		/// 設定ファイル名
		/// </summary>
		public static string SettingsFileName = "settings.ini";

		/// <summary>
		/// バージョン
		/// </summary>
		public static string MCAS_Version = "1.0.0";

		/// <summary>
		/// ファイルパスファイルへのパス
		/// </summary>
		public static string FilePathFile_Path = "FilePath.txt";

		/// <summary>
		/// バックアップファイルのディレクトリ(相対パス)(終端に\を付ける)
		/// </summary>
		public static string BackupDirectory = @"Backup\";

		/// <summary>
		/// バックアップの間隔[ms]
		/// </summary>
		public static int BackupInterval = 0;

		/// <summary>
		/// プログラム全体のループ間隔
		/// </summary>
		public static int LoopInterval = 100;
	}
}
