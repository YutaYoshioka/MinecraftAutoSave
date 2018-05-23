using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FileIO
{
	/// <summary>
	/// テキスト形式(Unicode)でのファイル読み書き
	/// </summary>
	class TextIO
	{
		/// <summary>
		/// 1行ごとのList形式でファイルの内容を読み取ります。outで参照渡しすることでファイルの有無のbool値になります。
		/// </summary>
		/// <param name="FilePath">ファイルへのパス</param>
		/// <returns>ファイルの内容</returns>
		public static List<string> ReadStrings(string FilePath)
		{
			using (StreamReader file = new StreamReader(FilePath, Encoding.Unicode))
			{
				string line = "";
				List<string> list = new List<string>(); // 空のListを作成する

				// 1行ずつ読み込んでいき、末端(何もない行)までwhile文で繰り返す
				while ((line = file.ReadLine()) != null)
				{
					list.Add(line);
				}

				return list;
			}
		}

		/// <summary>
		/// 1行ごとのList形式でファイルの内容を読み取ります。outで参照渡しすることでファイルの有無のbool値になります。
		/// </summary>
		/// <param name="FilePath">ファイルへのパス</param>
		/// <param name="strings">ファイルの内容</param>
		/// <returns>ファイルの有無(成功:true, 失敗:false)</returns>
		public static bool ReadStrings(string FilePath, out List<string> strings)
		{
			strings = new List<string>(); // 空のListを作成する

			// ファイルが存在しない場合、falseを返す。
			if (!File.Exists(FilePath))
			{
				return false;
			}

			using (StreamReader file = new StreamReader(FilePath, Encoding.Unicode))
			{
				string line = "";

				// 1行ずつ読み込んでいき、末端(何もない行)までwhile文で繰り返す
				while ((line = file.ReadLine()) != null)
				{
					strings.Add(line);
				}

				return true;
			}
		}


		/// <summary>
		/// ファイルに書き込みます。appendを指定しない場合、データはファイルに追加されます。
		/// </summary>
		/// <param name="FilePath">ファイルへのパス</param>
		/// <param name="strings">書き込み内容(1行ごとのstring配列)</param>
		/// <returns>成功:true、失敗:false</returns>
		public static bool WriteStrings(string FilePath, string[] strings)
		{
			bool r = true;
			using (StreamWriter file = new StreamWriter(FilePath, true, Encoding.Unicode))
			{
				for (int i = 0; i < strings.Length; i++)
				{
					file.WriteLine(strings[i]);
				}
			}

			return r;
		}

		/// <summary>
		/// ファイルに書き込みます。
		/// </summary>
		/// <param name="FilePath">ファイルへのパス</param>
		/// <param name="strings">書き込み内容(1行ごとのstring配列)</param>
		/// <param name="append">データをファイルに追加する場合は true、ファイルを上書きする場合は false。</param>
		/// <returns>成功:true、失敗:false</returns>
		public static bool WriteStrings(string FilePath, string[] strings, bool append)
		{
			bool r = true;
			using (StreamWriter file = new StreamWriter(FilePath, append, Encoding.Unicode))
			{
				for (int i = 0; i < strings.Length; i++)
				{
					file.WriteLine(strings[i]);
				}
			}

			return r;
		}

		/// <summary>
		/// ファイルに書き込みます。appendを指定しない場合、データはファイルに追加されます。
		/// </summary>
		/// <param name="FilePath">ファイルへのパス</param>
		/// <param name="strings">書き込み内容(1行ごとのList形式)</param>
		/// <returns>成功:true、失敗:false</returns>
		public static bool WriteStrings(string FilePath, List<string> strings)
		{
			bool r = true;
			using (StreamWriter file = new StreamWriter(FilePath, true, Encoding.Unicode))
			{
				string[] line = strings.ToArray();
				for (int i = 0; i < line.Length; i++)
				{
					file.WriteLine(line[i]);
				}
			}

			return r;
		}

		/// <summary>
		/// ファイルに書き込みます。
		/// </summary>
		/// <param name="FilePath">ファイルへのパス</param>
		/// <param name="strings">書き込み内容(1行ごとのList形式)</param>
		/// <param name="append">データをファイルに追加する場合は true、ファイルを上書きする場合は false。</param>
		/// <returns>成功:true、失敗:false</returns>
		public static bool WriteStrings(string FilePath, List<string> strings, bool append)
		{
			bool r = true;
			using (StreamWriter file = new StreamWriter(FilePath, append, Encoding.Unicode))
			{
				string[] line = strings.ToArray();
				for (int i = 0; i < line.Length; i++)
				{
					file.WriteLine(line[i]);
				}
			}

			return r;
		}

		/// <summary>
		/// ファイルに書き込みます。appendを指定しない場合、データはファイルに追加されます。
		/// </summary>
		/// <param name="FilePath">ファイルへのパス</param>
		/// <param name="strings">書き込み内容</param>
		/// <returns>成功:true、失敗:false</returns>
		public static bool WriteStrings(string FilePath, string strings)
		{
			bool r = true;
			using (StreamWriter file = new StreamWriter(FilePath, true, Encoding.Unicode))
			{
				if (strings != "")
				{
					file.WriteLine(strings);
				}
			}

			return r;
		}

		/// <summary>
		/// ファイルに書き込みます。
		/// </summary>
		/// <param name="FilePath">ファイルへのパス</param>
		/// <param name="strings">書き込み内容</param>
		/// <param name="append">データをファイルに追加する場合は true、ファイルを上書きする場合は false。</param>
		/// <returns>成功:true、失敗:false</returns>
		public static bool WriteStrings(string FilePath, string strings, bool append)
		{
			bool r = true;
			using (StreamWriter file = new StreamWriter(FilePath, append, Encoding.Unicode))
			{
				if (strings != "")
				{
					file.WriteLine(strings);
				}
			}

			return r;
		}


		/// <summary>
		/// ファイルの末尾に文字列を追加します。
		/// また、末尾に改行コードを書き込みます。
		/// </summary>
		/// <param name="FilePath">ファイルパス</param>
		/// <param name="AddString">追加する文字列</param>
		/// <returns>成功:true</returns>
		[Obsolete("このメソッドの使用は推薦しません。代わりに WriteStrings() の使用を推薦します。")]
		public static bool AddString(string FilePath, string AddString)
		{
			bool r = true;
			using (StreamWriter file = new StreamWriter(FilePath, true, Encoding.Unicode))
			{
				file.WriteLine(AddString);
			}

			return r;
		}

		/// <summary>
		/// ファイルの末尾に文字列を追加します。
		/// また、末尾に改行コードを書き込みます。
		/// </summary>
		/// <param name="FilePath">ファイルパス</param>
		/// <param name="AddString">追加する文字列</param>
		/// <param name="append">データをファイルに追加する場合は true、ファイルを上書きする場合は false。</param>
		/// <returns>成功:true</returns>
		[Obsolete("このメソッドの使用は推薦しません。代わりに WriteStrings() の使用を推薦します。")]
		public static bool AddString(string FilePath, string AddString, bool append)
		{
			bool r = true;
			using (StreamWriter file = new StreamWriter(FilePath, append, Encoding.Unicode))
			{
				file.WriteLine(AddString);
			}

			return r;
		}
	}

	/// <summary>
	/// ファイル関連メソッド
	/// </summary>
	class Files
	{
		/// <summary>
		/// 指定したディレクトリ内のすべてのファイル・フォルダをコピーします。(aとxを指定、a\b\... → x\b\...)
		/// </summary>
		/// <param name="OriginalDirectory">コピー元のディレクトリ</param>
		/// <param name="TargetDirectory">コピー先のディレクトリ</param>
		/// <returns>成功:true</returns>
		public static bool FilecopyOfDirectory(string OriginalDirectory, string TargetDirectory)
		{
			bool b = true;
			int OriginalName_Count = OriginalDirectory.Count();
			if (!Directory.Exists(OriginalDirectory))
			{
				return false;
			}

			// すべてのフォルダとファイルを取得
			string[] FileAndFolder = Directory.GetDirectories(OriginalDirectory, "*", SearchOption.AllDirectories);
			for (int i = 0; i < FileAndFolder.Length; i++)
			{
				Directory.CreateDirectory(TargetDirectory + FileAndFolder[i].Remove(0, OriginalName_Count));
			}
			FileAndFolder = Directory.GetFiles(OriginalDirectory, "*", SearchOption.AllDirectories);
			for (int i = 0; i < FileAndFolder.Length; i++)
			{
				File.Copy(FileAndFolder[i], TargetDirectory + FileAndFolder[i].Remove(0, OriginalName_Count));
			}

			return b;
		}
	}
}

