nkf32.dll を .NET から使いやすくするためのライブラリです。

ファイルのエンコードが UTF8 なのか SJIS なのか EUC なのか気にせず
文字列の読み込みをする事ができます。

Nkf.Net NuGet パッケージをインストールすることで利用できます。


【ファイルから読み込む例】

    // どんな文字コードでも読み込みできる。
    using (var sr = new Nkf.Net.NkfTextReader(fileName))
    {
        string s = sr.ReadToEnd();
        Console.WriteLine(s);
    }

【エンコードを気にせずバイト配列から文字列を取得できます。】

	string s = "漢字テスト";

	byte[] bUTF8 = System.Text.Encoding.UTF8.GetBytes(s);
	byte[] bSJIS = System.Text.Encoding.GetEncoding("SJIS").GetBytes(s);
	byte[] bEUC = System.Text.Encoding.GetEncoding("EUC-JP").GetBytes(s);

	// どんなエンコードのバイト配列でも自動認識して文字を取得できます。
	Nkf.Net.NkfEncoding enc = new NkfEncoding();

	string s1 = enc.GetString(bUTF8);
	string s2 = enc.GetString(bSJIS);
	string s3 = enc.GetString(bEUC);

	// 文字コード変換の確認
	Assert.AreEqual(s, s1);
	Assert.AreEqual(s, s2);
	Assert.AreEqual(s, s3);

【nkf の機能を直接利用する例】

	// バージョン番号表示
	Console.WriteLine(WrapNkf.GetNkfVersion());
	
	// ファイルをUTF8 変換（入力ファイルのエンコードは未指定）
	WrapNkf.SetNkfOption("-w");	// UTF-8
	WrapNkf.FileConvert2(inFile, outFile);

【ライセンス】
	
This software is released under the MIT License, see LICENSE.txt.
このソフトウェアは、MITライセンスのもとで公開されています。
LICENSE.txtをご覧ください。

同梱の nkf32.dll は gzip ライセンスのもと公開されています。
http://sourceforge.jp/projects/nkf/


