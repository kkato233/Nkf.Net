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

### 配列APIの範囲指定

`WrapNkf.NkfConvertSafe` / `NkfConvertSafeWithOption` の入出力長は、それぞれの配列の長さ以下の非負の値を指定してください。
`WrapNkf.NkfConvert` / `NkfEncoding.NkfConvert` / `NkfEncoding.GetChars` / `NkfEncoding.GetCharCount` では、開始位置と長さが配列内に収まる必要があります。配列末尾を開始位置として長さ `0` を指定することもできます。

`null` の配列には `ArgumentNullException`、負数や配列の範囲外を指定した場合には `ArgumentOutOfRangeException` を送出します。従来の `NkfEncoding.NkfConvert` による範囲の自動切り詰めは行いません。内部のバッファーサイズ計算で整数オーバーフローが発生する場合は `OverflowException` を送出します。
ネイティブ関数に渡す配列は呼び出し中に固定し、例外発生時も固定と一時領域を解放します。

出力容量が不足した場合、`WrapNkf.NkfConvertSafe` / `NkfConvertSafeWithOption` は `false` を返し、`bytes` には必要な出力バイト数を設定します。`bytes` は書き込み済みの長さではありません。不足時にはそのサイズの配列を確保して再実行してください。
`WrapNkf.NkfConvert` と `NkfTextReader` は内部の出力配列を必要量に拡張して再試行します。`NkfEncoding.GetCharCount` は実際の変換後の文字数を計算し、`GetChars` は出力配列が不足していれば `ArgumentException` を送出します。

### オプションと並列呼び出しの安全性

変換オプションには、半角スペースで区切った表示可能なASCII文字を指定してください。NUL、タブ、改行、非ASCII文字は `ArgumentException` で拒否します。
情報表示用の `-v` / `-V` / `--help` / `--version` は利用できません。`-Swv` のような複合指定も拒否します。情報取得には `WrapNkf.GetNkfVersion()` / `WrapNkf.Usage()` を利用してください。
短いオプション内のハイフンは `-f60-10` のような数値引数に限ります。`--fb-java` などの長いオプションは利用できます。

`WrapNkf.SetNkfOption` の設定はスレッドごとに保持されます。未設定時、または `null` でリセットした場合はUTF-8出力です。
同梱のネイティブライブラリ（nkf 4278df3 以降）は変換状態をスレッドローカルに保持するため、異なるスレッドから並列に呼び出せます。オプション設定と変換は同じスレッド上で続けて実行します。

### nkf の機能を直接利用してファイル変換する

``` C#
// ファイルをUTF8 変換（入力ファイルのエンコードは未指定）
Nkf.Net.WrapNkf.SetNkfOption("-w");	// UTF-8
Nkf.Net.WrapNkf.FileConvert2(inFile, outFile);
```

入力または出力ファイルを開けない場合、`FileConvert1` / `FileConvert2` は `false` を返します。Linux の `FileConvert1` は排他的に作成した一時ファイルを使用し、変換に成功した場合のみ元のファイルを置き換えます。

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
