nkf32.dll を .NET から使いやすくするためのライブラリ。

nkfを StreamReader や Encodier として 利用することができます。

【コードの例】

	// どんな文字コードでも読み込み可能
	using (var sr = new Nkf.Net.NkfTextReader(fileName))
    {
        string s = sr.ReadToEnd();
		Console.WriteLine(s);
	}
	
	// どんなエンコードでも自動的に変換できる。
	Nkf.Net.NkfEncoding enc = new NkfEncoding();
	string s = "漢字テスト";

	byte[] bUTF8 = System.Text.Encoding.UTF8.GetBytes(s);
	byte[] bSJIS = System.Text.Encoding.GetEncoding("SJIS").GetBytes(s);
	byte[] bEUC = System.Text.Encoding.GetEncoding("EUC-JP").GetBytes(s);

	string s1 = enc.GetString(bUTF8);
	string s2 = enc.GetString(bSJIS);
	string s3 = enc.GetString(bEUC);

	Assert.AreEqual(s, s1);
	Assert.AreEqual(s, s2);
	Assert.AreEqual(s, s3);

	// nkf の機能を直接利用する
	Console.WriteLine(WrapNkf.GetNkfVersion());
	
	WrapNkf.SetNkfOption("-w");	// UTF-8
	WrapNkf.FileConvert2(inFile, outFile);

	WrapNkf.SetNkfOption("-s");	// SJIS	
	WrapNkf.FileConvert1(convertFile);
	
	