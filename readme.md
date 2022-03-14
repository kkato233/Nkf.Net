## Nkf.Net 

���̂������Â������R�[�h�ϊ��v���O�����ł��� nkf �� .NET ����ȒP�ɗ��p���邽�߂̃��C�u�����ł�.

## �@�\

Windows  x86/x64 / Linux x64 ���� ���삵�܂��B

���͂��ꂽ���{��e�L�X�g�� JIS SJIS EUC UTF-8 ���� �����I�ɔF�����ēǂݎ�肷�鎖���ł��܂��B

�v���O�����̒��� ���̃t�@�C���� �G���R�[�h���C�ɂ��鎖�Ȃ� ���{��t�@�C����ǂݍ��ގ����ł��܂��B

## �R�[�h��

### ���O����

`Nkf.Net` nuget �p�b�P�[�W��ǉ����Ă��������B


### �t�@�C������ǂݍ���

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

### �G���R�[�h���C�ɂ��� �o�C�g�z��� ���{��ɕϊ����܂�

``` C#

string s = "�����e�X�g";

byte[] bUTF8 = System.Text.Encoding.UTF8.GetBytes(s);
byte[] bSJIS = System.Text.Encoding.GetEncoding("SJIS").GetBytes(s);
byte[] bEUC = System.Text.Encoding.GetEncoding("EUC-JP").GetBytes(s);

// �ǂ�ȃG���R�[�h�̃o�C�g�z��ł������F�����ĕ������擾�ł��܂��B
Nkf.Net.NkfEncoding enc = new Nkf.Net.NkfEncoding();

string s1 = enc.GetString(bUTF8);
string s2 = enc.GetString(bSJIS);
string s3 = enc.GetString(bEUC);

Console.WriteLine(s1);
Console.WriteLine(s2);
Console.WriteLine(s3);
```

### nkf �̋@�\�𒼐ڗ��p���ăt�@�C���ϊ�����

``` C#
// �t�@�C����UTF8 �ϊ��i���̓t�@�C���̃G���R�[�h�͖��w��j
WrapNkf.SetNkfOption("-w");	// UTF-8
WrapNkf.FileConvert2(inFile, outFile);
```

## ���C�Z���X

This software is released under the MIT License, see LICENSE.txt.
���̃\�t�g�E�F�A�́AMIT���C�Z���X�̂��ƂŌ��J����Ă��܂��B

LICENSE.txt���������������B

������ nkf32.dll �� gzip ���C�Z���X�̂��ƌ��J����Ă��܂��B

������ nkf32.dll �̓I���W�i���� nkf32.dll 

http://sourceforge.jp/projects/nkf/

�Ɂu�}���`�X���b�h�Ή��v�ux86/x64�ǂ���ł�����\�v�̋@�\��ǉ������o�[�W�����𗘗p���Ă��܂��B

�@�\�ǉ��ł̃\�[�X�Ǘ�URL

https://github.com/kkato233/nkf
