/// <summary>
///********************************************
/// ClassName    ：  AcosTable
/// Author       ：  LCG
/// CreateTime   ：  2022/5/11 星期三 
/// Description  ：  Acos数值查寻表
///********************************************/
/// </summary>

namespace UMaths
{
    class AcosTable
    {
        public static readonly int IndexCount;
        public static readonly int HalfIndexCount;
        public static readonly uint Multipler;
        public static readonly int[] table;
        static AcosTable()
        {
            IndexCount = 1024;
            HalfIndexCount = 512;
            Multipler = 10000;

            table = new int[]{
            31416,//0
            30791,//1
            30532,
            30333,
            30165,
            30017,
            29883,
            29760,
            29646,
            29538,
            29436,
            29339,
            29247,
            29158,
            29072,
            28989,
            28909,
            28832,
            28756,
            28683,
            28612,
            28542,
            28474,
            28407,
            28342,
            28278,
            28215,
            28154,
            28093,
            28034,
            27976,
            27918,
            27862,
            27806,
            27751,
            27697,
            27644,
            27591,
            27539,
            27488,
            27437,
            27387,
            27337,
            27288,
            27240,
            27192,
            27145,
            27098,
            27051,
            27005,
            26960,
            26915,
            26870,
            26826,
            26782,
            26738,
            26695,
            26652,
            26610,
            26568,
            26526,
            26485,
            26444,
            26403,
            26362,
            26322,
            26282,
            26243,
            26203,
            26164,
            26125,
            26087,
            26048,
            26010,
            25973,
            25935,
            25898,
            25860,
            25823,
            25787,
            25750,
            25714,
            25678,
            25642,
            25606,
            25571,
            25536,
            25500,
            25466,
            25431,
            25396,
            25362,
            25328,
            25293,
            25260,
            25226,
            25192,
            25159,
            25126,
            25092,
            25059,
            25027,
            24994,
            24961,
            24929,
            24897,
            24865,
            24833,
            24801,
            24769,
            24737,
            24706,
            24675,
            24643,
            24612,
            24581,
            24550,
            24520,
            24489,
            24459,
            24428,
            24398,
            24368,
            24337,
            24308,
            24278,
            24248,
            24218,
            24189,
            24159,
            24130,
            24100,
            24071,
            24042,
            24013,
            23984,
            23955,
            23927,
            23898,
            23869,
            23841,
            23813,
            23784,
            23756,
            23728,
            23700,
            23672,
            23644,
            23616,
            23589,
            23561,
            23533,
            23506,
            23478,
            23451,
            23424,
            23397,
            23369,
            23342,
            23315,
            23288,
            23262,
            23235,
            23208,
            23181,
            23155,
            23128,
            23102,
            23075,
            23049,
            23023,
            22997,
            22970,
            22944,
            22918,
            22892,
            22866,
            22840,
            22815,
            22789,
            22763,
            22738,
            22712,
            22687,
            22661,
            22636,
            22610,
            22585,
            22560,
            22535,
            22509,
            22484,
            22459,
            22434,
            22409,
            22384,
            22360,
            22335,
            22310,
            22285,
            22261,
            22236,
            22212,
            22187,
            22163,
            22138,
            22114,
            22089,
            22065,
            22041,
            22017,
            21992,
            21968,
            21944,
            21920,
            21896,
            21872,
            21848,
            21824,
            21801,
            21777,
            21753,
            21729,
            21706,
            21682,
            21658,
            21635,
            21611,
            21588,
            21564,
            21541,
            21518,
            21494,
            21471,
            21448,
            21424,
            21401,
            21378,
            21355,
            21332,
            21309,
            21286,
            21263,
            21240,
            21217,
            21194,
            21171,
            21148,
            21125,
            21103,
            21080,
            21057,
            21034,
            21012,
            20989,
            20967,
            20944,
            20921,
            20899,
            20876,
            20854,
            20832,
            20809,
            20787,
            20764,
            20742,
            20720,
            20698,
            20675,
            20653,
            20631,
            20609,
            20587,
            20565,
            20543,
            20520,
            20498,
            20476,
            20455,
            20433,
            20411,
            20389,
            20367,
            20345,
            20323,
            20301,
            20280,
            20258,
            20236,
            20214,
            20193,
            20171,
            20149,
            20128,
            20106,
            20085,
            20063,
            20042,
            20020,
            19999,
            19977,
            19956,
            19934,
            19913,
            19891,
            19870,
            19849,
            19827,
            19806,
            19785,
            19764,
            19742,
            19721,
            19700,
            19679,
            19658,
            19636,
            19615,
            19594,
            19573,
            19552,
            19531,
            19510,
            19489,
            19468,
            19447,
            19426,
            19405,
            19384,
            19363,
            19342,
            19321,
            19300,
            19280,
            19259,
            19238,
            19217,
            19196,
            19175,
            19155,
            19134,
            19113,
            19093,
            19072,
            19051,
            19030,
            19010,
            18989,
            18969,
            18948,
            18927,
            18907,
            18886,
            18866,
            18845,
            18825,
            18804,
            18784,
            18763,
            18743,
            18722,
            18702,
            18681,
            18661,
            18640,
            18620,
            18600,
            18579,
            18559,
            18539,
            18518,
            18498,
            18478,
            18457,
            18437,
            18417,
            18396,
            18376,
            18356,
            18336,
            18316,
            18295,
            18275,
            18255,
            18235,
            18215,
            18194,
            18174,
            18154,
            18134,
            18114,
            18094,
            18074,
            18054,
            18034,
            18013,
            17993,
            17973,
            17953,
            17933,
            17913,
            17893,
            17873,
            17853,
            17833,
            17813,
            17793,
            17773,
            17753,
            17734,
            17714,
            17694,
            17674,
            17654,
            17634,
            17614,
            17594,
            17574,
            17554,
            17535,
            17515,
            17495,
            17475,
            17455,
            17435,
            17415,
            17396,
            17376,
            17356,
            17336,
            17316,
            17297,
            17277,
            17257,
            17237,
            17218,
            17198,
            17178,
            17158,
            17139,
            17119,
            17099,
            17079,
            17060,
            17040,
            17020,
            17001,
            16981,
            16961,
            16942,
            16922,
            16902,
            16883,
            16863,
            16843,
            16824,
            16804,
            16784,
            16765,
            16745,
            16725,
            16706,
            16686,
            16666,
            16647,
            16627,
            16608,
            16588,
            16568,
            16549,
            16529,
            16510,
            16490,
            16470,
            16451,
            16431,
            16412,
            16392,
            16373,
            16353,
            16333,
            16314,
            16294,
            16275,
            16255,
            16236,
            16216,
            16196,
            16177,
            16157,
            16138,
            16118,
            16099,
            16079,
            16060,
            16040,
            16021,
            16001,
            15981,
            15962,
            15942,
            15923,
            15903,
            15884,
            15864,
            15845,
            15825,
            15806,
            15786,
            15767,
            15747,
            15727,
            15708,
            15688,
            15669,
            15649,
            15630,
            15610,
            15591,
            15571,
            15552,
            15532,
            15513,
            15493,
            15474,
            15454,
            15434,
            15415,
            15395,
            15376,
            15356,
            15337,
            15317,
            15298,
            15278,
            15259,
            15239,
            15219,
            15200,
            15180,
            15161,
            15141,
            15122,
            15102,
            15083,
            15063,
            15043,
            15024,
            15004,
            14985,
            14965,
            14946,
            14926,
            14906,
            14887,
            14867,
            14848,
            14828,
            14808,
            14789,
            14769,
            14749,
            14730,
            14710,
            14691,
            14671,
            14651,
            14632,
            14612,
            14592,
            14573,
            14553,
            14533,
            14514,
            14494,
            14474,
            14455,
            14435,
            14415,
            14396,
            14376,
            14356,
            14336,
            14317,
            14297,
            14277,
            14258,
            14238,
            14218,
            14198,
            14179,
            14159,
            14139,
            14119,
            14099,
            14080,
            14060,
            14040,
            14020,
            14000,
            13981,
            13961,
            13941,
            13921,
            13901,
            13881,
            13862,
            13842,
            13822,
            13802,
            13782,
            13762,
            13742,
            13722,
            13702,
            13682,
            13662,
            13643,
            13623,
            13603,
            13583,
            13563,
            13543,
            13523,
            13503,
            13483,
            13463,
            13443,
            13422,
            13402,
            13382,
            13362,
            13342,
            13322,
            13302,
            13282,
            13262,
            13242,
            13221,
            13201,
            13181,
            13161,
            13141,
            13121,
            13100,
            13080,
            13060,
            13040,
            13019,
            12999,
            12979,
            12959,
            12938,
            12918,
            12898,
            12877,
            12857,
            12837,
            12816,
            12796,
            12775,
            12755,
            12735,
            12714,
            12694,
            12673,
            12653,
            12632,
            12612,
            12591,
            12571,
            12550,
            12530,
            12509,
            12489,
            12468,
            12447,
            12427,
            12406,
            12385,
            12365,
            12344,
            12323,
            12303,
            12282,
            12261,
            12240,
            12220,
            12199,
            12178,
            12157,
            12136,
            12116,
            12095,
            12074,
            12053,
            12032,
            12011,
            11990,
            11969,
            11948,
            11927,
            11906,
            11885,
            11864,
            11843,
            11822,
            11801,
            11780,
            11758,
            11737,
            11716,
            11695,
            11674,
            11652,
            11631,
            11610,
            11589,
            11567,
            11546,
            11524,
            11503,
            11482,
            11460,
            11439,
            11417,
            11396,
            11374,
            11353,
            11331,
            11310,
            11288,
            11266,
            11245,
            11223,
            11202,
            11180,
            11158,
            11136,
            11115,
            11093,
            11071,
            11049,
            11027,
            11005,
            10983,
            10961,
            10939,
            10917,
            10895,
            10873,
            10851,
            10829,
            10807,
            10785,
            10763,
            10741,
            10718,
            10696,
            10674,
            10651,
            10629,
            10607,
            10584,
            10562,
            10540,
            10517,
            10495,
            10472,
            10449,
            10427,
            10404,
            10382,
            10359,
            10336,
            10313,
            10291,
            10268,
            10245,
            10222,
            10199,
            10176,
            10153,
            10130,
            10107,
            10084,
            10061,
            10038,
            10015,
            9992,
            9968,
            9945,
            9922,
            9898,
            9875,
            9852,
            9828,
            9805,
            9781,
            9758,
            9734,
            9710,
            9687,
            9663,
            9639,
            9615,
            9591,
            9568,
            9544,
            9520,
            9496,
            9472,
            9448,
            9423,
            9399,
            9375,
            9351,
            9327,
            9302,
            9278,
            9253,
            9229,
            9204,
            9180,
            9155,
            9131,
            9106,
            9081,
            9056,
            9031,
            9007,
            8982,
            8957,
            8932,
            8907,
            8881,
            8856,
            8831,
            8806,
            8780,
            8755,
            8729,
            8704,
            8678,
            8653,
            8627,
            8601,
            8575,
            8550,
            8524,
            8498,
            8472,
            8446,
            8419,
            8393,
            8367,
            8341,
            8314,
            8288,
            8261,
            8235,
            8208,
            8181,
            8154,
            8128,
            8101,
            8074,
            8047,
            8019,
            7992,
            7965,
            7938,
            7910,
            7883,
            7855,
            7827,
            7800,
            7772,
            7744,
            7716,
            7688,
            7660,
            7632,
            7603,
            7575,
            7546,
            7518,
            7489,
            7461,
            7432,
            7403,
            7374,
            7345,
            7315,
            7286,
            7257,
            7227,
            7198,
            7168,
            7138,
            7108,
            7078,
            7048,
            7018,
            6988,
            6957,
            6927,
            6896,
            6865,
            6835,
            6804,
            6773,
            6741,
            6710,
            6678,
            6647,
            6615,
            6583,
            6551,
            6519,
            6487,
            6455,
            6422,
            6389,
            6356,
            6324,
            6290,
            6257,
            6224,
            6190,
            6156,
            6122,
            6088,
            6054,
            6020,
            5985,
            5950,
            5915,
            5880,
            5845,
            5810,
            5774,
            5738,
            5702,
            5666,
            5629,
            5592,
            5556,
            5518,
            5481,
            5443,
            5406,
            5368,
            5329,
            5291,
            5252,
            5213,
            5173,
            5134,
            5094,
            5054,
            5013,
            4972,
            4931,
            4890,
            4848,
            4806,
            4764,
            4721,
            4678,
            4634,
            4590,
            4546,
            4501,
            4456,
            4411,
            4365,
            4318,
            4271,
            4224,
            4176,
            4128,
            4079,
            4029,
            3979,
            3928,
            3877,
            3825,
            3772,
            3719,
            3665,
            3610,
            3554,
            3498,
            3440,
            3382,
            3322,
            3262,
            3201,
            3138,
            3074,
            3009,
            2942,
            2874,
            2804,
            2733,
            2659,
            2584,
            2507,
            2427,
            2344,
            2258,
            2169,
            2077,
            1980,
            1878,
            1770,
            1655,
            1532,
            1399,
            1251,
            1083,
            884,
            625,
            0
            };
        }
    }
}
