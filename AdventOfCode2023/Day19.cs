namespace AdventOfCode2023;

public static class Day19
{
    public static void Solve()
    {
        Console.WriteLine("Day 19");
        Console.WriteLine($"Part 1: {Part1()}"); //406849
        Console.WriteLine($"Part 2: {Part2()}"); //138625360533574
    }

    public static long Part1()
    {
        var input = _input.AsSpan();

        var result = 0;

        var labels = new Dictionary<string, List<(Func<int, int, int, int, int> GetValue, Func<int, bool> IsValid, string Next)>>();
        var processParts = false;
        foreach (var line in input.EnumerateLines())
        {
            if (line.IsEmpty)
            {
                processParts = true;
                continue;
            }

            if (processParts)
            {
                var partValues = line[(line.IndexOf('{') + 1)..^1].ToString().Split(',')
                    .Select(v => int.Parse(v[(v.IndexOf('=') + 1)..]))
                    .ToArray();
                var (x, m, a, s) = (partValues[0], partValues[1], partValues[2], partValues[3]);

                var current = "in";
                while (current != "A" && current != "R")
                {
                    var ruleSet = labels[current];
                    var rule = ruleSet.First(r => r.IsValid(r.GetValue(x, m, a, s)));
                    current = rule.Next;
                }

                if (current == "A")
                {
                    result += x + m + a + s;
                }

                continue;
            }

            var label = line[..line.IndexOf('{')].ToString();
            var rules = new List<(Func<int, int, int, int, int> GetValue, Func<int, bool> IsValid, string Next)>();

            var parts = line[(line.IndexOf('{') + 1)..^1].ToString().Split(',');
            foreach (var part in parts)
            {
                var condition = (int _) => true;
                var getValue = (int _, int _, int _, int _) => 0;
                var labelI = part.IndexOf(':');
                if (labelI != -1)
                {
                    getValue = part[0] switch
                    {
                        'x' => (x, _, _, _) => x,
                        'm' => (_, m, _, _) => m,
                        'a' => (_, _, a, _) => a,
                        's' => (_, _, _, s) => s,
                    };

                    var comparisonI = part.IndexOfAny(['<', '>']);
                    var nextI = part.IndexOf(':');
                    var valueC = int.Parse(part[(comparisonI + 1)..nextI]);
                    condition = part[comparisonI] switch
                    {
                        '<' => value => value < valueC,
                        '>' => value => value > valueC,
                        _ => throw new("Invalid comparison")
                    };
                }

                var next = part[(labelI + 1)..];

                rules.Add((getValue, condition, next));
            }

            labels.Add(label, rules);
        }

        return result;
    }

    public static long Part2()
    {
        var input = _input.AsSpan();

        var result = 0L;

        var labels = new Dictionary<string, List<((char Field, char Condition, int Value), string Next)>>();
        foreach (var line in input.EnumerateLines())
        {
            if (line.IsEmpty)
            {
                break;
            }

            var label = line[..line.IndexOf('{')].ToString();
            var rules = new List<((char Field, char Condition, int Value), string Next)>();

            var parts = line[(line.IndexOf('{') + 1)..^1].ToString().Split(',');
            foreach (var part in parts)
            {
                var s = 'x';
                var c = '>';
                var v = 0;

                var labelI = part.IndexOf(':');
                if (labelI != -1)
                {
                    var comparisonI = part.IndexOfAny(['<', '>']);
                    var nextI = part.IndexOf(':');

                    s = part[0];
                    c = part[comparisonI];
                    v = int.Parse(part[(comparisonI + 1)..nextI]);
                }

                var next = part[(labelI + 1)..];

                rules.Add(((s, c, v), next));
            }

            labels.Add(label, rules);
        }

        var results = Evaluate(labels, "in", [((1, 4000), (1, 4000), (1, 4000), (1, 4000))]);

        foreach (var tuple in results)
        {
            var (x, m, a, s) = tuple;
            result += (x.Max - x.Min + 1L) * (m.Max - m.Min + 1L) * (a.Max - a.Min + 1L) * (s.Max - s.Min + 1L);
        }

        return result;
    }

    private static List<((int Min, int Max) X, (int Min, int Max) M, (int Min, int Max) A, (int Min, int Max) S)> Evaluate(
        Dictionary<string, List<((char Field, char Condition, int Value), string Next)>> states,
        string current,
        List<((int Min, int Max) X, (int Min, int Max) M, (int Min, int Max) A, (int Min, int Max) S)> accepted)
    {
        if (current == "R") return [];
        if (current == "A") return accepted;

        var acceptedRanges = new List<((int Min, int Max) X, (int Min, int Max) M, (int Min, int Max) A, (int Min, int Max) S)>();

        var ruleSet = states[current];
        foreach (var rule in ruleSet)
        {
            var (f, c, v) = rule.Item1;
            var trueConditions = new List<((int Min, int Max) X, (int Min, int Max) M, (int Min, int Max) A, (int Min, int Max) S)>();
            var falseConditions = new List<((int Min, int Max) X, (int Min, int Max) M, (int Min, int Max) A, (int Min, int Max) S)>();

            foreach (var range in accepted)
            {
                var t = f switch
                {
                    'x' => range with {X = ApplyCondition(range.X, c, v)},
                    'm' => range with {M = ApplyCondition(range.M, c, v)},
                    'a' => range with {A = ApplyCondition(range.A, c, v)},
                    's' => range with {S = ApplyCondition(range.S, c, v)},
                };
                var ut = f switch
                {
                    'x' => range with {X = ApplyReverseCondition(range.X, c, v)},
                    'm' => range with {M = ApplyReverseCondition(range.M, c, v)},
                    'a' => range with {A = ApplyReverseCondition(range.A, c, v)},
                    's' => range with {S = ApplyReverseCondition(range.S, c, v)},
                };

                trueConditions.Add(t);
                falseConditions.Add(ut);
            }

            acceptedRanges.AddRange(Evaluate(states, rule.Next, trueConditions));

            accepted = falseConditions;
        }

        return acceptedRanges;
    }

    private static (int Min, int Max) ApplyCondition((int Min, int Max) r, char condition, int value)
    {
        return condition switch
        {
            '<' => r with {Max = Math.Min(r.Max, value - 1)},
            '>' => r with {Min = Math.Max(r.Min, value + 1)},
            _ => throw new("Invalid comparison")
        };
    }

    private static (int Min, int Max) ApplyReverseCondition((int Min, int Max) r, char condition, int value)
    {
        return condition switch
        {
            '<' => r with {Min = Math.Max(r.Min, value)},
            '>' => r with {Max = Math.Min(r.Max, value)},
            _ => throw new("Invalid comparison")
        };
    }

    private const string _testInput = """
                                      px{a<2006:qkq,m>2090:A,rfg}
                                      pv{a>1716:R,A}
                                      lnx{m>1548:A,A}
                                      rfg{s<537:gd,x>2440:R,A}
                                      qs{s>3448:A,lnx}
                                      qkq{x<1416:A,crn}
                                      crn{x>2662:A,R}
                                      in{s<1351:px,qqz}
                                      qqz{s>2770:qs,m<1801:hdj,R}
                                      gd{a>3333:R,R}
                                      hdj{m>838:A,pv}

                                      {x=787,m=2655,a=1222,s=2876}
                                      {x=1679,m=44,a=2067,s=496}
                                      {x=2036,m=264,a=79,s=2244}
                                      {x=2461,m=1339,a=466,s=291}
                                      {x=2127,m=1623,a=2188,s=1013}
                                      """;

    private const string _input = """
                                  jtx{s<3181:R,R}
                                  rkd{s>2386:tdr,R}
                                  svj{x>1520:A,A}
                                  vkv{x<3627:A,x>3794:A,A}
                                  ksz{m<1450:R,a<2837:R,a<2928:R,A}
                                  fp{x<1293:sjt,bv}
                                  jq{m>1493:R,m>1377:A,m<1344:R,R}
                                  gts{m<3166:R,A}
                                  xxr{s<231:R,x<3055:R,R}
                                  rz{a<2982:A,m<1763:R,x>1878:A,A}
                                  kq{s<2900:R,s>3591:A,R}
                                  tmg{s<1500:A,x>2504:R,a<1166:A,R}
                                  dvn{s<1632:R,A}
                                  znj{m>1884:A,R}
                                  db{m<1554:cg,m>3173:R,x<1203:A,xsm}
                                  chr{a<682:R,x>3120:R,m>2313:A,R}
                                  pd{x<1433:kp,a<2715:fjd,xtx}
                                  qkd{s<666:R,R}
                                  rbf{x>2541:R,R}
                                  gx{s>1347:A,m<812:gxz,s>1079:tpg,lt}
                                  fqb{x<1290:A,R}
                                  jf{x<1340:A,s>2972:dq,x>1378:bqh,jps}
                                  sr{m>776:A,A}
                                  vs{s<1377:R,R}
                                  vx{x>1237:R,A}
                                  dz{m<192:A,m<271:A,R}
                                  mhr{m<2568:R,m<3466:R,a>3791:A,R}
                                  vv{m<3559:A,s>3108:R,R}
                                  psk{a>2830:A,a<2278:A,A}
                                  gvv{a>2993:A,a>2553:A,R}
                                  gsx{x<2656:A,A}
                                  jps{a<1216:A,R}
                                  lt{s<967:A,A}
                                  ng{s<3259:R,s<3674:R,A}
                                  zd{s>2267:R,R}
                                  ks{a>1951:bg,m<1522:gpf,gj}
                                  prk{m<2242:R,s>56:R,R}
                                  nr{x>3432:R,sg}
                                  fkd{s>2014:mxf,sps}
                                  tk{x<1133:jzj,vq}
                                  sgl{a<1237:R,A}
                                  smg{x<2071:R,m<387:R,A}
                                  qd{s<3197:sl,x>1479:lqs,m>1300:rrp,gm}
                                  tr{s<1306:R,x>1183:A,A}
                                  bxh{a>1123:fpp,R}
                                  hjp{a>2545:nb,a>2381:qx,m>2463:nn,bt}
                                  zg{s<1932:smc,m<1443:rq,jf}
                                  nnq{m>3341:lx,s<2737:A,a<3408:A,R}
                                  lx{s>3219:A,a>2886:A,m<3691:A,R}
                                  stl{s<2701:R,m>3442:A,s<3282:A,A}
                                  gsp{m<619:R,s<529:R,A}
                                  fkk{x<3359:svx,sp}
                                  dsk{x<1499:kn,xj}
                                  jmx{m<1426:zq,fvz}
                                  st{s>2257:slt,rvv}
                                  prm{a<470:A,tts}
                                  fkj{x<2514:R,x>2784:R,a>2830:R,qft}
                                  bh{m<1740:mmf,a>503:zn,s>1702:vfp,fft}
                                  rv{m<2642:A,sfv}
                                  qk{s<1846:A,A}
                                  nqr{a>786:ssx,qbh}
                                  kng{a<3059:A,a<3463:A,a<3661:A,A}
                                  tcd{m<3753:R,a<2566:pq,m<3792:R,hsq}
                                  rhc{x>1415:mzt,x>1395:qp,x>1386:rk,nnq}
                                  hn{x>3532:A,a>1227:R,a>1109:A,R}
                                  btd{s<2462:ftz,A}
                                  xv{s>1573:pv,m>730:pn,s<1201:vtz,fjk}
                                  sm{a>787:R,R}
                                  mqz{m<645:R,m<682:A,A}
                                  rvv{m<867:vkv,a>556:R,mx}
                                  fft{x>581:jb,a<314:R,R}
                                  txz{s<1338:R,x>2633:A,m<1168:A,A}
                                  pcq{x<1531:A,R}
                                  kvj{s<2721:A,x>2637:A,R}
                                  kfj{x<1129:kz,s>1501:zfm,pk}
                                  rd{m<3742:A,A}
                                  qft{x<2653:A,s>1281:A,A}
                                  zzr{a<3875:R,m>249:A,a<3917:R,A}
                                  nh{a>3744:zcn,gxg}
                                  fsp{m>3306:R,R}
                                  rt{m<310:zlk,qgj}
                                  mpb{m>2954:R,R}
                                  qgj{s>1083:R,m<502:A,s<949:A,A}
                                  hq{x<1527:dfg,m>3017:jvm,a>947:jts,gqf}
                                  bfc{x>1423:R,a<2789:A,A}
                                  bnv{a>900:R,A}
                                  tdr{a>518:R,m>657:A,x>2576:R,R}
                                  jmm{a<3410:mq,fv}
                                  qjr{a>2534:R,s<1365:A,m<261:A,R}
                                  in{x<1624:zsf,ks}
                                  jgs{m<702:R,A}
                                  qg{a<1733:A,R}
                                  pv{x<1559:hnp,x>1588:gbv,R}
                                  rpx{m<2361:zv,A}
                                  bzm{x<1510:rhc,x<1580:lct,m>3357:rrj,zld}
                                  dnf{x>2931:xkp,s>1747:A,s<858:qhb,zf}
                                  sk{x>3528:A,x<3113:bnx,psk}
                                  xgt{s>2544:vn,sft}
                                  kjv{s>1334:gsh,a>95:rbf,a>50:bmf,trd}
                                  pvt{s>2784:A,s<2303:qk,a<1589:A,R}
                                  nv{a>3108:R,x>1923:R,s>652:A,A}
                                  nlg{m>383:A,s>1520:A,s>749:R,R}
                                  lk{a<2425:R,s>598:R,a>2517:R,A}
                                  nt{m>1682:R,m>999:A,m>428:A,A}
                                  rmp{m>2634:R,s<317:nt,lk}
                                  fs{m<2408:A,x<1842:R,R}
                                  sjt{x<1133:nqr,vt}
                                  tbk{m<2779:R,a>3554:R,a>3415:A,R}
                                  jr{m>3312:A,R}
                                  bmf{s<509:xxs,a<68:qxr,R}
                                  xz{s<1845:A,x<1392:A,A}
                                  tpn{m>2419:A,x>2999:A,s<2347:A,R}
                                  fcg{a<196:A,m>1248:nbt,a>276:R,R}
                                  kv{x>1467:R,m>891:R,s>2429:A,A}
                                  rq{m>570:bft,x<1349:R,m<215:R,A}
                                  cpr{s>140:cdl,m<2079:znj,prk}
                                  fc{a<3507:R,x<1454:A,s<1296:A,A}
                                  kfs{x<2459:R,A}
                                  vr{m<3596:A,a<2837:R,A}
                                  vg{a>3513:xl,x<569:sr,x>740:bp,qxs}
                                  mzc{m<3026:R,a>1169:A,A}
                                  jhk{m<3520:A,m<3692:A,a<701:R,A}
                                  qx{s<1241:A,A}
                                  bv{x<1412:zg,m<1721:hh,hq}
                                  pj{s>3641:A,x>1535:mj,m<1145:A,jxb}
                                  svx{x<3194:A,m<1734:gsp,m<2937:zk,kng}
                                  kp{x<1404:jj,m>1015:R,x>1415:bfc,A}
                                  cxh{m>619:R,m>536:A,R}
                                  ffj{x<2728:R,s<231:A,R}
                                  fh{m<1629:A,a<1260:A,A}
                                  trd{a>29:qc,A}
                                  hj{a<706:kvj,s<3197:grd,a>1057:mzc,gsx}
                                  qcx{a<265:R,R}
                                  bhz{x<1415:R,A}
                                  ccq{a<3195:mtr,a<3700:jzv,x>1501:xv,tj}
                                  qpn{s>499:vpz,a>804:qsc,lsm}
                                  jk{x<3497:R,x>3734:A,A}
                                  bz{x>568:A,a<773:A,R}
                                  hkv{a<1753:R,x<2544:A,a>1849:R,ps}
                                  gj{m>2405:fkd,s>1405:pf,qpn}
                                  lg{a<1611:rlx,a<1731:fps,A}
                                  ln{a<1637:R,s<3175:A,A}
                                  shk{a<1729:htt,m>1833:R,A}
                                  sct{m>1497:A,R}
                                  rnj{a>2878:R,m>1475:R,a<2792:R,A}
                                  qp{a<3308:R,m>3268:vv,A}
                                  vq{s<2622:R,R}
                                  nbt{s<1656:A,m<1413:R,m>1485:A,A}
                                  bdm{x<2476:gv,x>2650:vhv,kh}
                                  bjh{x<2660:R,A}
                                  fnx{a<3874:A,A}
                                  qzt{s>3620:A,A}
                                  ft{a<1286:A,m>1846:R,R}
                                  cx{a>2841:A,m<425:qjr,s>1229:qm,R}
                                  rrj{m<3695:nm,m>3867:sz,a>3080:cd,tcd}
                                  ghj{s<2416:A,m<2698:R,R}
                                  xm{a<3638:pcq,m>2433:pnx,tsc}
                                  hkg{a<1186:A,R}
                                  ts{s>905:A,m<299:R,R}
                                  dbf{m<1691:sct,s<3740:mnr,A}
                                  ssq{s>3183:A,m<2388:A,A}
                                  bd{s>291:jbg,s<217:R,s<250:ffj,vl}
                                  dq{a<1136:R,R}
                                  rpf{s>2706:ll,x<2910:ghj,A}
                                  qxr{s<877:A,A}
                                  lct{s>3272:kx,hcp}
                                  bbc{a<3031:R,x<1554:A,R}
                                  zlk{m>176:R,A}
                                  lh{a<3217:fld,x<504:jr,rgg}
                                  zct{x>2738:sk,x>2209:bdm,gdm}
                                  dj{x>1137:A,a>2578:A,R}
                                  gc{a>2861:R,a<2739:A,A}
                                  lhn{s>2175:jz,m>1213:hcq,lxj}
                                  qm{x<1501:R,s>1724:R,A}
                                  bnx{s>3556:R,x<2924:A,a>2767:A,R}
                                  zr{s<2643:A,ms}
                                  smc{x<1354:R,x>1374:R,s>1279:fh,cxb}
                                  jvm{s>1609:zmn,a<1172:jhk,cxl}
                                  sz{s>3310:A,R}
                                  bxf{a>2995:A,rnj}
                                  qn{x>1577:A,R}
                                  rm{x>3502:sm,x<3166:rzf,R}
                                  zzd{a<2945:A,s<530:A,a<3360:A,A}
                                  ns{x<1515:A,a<2501:A,R}
                                  rnb{s<1360:A,m>2498:R,s<1805:R,A}
                                  srj{x>1408:R,R}
                                  jzv{m>750:ltd,s<1511:rt,tcc}
                                  gps{a>3444:A,x>1039:R,x<989:R,A}
                                  cmr{x<2302:A,R}
                                  mgq{a>3431:A,m<2513:R,R}
                                  zps{m<2495:R,x>1582:A,a>3314:R,A}
                                  xj{s<2468:A,x>1516:A,a>3292:A,A}
                                  hfv{a>2835:phz,x<1072:jc,dj}
                                  vmf{a<2879:qdv,m<1506:xsn,x>2082:rr,xdq}
                                  ndj{m>605:A,a>1367:R,A}
                                  sd{x>1431:A,a<3334:A,s>3667:A,A}
                                  fjt{a<2589:R,A}
                                  slp{x<583:A,A}
                                  jfx{s>639:R,s>549:R,zc}
                                  zrc{m<2490:A,fsp}
                                  sqf{s>3315:A,m<2054:A,R}
                                  hlm{a<3112:R,m>1347:A,R}
                                  bp{s>2210:A,R}
                                  htt{a<1620:R,R}
                                  lhg{x>1534:rb,m>1305:qmz,a>2889:dsk,rs}
                                  cxl{a<1794:R,a<2060:R,m>3613:R,R}
                                  lrt{x>1214:A,m<826:A,x<1148:R,A}
                                  kdt{a>3422:A,a>3354:vx,a<3313:A,ff}
                                  qc{s>566:R,a<43:R,A}
                                  jnn{s>1664:zsx,m<3370:tmg,m<3740:R,lj}
                                  rlx{s>2568:A,s>1841:R,x<2496:A,R}
                                  cxb{x>1364:R,A}
                                  vp{m<625:mt,m>1209:psm,prx}
                                  jbz{x<1581:A,R}
                                  fld{x>580:A,a<2939:vz,qt}
                                  gz{m<2078:A,m>2102:A,m>2086:R,A}
                                  pq{s<3162:R,s<3713:R,R}
                                  nvr{a>3064:R,x>1573:A,a>2695:R,A}
                                  mt{m<212:xgt,m<391:msk,a<1427:dh,nqj}
                                  qq{m<1871:A,x>2374:A,m>2819:R,R}
                                  kx{s>3712:R,a<3138:A,a>3552:A,sb}
                                  zl{a<3279:R,s>3644:R,s<3369:R,A}
                                  zhr{x>1414:mhr,s<1315:R,R}
                                  lr{a<3472:fl,x<1240:qzt,m>1702:A,A}
                                  zn{m>3032:bz,R}
                                  kdb{m>2017:lh,vg}
                                  xdq{a>3298:fs,s>454:nv,R}
                                  jj{x<1385:R,s>2524:A,a<2778:R,R}
                                  hh{s>1624:bqx,m<725:lf,m<1340:hkg,qkd}
                                  tt{x>3601:A,R}
                                  ff{m>2244:A,x>1175:R,s>652:A,A}
                                  nb{m<2342:sx,s<1466:A,gc}
                                  gg{x>2442:R,A}
                                  zf{x>2749:A,R}
                                  ms{a<3730:A,A}
                                  pnx{a>3878:A,R}
                                  thm{a<2994:R,m<2863:R,R}
                                  dcn{x<1937:R,x>2071:A,m<1510:R,R}
                                  zh{x<1391:A,a<2729:A,R}
                                  mzt{m>3358:R,s>2801:thm,x>1477:kzl,A}
                                  vk{x<1245:hfv,a<2636:rmp,ldb}
                                  cdl{x>2787:A,A}
                                  gsh{x>2372:A,s<2731:tgk,ng}
                                  ss{s>1887:A,R}
                                  lsm{a<346:qqd,s<179:pls,a<518:hxk,bd}
                                  kl{m<767:cm,s>2943:hlm,s<2861:R,R}
                                  jc{a>2461:R,R}
                                  lq{x<3091:A,m>3019:R,s<338:R,A}
                                  dc{m>3120:R,s<2936:R,A}
                                  fjx{a<3729:R,m>577:bhz,A}
                                  slh{s<2472:A,x>841:A,x>782:A,R}
                                  jn{a>3008:A,m<154:R,A}
                                  rnt{m<1544:fqb,pt}
                                  lzl{a>2640:kk,s<3306:A,x<1073:R,kft}
                                  rjm{a<2579:R,x<1122:A,x<1178:A,A}
                                  xxn{a>3097:R,x>2616:R,x>2520:A,R}
                                  gmk{a>432:rkd,m<699:mp,np}
                                  gk{a<343:xnh,st}
                                  mtr{m<974:cx,a>2634:lp,a>2480:mb,vvl}
                                  sn{s<1735:R,x>3011:R,A}
                                  hcp{m<2836:A,A}
                                  bzv{m<2882:A,m>3102:A,x>1615:A,R}
                                  gpf{a>786:vp,x>3204:gk,fgh}
                                  bqh{x>1390:A,a<781:A,a<1479:R,A}
                                  zk{s<581:R,a>3168:R,A}
                                  rzf{x>2996:R,A}
                                  pt{m>2840:R,sqf}
                                  dr{a<1633:R,rx}
                                  bj{m<2330:A,R}
                                  rp{a<2416:A,x<1528:A,R}
                                  fv{x<1478:zhr,xm}
                                  zkq{s>2684:zct,jmx}
                                  hg{x>1427:xkb,s>2386:R,s>2348:jgs,R}
                                  ljp{x<3070:qmk,vkc}
                                  qcd{x>2958:kq,m<1982:R,A}
                                  dh{a<1030:A,x<2896:tns,s>1349:hn,A}
                                  nn{a<2330:A,s<1631:A,x>1119:R,A}
                                  tcc{x<1454:xz,s>1712:xb,x>1524:dvn,tb}
                                  prd{m>3593:R,s<3539:A,A}
                                  prx{s<1392:pm,a<1410:bxh,lg}
                                  dnj{s>2561:zr,m>1174:nh,s<2439:hg,fjx}
                                  ltr{s<2819:chr,x>3007:fmv,ndx}
                                  zls{s<2833:R,x>2257:R,R}
                                  tj{a>3851:bfb,gx}
                                  cqn{x>1072:rjm,s<3160:ktb,fjt}
                                  sps{a<845:gts,a>1373:hkv,s>1282:jnn,mcd}
                                  pk{x>1234:A,s>1197:tr,x<1173:cl,R}
                                  lp{a>3002:R,s>1470:R,m<1336:A,ksz}
                                  bnl{a<1615:A,s<1889:R,a<2234:R,R}
                                  px{x>2650:ln,m>3593:A,zls}
                                  bqx{m<671:zvx,m>1076:A,x>1511:R,kv}
                                  tg{x<1547:R,a>3186:A,a<2861:R,A}
                                  bbv{x>1046:A,R}
                                  nl{m>2498:A,s<1270:R,s>1844:R,R}
                                  gxg{a<3560:A,A}
                                  htm{a>1548:R,ft}
                                  jzj{a<3414:R,s>2535:R,a<3803:A,R}
                                  tl{m>591:R,s<2128:R,A}
                                  lj{a>1135:R,A}
                                  qt{x>217:A,a>3101:A,R}
                                  zmn{x>1568:R,m<3662:A,s>2942:R,A}
                                  gqf{a<319:A,lc}
                                  pn{x<1566:fnx,R}
                                  lm{x<748:A,m>1097:slh,mnc}
                                  xb{a>3483:A,R}
                                  hbn{x<2347:rz,s<1021:xxn,bj}
                                  fps{s<2809:R,m>978:R,a<1677:R,A}
                                  tns{m>508:A,R}
                                  ftz{m<1047:A,R}
                                  hsq{s<3306:A,R}
                                  rcf{m<789:R,a>3811:A,R}
                                  zsx{m<3362:A,A}
                                  qnm{m<1707:jq,m>1918:A,klh}
                                  ktb{a>2476:R,a<2324:R,m<1618:R,R}
                                  tts{s<974:A,a>655:R,x>2433:A,A}
                                  nf{a<3288:vk,zpd}
                                  cj{x<1368:dll,m>2179:bzm,s<2729:ntm,qd}
                                  vtz{s>967:zzr,x<1579:R,ts}
                                  zv{a>3078:R,A}
                                  xxs{a>80:R,m<655:R,A}
                                  tsc{a<3864:A,a>3938:R,m>1924:R,R}
                                  bf{x>2895:fkk,vmf}
                                  rk{m>3266:R,m>2870:dc,a<3009:zh,mgq}
                                  nqj{s>1829:R,s>913:vs,mnk}
                                  fd{s<871:A,R}
                                  mj{m>871:A,a>2639:R,a<2371:A,A}
                                  qbm{m>1109:A,s>3601:R,x>1512:R,A}
                                  dpc{m<2093:sgl,R}
                                  vkc{a>3311:ghs,a>2714:bxf,rv}
                                  sx{s>1313:R,A}
                                  gl{x>1498:qbm,m<1020:rg,A}
                                  jbx{a>706:R,x>2598:A,pb}
                                  hxk{a>436:tgn,gs}
                                  tpg{s>1250:R,m>1185:A,s<1173:A,R}
                                  qdv{s<616:mkj,qq}
                                  msk{a<1506:sn,s>2324:A,qg}
                                  xjk{s>2290:R,R}
                                  grd{s<2548:R,A}
                                  xlg{s>3562:A,a>543:A,A}
                                  xnh{x<3580:nr,m>777:fcg,m<313:vpk,vbp}
                                  kh{m<1498:A,A}
                                  zsf{x<948:mf,a<2235:fp,s>2045:cj,zhj}
                                  xsm{s<799:R,x<1241:R,a>872:A,R}
                                  xp{m<1016:R,R}
                                  vpk{x<3813:tv,R}
                                  rr{s<604:R,R}
                                  slt{s<3054:tt,s>3473:A,R}
                                  pxn{m<2918:R,R}
                                  tqg{m>2713:gvv,x<1585:zps,A}
                                  xl{a<3724:R,m<814:A,m<1249:R,R}
                                  lqs{a<3226:pj,x<1533:gl,a>3574:nqz,cnm}
                                  qhb{m>398:R,s>521:A,x<2791:A,R}
                                  pp{a>3287:A,s<2490:R,a<3039:A,A}
                                  klh{a>3097:A,a<2627:A,x>1416:A,R}
                                  kzl{x>1498:A,R}
                                  cd{m>3789:R,s>2774:rd,jjh}
                                  qvj{s>2916:lb,m>1773:gg,A}
                                  nm{m<3503:stl,a>3204:jtx,s<3198:vr,prd}
                                  zfm{a<3405:cf,x>1270:dk,A}
                                  dll{a>2858:kr,x<1210:cq,rnt}
                                  zvx{m<440:A,R}
                                  rs{s>2487:A,m>583:A,m<352:zvt,ns}
                                  gv{x>2386:A,x<2306:A,A}
                                  tsg{x>1593:R,A}
                                  bft{m<1111:R,A}
                                  zld{x<1595:tqg,a<3278:pxn,ds}
                                  nlj{s<37:A,R}
                                  dd{m<360:jn,x<1406:xlx,zl}
                                  xkp{a>723:R,a>685:A,a>649:A,A}
                                  phz{a>3010:R,R}
                                  zvv{m>587:btd,m<276:jbx,x>2637:dnf,smg}
                                  ldb{s>500:R,s<176:A,x>1432:svj,sv}
                                  gm{m<544:dd,sd}
                                  rrp{s>3471:dbf,qnm}
                                  tgk{s>1833:R,m>698:A,a<170:A,A}
                                  tf{a<2989:R,s<2648:R,m<978:A,A}
                                  jv{a>1049:bk,bh}
                                  zjq{x<394:A,s<2172:A,x>409:R,A}
                                  sb{a>3392:A,s<3499:A,s<3590:A,A}
                                  gbv{s>1783:R,m<518:A,R}
                                  mb{a<2566:R,A}
                                  jbg{x<2977:A,R}
                                  cq{m>2591:lzl,cqn}
                                  gpp{s>127:A,R}
                                  qxs{x<630:R,R}
                                  ltd{m<1040:fc,s<1339:R,fzh}
                                  tv{m<191:R,R}
                                  vbp{m>508:A,a<198:hc,m<391:R,qcx}
                                  qhx{s<1393:A,A}
                                  zt{x<829:A,s>2272:A,m>3119:R,A}
                                  xlx{s<3549:R,s<3703:R,a<2825:A,A}
                                  mnc{m>410:R,x<820:R,A}
                                  jts{s>2528:A,a>1459:A,nl}
                                  vfp{a<268:slp,a<349:A,A}
                                  xkb{s>2400:A,a>3661:R,s<2364:A,R}
                                  jxb{x>1512:R,m>1668:R,s<3371:R,A}
                                  jjh{s>2302:A,s<2157:R,R}
                                  dfg{m<3080:bnv,R}
                                  sft{m>85:R,A}
                                  kft{s<3765:R,x<1151:A,A}
                                  ql{m<2989:bnl,A}
                                  hmq{s>328:A,R}
                                  zgl{s<3038:xp,s>3136:R,tg}
                                  jz{x<1535:nz,vcx}
                                  fpp{a<1269:A,s<2540:R,m<877:A,A}
                                  sl{x<1465:kl,zgl}
                                  ghs{s<1180:jk,A}
                                  gdm{s<3410:A,dcn}
                                  mp{a>355:nlg,x>2257:A,R}
                                  tgn{s>384:A,A}
                                  rck{a>3096:A,m<729:R,R}
                                  bfh{s<2430:A,A}
                                  dm{x<3740:A,a<84:A,R}
                                  cl{x>1152:A,a>3659:A,A}
                                  lf{s<590:A,m<384:dz,ndj}
                                  kr{s<3089:tk,x>1116:lr,fq}
                                  ps{x<3131:R,a>1808:R,R}
                                  zc{a<3742:R,s<459:A,A}
                                  hdq{a>1180:A,a<969:A,m<1327:xjk,vxc}
                                  vtn{x>2571:gz,A}
                                  vsp{m<1477:kf,m>1506:A,x<1988:A,R}
                                  pf{m<1916:zj,nsl}
                                  qqb{a<3381:R,a<3544:A,R}
                                  qmk{s>1137:fkj,hbn}
                                  fjk{m<484:qhx,s>1346:mqz,a<3843:qn,cxh}
                                  fzh{a>3465:R,s<1753:A,R}
                                  ll{a<1599:A,m>2800:R,m<2548:R,R}
                                  pm{x<2958:A,A}
                                  ds{s>3322:A,x>1607:bzv,s<2559:A,tbk}
                                  qqd{a>147:xxr,x<2716:hmq,x<3308:R,dm}
                                  zj{x>2856:rm,a>694:qvj,jkr}
                                  ntm{s<2328:lhn,x>1477:lhg,a<3351:pd,dnj}
                                  gs{x>3009:R,m>1822:R,m>1623:A,A}
                                  vpz{a<955:prm,htm}
                                  fl{s<3647:A,R}
                                  vf{m>1405:R,x<364:R,x<439:zjq,A}
                                  sfv{m>3249:R,a<2323:A,a<2508:A,R}
                                  ssx{s<1450:R,R}
                                  xtx{s<2531:bfh,tf}
                                  fvz{s<1875:R,a<2683:tpn,s<2272:kfs,pp}
                                  zpd{s<374:gpp,a<3568:kdt,jfx}
                                  bq{a>2468:A,R}
                                  mx{x>3691:R,A}
                                  zvt{m<232:R,x<1498:R,R}
                                  qbh{a>359:bbv,A}
                                  rx{s<1895:R,x<93:A,R}
                                  pls{s<102:nlj,m>1957:A,x>2610:R,R}
                                  gxz{x<1441:A,a>3782:A,a<3736:A,A}
                                  vcx{x<1581:A,x>1598:R,A}
                                  mnk{x>2686:R,A}
                                  hc{m>386:R,s<1896:R,x>3841:A,R}
                                  jkr{x<2289:R,a<434:bjh,s>2706:xlg,ddr}
                                  lxj{x<1526:A,a<2868:tl,s<2105:A,R}
                                  cs{s>2587:A,s<2454:R,x>1605:A,A}
                                  tb{a<3403:R,x<1483:A,m<408:A,R}
                                  jrk{x<1493:R,A}
                                  np{s<2392:txz,R}
                                  fgh{a<278:kjv,a>610:zvv,gmk}
                                  nsl{a>1286:pvt,m>2128:ltr,m<2048:qcd,vtn}
                                  sp{x<3783:zzd,R}
                                  mnr{a<2963:R,R}
                                  kn{s>2492:R,R}
                                  sg{x>3309:R,A}
                                  kf{x>1948:A,A}
                                  sv{x>1311:A,m>2072:A,R}
                                  ddr{a>588:R,A}
                                  mmf{s>1614:A,a<588:fd,s>637:A,R}
                                  mzd{a>3041:kfj,hjp}
                                  jb{x>749:A,x<663:R,a<217:R,A}
                                  zq{x<2680:rck,zd}
                                  kk{s>3062:A,x<1054:R,x>1150:R,A}
                                  mcd{s>737:R,lq}
                                  km{a<454:ssq,x>1215:A,R}
                                  jp{m>1384:R,a>1361:A,m<1302:R,A}
                                  zcn{m>1641:A,x<1412:R,a<3907:R,R}
                                  hnp{m<624:R,m<1118:R,a>3867:A,A}
                                  vvl{a>2360:jrk,A}
                                  vt{a>1209:zrc,s>2239:km,a>526:db,rnb}
                                  xsn{a<3485:A,x>2424:rcf,x<2143:R,cmr}
                                  fmv{m>2241:A,A}
                                  mq{a<2726:rp,rpx}
                                  qsc{a>1561:shk,s>289:dpc,cpr}
                                  vn{x<2495:A,R}
                                  zhj{s<820:nf,x<1349:mzd,m<1523:ccq,jmm}
                                  rb{x>1585:cs,x<1567:bbc,m>1281:nvr,R}
                                  vl{a<701:A,a<749:R,R}
                                  bk{m>2181:ql,x>534:lm,x>246:vf,dr}
                                  bt{s<1575:R,x>1110:lrt,A}
                                  cg{x<1226:R,R}
                                  pb{s>1976:R,R}
                                  qmz{x>1511:R,hx}
                                  hx{s<2479:A,a>3026:R,x>1492:A,R}
                                  vxc{s<1908:R,R}
                                  psm{x>2500:vmq,m<1395:hdq,vsp}
                                  bg{s>1448:zkq,s<952:bf,ljp}
                                  rg{a<3524:R,R}
                                  mxf{a<1247:hj,m<3013:rpf,px}
                                  cnm{m<854:A,s>3702:A,A}
                                  fjd{m>911:A,s>2513:bq,A}
                                  mkj{x>2290:R,m<1691:A,s>347:A,A}
                                  vhv{s>3440:A,x<2695:R,R}
                                  hcq{a>2835:R,R}
                                  vmq{s>2256:A,s<1332:jp,x<3237:ss,A}
                                  nz{a<3118:A,x<1466:R,s<2259:A,R}
                                  ndx{m>2236:R,x>2459:A,s<3532:R,A}
                                  kz{s<1342:R,R}
                                  lb{s>3471:A,a>1457:A,A}
                                  vz{a>2782:R,A}
                                  cm{a<3264:R,a>3609:R,A}
                                  nqz{s>3591:R,m>1381:jbz,m<596:A,tsg}
                                  dk{x<1297:A,a>3642:R,A}
                                  mf{a<2621:jv,kdb}
                                  lc{x<1574:A,m>2523:R,a<541:A,A}
                                  cf{a<3177:R,R}
                                  rgg{x>754:zt,a<3650:qqb,mpb}
                                  fq{s<3534:R,s<3736:gps,R}
                                  bfb{s>1404:R,s>1128:R,srj}

                                  {x=61,m=577,a=1750,s=1892}
                                  {x=8,m=603,a=567,s=987}
                                  {x=1398,m=1265,a=702,s=949}
                                  {x=2322,m=693,a=2401,s=946}
                                  {x=348,m=2812,a=613,s=1788}
                                  {x=3353,m=2070,a=1617,s=3088}
                                  {x=24,m=963,a=4,s=1053}
                                  {x=396,m=877,a=3375,s=127}
                                  {x=1490,m=100,a=1060,s=724}
                                  {x=413,m=95,a=944,s=847}
                                  {x=1580,m=960,a=880,s=1400}
                                  {x=1956,m=597,a=3294,s=284}
                                  {x=1924,m=21,a=726,s=1276}
                                  {x=1338,m=1630,a=348,s=114}
                                  {x=11,m=1649,a=475,s=1091}
                                  {x=1660,m=563,a=1047,s=150}
                                  {x=18,m=226,a=33,s=3744}
                                  {x=3145,m=482,a=866,s=629}
                                  {x=474,m=260,a=1408,s=9}
                                  {x=361,m=1348,a=592,s=380}
                                  {x=2632,m=1281,a=234,s=1114}
                                  {x=1871,m=543,a=1285,s=383}
                                  {x=1870,m=364,a=106,s=70}
                                  {x=536,m=528,a=272,s=2050}
                                  {x=1519,m=1564,a=434,s=103}
                                  {x=301,m=2005,a=1069,s=79}
                                  {x=293,m=143,a=3201,s=68}
                                  {x=1308,m=1701,a=271,s=1627}
                                  {x=1538,m=1576,a=249,s=290}
                                  {x=320,m=110,a=1490,s=492}
                                  {x=494,m=904,a=2049,s=1122}
                                  {x=13,m=695,a=1851,s=145}
                                  {x=136,m=1102,a=90,s=879}
                                  {x=1786,m=8,a=663,s=86}
                                  {x=238,m=2582,a=1288,s=3169}
                                  {x=1628,m=1807,a=373,s=116}
                                  {x=86,m=51,a=500,s=1893}
                                  {x=2848,m=36,a=2897,s=1757}
                                  {x=774,m=530,a=970,s=2379}
                                  {x=353,m=1490,a=844,s=2085}
                                  {x=1363,m=938,a=1103,s=1200}
                                  {x=1843,m=6,a=1853,s=882}
                                  {x=1966,m=3149,a=327,s=201}
                                  {x=1116,m=849,a=1822,s=326}
                                  {x=233,m=3172,a=32,s=1141}
                                  {x=3523,m=19,a=38,s=176}
                                  {x=2896,m=511,a=1180,s=2859}
                                  {x=292,m=843,a=98,s=98}
                                  {x=923,m=606,a=2898,s=2343}
                                  {x=46,m=1317,a=78,s=51}
                                  {x=462,m=1255,a=1485,s=737}
                                  {x=1238,m=1292,a=27,s=1525}
                                  {x=2731,m=411,a=1831,s=1574}
                                  {x=202,m=426,a=483,s=349}
                                  {x=33,m=394,a=941,s=1705}
                                  {x=2648,m=510,a=703,s=2332}
                                  {x=1429,m=836,a=316,s=63}
                                  {x=2817,m=1701,a=174,s=160}
                                  {x=261,m=206,a=76,s=2192}
                                  {x=204,m=2989,a=283,s=597}
                                  {x=1068,m=1881,a=3293,s=2503}
                                  {x=31,m=37,a=826,s=2983}
                                  {x=547,m=466,a=1586,s=481}
                                  {x=1419,m=822,a=273,s=1670}
                                  {x=392,m=168,a=452,s=61}
                                  {x=1245,m=2719,a=778,s=2034}
                                  {x=1150,m=1565,a=1098,s=502}
                                  {x=1180,m=30,a=1228,s=223}
                                  {x=727,m=6,a=224,s=144}
                                  {x=1784,m=863,a=741,s=1461}
                                  {x=1200,m=1331,a=113,s=1114}
                                  {x=3108,m=312,a=246,s=1943}
                                  {x=970,m=1845,a=3752,s=940}
                                  {x=2062,m=420,a=290,s=264}
                                  {x=1626,m=1844,a=337,s=321}
                                  {x=1124,m=1302,a=335,s=1250}
                                  {x=205,m=203,a=102,s=292}
                                  {x=228,m=1798,a=562,s=326}
                                  {x=897,m=832,a=1193,s=1826}
                                  {x=336,m=1941,a=1348,s=2270}
                                  {x=162,m=275,a=3043,s=1022}
                                  {x=361,m=268,a=2199,s=1749}
                                  {x=328,m=1482,a=1865,s=1420}
                                  {x=474,m=2434,a=2447,s=1199}
                                  {x=1066,m=3051,a=1482,s=50}
                                  {x=252,m=1317,a=349,s=894}
                                  {x=1241,m=283,a=3431,s=503}
                                  {x=958,m=403,a=928,s=487}
                                  {x=1193,m=830,a=3266,s=2617}
                                  {x=914,m=1239,a=275,s=3325}
                                  {x=2885,m=1206,a=1998,s=313}
                                  {x=831,m=1327,a=447,s=181}
                                  {x=3912,m=2397,a=522,s=1136}
                                  {x=2222,m=375,a=1129,s=595}
                                  {x=3340,m=205,a=1197,s=1910}
                                  {x=2206,m=20,a=1354,s=63}
                                  {x=1068,m=916,a=10,s=1564}
                                  {x=316,m=827,a=371,s=1808}
                                  {x=3287,m=329,a=2730,s=22}
                                  {x=863,m=2792,a=321,s=1546}
                                  {x=1470,m=301,a=795,s=249}
                                  {x=582,m=96,a=2814,s=1126}
                                  {x=387,m=461,a=1526,s=355}
                                  {x=23,m=1590,a=1448,s=269}
                                  {x=1983,m=165,a=714,s=1140}
                                  {x=513,m=1006,a=835,s=1438}
                                  {x=25,m=280,a=1263,s=2950}
                                  {x=823,m=973,a=949,s=166}
                                  {x=720,m=255,a=92,s=184}
                                  {x=867,m=1262,a=153,s=569}
                                  {x=443,m=376,a=4,s=656}
                                  {x=108,m=600,a=958,s=64}
                                  {x=609,m=1212,a=3436,s=1060}
                                  {x=267,m=645,a=2807,s=959}
                                  {x=159,m=2,a=557,s=1673}
                                  {x=2744,m=49,a=49,s=2043}
                                  {x=835,m=33,a=1119,s=440}
                                  {x=2697,m=499,a=386,s=1553}
                                  {x=456,m=110,a=666,s=625}
                                  {x=1274,m=1335,a=336,s=124}
                                  {x=2341,m=1835,a=100,s=2163}
                                  {x=2104,m=1161,a=234,s=1831}
                                  {x=915,m=1660,a=792,s=429}
                                  {x=314,m=384,a=33,s=294}
                                  {x=628,m=579,a=2605,s=1746}
                                  {x=2,m=2227,a=1151,s=38}
                                  {x=294,m=1196,a=1330,s=295}
                                  {x=517,m=2634,a=994,s=267}
                                  {x=272,m=25,a=882,s=217}
                                  {x=599,m=652,a=951,s=126}
                                  {x=255,m=650,a=955,s=102}
                                  {x=25,m=1704,a=2216,s=13}
                                  {x=142,m=817,a=208,s=921}
                                  {x=1932,m=746,a=1646,s=2252}
                                  {x=1157,m=6,a=2213,s=3366}
                                  {x=595,m=10,a=2,s=2750}
                                  {x=696,m=955,a=3215,s=8}
                                  {x=2223,m=3582,a=47,s=1055}
                                  {x=302,m=330,a=420,s=495}
                                  {x=128,m=2241,a=1155,s=387}
                                  {x=225,m=151,a=682,s=916}
                                  {x=844,m=181,a=1842,s=2310}
                                  {x=270,m=837,a=1311,s=2122}
                                  {x=3135,m=829,a=1974,s=164}
                                  {x=1061,m=1019,a=2238,s=571}
                                  {x=2109,m=1065,a=316,s=1054}
                                  {x=1523,m=512,a=42,s=342}
                                  {x=1378,m=2105,a=1968,s=3674}
                                  {x=51,m=94,a=683,s=832}
                                  {x=235,m=1036,a=121,s=741}
                                  {x=18,m=713,a=250,s=484}
                                  {x=193,m=2650,a=569,s=210}
                                  {x=1203,m=2854,a=644,s=515}
                                  {x=3,m=881,a=967,s=1500}
                                  {x=375,m=2028,a=214,s=135}
                                  {x=1693,m=696,a=2186,s=138}
                                  {x=1347,m=655,a=722,s=111}
                                  {x=678,m=995,a=3274,s=183}
                                  {x=1720,m=242,a=1637,s=933}
                                  {x=798,m=1710,a=3735,s=1521}
                                  {x=3046,m=335,a=1055,s=296}
                                  {x=632,m=6,a=207,s=591}
                                  {x=109,m=2344,a=40,s=413}
                                  {x=387,m=121,a=171,s=2554}
                                  {x=1546,m=159,a=2373,s=1578}
                                  {x=875,m=21,a=377,s=2524}
                                  {x=1641,m=1125,a=694,s=2737}
                                  {x=188,m=310,a=378,s=498}
                                  {x=589,m=174,a=2495,s=2769}
                                  {x=2918,m=258,a=643,s=47}
                                  {x=833,m=214,a=333,s=24}
                                  {x=1982,m=200,a=569,s=1014}
                                  {x=341,m=196,a=1404,s=286}
                                  {x=1009,m=248,a=281,s=1903}
                                  {x=547,m=2531,a=215,s=117}
                                  {x=904,m=249,a=46,s=109}
                                  {x=186,m=1706,a=1307,s=732}
                                  {x=1182,m=1615,a=299,s=1275}
                                  {x=2136,m=1349,a=1907,s=523}
                                  {x=13,m=1375,a=1617,s=1599}
                                  {x=786,m=131,a=541,s=67}
                                  {x=12,m=223,a=1565,s=129}
                                  {x=837,m=261,a=22,s=363}
                                  {x=1011,m=249,a=146,s=251}
                                  {x=711,m=230,a=3042,s=246}
                                  {x=2794,m=1052,a=243,s=457}
                                  {x=2289,m=1093,a=2061,s=1219}
                                  {x=2223,m=176,a=1227,s=444}
                                  {x=1744,m=37,a=1590,s=87}
                                  {x=2590,m=2444,a=894,s=1864}
                                  {x=13,m=1356,a=648,s=121}
                                  {x=117,m=125,a=1222,s=669}
                                  {x=1048,m=419,a=1031,s=210}
                                  {x=397,m=2126,a=412,s=977}
                                  {x=1256,m=875,a=3898,s=3195}
                                  {x=3273,m=322,a=702,s=4}
                                  {x=956,m=500,a=522,s=596}
                                  {x=339,m=1107,a=2588,s=970}
                                  {x=493,m=43,a=1683,s=1244}
                                  {x=303,m=484,a=425,s=207}
                                  """;
}
