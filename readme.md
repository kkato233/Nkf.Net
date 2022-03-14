## Nkf.Net 

ものすごく古い漢字コード変換プログラムである nkf を .NET から簡単に利用するためのライブラリです.

## 機能

Windows  x86/x64 / Linux x64 環境で 動作します。

入力された日本語テキストが JIS SJIS EUC UTF-8 等を 自動的に認識して読み取りする事ができます。

プログラムの中で そのファイルの エンコードを気にする事なく 日本語ファイルを読み込む事ができます。

## コード例

### 事前準備

`Nkf.Net` nuget パッケージを追加してください。


### ファイルから読み込み

``` C#
using (var sr = new Nkf.Net.NkfTextReader(fileName))
{
    string s = sr.ReadLine();
    while(s != null) {
        Console.WriteLine(s);
        s = sr.ReadLine();
    }
}
```

### エンコードを気にせず バイト配列を 日本語に変換します

``` C#

string s = "漢字テスト";

byte[] bUTF8 = System.Text.Encoding.UTF8.GetBytes(s);
byte[] bSJIS = System.Text.Encoding.GetEncoding("SJIS").GetBytes(s);
byte[] bEUC = System.Text.Encoding.GetEncoding("EUC-JP").GetBytes(s);

// どんなエンコードのバイト配列でも自動認識して文字を取得できます。
Nkf.Net.NkfEncoding enc = new Nkf.Net.NkfEncoding();

string s1 = enc.GetString(bUTF8);
string s2 = enc.GetString(bSJIS);
string s3 = enc.GetString(bEUC);

Console.WriteLine(s1);
Console.WriteLine(s2);
Console.WriteLine(s3);
```

### nkf の機能を直接利用してファイル変換する

``` C#
// ファイルをUTF8 変換（入力ファイルのエンコードは未指定）
Nkf.Net.WrapNkf.SetNkfOption("-w");	// UTF-8
Nkf.Net.WrapNkf.FileConvert2(inFile, outFile);
```

## ライセンス

This software is released under the MIT License, see LICENSE.txt.
このソフトウェアは、MITライセンスのもとで公開されています。

LICENSE.txtをご覧ください。

同梱の nkf32.dll は gzip ライセンスのもと公開されています。

同梱の nkf32.dll はオリジナルの nkf32.dll 

http://sourceforge.jp/projects/nkf/

に「マルチスレッド対応」「x86/x64どちらでも動作可能」の機能を追加したバージョンを利用しています。

機能追加版のソース管理URL

https://github.com/kkato233/nkf
