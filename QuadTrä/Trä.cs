using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Lite_olika_test.QuadTrä
{
    public class Trä<T>
    {
        Trä<T> NW;
        Trä<T> NE;
        Trä<T> SW;
        Trä<T> SE;
        int AntalGräns = 5;
        Point MinGräns = new Point(20,20);
        List<T> Objekt = new List<T>();
        public Trä()
        {
            
        }
    }
}