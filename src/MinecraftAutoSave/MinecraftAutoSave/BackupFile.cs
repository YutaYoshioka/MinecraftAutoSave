using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FileIO;

namespace MinecraftAutoSave
{
	class Backup
	{
		/// <summary>
		/// バックアップファイルで使用されているインデックスの一覧を返します。
		/// </summary>
		/// <returns>使用しているインデックス</returns>
		public static int[] SearchUsedIndex()
		{
			string[] BuckupFolders = System.IO.Directory.GetDirectories(Define.BackupDirectory, "*");
			var Indexes = new List<int>();
			for (int i = 0; i < BuckupFolders.Length; i++)
			{
				BuckupFolders[i] = BuckupFolders[i].Replace(Define.BackupDirectory, "");
				if (int.TryParse(BuckupFolders[i], out int j))
				{
					Indexes.Add(j);
				}
			}

			return Indexes.ToArray();
		}

		/// <summary>
		/// Indexのリストのすべてのバックアップを作成します。
		/// </summary>
		/// <param name="IndexList">バックアップ対象のIndex</param>
		/// <param name="AnactiveIndex">無効化されているIndex</param>
		/// <returns></returns>
		public static bool CreateAllBackup(ref List<Index> IndexList, List<Index> AnactiveIndex)
		{
			for (int i = 0; i < IndexList.Count; i++)
			{
				// 最終更新時刻が前回のバックアップ時刻より後ならバックアップ
				if (File.GetLastWriteTime(IndexList[i].FilePath).Ticks > IndexList[i].LastBackupTime)
				{
					// Indexのフォルダがなければ新規作成
					if (!Directory.Exists(Define.BackupDirectory + IndexList[i].IndexNum))
					{
						Directory.CreateDirectory(Define.BackupDirectory + IndexList[i].IndexNum);
					}
					Index OneIndex = IndexList[i];
					CreateBackup(ref OneIndex);
					IndexList[i] = OneIndex;
				}
			}

			return true;
		}

		/// <summary>
		/// Indexのリストのすべてのバックアップを作成します。
		/// </summary>
		/// <param name="IndexList">バックアップ対象のIndex</param>
		/// <param name="AnactiveIndex">無効化されているIndex</param>
		/// <param name="Forced">trueでバックアップの作成判断を無視し、強制的に作成します。</param>
		/// <returns></returns>
		public static bool CreateAllBackup(ref List<Index> IndexList, List<Index> AnactiveIndex, bool Forced)
		{
			if (Forced)
			{
				for (int i = 0; i < IndexList.Count; i++)
				{
					// Indexのフォルダがなければ新規作成
					if (!Directory.Exists(Define.BackupDirectory + IndexList[i].IndexNum))
					{
						Directory.CreateDirectory(Define.BackupDirectory + IndexList[i].IndexNum);
					}
					Index OneIndex = IndexList[i];
					CreateBackup(ref OneIndex);
					IndexList[i] = OneIndex;
				}
			}
			else
			{
				CreateAllBackup(ref IndexList, AnactiveIndex);
			}

			return true;
		}

		/// <summary>
		/// 渡されたIndexのバックアップを作成します。
		/// </summary>
		/// <param name="BackupIndex">バックアップ対象のIndex</param>
		/// <returns>成功:true</returns>
		public static bool CreateBackup(ref Index BackupIndex)
		{
			var BackupTime = DateTime.Now;
			BackupIndex.LastBackupTime = BackupTime.Ticks;
			// Files.FilecopyOfDirectory(BackupIndex.FilePath, Define.BackupDirectory + BackupIndex.IndexNum + @"\" + BackupIndex.LastBackupTime);

			Index BackupIndex_local = BackupIndex;

			Task.Run(() => 
				CreateZipBackup(BackupIndex_local, BackupIndex_local.FilePath + " のバックアップを作成しました(時刻: " + BackupTime.ToString() + " (" + BackupIndex_local.LastBackupTime + "))")
			);

			BackupIndex.LastBackupfileTime = File.GetLastWriteTime(BackupIndex.FilePath).Ticks;
			ConsoleIO.WriteLine("バックアップを作成中です");
			return true;
		}


		static void CreateZipBackup(Index BackupIndex_local,string ResultMessage)
		{
			Files.FilecopyOfDirectory(BackupIndex_local.FilePath, Define.BackupDirectory + BackupIndex_local.IndexNum + @"\" + BackupIndex_local.LastBackupTime);
			System.IO.Compression.ZipFile.CreateFromDirectory(
				Define.BackupDirectory + BackupIndex_local.IndexNum + @"\" + BackupIndex_local.LastBackupTime,
				Define.BackupDirectory + BackupIndex_local.IndexNum + @"\" + BackupIndex_local.LastBackupTime + ".zip",
				System.IO.Compression.CompressionLevel.Optimal,
				false,
				Encoding.GetEncoding("shift_jis"));
			Directory.Delete(Define.BackupDirectory + BackupIndex_local.IndexNum + @"\" + BackupIndex_local.LastBackupTime, true);
			ConsoleIO.WriteLine(ResultMessage);
		}
	}
}
