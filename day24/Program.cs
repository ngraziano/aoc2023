

var listHail = File.ReadLines("input.txt").Select(line => line.Split(" @ ")).Select(parts =>

    parts switch
    {
        [var src, var dir] => (src.Split(',').Select(s => s.Trim()).Select(Int128.Parse).ToArray(),
                               dir.Split(',').Select(s => s.Trim()).Select(Int128.Parse).ToArray()),
        _ => throw new InvalidOperationException()

    }
).ToList();



/* 
  Intersections
    a1+b1*t1 = c1+d1*t2
    a2+b2*t1 = c2+d2*t2

    a1*d2 + b1*d2*t1 = c1*d2 + d1*d2*t2
    a2*d1 + b2*d1*t1 = c2*d1 + d2*d1*t2

    a1*d2 - a2*d1 + (b1*d2-b2*d1)*t1 = c1*d2-c2*d1

    t1 = (c1*d2 -c2*d1 -a1*d2 + a2*d1)/(b1*d2-b2*d1)

    d1*t2 = a1 + b1*t1 -c1

 */

var minArea = 200000000000000L;
var maxArea = 400000000000000L;

var nbIntersect = listHail[..^1].SelectMany((ab, idx) => listHail[(idx + 1)..].Select(cd =>
{
    var a1 = ab.Item1[0];
    var a2 = ab.Item1[1];
    var b1 = ab.Item2[0];
    var b2 = ab.Item2[1];
    var c1 = cd.Item1[0];
    var c2 = cd.Item1[1];
    var d1 = cd.Item2[0];
    var d2 = cd.Item2[1];


    if (b1 * d2 == b2 * d1)
        return false;

    var t1 = (c1 * d2 - c2 * d1 - a1 * d2 + a2 * d1) / (b1 * d2 - b2 * d1);
    var t2 = (a1 + b1 * t1 - c1) / d1;
    if (t1 < 0 || t2 < 0)
        return false;

    var x = a1 + b1 * t1;
    var y = a2 + b2 * t1;

    return x >= minArea && y >= minArea
        && x <= maxArea && y <= maxArea;
})).Count(c => c);

Console.WriteLine($"Part 1 {nbIntersect}");


/* Part 2
   a1+b1*t1 = m1+n1*t1                             
   a2+b2*t1 = m2+n2*t1
   a3+b3*t1 = m3+n3*t1

   a'1+b'1*t2 = m1+n1*t2
   a'2+b'2*t2 = m2+n2*t2
   a'3+b'3*t2 = m3+n3*t2

   a''1+b''1*t3 = m1+n1*t3
   a''2+b''2*t3 = m2+n2*t3
   a''3+b''3*t3 = m3+n3*t3


  // supplement
   a'''1+b'''1*t4 = m1+n1*t4
   a'''2+b'''2*t4 = m2+n2*t4
   a'''3+b'''3*t4 = m3+n3*t4


   // we search m1+m2+m3

    a + b * t1 = m + n * t1   
    a' + b' * t2 = m + n * t2   
    a'' + b'' * t3 = m + n * t3

   t1 = (a-m)/(n-b)
   t2 = (a'-m)/(n-b')
   t3 = (a''-m)/(n-b'')

    a1 + a'2 + a''3 + b1*t1 +b'2*t2 + b''3*t3 = m + n1*t1 +n2*t2 +n3*t3
    a2 + a'3 + a''1 + b2*t1 +b'3*t2 + b''1*t3 = m + n2*t1 +n3*t2 +n1*t3
    a3 + a'1 + a''2 + b3*t1 +b'1*t2 + b''2*t3 = m + n3*t1 +n1*t2 +n2*t3
    a1 + a'3 + a''2 + b1*t1 +b'3*t2 + b''2*t3 = m + n1*t1 + n3*t2 +n2*t3
    a2 + a'1 + a''3 + b2*t1 +b'1*t2 + b''3*t3 = m + n2*t1 + n1*t2 +n3*t3
    a3 + a'2 + a''3 + b2*t1 +b'1*t2 + b''1*t3 = m + n3*t1 + n2*t2 +n1*t3


    

   

*/



var vitesse= Enumerable.Range(0, 3).Select(coor =>
{

    var possibleValue = Enumerable.Range(-1000, 2000).ToList();

    //  search same x velocity
    foreach (var (idx1, idx2, ab, cd) in
        listHail[..^1].SelectMany((ab, idx) => listHail[(idx + 1)..].Select((cd, idx2) => (cd, idx2)).Where(e => ab.Item2[coor] == e.cd.Item2[coor]).Select(e => (idx, e.idx2, ab, e.cd))))
    {
        var a = ab.Item1[coor];
        var b = ab.Item2[coor];
        var c = cd.Item1[coor];
        var d = cd.Item2[coor];


        /* 
        a+b*t1 = m + vx * t1 
        c+d*t2 = m + vx * t2

        a -c + b* t1 - d* t2 = vx (t1 - t2)
        or b=d
        (a-c) + b(t1-t2) = vx (t1-t2)
        (a-c)= (vx-b)(t1-t2)

        all integer
        so (a-c)%(vx-b) =0

         */
        possibleValue = possibleValue.Where(v => v != b && (a - c) % (v - b) == 0).ToList();
        Console.WriteLine($"idx1 {idx1} idx2 {idx2} , nbValue = {possibleValue.Count}");

        if (possibleValue.Count == 1)
        {
            var v = possibleValue[0];

            Console.WriteLine($"Found {v}");

            break;
        }
    }
    if (possibleValue.Count != 1)
    {
        throw new Exception("Fail to found");
    }
    return possibleValue[0];

}).ToList();



/* 
  a1+b1*t = m1 + v1 * t 
  a2+b2*t = m2 + v2 * t 
  a3+b3*t = m3 + v3 * t 

  a1  = m1 + (v1 - b1) * t 
  a2  = m2 + (v2 - b2) * t 

  a1(v2-b2) - a2(v1-b1) = m1(v2-b2) - m2(v1-b1)
 
  a'1+b'1*t' = m1 + v1 * t' 
  a'2+b'2*t' = m2 + v2 * t'
  
  a'1(v2-b'2) - a'2(v1-b'1) = m1(v2-b'2) - m2(v1-b'1)
  a1(v2-b2)   - a2(v1-b1)   = m1(v2-b2)  - m2(v1-b1)
 
  (a'1(v2-b'2) - a'2(v1-b'1))(v1-b1) - (a1(v2-b2)   - a2(v1-b1))(v1-b'1) = m1 ( (v2-b'2)*(v1-b1) - (v2-b2)*(v1-b'1))

 

*/

var a1 = listHail[0].Item1[0];
var a2 = listHail[0].Item1[1];
var a3 = listHail[0].Item1[2];
var b1 = listHail[0].Item2[0];
var b2 = listHail[0].Item2[1];
var b3 = listHail[0].Item2[2];


var ap1 = listHail[1].Item1[0];
var ap2 = listHail[1].Item1[1];
var bp1 = listHail[1].Item2[0];
var bp2 = listHail[1].Item2[1];

var v1 = vitesse[0];
var v2 = vitesse[1];
var v3 = vitesse[2];

var m1 = ((ap1 * (v2 - bp2) - ap2 * (v1 - bp1)) * (v1 - b1) - (a1 * (v2 - b2) - a2 * (v1 - b1)) * (v1 - bp1)) / ((v2 - bp2) * (v1 - b1) - (v2 - b2) * (v1 - bp1));


Console.WriteLine($"m1 = { m1}");
var t = (a1 - m1) / (v1 - b1);
Console.WriteLine($"t = { t}");
var m2 = a2 + (b2-v2)*t;
Console.WriteLine($"m2 = {m2}");
var m3 = a3 + (b3 - v3) * t;
Console.WriteLine($"m3 = {m3}");
Console.WriteLine($"Part2 = {m1+m2+m3}");
