using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monaco.PathTree.ConsoleSample;

public class Rgba32Resource : Resource
{
    public int R { get; set; }
    public int G { get; set; }
    public int B { get; set; }
    public int A { get; set; }

    public Rgba32Resource(string name, int r, int g, int b, int a)
    {
        Name = name;

        R = r;
        G = g;
        B = b;
        A = a;
    }
}
