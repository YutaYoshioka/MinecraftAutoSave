using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileIO;
using Microsoft.VisualBasic.FileIO;

namespace MinecraftAutoSave
{

	public struct IndexHeader
	{
		/// <summary>
		/// バージョン
		/// </summary>
		public string MCAS_Ver;

		/// <summary>
		/// 使用済みインデックスの最大値
		/// </summary>
		public int MaxIndex;
	}

	/// <summary>
	/// バックアップファイル関連情報の構造体
	/// </summary>
	public struct Index
	{
		/// <summary>
		/// ファイルパス(絶対パス)
		/// </summary>
		public string FilePath;

		/// <summary>
		/// インデックス(1以上)
		/// </summary>
		public int IndexNum;

		/// <summary>
		/// 前回のバックアップ時間(0で取得失敗)
		/// </summary>
		public long LastBackupTime;

		/// <summary>
		/// 前回のバックアップファイルの更新日時(0で取得失敗)
		/// </summary>
		public long LastBackupfileTime;

		/// <summary>
		/// 要素からIndexを生成します。
		/// </summary>
		/// <param name="FilePath">ファイルパス(絶対パス)</param>
		/// <param name="IndexNum">インデックス(1以上)</param>
		/// <param name="LastBackupTime">前回のバックアップ時間(0で取得失敗)</param>
		/// <param name="LastBackupfileTime">前回のバックアップファイルの更新日時(0で取得失敗)</param>
		/// <returns></returns>
		public static Index Create(string FilePath, int IndexNum, long LastBackupTime, long LastBackupfileTime) => new Index
		{
			FilePath = FilePath,
			IndexNum = IndexNum,
			LastBackupTime = LastBackupTime,
			LastBackupfileTime = LastBackupfileTime
		};
	}

	/// <summary>
	/// インデックスファイル関連メソッドです。
	/// </summary>
	class IndexFile
	{
		// ファイルの仕様
		// [Index.save]
		// 0. "MCAS_Index_x.x.x"
		// 1. "null"(予約)
		// 2. 使用済みのインデックスの最大値
		// 3. "null"(予約)
		// 4. "EOH"(ヘッダー終了)
		// 5. ファイルパス(絶対パス)
		// 6. インデックス(1以上のint)
		// 7. 前回のバックアップ時間
		// 8. 前回のバックアップファイルの更新日時
		// 9. "null"(予約)
		// 10.ファイルパス(絶対パス)
		// 11.インデックス(1以上のint)
		// 12.前回のバックアップ時間
		// 13.前回のバックアップファイルの更新日時
		// 14."null"(予約)


		/// <summary>
		/// Indexファイルを作成(更新)します。既に存在する場合は上書きします。
		/// </summary>
		/// <param name="IndexList">Indexリスト</param>
		/// <param name="Header">Indexファイルのヘッダー</param>
		/// <returns>成功:true、失敗:false</returns>
		public static bool CreateIndexFile(List<Index> IndexList, IndexHeader Header)
		{
			var Strings = new List<string>();
			// ヘッダを作成
			Strings.Add(Header.MCAS_Ver);
			Strings.Add("null");
			Strings.Add(Header.MaxIndex.ToString());
			Strings.Add("null");
			Strings.Add("EOH");

			// IndexListがnullならファイルのみ作成
			if (IndexList != null)
			{
				for (int i = 0; i < IndexList.Count; i++)
				{
					Strings.Add(IndexList[i].FilePath);
					Strings.Add(IndexList[i].IndexNum.ToString());
					Strings.Add(IndexList[i].LastBackupTime.ToString());
					Strings.Add(IndexList[i].LastBackupfileTime.ToString());
					Strings.Add("null");
				}
			}
			TextIO.WriteStrings(Define.IndexFile_Path, Strings, false);

			return true;
		}

		/// <summary>
		/// Indexファイルを作成(更新)します。既に存在する場合は上書きします。
		/// </summary>
		/// <param name="IndexList1">Indexリスト1</param>
		/// <param name="IndexList2">Indexリスト2</param>
		/// <param name="Header">Indexファイルのヘッダー</param>
		/// <returns>成功:true、失敗:false</returns>
		public static bool CreateIndexFile(List<Index> IndexList1, List<Index> IndexList2, IndexHeader Header)
		{
			var Strings = new List<string>();
			// ヘッダを作成
			Strings.Add(Header.MCAS_Ver);
			Strings.Add("null");
			Strings.Add(Header.MaxIndex.ToString());
			Strings.Add("null");
			Strings.Add("EOH");

			// IndexListがnullならファイルのみ作成
			if (IndexList1 != null)
			{
				for (int i = 0; i < IndexList1.Count; i++)
				{
					Strings.Add(IndexList1[i].FilePath);
					Strings.Add(IndexList1[i].IndexNum.ToString());
					Strings.Add(IndexList1[i].LastBackupTime.ToString());
					Strings.Add(IndexList1[i].LastBackupfileTime.ToString());
					Strings.Add("null");
				}
			}
			if (IndexList2 != null)
			{
				for (int i = 0; i < IndexList2.Count; i++)
				{
					Strings.Add(IndexList2[i].FilePath);
					Strings.Add(IndexList2[i].IndexNum.ToString());
					Strings.Add(IndexList2[i].LastBackupTime.ToString());
					Strings.Add(IndexList2[i].LastBackupfileTime.ToString());
					Strings.Add("null");
				}
			}
			TextIO.WriteStrings(Define.IndexFile_Path, Strings, false);

			return true;
		}

		/// <summary>
		/// Indexファイルを作成(更新)します。既に存在する場合は上書きします。
		/// </summary>
		/// <param name="WorldsPath">ワールドデータのパスのリスト</param>
		/// <param name="MaxIndex">使用済みインデックスの最大値</param>
		/// <returns>成功:true、失敗:false</returns>
		public static bool CreateIndexFile(List<string> WorldsPath, int MaxIndex)
		{
			var Strings = new List<string>();

			// ヘッダを作成
			Strings.Add("MCAS_Index_" + Define.MCAS_Version);
			Strings.Add("null");
			Strings.Add("0");
			Strings.Add("null");
			Strings.Add("EOH");

			int i = 0;
			// WorldPathがnullならファイルのみ作成
			if (WorldsPath != null)
			{
				for (i = 0; i < WorldsPath.Count; i++)
				{
					Strings.Add(WorldsPath[i]);
					Strings.Add((i + MaxIndex + 1).ToString());
					Strings.Add("0");
					Strings.Add("0");
					Strings.Add("null");
				}
			}
			Strings[2] = (i + MaxIndex).ToString();

			TextIO.WriteStrings(Define.IndexFile_Path, Strings);

			return true;
		}

		/// <summary>
		/// Indexファイル読み込み、IndexのListへ格納します。
		/// </summary>
		/// <param name="Header">Indexファイルのヘッダー</param>
		/// <returns>Indexファイルの読み込み結果(失敗:null)</returns>
		public static List<Index> ReadIndexfile(out IndexHeader Header)
		{
			var IndexList = new List<Index>();
			Header = new IndexHeader();
			var IndexFile = TextIO.ReadStrings(Define.IndexFile_Path);
			int i = 0;

			// ヘッダー解析
			Header.MCAS_Ver = IndexFile[0];
			if (Header.MCAS_Ver != "MCAS_Index_" + Define.MCAS_Version)
			{
				ConsoleIO.WriteError("Indexファイルのバージョンが一致しません。");
				return null;
			}

			if (!int.TryParse(IndexFile[2], out Header.MaxIndex))
			{
				ConsoleIO.WriteError("Indexファイルのインデックスの最大値の読み込みに失敗しました。");
				return null;
			}

			// EOH の要素を検索し、それを次の要素番号にセット
			i = 3;
			while (IndexFile[i] != "EOH")
			{
				i++;
			}
			i++;


			for (; i < IndexFile.Count; i += 5)
			{
				var OneIndex = new Index();

				OneIndex.FilePath = IndexFile[i];
				if (!int.TryParse(IndexFile[i + 1], out OneIndex.IndexNum))
				{
					OneIndex.LastBackupTime = Header.MaxIndex + 1;
					Header.MaxIndex++;
					ConsoleIO.WriteWarning("Indexファイルの" + IndexFile[i] + "のインデックスの読み込みに失敗しました。");
				}
				if (!long.TryParse(IndexFile[i + 2], out OneIndex.LastBackupTime))
				{
					OneIndex.LastBackupTime = 0;
					ConsoleIO.WriteWarning("Indexファイルの" + IndexFile[i] + "の前回のバックアップ時間の読み込みに失敗しました。");
				}
				if (!long.TryParse(IndexFile[i + 3], out OneIndex.LastBackupfileTime))
				{
					OneIndex.LastBackupTime = 0;
					ConsoleIO.WriteWarning("Indexファイルの" + IndexFile[i] + "の前回のバックアップファイルの更新日時の読み込みに失敗しました。");
				}

				IndexList.Add(OneIndex);
			}


			/*
			List<string> IndexStrings = TextIO.ReadStrings(Define.IndexFilePath);
			List<Index> IndexList;
			for (int i = 0; i < IndexStrings.Count; i++)
			{
				Index 
				IndexList[i].FilePath = IndexStrings[i];
				if(!(long.TryParse(IndexStrings[i + 1], out IndexList[i].LastBackupTime)))
				{
					IndexList[i].LastBackupTime = 0;
					ConsoleIO.WriteWarning("Warning: Indexファイルの" + IndexStrings[i] + "の前回のバックアップ時間の読み込みに失敗しました。");
				}
				if(!(long.TryParse(IndexStrings[i + 2], out IndexList[i].LastBackupfileTime)))
				{
					IndexList[i].LastBackupTime = 0;
					ConsoleIO.WriteWarning("Warning: Indexファイルの" + IndexStrings[i] + "の前回のバックアップファイルの更新日時の読み込みに失敗しました。");
				}
			}
			*/

			return IndexList;
		}

		/// <summary>
		/// ワールドデータパスに対するIndexFileの過不足を判断し、有効なIndexを返します。ワールドデータパスにあってIndexFileにない場合、新たにIndexを追加します。
		/// </summary>
		/// <param name="IndexFile">Indexファイルの読み込み結果</param>
		/// <param name="WorldsPath">ワールドデータへのパス</param>
		/// <param name="MaxIndex">使用済みインデックスの最大値</param>
		/// <param name="NewIndexNum">Indexファイルにないワールドデータの数</param>
		/// <returns>有効なIndex</returns>
		public static List<Index> ComparisonOfWorldPathFile(ref List<Index> IndexFile, List<string> WorldsPath, ref int MaxIndex, out int NewIndexNum)
		{
			var ActiveIndex = new List<Index>();
			NewIndexNum = 0;
			for (int i = 0; i < WorldsPath.Count; i++)
			{
				int IndexNum = -1;
				for (int j = 0; j < IndexFile.Count; j++)
				{
					if (IndexFile[j].FilePath == WorldsPath[i])
					{
						IndexNum = j;
						break;
					}
				}

				if (IndexNum != -1)
				{
					ActiveIndex.Add(IndexFile[IndexNum]);
					IndexFile.RemoveAt(IndexNum);
				}
				else
				{
					NewIndexNum++;
					ActiveIndex.Add(new Index
					{
						FilePath = WorldsPath[i],
						IndexNum = ++MaxIndex,
						LastBackupTime = 0,
						LastBackupfileTime = 0
					});
				}
			}

			return ActiveIndex;
		}

		/// <summary>
		/// Indexの要素からIndex型を生成します。
		/// </summary>
		/// <param name="FilePath"></param>
		/// <param name="LastBackupTime"></param>
		/// <param name="LastBackupfileTime"></param>
		/// <returns></returns>
		public static Index CreateIndex(string FilePath, long LastBackupTime, long LastBackupfileTime)
		{
			return new Index
			{
				FilePath = FilePath,
				LastBackupTime = LastBackupTime,
				LastBackupfileTime = LastBackupfileTime
			};
		}
	}
}
