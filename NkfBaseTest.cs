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

            /*
            

             */

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

            //Test("-wZ", "-wZ", x("\xE3\x80\x80\xEF\xBD\x81\xEF\xBC\xA1&\xE3\x82\xA2"), x("\xE3\x80\x80aA&\xE3\x82\xA2"));
            //Test("-wZ0", "-wZ0", x("\xE3\x80\x80\xEF\xBD\x81\xEF\xBC\xA1&\xE3\x82\xA2"), x("\xE3\x80\x80aA&\xE3\x82\xA2"));
            Test("-wZ1", "-wZ1", x("\xE3\x80\x80\xEF\xBD\x81\xEF\xBC\xA1&\xE3\x82\xA2"), x(" aA&\xE3\x82\xA2"));
            Test("-wZ2", "-wZ2", x("\xE3\x80\x80\xEF\xBD\x81\xEF\xBC\xA1&\xE3\x82\xA2"), x("  aA&\xE3\x82\xA2"));
            //Test("-wZ3", "-wZ3", x("\xE3\x80\x80\xEF\xBD\x81\xEF\xBC\xA1&\xE3\x82\xA2"), x("\xE3\x80\x80aA&amp;\xE3\x82\xA2"));
            //Test("-wZ4", "-wZ4", x("\xE3\x80\x80\xEF\xBD\x81\xEF\xBC\xA1&\xE3\x82\xA2"), x("\xE3\x80\x80aA&\xEF\xBD\xB1"));

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
            /*
            # 
#
printf "%-40s",  "test_data/Xx0213    ";
    &test("$nkf -X -W --oc=euc-jisx0213",$example{'test_data/Xx0213'},$example{'test_data/Xx0213.ans'});
             * */

        }

        [TestMethod]
        public void TestNG1()
        {
            LoadTestData();

            Test("-wZ", "-wZ", x("\xE3\x80\x80\xEF\xBD\x81\xEF\xBC\xA1&\xE3\x82\xA2"), x("\xE3\x80\x80aA&\xE3\x82\xA2"));
            Test("-wZ0", "-wZ0", x("\xE3\x80\x80\xEF\xBD\x81\xEF\xBC\xA1&\xE3\x82\xA2"), x("\xE3\x80\x80aA&\xE3\x82\xA2"));
            Test("-wZ3", "-wZ3", x("\xE3\x80\x80\xEF\xBD\x81\xEF\xBC\xA1&\xE3\x82\xA2"), x("\xE3\x80\x80aA&amp;\xE3\x82\xA2"));
            Test("-wZ4", "-wZ4", x("\xE3\x80\x80\xEF\xBD\x81\xEF\xBC\xA1&\xE3\x82\xA2"), x("\xE3\x80\x80aA&\xEF\xBD\xB1"));

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

        private List<Byte> x(string s)
        {
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
        }
        private void Test(string title, string nkfOption, List<byte> dataIn, List<byte> dataOut)
        {
            int dataSize = Math.Max(dataIn.Count,dataOut.Count) * 5;
            byte []data = new byte[dataSize];
            int convertLen ;
            Nkf.Net.WrapNkf.SetNkfOption(nkfOption);
            bool result = Nkf.Net.WrapNkf.NkfConvertSafe(data,dataSize,out convertLen,dataIn.ToArray(),dataIn.Count);
            Assert.IsTrue(result,title);

            int ngCount = 0;
            int ngPoint = 0;
            if (convertLen >= dataOut.Count)
            {
                // オーバー領域は ０ であることのチェック
                for (int i = dataOut.Count; i < convertLen; i++)
                {
                    if (data[i] != 0)
                    {
                        ngCount++;
                        ngPoint = i;
                    }
                }
            }
            for (int i = 0; i < Math.Min(convertLen,dataOut.Count); i++)
            {
                if (data[i] != dataOut[i])
                {
                    ngCount++;
                    ngPoint = i;
                }
            }
            if (ngCount > 0)
            {
                for (int i = 0; i < dataIn.Count; i++)
                {
                    Console.Write("\\x");
                    Console.Write(((int)dataIn[i]).ToString("X2"));
                }
                Console.WriteLine("");
                for (int i = 0; i < convertLen; i++)
                {
                    Console.Write("\\x");
                    Console.Write(((int)data[i]).ToString("X2"));
                }
                Console.WriteLine("");
                for (int i = 0; i < Math.Min(convertLen, dataOut.Count); i++)
                {
                    Console.Write("\\x");
                    Console.Write(((int)dataOut[i]).ToString("X2"));
                }
                Console.WriteLine("");
                Assert.Fail("error in " + ngPoint);
            }
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
                        example[key] = bytes;
                    }

                    s = sr.ReadLine();
                }
            }

            // FixData: データがおかしいので修正
            var x201sjis = example["x0201.sjis"];
            x201sjis = x201sjis.Take(x201sjis.Count -1).ToList();
            example["x0201.sjis"] = x201sjis;
        }

        private Dictionary<string, List<Byte>> example = new Dictionary<string, List<byte>>(StringComparer.OrdinalIgnoreCase);

        private IEnumerable<byte> UUDecode(string s)
        {
            UUCodec.UUCodec dec = new UUCodec.UUCodec();

            return dec.EncodeLine(s);

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

        private string GetTestDataFileName()
        {
            string file = "nkfTestData.txt";
            string fileName = "TestData\\" + file;
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(System.Environment.CurrentDirectory);
            if (System.IO.File.Exists(fileName) == false)
            {
                dir = dir.Parent.Parent;
                dir = new System.IO.DirectoryInfo(System.IO.Path.Combine(dir.FullName, "TestData"));

                fileName = System.IO.Path.Combine(dir.FullName, file);
            }

            return fileName;
        }
    }
}
