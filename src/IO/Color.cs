using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    public class Color
    {
        public int r = 255;
        public int g = 255;
        public int b = 255;

        public Color(int r, int g, int b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public Color Apply(float lightLevel)
        {
            return new Color((int)(r * lightLevel), (int)(g * lightLevel), (int)(b * lightLevel));
        }

        public String ToCode()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\u001b[38;2;");
            sb.Append(r);
            sb.Append(";");
            sb.Append(g);
            sb.Append(";");
            sb.Append(b);
            sb.Append("m");
            return sb.ToString();
        }

        public void Set(int r, int g, int b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
    }
}
