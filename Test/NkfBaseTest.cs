using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Nkf.Net.Test
{
    /// <summary>
    /// 基本的な動作テスト
    /// </summary>
    [TestClass]
    public class NkfBaseTest
    {
        [TestMethod]
        public void Test_Convert()
        {
            LoadTestData();

            Test("JIS to JIS ...", "-j", example["jis"], example["jis"]);
            Test("JIS to SJIS...", "-s", example["jis"], example["sjis"]);
            Test("JIS to EUC ...", "-e", example["jis"], example["euc"]);
            Test("JIS to UTF8...", "-w", example["jis"], example["utf8N"]);
            Test("JIS to U16L...", "-w16L", example["jis"], example["u16L"]);
            Test("JIS to U16B...", "-w16B", example["jis"], example["u16B"]);
            Test("JIS to JIS ...", "--ic=iso-2022-jp --oc=iso-2022-jp", example["jis"], example["jis"]);
            Test("JIS to SJIS...", "--ic=iso-2022-jp --oc=shift_jis", example["jis"], example["sjis"]);
            Test("JIS to EUC ...", "--ic=iso-2022-jp --oc=euc-jp", example["jis"], example["euc"]);
            Test("JIS to UTF8...", "--ic=iso-2022-jp --oc=utf-8n", example["jis"], example["utf8N"]);
            Test("JIS to U16L...", "--ic=iso-2022-jp --oc=utf-16le-bom", example["jis"], example["u16L"]);
            Test("JIS to U16B...", "--ic=iso-2022-jp --oc=utf-16be-bom", example["jis"], example["u16B"]);

            // From SJIS

            Test("SJIS to JIS ...", "-j", example["sjis"], example["jis"]);
            Test("SJIS to SJIS...", "-s", example["sjis"], example["sjis"]);
            Test("SJIS to EUC ...", "-e", example["sjis"], example["euc"]);
            Test("SJIS to UTF8...", "-w", example["sjis"], example["utf8N"]);
            Test("SJIS to U16L...", "-w16L", example["sjis"], example["u16L"]);
            Test("SJIS to U16B...", "-w16B", example["sjis"], example["u16B"]);
            Test("SJIS to JIS ...", "--ic=shift_jis --oc=iso-2022-jp", example["sjis"], example["jis"]);
            Test("SJIS to SJIS...", "--ic=shift_jis --oc=shift_jis", example["sjis"], example["sjis"]);
            Test("SJIS to EUC ...", "--ic=shift_jis --oc=euc-jp", example["sjis"], example["euc"]);
            Test("SJIS to UTF8...", "--ic=shift_jis --oc=utf-8n", example["sjis"], example["utf8N"]);
            Test("SJIS to U16L...", "--ic=shift_jis --oc=utf-16le-bom", example["sjis"], example["u16L"]);
            Test("SJIS to U16B...", "--ic=shift_jis --oc=utf-16be-bom", example["sjis"], example["u16B"]);

            // From EUC
            
            Test("EUC to JIS ...", "-j", example["euc"], example["jis"]);
            Test("EUC to SJIS...", "-s", example["euc"], example["sjis"]);
            Test("EUC to EUC ...", "-e", example["euc"], example["euc"]);
            Test("EUC to UTF8...", "-w", example["euc"], example["utf8N"]);
            Test("EUC to U16L...", "-w16L", example["euc"], example["u16L"]);
            Test("EUC to U16B...", "-w16B", example["euc"], example["u16B"]);
            Test("EUC to JIS ...", "--ic=euc-jp --oc=iso-2022-jp", example["euc"], example["jis"]);
            Test("EUC to SJIS...", "--ic=euc-jp --oc=shift_jis", example["euc"], example["sjis"]);
            Test("EUC to EUC ...", "--ic=euc-jp --oc=euc-jp", example["euc"], example["euc"]);
            Test("EUC to UTF8...", "--ic=euc-jp --oc=utf-8n", example["euc"], example["utf8N"]);
            Test("EUC to U16L...", "--ic=euc-jp --oc=utf-16le-bom", example["euc"], example["u16L"]);
            Test("EUC to U16B...", "--ic=euc-jp --oc=utf-16be-bom", example["euc"], example["u16B"]);

            // # From UTF8
                        
            Test("UTF8 to JIS ...","-j",	example["utf8N"],example["jis"]);
            Test("UTF8 to SJIS...","-s",	example["utf8N"],example["sjis"]);
            Test("UTF8 to EUC ...","-e",	example["utf8N"],example["euc"]);
            Test("UTF8 to UTF8N..","-w",	example["utf8N"],example["utf8N"]);
            Test("UTF8 to UTF8...","-w8",	example["utf8N"],example["utf8"]);
            Test("UTF8 to UTF8N..","-w80",	example["utf8N"],example["utf8N"]);
            Test("UTF8 to U16L...","-w16L",	example["utf8N"],example["u16L"]);
            Test("UTF8 to U16L0..","-w16L0",	example["utf8N"],example["u16L0"]);
            Test("UTF8 to U16B...","-w16B",	example["utf8N"],example["u16B"]);
            Test("UTF8 to U16B0..","-w16B0",	example["utf8N"],example["u16B0"]);
            Test("UTF8 to JIS ...","--ic=utf-8 --oc=iso-2022-jp",	example["utf8N"],example["jis"]);
            Test("UTF8 to SJIS...","--ic=utf-8 --oc=shift_jis",	example["utf8N"],example["sjis"]);
            Test("UTF8 to EUC ...","--ic=utf-8 --oc=euc-jp",		example["utf8N"],example["euc"]);
            Test("UTF8 to UTF8N..","--ic=utf-8 --oc=utf-8",		example["utf8N"],example["utf8N"]);
            Test("UTF8 to UTF8BOM","--ic=utf-8 --oc=utf-8-bom",	example["utf8N"],example["utf8"]);
            Test("UTF8 to UTF8N..","--ic=utf-8 --oc=utf-8n",		example["utf8N"],example["utf8N"]);
            Test("UTF8 to U16L...","--ic=utf-8 --oc=utf-16le-bom",	example["utf8N"],example["u16L"]);
            Test("UTF8 to U16L0..","--ic=utf-8 --oc=utf-16le",		example["utf8N"],example["u16L0"]);
            Test("UTF8 to U16B...","--ic=utf-8 --oc=utf-16be-bom",	example["utf8N"],example["u16B"]);
            Test("UTF8 to U16B0..","--ic=utf-8 --oc=utf-16be",		example["utf8N"],example["u16B0"]);

            Test("UTF8 to UTF8...","-w","\xf0\xa0\x80\x8b","\xf0\xa0\x80\x8b");

            // From JIS
            
            Test("JIS  to JIS ...","-j",example["jis1"],example["jis1"]);
            Test("JIS  to SJIS...","-s",example["jis1"],example["sjis1"]);
            Test("JIS  to EUC ...","-e",example["jis1"],example["euc1"]);
            Test("JIS  to UTF8...","-w",example["jis1"],example["utf1"]);
            
            // From SJIS

            Test("SJIS to JIS ...", "-j", example["sjis1"], example["jis1"]);
            Test("SJIS to SJIS...", "-s", example["sjis1"], example["sjis1"]);
            Test("SJIS to EUC ...", "-e", example["sjis1"], example["euc1"]);
            Test("SJIS to UTF8...", "-w", example["sjis1"], example["utf1"]);

            // From EUC
            Test("EUC to JIS ...", "-j", example["euc1"], example["jis1"]);
            Test("EUC to SJIS...", "-s", example["euc1"], example["sjis1"]);
            Test("EUC to EUC ...", "-e", example["euc1"], example["euc1"]);
            Test("EUC to UTF8...", "-w", example["euc1"], example["utf1"]);

            // From UTF8
            Test("UTF8 to JIS ...", "-j", example["utf1"], example["jis1"]);
            Test("UTF8 to SJIS...", "-s", example["utf1"], example["sjis1"]);
            Test("UTF8 to EUC ...", "-e", example["utf1"], example["euc1"]);
            Test("UTF8 to UTF8...", "-w", example["utf1"], example["utf1"]);

            // # UTF
            Test("SJIS to -w...          ", "-w", h("82A0"), h("E38182"));
            Test("SJIS to -w8...         ", "-w8", h("82A0"), h("EFBBBFE38182"));
            Test("SJIS to -w80...        ", "-w80", h("82A0"), h("E38182"));
            Test("SJIS to UTF-8...       ", "--oc=UTF-8", h("82A0"), h("E38182"));
            Test("SJIS to UTF-8N...      ", "--oc=UTF-8N", h("82A0"), h("E38182"));
            Test("SJIS to UTF-8-BOM...   ", "--oc=UTF-8-BOM", h("82A0"), h("EFBBBFE38182"));
            Test("SJIS to -w16...        ", "-w16", h("82A0"), h("FEFF3042"));
            Test("SJIS to UTF-16...      ", "--oc=UTF-16", h("82A0"), h("FEFF3042"));
            Test("SJIS to -w16B...       ", "-w16B", h("82A0"), h("FEFF3042"));
            Test("SJIS to -w16B0...      ", "-w16B0", h("82A0"), h("3042"));
            Test("SJIS to UTF-16BE...    ", "--oc=UTF-16BE", h("82A0"), h("3042"));
            Test("SJIS to UTF-16BE-BOM...", "--oc=UTF-16BE-BOM", h("82A0"), h("FEFF3042"));
            Test("SJIS to -w16L...       ", "-w16L", h("82A0"), h("FFFE4230"));
            Test("SJIS to -w16L0...      ", "-w16L0", h("82A0"), h("4230"));
            Test("SJIS to UTF-16LE...    ", "--oc=UTF-16LE", h("82A0"), h("4230"));
            Test("SJIS to UTF-16LE-BOM...", "--oc=UTF-16LE-BOM", h("82A0"), h("FFFE4230"));
            Test("SJIS to -w32...        ", "-w32", h("82A0"), h("0000FEFF00003042"));
            Test("SJIS to UTF-32...      ", "--oc=UTF-32", h("82A0"), h("0000FEFF00003042"));
            Test("SJIS to -w32B...       ", "-w32B", h("82A0"), h("0000FEFF00003042"));
            Test("SJIS to -w32B0...      ", "-w32B0", h("82A0"), h("00003042"));
            Test("SJIS to UTF-32BE...    ", "--oc=UTF-32BE", h("82A0"), h("00003042"));
            Test("SJIS to UTF-32BE-BOM...", "--oc=UTF-32BE-BOM", h("82A0"), h("0000FEFF00003042"));
            Test("SJIS to -w32L...       ", "-w32L", h("82A0"), h("FFFE000042300000"));
            Test("SJIS to -w32L0...      ", "-w32L0", h("82A0"), h("42300000"));
            Test("SJIS to UTF-32LE...    ", "--oc=UTF-32LE", h("82A0"), h("42300000"));
            Test("SJIS to UTF-32LE-BOM...", "--oc=UTF-32LE-BOM", h("82A0"), h("FFFE000042300000"));

            // Ambigous Case
            Test("Ambiguous Case.", "-j", example["amb"], example["amb.euc"]);

            // Input assumption
            Test("SJIS  Input assumption", "-jSx", example["amb"], example["amb.sjis"]);

            // UTF8_STR_OF_JIS_SECOND_LEVEL_KANJI
            example["utf8_str_of_jis_second_level_kanji"] = x("\xe9\xa4\x83\xe5\xad\x90");

            Test("UTF8_STR_OF_JIS_SECOND_LEVEL_KANJI", "-w", example["utf8_str_of_jis_second_level_kanji"],
                example["utf8_str_of_jis_second_level_kanji"]);

            // Broken JIS
            var input = example["jis"].Where(r => r != 0x33).ToList();

            Test("Broken JIS is safe on Normal JIS?","-Be",input,example["euc"]);
            Test("Broken JIS is safe on Normal JIS?", "-Be", example["jis"], example["euc"]);

            // test_data/cp932
            Test("test_data/cp932", "-eS", example["test_data/cp932"], example["test_data/cp932.ans"]);

            // test_data/cp932inv
            Test("test_data/cp932inv", "-sE --cp932", example["test_data/cp932.ans"], example["test_data/cp932"]);

            // test_data/no-cp932inv
            Test("test_data/cp932inv", "-sE --no-cp932", example["test_data/cp932.ans"], example["test_data/no-cp932inv.ans"]);

            // JIS X 0212
            example["jisx0212_euc"] = x("\x8F\xA2\xAF\x8F\xED\xE3");
            example["jisx0212_jis"] = x("\x1b\x24\x28\x44\x22\x2f\x6d\x63\x1b\x28\x42");

            Test("ISO-2022-JP-1 to EUC-JP", "--ic=ISO-2022-JP-1 --oc=EUC-JP", example["jisx0212_jis"], example["jisx0212_euc"]);
            Test("EUC-JP to ISO-2022-JP-1", "--ic=EUC-JP --oc=ISO-2022-JP-1", example["jisx0212_euc"], example["jisx0212_jis"]);

            // JIS X 0213

            Test("Shift_JISX0213 to EUC-JISX0213", "--ic=Shift_JISX0213 --oc=EUC-JISX0213", example["jisx0213_sjis"], example["jisx0213_euc"]);

            Test("EUC-JISX0213 to Shift_JISX0213   ", "--ic=EUC-JISX0213 --oc=Shift_JISX0213  ", example["jisx0213_euc"], example["jisx0213_sjis"]);
            Test("ISO-2022-JP-3 to EUC-JISX0213    ", "--ic=ISO-2022-JP-3 --oc=EUC-JISX0213   ", example["jisx0213_jis2000"], example["jisx0213_euc"]);
            Test("ISO-2022-JP-2004 to EUC-JISX0213 ", "--ic=ISO-2022-JP-2004 --oc=EUC-JISX0213", example["jisx0213_jis2004"], example["jisx0213_euc"]);
            Test("EUC-JISX0213 to ISO-2022-JP-2004 ", "--ic=EUC-JISX0213 --oc=ISO-2022-JP-2004", example["jisx0213_euc"], example["jisx0213_jis2004"]);
            Test("EUC-JISX0213 to UTF-8            ", "--ic=EUC-JISX0213 -w                   ", example["jisx0213_euc"], example["jisx0213_utf8"]);
            Test("UTF-8 to EUC-JISX0213            ", "-W --oc=EUC-JISX0213                   ", example["jisx0213_utf8"], example["jisx0213_euc"]);

            Test("ISO-2022-JP-{1,3,2004} to UTF-8", "--ic=iso-2022-jp-2004 -w",
                x("\x1b$B5Y\x1b$(O~e\x1b$(Q.!\x1b$(P#M\x1b$(D\\e\x1b(B"),
                x("\xe4\xbc\x91\xe9\xb7\x97\xe4\xbf\xb1\xe5\x8c\x8b\xe8\xa4\xb1"));

            Test("UTF-8 to ISO-2022-JP-2004", "-W --oc=iso-2022-jp-2004",
                x("\xe4\xbc\x91\xe9\xb7\x97\xe4\xbf\xb1\xe5\x8c\x8b\xe8\xa4\xb1"),
                x("\x1b$(Q5Y~e.!\x1b$(P#M\x1b(B"));

            // test_data/jisx0213needx0213_f

            Test("test_data/jisx0213needx0213_f", "-W --oc=euc-jisx0213", example["test_data/jisx0213needx0213_f"], example["test_data/jisx0213needx0213_f.ans"]);

            // jisx0213conflict-ibmext
            example["shift_jisx0213conflict-ibmext"] = x("\x87\x40\xed\x40\xee\xf6\xfa\x52\xfb\x45\xfb\xfc\xfc\x4b");
            example["shift_jisx0213conflict-ibmext.x0213utf8"] = x("\xe2\x91\xa0\xe7\xa1\x83\xe9\x86\x9e\xe8\xb4\x89\xe9\x8c\x8d\xe9\xa8\xa0\xf0\xa9\xa9\xb2");
            example["shift_jisx0213conflict-ibmext.cp932utf8"] = x("\xe2\x91\xa0\xe7\xba\x8a\xe2\x85\xb7\xe2\x85\xa8\xe6\xb7\xbc\xe9\xab\x99\xe9\xbb\x91");

            Test("Shift_JISX0213 to UTF-8 (ibmext etc)","--ic=shift_jisx0213 -w",
                example["shift_jisx0213conflict-ibmext"],
                example["shift_jisx0213conflict-ibmext.x0213utf8"]);

            Test("CP932 to UTF-8 (ibmext etc)", "--ic=cp932 -w",
                example["shift_jisx0213conflict-ibmext"],
                example["shift_jisx0213conflict-ibmext.cp932utf8"]);
            
            Test("UTF-8 to Shift_JISX0213 (ibmext etc)", "--oc=shift_jisx0213 -W",
                example["shift_jisx0213conflict-ibmext.x0213utf8"],
                example["shift_jisx0213conflict-ibmext"]);

            Test("UTF-8 to CP932 (ibmext etc)", "--oc=cp932 -W --cp932inv",
                example["shift_jisx0213conflict-ibmext.cp932utf8"],
                x("\x87\x40\xfa\x5c\xfa\x47\x87\x5c\xfb\x45\xfb\xfc\xfc\x4b"));

            // test_data/jisx0213nonbmp
            Test("EUC-JISX0213 to UTF-8 (not in BMP)", "--ic=euc-jisx0213 -w", example["test_data/jisx0213nonbmp"], example["test_data/jisx0213nonbmp.ans"]);
            Test("UTF-8 to EUC-JISX0213 (not in BMP)", "-W --oc=euc-jisx0213", example["test_data/jisx0213nonbmp.ans"], example["test_data/jisx0213nonbmp"]);

            // test_data/shift_jisx0213-utf8-need-no-cp932
            Test("test_data/shift_jisx0213-utf8-need-no-cp932", "--ic=shift_jisx0213 -w", example["test_data/shift_jisx0213-utf8-need-no-cp932"], example["test_data/shift_jisx0213-utf8-need-no-cp932.ans"]);

            // test_data/jisx0213utf8comb
            Test("EUC-JISX0213 to UTF-8 (combining)", "--ic=euc-jisx0213 -w", example["test_data/jisx0213utf8comb"], example["test_data/jisx0213utf8comb.ans"]);
            Test("UTF-8 to EUC-JISX0213 (combining)", "-W --oc=euc-jisx0213", example["test_data/jisx0213utf8comb.ans"], example["test_data/jisx0213utf8combr.ans"]);

            // test_data/jisx0213comb
            Test("UTF-8 to UTF-16BE (combining)", "-W --oc=utf-16be", example["test_data/jisx0213comb"], example["test_data/jisx0213comb.ans"]);
            Test("UTF-16BE to UTF-8 (combining)", "--ic=utf-16be -w", example["test_data/jisx0213comb.ans"], example["test_data/jisx0213comb"]);
            Test("UTF-32BE to EUC-JISX0213 (w/o comb char)", "--ic=UTF-32BE --oc=euc-jisx0213",h("000030AB"),h("A5AB"));
            Test("UTF-16LE to EUC-JISX0213 (w/o comb char)", "--ic=utf-16le --oc=euc-jisx0213",h("AB30"),h("A5AB"));
            Test("UTF-8 to EUC-JISX0213 (w/o comb char)", "--ic=utf-8 --oc=euc-jisx0213", h("E382AB"), h("A5AB"));

            // test_data/no_best_fit_chars
            Test("no_best_fit_chars (eucJP-ascii)", "-W --oc=eucJP-ascii --no-best-fit-chars", example["test_data/no_best_fit_chars"], new List<Byte>());
            Test("no_best_fit_chars (eucJP-ms)", "-W --oc=eucJP-ms --no-best-fit-chars", example["test_data/no_best_fit_chars_ms"], new List<Byte>());

            Test("no_best_fit_chars (cp932)", "-W --oc=CP932 --no-best-fit-chars", example["test_data/no_best_fit_chars_cp932"], new List<Byte>());

            // UCS Mapping Test
            example["ms_ucs_map_1_sjis"] = x("\x81\x60\x81\x61\x81\x7C\x81\x91\x81\x92\x81\xCA");
            example["ms_ucs_map_1_utf16"] = x("\x30\x1C\x20\x16\x22\x12\x00\xA2\x00\xA3\x00\xAC");
            example["ms_ucs_map_1_utf16_ms"] = x("\xFF\x5E\x22\x25\xFF\x0D\xFF\xE0\xFF\xE1\xFF\xE2");

            Test("Normal UCS Mapping :", "-w16B0 --ic=Shift_JIS", example["ms_ucs_map_1_sjis"], example["ms_ucs_map_1_utf16"]);
            Test("Microsoft UCS Mapping :", "-w16B0 -S --ms-ucs-map", example["ms_ucs_map_1_sjis"], example["ms_ucs_map_1_utf16_ms"]);
            Test("CP932 to UTF-16BE :", "--ic=cp932 --oc=utf-16be", example["ms_ucs_map_1_sjis"], example["ms_ucs_map_1_utf16_ms"]);

            // X0201 仮名
            // X0201->X0208 conversion
            // X0208 aphabet -> ASCII
            // X0201 相互変換

            // -X is necessary to allow X0201 in SJIS
            // -Z convert X0208 alphabet to ASCII

            
            Test("X0201 conversion: SJIS ", "-jXZ", example["x0201.sjis"], example["x0201.x0208"]);
            Test("X0201 conversion: JIS  ", "-jZ ", example["x0201.jis"], example["x0201.x0208"]);
            Test("X0201 conversion: SI/SO", "-jZ ", example["x0201.sosi"], example["x0201.x0208"]);
            Test("X0201 conversion: EUC  ", "-jZ ", example["x0201.euc"], example["x0201.x0208"]);
            Test("X0201 conversion: UTF8 ", "-jZ ", example["x0201.utf"], example["x0201.x0208"]);

            Test("-wZ", "-wZ", x(@"\xE3\x80\x80\xEF\xBD\x81\xEF\xBC\xA1&\xE3\x82\xA2"), x(@"\xE3\x80\x80aA&\xE3\x82\xA2"));
            Test("-wZ0", "-wZ0", x(@"\xE3\x80\x80\xEF\xBD\x81\xEF\xBC\xA1&\xE3\x82\xA2"), x(@"\xE3\x80\x80aA&\xE3\x82\xA2"));
            Test("-wZ1", "-wZ1", x("\xE3\x80\x80\xEF\xBD\x81\xEF\xBC\xA1&\xE3\x82\xA2"), x(" aA&\xE3\x82\xA2"));
            Test("-wZ2", "-wZ2", x("\xE3\x80\x80\xEF\xBD\x81\xEF\xBC\xA1&\xE3\x82\xA2"), x("  aA&\xE3\x82\xA2"));
            Test("-wZ3", "-wZ3", x(@"\xE3\x80\x80\xEF\xBD\x81\xEF\xBC\xA1&\xE3\x82\xA2"), x(@"\xE3\x80\x80aA&amp;\xE3\x82\xA2"));
            Test("-wZ4", "-wZ4", x(@"\xE3\x80\x80\xEF\xBD\x81\xEF\xBC\xA1&\xE3\x82\xA2"), x(@"\xE3\x80\x80aA&\xEF\xBD\xB1"));

            // -x means X0201 output

            Test("X0201 output: SJIS", "-xs", example["x0201.euc"], example["x0201.sjis"]);
            Test("X0201 output: JIS", "-xj", example["x0201.sjis"], example["x0201.jis"]);
            Test("X0201 output: EUC", "-xe", example["x0201.jis"], example["x0201.euc"]);
            Test("X0201 output: UTF8", "-xw", example["x0201.jis"], example["x0201.utf"]);

            // test_data/x0201jis=
            Test("X0201 JIS contains '='","-xs",
                x("\x1b(I!\"#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_\x1b(B"),
                example["test_data/x0201jis=.ans"]);

            // test_data/Xx0213
            Test("test_data/Xx0213    ", "-X -W --oc=euc-jisx0213", example["test_data/Xx0213"], example["test_data/Xx0213.ans"]);
			
			// test_data/xx0213
			Test("test_data/xx0213",
				"-x -W --oc=euc-jisx0213",
                example["test_data/xx0213"],example["test_data/xx0213.ans"]);

			// test_data/Z4x0213
			Test("test_data/Z4x0213",
				"-Z4 --ic=euc-jisx0213 -w",
                example["test_data/Z4x0213"],example["test_data/Z4x0213.ans"]);
			
			// test_data/Z4comb
			Test("test_data/Z4comb",
				"-Z4 -W --oc=euc-jisx0213",
                example["test_data/Z4comb"],example["test_data/Z4comb.ans"]);
			
			// MIME decode
			// MIME ISO-2022-JP
			/* MIME テストは正しくテストコードを描いていない・・
			# printf "%-40s", "Next test is expected to Fail.\n";
printf "%-40s", "MIME decode (strict)";
    $tmp = &test("$nkf -jmS",$example{'mime.iso2022'},$example{'mime.ans.strict'});
			*/
			/*
			printf "%-40s", "MIME decode (nonstrict)";
    $tmp = &test("$nkf -jmN",$example{'mime.iso2022'},$example{'mime.ans'},$example{'mime.ans.alt'});
    # open(OUT,">tmp1");printf "%-40s", OUT pack('u',$tmp);close(OUT);
# unbuf mode implies more pessimistic decode
printf "%-40s", "MIME decode (unbuf)";
    $tmp = &test("$nkf -jmNu",$example{'mime.iso2022'},$example{'mime.unbuf'},$example{'mime.unbuf.alt'});
    # open(OUT,">tmp2");printf "%-40s", OUT pack('u',$tmp);close(OUT);
#MIME BASE64 must be LF?
			*/
			Test("MIME decode (base64)",
				"-jmB",
                example["mime.base64"],example["mime.base64.ans"]);
				
			// MIME ISO-8859-1
			
			// Without -l, ISO-8859-1 was handled as X0201.
			Test("MIME ISO-8859-1 (Q)",
				"-jml",
                example["mime.is8859"],example["mime.is8859.ans"]);
				
			// test_data/cr
			Test("test_data/cr",
				"-jd",
                example["test_data/cr"],example["test_data/cr.ans"]);
				
			// test_data/fixed-qencode
			Test("test_data/fixed-qencode",
				"-jmQ",
                example["test_data/fixed-qencode"],example["test_data/fixed-qencode.ans"]);

			// test_data/long-fold-1
			Test("test_data/long-fold-1",
				"-jF60",
                example["test_data/long-fold-1"],example["test_data/long-fold-1.ans"]);
			
			// test_data/long-fold
			Test("test_data/long-fold",
                "-jf60",
                example["test_data/long-fold"],example["test_data/long-fold.ans"]);
				
			// test_data/mime_out
			
			/*
			printf "%-40s", "test_data/mime_out";
    &test("$nkf -jM",$example{'test_data/mime_out'},$example{'test_data/mime_out.ans'},$example{'test_data/mime_out.ans.alt'},$example{'test_data/mime_out.ans.alt2'},$example{'test_data/mime_out.ans.alt3'});
			*/
			Test("test_data/mime_out",
				"-jM",
                example["test_data/mime_out"],
				example["test_data/mime_out.ans"],example["test_data/mime_out.ans.alt"],
				example["test_data/mime_out.ans.alt2"],example["test_data/mime_out.ans.alt3"]);
				
			// test_data/mime_out3
			Test("test_data/mime_out3",
				"-jSM",
                x("\x82\xD9\x82\xB0 A"), x("=?ISO-2022-JP?B?GyRCJFskMhsoQg==?= A"));

            // test_data/multi-line
            Test("test_data/multi-line",
                "-e",
                example["test_data/multi-line"], example["test_data/multi-line.ans"]);

            // test_data/-Z4
            Test("test_data/-Z4    ",
                "-eEZ4",
                example["test_data/-Z4"], example["test_data/-Z4.ans"]);

            // test_data/nkf-19-bug-1
            Test("test_data/nkf-19-bug-1",
                "-Ej",
                example["test_data/nkf-19-bug-1"], example["test_data/nkf-19-bug-1.ans"]);

            // test_data/nkf-19-bug-2
            Test("test_data/nkf-19-bug-2",
                "-Ee",
                example["test_data/nkf-19-bug-2"], example["test_data/nkf-19-bug-2.ans"]);

            // test_data/nkf-19-bug-3
            Test("test_data/nkf-19-bug-3",
                "-Ee",
                example["test_data/nkf-19-bug-3"], example["test_data/nkf-19-bug-3.ans"]);

            // test_data/non-strict-mime
            Test("test_data/non-strict-mime",
                "-jmN",
                example["test_data/non-strict-mime"], example["test_data/non-strict-mime.ans"]);

            // test_data/q-encode-softrap
            Test("test_data/q-encode-softrap",
                "-jmQ",
                example["test_data/q-encode-softrap"], example["test_data/q-encode-softrap.ans"]);

            // test_data/q-encode-utf-8

            example["test_data/q-encode-utf-8"] = x(@"=?utf-8?Q?=E3=81=82=E3=81=84=E3=81=86=E3=81=88=E3=81=8A?=
=?utf-8?Q?=E3=81=8B=E3=81=8D=E3=81=8F=E3=81=91=E3=81=93?=");


            example["test_data/q-encode-utf-8.ans"] = x(
@"\xE3\x81\x82\xE3\x81\x84\xE3\x81\x86\xE3\x81\x88\xE3\x81\x8A
\xE3\x81\x8B\xE3\x81\x8D\xE3\x81\x8F\xE3\x81\x91\xE3\x81\x93");

            Test("test_data/q-encode-utf-8",
                "-w",
                example["test_data/q-encode-utf-8"], example["test_data/q-encode-utf-8.ans"]);

            // test_data/rot13
            Test("test_data/rot13",
                "-jr",
                example["test_data/rot13"], example["test_data/rot13.ans"]);

            // test_data/slash
            Test("test_data/slash",
                "-j",
                example["test_data/slash"], example["test_data/slash.ans"]);

            // test_data/z1space-0
            Test("test_data/z1space-0",
                "-e -Z",
                example["test_data/z1space-0"], example["test_data/z1space-0.ans"]);

            // test_data/z1space-1
            Test("test_data/z1space-1",
                "-e -Z1",
                example["test_data/z1space-1"], example["test_data/z1space-1.ans"]);

            // test_data/z1space-2
            Test("test_data/z1space-2",
                "-e -Z2",
                example["test_data/z1space-2"], example["test_data/z1space-2.ans"]);

            // test_data/bug2273
            Test("test_data/bug2273",
                "-e -Z2",
                example["test_data/bug2273"], example["test_data/bug2273.ans"]);

            // test_data/forum15899
            Test("test_data/forum15899",
                "-Mj",
                example["test_data/forum15899"], example["test_data/forum15899.ans"]);

            // test_data/bugs10904
            Test("test_data/bugs10904",
                "-Mj",
                example["test_data/bugs10904"], example["test_data/bugs10904.ans"]);

            // test_data/ruby-dev:39722
            Test("test_data/ruby-dev:39722",
                "-MjW",
                h3(@"aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa\xE3\x81\x82"),
                s(@"=?US-ASCII?Q?aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa?=" + "\x0a" +
" =?US-ASCII?Q?aaaaaaaaaaaaaaaaa?= =?ISO-2022-JP?B?GyRCJCIbKEI=?="));

            // test_data/bug19779
            Test("test_data/bug19779",
                "-Mj",
                example["test_data/bug19779"], example["test_data/bug19779.ans"]);

            Test("[nkf-forum:47327]    ",
                "-wM",
                h("feffd852de76d814dc45000a"), s("=?UTF-8?B?8KSptvCVgYU=?=\n"));

            Test("[nkf-forum:47334]    ",
                "-w",
                h("feff006100620063000a"), s("abc\n"));

            Test("[nkf-bug:20079]    ",
               "-jSxM",
               h3(@"\xBB \xBB"), s("=?ISO-2022-JP?B?GyhJOxsoQiAbKEk7GyhC?="));

            Test("[nkf-bug:20079]    ",
               "-SxMw8",
               h3(@"\xBB \xBB"), s("=?UTF-8?B?77u/7727IO+9uw==?="));

            // 変な文字が2文字多い
            Test("[nkf-forum:48850]    ",
                "-jSM",
                h3(@"From: \x82\xA0\x82\xA0\x82\xA0\x82\xA0\x82\xA0\x82\xA0\x82\xA0\x82\xA0\x82\xA0 <x-xxxx@xxxxxxxxxxxx.co.jp>\n"),
                h3(@"From: =?ISO-2022-JP?B?GyRCJCIkIiQiJCIkIiQiJCIkIiQiGyhC?= <x-xxxx@xxxxxxxxxxxx.co.jp>\n"));

            Test("[nkf-bug:21393]-x    ",
                "--ic=UTF-8 --oc=CP932",
                h3(@"\xEF\xBD\xBC\xEF\xBE\x9E\xEF\xBD\xAC\xEF\xBD\xB0\xEF\xBE\x8F\xEF\xBE\x9D\xEF\xBD\xA5\xEF\xBE\x8E\xEF\xBE\x9F\xEF\xBE\x83\xEF\xBE\x84\xEF\xBD\xA1"),
                h3(@"\xBC\xDE\xAC\xB0\xCF\xDD\xA5\xCE\xDF\xC3\xC4\xA1"));

            Test("[nkf-bug:21393]-x    ",
                "--ic=UTF-8 --oc=CP932 -X",
                h3(@"\xEF\xBD\xBC\xEF\xBE\x9E\xEF\xBD\xAC\xEF\xBD\xB0\xEF\xBE\x8F\xEF\xBE\x9D\xEF\xBD\xA5\xEF\xBE\x8E\xEF\xBE\x9F\xEF\xBE\x83\xEF\xBE\x84\xEF\xBD\xA1"),
                h3(@"\x83W\x83\x83\x81[\x83}\x83\x93\x81E\x83|\x83e\x83g\x81B"));

            Test("[nkf-forum:65316]  ",
                "-xwW -f10",
                h3(@"\xEF\xBD\xB1\xEF\xBD\xB2\xEF\xBD\xB3\xEF\xBD\xB4\xEF\xBD\xB5\xEF\xBD\xB6\xEF\xBD\xB7\xEF\xBD\xB8\xEF\xBD\xB9\xEF\xBD\xBA\xEF\xBD\xBB\xEF\xBD\xBC\xEF\xBD\xBD\xEF\xBD\xBE\xEF\xBD\xBF\xEF\xBE\x80\xEF\xBE\x81\xEF\xBE\x82\xEF\xBE\x83\xEF\xBE\x84"),
                h3(@"\xEF\xBD\xB1\xEF\xBD\xB2\xEF\xBD\xB3\xEF\xBD\xB4\xEF\xBD\xB5\xEF\xBD\xB6\xEF\xBD\xB7\xEF\xBD\xB8\xEF\xBD\xB9\xEF\xBD\xBA\n\xEF\xBD\xBB\xEF\xBD\xBC\xEF\xBD\xBD\xEF\xBD\xBE\xEF\xBD\xBF\xEF\xBE\x80\xEF\xBE\x81\xEF\xBE\x82\xEF\xBE\x83\xEF\xBE\x84\n"));

            Test("[nkf-forum:65482]",
                "--ic=CP50221 --oc=CP932",
                h3(@"\x1b\x24\x42\x7f\x21\x80\x21\x1b\x28\x42\n"),
                h3(@"\xf0\x40\xf0\x9f\x0a"));

            Test("[nkf-forum:65316]",
                "-xwW -f10",
                h3(@"\xEF\xBD\xB1\xEF\xBD\xB2\xEF\xBD\xB3\xEF\xBD\xB4\xEF\xBD\xB5\xEF\xBD\xB6\xEF\xBD\xB7\xEF\xBD\xB8\xEF\xBD\xB9\xEF\xBD\xBA\xEF\xBD\xBB\xEF\xBD\xBC\xEF\xBD\xBD\xEF\xBD\xBE\xEF\xBD\xBF\xEF\xBE\x80\xEF\xBE\x81\xEF\xBE\x82\xEF\xBE\x83\xEF\xBE\x84"),
                h3(@"\xEF\xBD\xB1\xEF\xBD\xB2\xEF\xBD\xB3\xEF\xBD\xB4\xEF\xBD\xB5\xEF\xBD\xB6\xEF\xBD\xB7\xEF\xBD\xB8\xEF\xBD\xB9\xEF\xBD\xBA\n\xEF\xBD\xBB\xEF\xBD\xBC\xEF\xBD\xBD\xEF\xBD\xBE\xEF\xBD\xBF\xEF\xBE\x80\xEF\xBE\x81\xEF\xBE\x82\xEF\xBE\x83\xEF\xBE\x84\n"));

            Test("[nkf-forum:65482]",
                "--ic=CP50221 --oc=CP932",
                h3(@"\x1b\x24\x42\x7f\x21\x80\x21\x1b\x28\x42\n"),
                h3(@"\xf0\x40\xf0\x9f\x0a"));

            Test("[nkf-bug:32328] SJIS",
                "-Sw",
                h3(@"\x1b\x82\xa0"),
                h3(@"\x1b\xe3\x81\x82"));

            Test("[nkf-bug:32328] JIS",
                "-Jw",
                h3(@"\x1b\x1b$B$\x22\x1b(B"),
                h3(@"\x1b\xe3\x81\x82"));
        }

        [TestMethod]
        public void TestGuessNL()
        {
            // $nkf --guess","none",      "ASCII\n";
            CheckGuess("none", "ASCII\n");
            CheckGuess("\n",        "ASCII (LF)\n");
            CheckGuess("\n\n",      "ASCII (LF)\n");
            CheckGuess("\n\r",      "ASCII (MIXED NL)\n");
            CheckGuess("\n\r\n",    "ASCII (MIXED NL)\n");
            CheckGuess("\n.\n",     "ASCII (LF)\n");
            CheckGuess("\n.\r",     "ASCII (MIXED NL)\n");
            CheckGuess("\n.\r\n",   "ASCII (MIXED NL)\n");
            CheckGuess("\r",        "ASCII (CR)\n");
            CheckGuess("\r\r",      "ASCII (CR)\n");
            CheckGuess("\r\r\n",    "ASCII (MIXED NL)\n");
            CheckGuess("\r.\n",     "ASCII (MIXED NL)\n");
            CheckGuess("\r.\r",     "ASCII (CR)\n");
            CheckGuess("\r.\r\n",   "ASCII (MIXED NL)\n");
            CheckGuess("\r\n",      "ASCII (CRLF)\n");
            CheckGuess("\r\n\n",    "ASCII (MIXED NL)\n");
            CheckGuess("\r\n\r",    "ASCII (MIXED NL)\n");
            CheckGuess("\r\n\r\n",  "ASCII (CRLF)\n");
            CheckGuess("\r\n.\n",   "ASCII (MIXED NL)\n");
            CheckGuess("\r\n.\r",   "ASCII (MIXED NL)\n");
            CheckGuess("\r\n.\r\n", "ASCII (CRLF)\n");
        }

        [TestMethod]
        public void TestConvertLNtoLF()
        {
            Test("Convert NL to LF", "-jLu", "none", "none");
            Test("Convert NL to LF", "-jLu", "\n", "\n");
            Test("Convert NL to LF", "-jLu", "\n\n", "\n\n");
            Test("Convert NL to LF", "-jLu", "\n\r", "\n\n");
            Test("Convert NL to LF", "-jLu","\n\r",      "\n\n");
            Test("Convert NL to LF", "-jLu","\n\r\n",    "\n\n");
            Test("Convert NL to LF", "-jLu","\n.\n",     "\n.\n");
            Test("Convert NL to LF", "-jLu","\n.\r",     "\n.\n");
            Test("Convert NL to LF", "-jLu","\n.\r\n",   "\n.\n");
            Test("Convert NL to LF", "-jLu","\r",        "\n");
            Test("Convert NL to LF", "-jLu","\r\r",      "\n\n");
            Test("Convert NL to LF", "-jLu","\r\r\n",    "\n\n");
            Test("Convert NL to LF", "-jLu","\r.\n",     "\n.\n");
            Test("Convert NL to LF", "-jLu","\r.\r",     "\n.\n");
            Test("Convert NL to LF", "-jLu","\r.\r\n",   "\n.\n");
            Test("Convert NL to LF", "-jLu","\r\n",      "\n");
            Test("Convert NL to LF", "-jLu","\r\n\n",    "\n\n");
            Test("Convert NL to LF", "-jLu","\r\n\r",    "\n\n");
            Test("Convert NL to LF", "-jLu","\r\n\r\n",  "\n\n");
            Test("Convert NL to LF", "-jLu","\r\n.\n",   "\n.\n");
            Test("Convert NL to LF", "-jLu","\r\n.\r",   "\n.\n");
            Test("Convert NL to LF", "-jLu","\r\n.\r\n", "\n.\n");
            
        }

        [TestMethod]
        public void TestConvertLNtoLF2()
        {
            Test("Convert NL to LF", "-jLm", "none", "none");
            Test("Convert NL to LF", "-jLm", "\n", "\r");
            Test("Convert NL to LF", "-jLm", "\n\n", "\r\r");
            Test("Convert NL to LF", "-jLm", "\n\r", "\r\r");
            Test("Convert NL to LF", "-jLm", "\n\r", "\r\r");
            Test("Convert NL to LF", "-jLm", "\n\r\n", "\r\r");
            Test("Convert NL to LF", "-jLm", "\n.\n", "\r.\r");
            Test("Convert NL to LF", "-jLm", "\n.\r", "\r.\r");
            Test("Convert NL to LF", "-jLm", "\n.\r\n", "\r.\r");
            Test("Convert NL to LF", "-jLm", "\r", "\r");
            Test("Convert NL to LF", "-jLm", "\r\r", "\r\r");
            Test("Convert NL to LF", "-jLm", "\r\r\n", "\r\r");
            Test("Convert NL to LF", "-jLm", "\r.\n", "\r.\r");
            Test("Convert NL to LF", "-jLm", "\r.\r", "\r.\r");
            Test("Convert NL to LF", "-jLm", "\r.\r\n", "\r.\r");
            Test("Convert NL to LF", "-jLm", "\r\n", "\r");
            Test("Convert NL to LF", "-jLm", "\r\n\n", "\r\r");
            Test("Convert NL to LF", "-jLm", "\r\n\r", "\r\r");
            Test("Convert NL to LF", "-jLm", "\r\n\r\n", "\r\r");
            Test("Convert NL to LF", "-jLm", "\r\n.\n", "\r.\r");
            Test("Convert NL to LF", "-jLm", "\r\n.\r", "\r.\r");
            Test("Convert NL to LF", "-jLm", "\r\n.\r\n", "\r.\r");
        }
        [TestMethod]
        public void TestConvertLNtoCRLF()
        {
            Test("Convert NL to CRLF", "-jLw", "none",      "none");
            Test("Convert NL to CRLF", "-jLw", "\n",        "\r\n");
            Test("Convert NL to CRLF", "-jLw", "\n\n",      "\r\n\r\n");
            Test("Convert NL to CRLF", "-jLw", "\n\r",      "\r\n\r\n");
            Test("Convert NL to CRLF", "-jLw", "\n\r\n",    "\r\n\r\n");
            Test("Convert NL to CRLF", "-jLw", "\n.\n",     "\r\n.\r\n");
            Test("Convert NL to CRLF", "-jLw", "\n.\r",     "\r\n.\r\n");
            Test("Convert NL to CRLF", "-jLw", "\n.\r\n",   "\r\n.\r\n");
            Test("Convert NL to CRLF", "-jLw", "\r",        "\r\n");
            Test("Convert NL to CRLF", "-jLw", "\r\r",      "\r\n\r\n");
            Test("Convert NL to CRLF", "-jLw", "\r\r\n",    "\r\n\r\n");
            Test("Convert NL to CRLF", "-jLw", "\r.\n",     "\r\n.\r\n");
            Test("Convert NL to CRLF", "-jLw", "\r.\r",     "\r\n.\r\n");
            Test("Convert NL to CRLF", "-jLw", "\r.\r\n",   "\r\n.\r\n");
            Test("Convert NL to CRLF", "-jLw", "\r\n",      "\r\n");
            Test("Convert NL to CRLF", "-jLw", "\r\n\n",    "\r\n\r\n");
            Test("Convert NL to CRLF", "-jLw", "\r\n\r",    "\r\n\r\n");
            Test("Convert NL to CRLF", "-jLw", "\r\n\r\r\n",  "\r\n\r\n\r\n");
            Test("Convert NL to CRLF", "-jLw", "\r\n.\n",   "\r\n.\r\n");
            Test("Convert NL to CRLF", "-jLw", "\r\n.\r", "\r\n.\r\n");
            Test("Convert NL to CRLF", "-jLw", "\r\n.\r\n", "\r\n.\r\n");
        }

        private void CheckGuess(string testData, string guess)
        {
            var data = h3(testData);
            Nkf.Net.WrapNkf.SetNkfOption("--guess");
            int len;
            byte[] dataOut = new byte[256];
            Net.WrapNkf.NkfConvertSafe(dataOut,dataOut.Length,out len, data.ToArray(), data.Count);
            string guessResult = Net.WrapNkf.GetGuess();

            Assert.AreEqual(guess.Trim(), guessResult);
        }

        [TestMethod]
        public void TestNG1()
        {
            
        }

        [TestMethod]
        public void TestExportFiles()
        {
            LoadTestData();
            foreach (string key in example.Keys)
            {
                string file = key.Replace("/", "_");
                Console.WriteLine(file);
                using (System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    fs.Write(example[key].ToArray(), 0, example[key].Count);
                    fs.Flush();
                    fs.Close();
                }
            }
        }

        private List<Byte> h(string s)
        {
            List<Byte> ans = new List<byte>();
            for(int i=0;i<s.Length; i+=2)
            {
                int hex = hexToInt(s[i])* 16 + hexToInt(s[i+1]);
                ans.Add((byte) hex);
            }
            return ans;
        }

        /// <summary>
        /// \xA0 の形式の文字列
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private List<Byte> h3(string s)
        {
            List<Byte> ans = new List<byte>();
            int i = 0;
            while(i < s.Length)
            {
                if (s[i] == '\r' || s[i] == '\n')
                {
                    ans.Add((byte)s[i]);
                    i++;
                    continue;
                }
                if (i <= s.Length - 1 && s[i] == '\\' && (s[i + 1] == 'n'))
                {
                    // \n -> 0x0a に変換
                    ans.Add((byte) 0x0a);
                    i = i + 2;
                }
                else if (i < s.Length - 2 && s[i] == '\\' && s[i + 1] == 'x')
                {
                    Assert.IsTrue(s[i] == '\\' && s[i + 1] == 'x', s + ":" + i);

                    int hex = hexToInt(s[i + 2]) * 16 + hexToInt(s[i + 3]);
                    ans.Add((byte)hex);
                    i = i + 4;
                }
                else
                {
                    ans.Add((byte)s[i]);
                    i = i + 1;
                }

            }
            return ans;
        }

        private List<Byte> x(string s)
        {
            if (s.StartsWith("\\x")) {
                return h3(s);
            }
            List<Byte> ans = new List<byte>();
            for(int i=0;i<s.Length; i++)
            {
                ans.Add((byte) s[i]);
            }
            return ans;
        }
        private int hexToInt(char h)
        {
            string key = "0123456789ABCDEFabcdef";
            int pos = key.IndexOf(h);
            if (pos < 0)
            {
                throw new InvalidOperationException();
            }
            if (pos >= 16)
            {
                pos = pos - 6;
            }
            return pos;
        }
        private void Test(string title, string nkfOption, string sIn, string sOut)
        {
            List<byte> dataIn = new List<byte>();
            dataIn.AddRange(System.Text.Encoding.ASCII.GetBytes(sIn));
            List<byte> dataOut = new List<byte>();
            dataOut.AddRange(System.Text.Encoding.ASCII.GetBytes(sOut));
            Test(title, nkfOption, dataIn, dataOut);

            if (nkfOption == "--guess")
            {
                string guess = WrapNkf.GetGuess();
                Assert.AreEqual(guess, sOut);
            }
        }

        private List<byte> s(string sIn)
        {
            List<byte> data = new List<byte>();
            data.AddRange(System.Text.Encoding.ASCII.GetBytes(sIn));

            return data;
        }
        private void Test(string title, string nkfOption, List<byte> dataIn, List<byte> dataOut, List<byte> dataOut2, List<byte> dataOut3, List<byte> dataOut4)
        {
            List<List<byte>> list = new List<List<byte>>();
            list.Add(dataOut);
            list.Add(dataOut2);
            list.Add(dataOut3);
            list.Add(dataOut4);

            List<string> ngMessages = new List<string>();
            // どれか１つが処理されればOK
            foreach (var data in list)
            {
                if (data == null) continue;

                var result = Test_Internal(nkfOption, dataIn, data);
                if (result.Result == true)
                {
                    // oK
                    return;
                } else {
                    ngMessages.Add(result.ErrorMessage);
                }
            }

            Console.WriteLine();
            foreach (string s in ngMessages)
            {
                Console.WriteLine(s);
                Console.WriteLine();
            }
            Assert.Fail(title);
        }
        private void Test(string title, string nkfOption, List<byte> dataIn, List<byte> dataOut)
        {
            var result = Test_Internal(nkfOption,dataIn,dataOut);
            if (result.Result == false)
            {
                Console.WriteLine(result.ErrorMessage);
                Assert.Fail(title);
            }
        }
        private Test_Internal_Result Test_Internal(string nkfOption, List<byte> dataIn, List<byte> dataOut)
        {
            Test_Internal_Result result = new Test_Internal_Result()
            {
                ErrorMessage = "",
            };

            int dataSize = Math.Max(dataIn.Count, dataOut.Count) * 5;
            byte[] data = new byte[dataSize];
            int convertLen;
            Nkf.Net.WrapNkf.SetNkfOption(nkfOption);
            bool convResult = Nkf.Net.WrapNkf.NkfConvertSafe(data, dataSize, out convertLen, dataIn.ToArray(), dataIn.Count);

            if (convResult == false)
            {
                result.Result = false;
                result.ErrorMessage = "Error in NkfConvertSave()"; 
                return result;
            }
            int ngCount = 0;
            int ngPoint = 0;
            
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (convertLen != dataOut.Count)
            {
                ngCount++;
                sb.Append("sizeError\r\n");
            }
            
            for (int i = 0; i < Math.Min(convertLen, dataOut.Count); i++)
            {
                if (data[i] != dataOut[i])
                {
                    ngCount++;
                    ngPoint = i;
                    break;
                }
            }
            if (ngCount > 0)
            {
                
                sb.Append("NGPos:" + ngPoint);
                sb.Append("\r\n");
                for (int i = 0; i < ngPoint; i++)
                {
                    sb.Append("    ");
                }
                sb.Append("|\n");

                sb.Append("DataIn :");
                for (int i = 0; i < dataIn.Count; i++)
                {
                    sb.Append("\\x");
                    sb.Append(((int)dataIn[i]).ToString("X2"));
                }
                sb.Append("\r\n");
                sb.Append("Convert:");
                for (int i = 0; i < convertLen; i++)
                {
                    sb.Append("\\x");
                    sb.Append(((int)data[i]).ToString("X2"));
                }
                sb.Append("\r\n");
                sb.Append("DataOut:");
                for (int i = 0; i < dataOut.Count; i++)
                {
                    sb.Append("\\x");
                    sb.Append(((int)dataOut[i]).ToString("X2"));
                }
                sb.Append("\r\n");
                result.ErrorMessage = sb.ToString();
                result.Result = false;
            }
            else
            {
                result.ErrorMessage = "";
                result.Result = true;
            }

            return result;
        }

        class Test_Internal_Result
        {
            public bool Result;
            public string ErrorMessage;
        }
        private void LoadTestData()
        {
            example.Clear();

            using (System.IO.StreamReader sr = new System.IO.StreamReader(GetTestDataFileName()))
            {
                string s = sr.ReadLine();
                while (s != null)
                {
                    if (s.StartsWith("@"))
                    {
                        string key = s.Substring(1);
                        s = sr.ReadLine().Trim();
                        List<Byte> bytes = new List<byte>();
                        while (s == "")
                        {
                            s = sr.ReadLine().Trim();
                        }
                        while (string.IsNullOrEmpty(s) == false)
                        {
                            bytes.AddRange(UUDecode(s));
                            s = sr.ReadLine();
                        }
                        // キーの重複チェック
                        if (example.ContainsKey(key))
                        {
                            Console.WriteLine("NG: キーが重複しています。" + key);
                            Assert.Fail();
                        }
                        example[key] = bytes;
                    }

                    s = sr.ReadLine();
                }
            }

            // FixData: データがおかしいので修正
            //example["x0201.sjis"] = TrimOne(example["x0201.sjis"],1);
            //example["test_data/long-fold-1"] = TrimOne(example["test_data/long-fold-1"],1);
            //example["test_data/long-fold"] = TrimOne(example["test_data/long-fold"], 2);
            //example["test_data/long-fold.ans"] = TrimOne(example["test_data/long-fold.ans"], 3);
        }

        List<byte>TrimOne(List<Byte> data,int delSize)
        {
            List<Byte> ans = data.Take(data.Count - delSize).ToList();
            return ans;
        }

        //private Dictionary<string, List<Byte>> example = new Dictionary<string, List<byte>>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, List<Byte>> example = new Dictionary<string, List<byte>>();

        private IEnumerable<byte> UUDecode(string s)
        {
            UUCodec.UUCodec dec = new UUCodec.UUCodec();

            var data = dec.EncodeLine(s);
            return data;

            List<byte> inBytes = new List<byte>();
            for (int i = 0; i < s.Length; i++)
            {
                inBytes.Add((byte)s[i]);
            }

            byte[] outBytes = new byte[s.Length * 2];
            int convertLen = dec.DecodeLine(inBytes.ToArray(),inBytes.Count/2,outBytes);

            List<byte> ans = new List<byte>();
            for (int i = 0; i < convertLen; i++)
            {
                ans.Add(outBytes[i]);
            }
            return ans;
                for (int i = 0; i < s.Length; i++)
                {
                    char c = s[i];

                    //int b = ((c) - ' ') & 077;
                    int b = ((char)(((int)c - 32) * 4 + (c - 32) / 16));

                    ans.Add((byte)b);
                }
            return ans;
        }

        [TestMethod]
        public void TestUUEnc1()
        {
            string s = "?4U0Q0V%H<V]1:4%B2D5)-V)J9WI45D5B2T5)/3\\]\"@``\\";
            var data = UUDecode(s);
            Assert.IsTrue(data != null);
        }

        private string GetTestDataFileName()
        {
            string file = "nkfTestData.txt";
            string fileName = "TestData" + System.IO.Path.DirectorySeparatorChar + file;
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(System.Environment.CurrentDirectory);
            if (System.IO.File.Exists(fileName) == false)
            {
                dir = dir.Parent.Parent;
                dir = new System.IO.DirectoryInfo(System.IO.Path.Combine(dir.FullName, "TestData"));

                fileName = System.IO.Path.Combine(dir.FullName, file);
            }

            return fileName;
        }

        List<byte> Join(List<byte> l1,List<byte> l2)
        {
            List<byte> l = new List<byte>();
            l.AddRange(l1);
            l.AddRange(l2);
            return l;
        }

        // nkf-bug:36572
        [TestMethod]
        public void TestFix_36572()
        {
            Test("[nkf-bug:36572]    ",
                "-sW --fb-html",
                h3("\xe6\xbf\xb9\xe4\xb8\x8a"), Join(s("&#28665;"), h3("\x8f\xe3")));
            /*&test("$nkf -sW --fb-html", 
            +    "\xe6\xbf\xb9\xe4\xb8\x8a", 
            +    "&#28665;\x8f\xe3"); 
            */
        }
    }
}
