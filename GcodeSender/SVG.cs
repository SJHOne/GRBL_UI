// Copyright (c) 2010 Chris Yerga
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace GrblOutput
{
    /// <summary>
    /// Interface for a drawable object found in an SVG file. This
    /// is not a general-purpose SVG library -- it only does the
    /// bare minimum necessary to drive a CNC machine so this is
    /// mostly just to handle vectors and paths.
    /// </summary>
    public interface ISVGElement
    {
        // Retrieve the list of contours for this shape
        List<List<PointF>> GetContours();

        // System.Drawing Path
        GraphicsPath GetPath();

        // Fill and outline
        double OutlineWidth { get; }
        Color OutlineColor { get; }
        Color FillColor { get; }
    }

    /// <summary>
    /// A contour is a set of points describing a closed polygon.
    /// </summary>
    public class VectorContour
    {
        public double Brightness { get; set; }
        public Color Color { get; set; }
        public IEnumerable<PointF> Points { get; set; }
    }

    /// <summary>
    /// Base class for shapes found in SVG file. This handles common tasks
    /// such as parsing styles, applying transforms, etc.
    /// </summary>
    public class SVGShapeBase
    {
        /// <summary>
        /// Constructor for SVGShapeBase class. Called from derived class 
        /// constructor.
        /// </summary>
        /// <param name="reader">XmlTextReader positioned at the XML element
        /// for the shape being constructed. This class uses it to look
        /// for style/transform attributes to apply to the shape</param>
        /// <param name="styleDictionary">Dictionary of named styles
        /// defined earlier in the SVG document, to be used should an
        /// XML style attribute with a name be encountered.</param>
        public SVGShapeBase(XmlTextReader reader, Dictionary<string, SVGStyle> styleDictionary)
        {
            string styleText = reader.GetAttribute("class");

            if (styleText != null)
            {
                string[] styleNames = styleText.Split(new char[] { ' ', '\t' });

                foreach (string styleName in styleNames)
                {
                    SVGStyle style = styleDictionary[styleName];

                    if (style.FillColorPresent)
                    {
                        FillColor = style.FillColor;
                    }
                    if (style.OutlineColorPresent)
                    {
                        OutlineColor = style.OutlineColor;
                    }
                    if (style.OutlineWidthPresent)
                    {
                        OutlineWidth = style.OutlineWidth;
                    }
                }
            }

            string xfs = reader.GetAttribute("transform");
            if (xfs != null)
            {
                if (xfs.StartsWith("matrix"))
                {
                    xfs = xfs.Substring(6);
                }
                xfs = xfs.Trim(new char[] { '(', ')' });
                string[] elements = xfs.Split(new char[] { ' ', '\t' });

                matrix = new Matrix(
                    float.Parse(elements[0]),
                    float.Parse(elements[1]),
                    float.Parse(elements[2]),
                    float.Parse(elements[3]),
                    float.Parse(elements[4]),
                    float.Parse(elements[5]));
            }
        }

        /// <summary>
        /// Transform the geometry of this shape as appropriate
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public List<PointF> Transform(List<PointF> points)
        {
            PointF[] pts = points.ToArray();

            matrix.TransformPoints(pts);
            return new List<PointF>(pts);
        }

        internal Matrix matrix = new Matrix();
        internal GraphicsPath _path;
        public GraphicsPath GetPath() { return _path; }
        public double OutlineWidth { get; set; }
        public Color OutlineColor { get; set; }
        public Color FillColor { get; set; }
    }

    /// <summary>
    /// SVG Rectangle
    /// </summary>
    public class SVGRect : SVGShapeBase, ISVGElement
    {
        private List<PointF> points = new List<PointF>();

        public SVGRect(XmlTextReader reader, Dictionary<string, SVGStyle> styleDictionary)
            : base(reader, styleDictionary)
        {
            float x = 0;
            try
            {
                x = float.Parse(reader.GetAttribute("x"));
            }
            catch (ArgumentNullException) { }

            float y = 0;
            try
            {
                y = float.Parse(reader.GetAttribute("y"));
            }
            catch (ArgumentNullException) { }
            float w = float.Parse(reader.GetAttribute("width"));
            float h = float.Parse(reader.GetAttribute("height"));

            points.Add(new PointF(x, y));
            points.Add(new PointF(x + w, y));
            points.Add(new PointF(x + w, y + h));
            points.Add(new PointF(x, y + h));
            points.Add(new PointF(x, y));

            points = Transform(points);

            _path = new GraphicsPath();
            _path.AddPolygon(points.ToArray());
        }

        public List<List<PointF>> GetContours()
        {
            var result = new List<List<PointF>>();
            result.Add(points);

            return result;
        }
    }

    /// <summary>
    /// SVG Image. This is handled differently from the rest of the shapes
    /// as it cannot be represented as vector contours. It loads the bitmap
    /// and elsewhere encapsulation is broken willy-nilly and the client
    /// code reaches in and grabs said bits. If you don't like it, go on
    /// the internet and complain.
    /// </summary>
    public class SVGImage : SVGShapeBase, ISVGElement
    {
        float x = 0;
        float y = 0;
        float width, height;
        public Image bits;
        public RectangleF DestBounds { get; set; }

        public SVGImage(XmlTextReader reader, Dictionary<string, SVGStyle> styleDictionary, string baseDocPath)
            : base(reader, styleDictionary)
        {
            try
            {
                x = float.Parse(reader.GetAttribute("x"));
            }
            catch { }

            try
            {
                y = float.Parse(reader.GetAttribute("y"));
            }
            catch { }
            width = float.Parse(reader.GetAttribute("width"));
            height = float.Parse(reader.GetAttribute("height"));
            string path = reader.GetAttribute("xlink:href");

            string dir = Path.GetDirectoryName(baseDocPath);
            string bitspath = Path.Combine(dir, path);
            bits = Image.FromFile(bitspath);

            PointF[] pts = new PointF[2];
            pts[0].X = x;
            pts[0].Y = y;
            pts[1].X = x + width;
            pts[1].Y = y + height;
            matrix.TransformPoints(pts);

            DestBounds = new RectangleF(pts[0].X, pts[0].Y, pts[1].X - pts[0].X, pts[1].Y - pts[0].Y);
        }

        public List<List<PointF>> GetContours()
        {
            var result = new List<List<PointF>>();

            return result;
        }
    }

    /// <summary>
    /// SVG Circle. We like polygons, so we just turn it into one of them.
    /// </summary>
    public class SVGCircle : SVGShapeBase, ISVGElement
    {
        private List<PointF> points = new List<PointF>();

        public SVGCircle(XmlTextReader reader, Dictionary<string, SVGStyle> styleDictionary)
            : base(reader, styleDictionary)
        {
            float cx = 0;
            try
            {
                cx = float.Parse(reader.GetAttribute("cx"));
            }
            catch { }

            float cy = 0;
            try
            {
                cy = float.Parse(reader.GetAttribute("cy"));
            }
            catch { }
            float r = float.Parse(reader.GetAttribute("r"));

            for (double theta = 0.0; theta < 2.0 * Math.PI; theta += Math.PI / 50.0)
            {
                double x = Math.Sin(theta) * r + cx;
                double y = Math.Cos(theta) * r + cy;

                points.Add(new PointF((float)x, (float)y));
            }
            points = Transform(points);

            _path = new GraphicsPath();
            _path.AddPolygon(points.ToArray());
        }

        public List<List<PointF>> GetContours()
        {
            var result = new List<List<PointF>>();
            result.Add(points);

            return result;
        }
    }

    /// <summary>
    /// SVG polygon. This maps directly to our canonical representation,
    /// so nothing fancy going on in here.
    /// </summary>
    public class SVGPolygon : SVGShapeBase, ISVGElement
    {
        private List<List<PointF>> contours = new List<List<PointF>>();
        private List<PointF> currentContour = new List<PointF>();


        public SVGPolygon(XmlTextReader reader, Dictionary<string, SVGStyle> styleDictionary)
            : base(reader, styleDictionary)
        {
            string data = reader.GetAttribute("points");
            string[] textPoints = data.Split(new char[] { ' ', '\t' });

            foreach (string textPoint in textPoints)
            {
                string[] ordinates = textPoint.Split(new char[] { ',' });
                if (ordinates.Length > 1)
                {

                    currentContour.Add(new PointF(float.Parse(ordinates[0]), float.Parse(ordinates[1])));
                }
            }

            if (currentContour.Count > 2)
            {
                float deltaX = currentContour[0].X - currentContour[currentContour.Count - 1].X;
                float deltaY = currentContour[0].Y - currentContour[currentContour.Count - 1].Y;

                if (Math.Abs(deltaX) + Math.Abs(deltaY) > 0.001)
                {
                    currentContour.Add(currentContour[0]);
                }
            }

            currentContour = Transform(currentContour);
            contours.Add(currentContour);

            _path = new GraphicsPath();
            if (currentContour.Count > 2)
            {
                _path.AddPolygon(currentContour.ToArray());
            }
        }

        public List<List<PointF>> GetContours()
        {
            return contours;
        }
    }

    /// <summary>
    /// SVG path. The XML mini-language is full-featured and complex, making 
    /// the parsing here the bulk of the work. Also, for curved portions of
    /// the path we approximate with polygons.
    /// </summary>
    public class SVGPath : SVGShapeBase, ISVGElement
    {
        private List<List<PointF>> contours = new List<List<PointF>>();
        private List<PointF> currentContour = new List<PointF>();

        enum ParseState
        {
            None,
            MoveToAbs,
            MoveToRel,
            CurveToAbs,
            CurveToRel,
            LineToAbs,
            LineToRel
        };
        ParseState state = ParseState.None;

        public float GetFloat(string data, ref int index)
        {
            StringBuilder builder = new StringBuilder();
            bool isnum = true;

            while (isnum)
            {
                if (index >= data.Length)
                {
                    isnum = false;
                }
                else
                {
                    switch (data[index])
                    {
                        case '0': break;
                        case '1': break;
                        case '2': break;
                        case '3': break;
                        case '4': break;
                        case '5': break;
                        case '6': break;
                        case '7': break;
                        case '8': break;
                        case '9': break;
                        case '-': break;
                        case '.': break;
                        case 'e': break;
                        case '+': break;

                        default: isnum = false; break;
                    }
                }

                if (isnum)
                {
                    builder.Append(data[index]);
                    ++index;
                }
            }

            return float.Parse(builder.ToString());
        }

        public SVGPath(XmlTextReader reader, Dictionary<string, SVGStyle> styleDictionary)
            : base(reader, styleDictionary)
        {
            _path = new GraphicsPath();

            string data = reader.GetAttribute("d");
            if (data == null)
            {
                return;
            }

            int index = 0;
            bool done = false;

            while (index < data.Length)
            {
                SkipSpace(data, ref index);

                switch (data[index])
                {
                    case 'M': state = ParseState.MoveToAbs; ++index; break;
                    case 'm': state = ParseState.MoveToRel; ++index; break;
                    case 'c': state = ParseState.CurveToRel; ++index; break;
                    case 'l': state = ParseState.LineToRel; ++index; break;
                    case 'L': state = ParseState.LineToAbs; ++index; break;
                    case 'z':
                        state = ParseState.None; ++index;
                        // Close current contour and open a new one
                        currentContour.Add(currentContour.First());
                        currentContour = Transform(currentContour);
                        contours.Add(currentContour);
                        _path.AddPolygon(currentContour.ToArray());
                        currentContour = new List<PointF>();
                        continue;

                    case '0': break;
                    case '1': break;
                    case '2': break;
                    case '3': break;
                    case '4': break;
                    case '5': break;
                    case '6': break;
                    case '7': break;
                    case '8': break;
                    case '9': break;
                    case '-': break;
                    case ' ': break;

                    default: throw new ApplicationException(string.Format("Unexpected input {0}", data[index]));
                }

                if (done)
                {
                    break;
                }

                SkipSpace(data, ref index);

                if (state == ParseState.MoveToAbs)
                {
                    float x = GetFloat(data, ref index);
                    SkipSpaceOrComma(data, ref index);
                    float y = GetFloat(data, ref index);

                    currentContour.Add(new PointF(x, y));
                }
                else if (state == ParseState.MoveToRel)
                {
                    float x = GetFloat(data, ref index);
                    SkipSpaceOrComma(data, ref index);
                    float y = GetFloat(data, ref index);

                    currentContour.Add(new PointF(PreviousPoint().X + x, PreviousPoint().Y + y));
                }
                else if (state == ParseState.LineToRel)
                {
                    float x = GetFloat(data, ref index);
                    SkipSpaceOrComma(data, ref index);
                    float y = GetFloat(data, ref index);

                    currentContour.Add(new PointF(PreviousPoint().X + x, PreviousPoint().Y + y));
                }
                else if (state == ParseState.LineToAbs)
                {
                    float x = GetFloat(data, ref index);
                    SkipSpaceOrComma(data, ref index);
                    float y = GetFloat(data, ref index);

                    currentContour.Add(new PointF(x, y));
                }
                else if (state == ParseState.CurveToRel)
                {
                    float cx1 = GetFloat(data, ref index);
                    if (data[index] != ',')
                    {
                        throw new ApplicationException("Expected comma");
                    }
                    ++index;
                    float cy1 = GetFloat(data, ref index);

                    if (data[index] != ' ')
                    {
                        throw new ApplicationException("Expected space");
                    }
                    ++index;

                    float cx2 = GetFloat(data, ref index);
                    if (data[index] != ',')
                    {
                        throw new ApplicationException("Expected comma");
                    }
                    ++index;
                    float cy2 = GetFloat(data, ref index);

                    if (data[index] != ' ')
                    {
                        throw new ApplicationException("Expected space");
                    }
                    ++index;

                    float x = GetFloat(data, ref index);
                    if (data[index] != ',')
                    {
                        throw new ApplicationException("Expected comma");
                    }
                    ++index;
                    float y = GetFloat(data, ref index);

                    float lx = PreviousPoint().X;
                    float ly = PreviousPoint().Y;

                    AddBezierPoints(lx, ly, lx + cx1, ly + cy1, lx + cx2, ly + cy2, lx + x, ly + y);
                }
            }

            if (currentContour.Count > 0)
            {
                if (currentContour.Count <= 2)
                {
                    // Happens sometimes. This is either a point or
                    // a line. Empty area, so just toss it.
                }
                else
                {
                    currentContour.Add(currentContour.First());
                    currentContour = Transform(currentContour);
                    contours.Add(currentContour);
                    _path.AddPolygon(currentContour.ToArray());
                }
            }
        }

        private PointF PreviousPoint()
        {
            if (currentContour.Count > 0)
            {
                return currentContour.Last();
            }
            if (contours.Count > 0)
            {
                return contours.Last().Last();
            }
            return new PointF(0, 0);
        }

        private void AddBezierPoints(float x1, float y1, float cx1, float cy1, float cx2, float cy2, float x3, float y3)
        {
            List<PointF> pointList = new List<PointF>();

            // First subdivide the Bezier into 250 line segments. This number is fairly arbitrary
            // and anything we pick is wrong because for small curves you'll have multiple segments
            // smaller than a pixel and for huge curves no number is large enough. We pick something
            // fairly big and then the polygon gets thinned in two separate stages afterwards. Many
            // of these get reduced to just a handful of vertices by the time we emit GCode.
            pointList.Add(new PointF(x1, y1));
            float stepDelta = 1.0f / 250.0f;

            for (float t = stepDelta; t < 1.0f; t += stepDelta) // Parametric value
            {
                float fW = 1 - t;
                float fA = fW * fW * fW;
                float fB = 3 * t * fW * fW;
                float fC = 3 * t * t * fW;
                float fD = t * t * t;

                float fX = fA * x1 + fB * cx1 + fC * cx2 + fD * x3;
                float fY = fA * y1 + fB * cy1 + fC * cy2 + fD * y3;

                pointList.Add(new PointF(fX, fY));
            }
            pointList.Add(new PointF(x3, y3));

            // Next thin the points based on a flatness test.
            bool done = true;
            do
            {
                done = true;
                int pointIndex = 0;
                do
                {
                    PointF p1 = pointList[pointIndex];
                    PointF p2 = pointList[pointIndex + 1];
                    PointF p3 = pointList[pointIndex + 2];
                    PointF pb = new PointF((p1.X + p3.X) / 2, (p1.Y + p3.Y) / 2);

                    double err = Math.Sqrt(Math.Abs(p2.X - pb.X) * Math.Abs(p2.X - pb.X) +
                                            Math.Abs(p2.Y - pb.Y) * Math.Abs(p2.Y - pb.Y));
                    double dist = Math.Sqrt(Math.Abs(p3.X - p1.X) * Math.Abs(p3.X - p1.X) +
                                            Math.Abs(p3.Y - p1.Y) * Math.Abs(p3.Y - p1.Y));
                    double relativeErr = err / dist;

                    // If the subdivided portion is within a pixel at 1000dpi
                    // then it's flat enough to remove the intermediate vertex.
                    if (relativeErr < 0.001)
                    {
                        pointList.RemoveAt(pointIndex + 1);
                        done = false;
                    }

                    ++pointIndex;
                } while (pointIndex < pointList.Count - 2);
            } while (!done);

            foreach (PointF point in pointList)
            {
                currentContour.Add(point);
            }
        }

        public void SkipSpace(string data, ref int index)
        {
            while (data[index] < 33)
            {
                ++index;
            }
        }

        public void SkipSpaceOrComma(string data, ref int index)
        {
            while (data[index] < 33 || data[index] == ',')
            {
                ++index;
            }
        }

        public List<List<PointF>> GetContours()
        {
            return contours;
        }
    }

    /// <summary>
    /// Styles contain colors, stroke widths, etc. We use them to differentiate
    /// vector vs. raster portions of the document.
    /// </summary>
    public class SVGStyle
    {
        public string Name { get; set; }

        public bool OutlineWidthPresent { get; set; }
        public double OutlineWidth { get; set; }

        public bool OutlineColorPresent { get; set; }
        public Color OutlineColor { get; set; }

        public bool FillColorPresent { get; set; }
        public Color FillColor { get; set; }

        static public Color ParseColor(string c)
        {
            Color result;

            if (c.Length == 7 && c[0] == '#')
            {
                string s1 = c.Substring(1, 2);
                string s2 = c.Substring(3, 2);
                string s3 = c.Substring(5, 2);

                byte r = 0;
                byte g = 0;
                byte b = 0;

                try
                {
                    r = Convert.ToByte(s1, 16);
                    g = Convert.ToByte(s2, 16);
                    b = Convert.ToByte(s3, 16);
                }
                catch
                {
                }

                result = Color.FromArgb(r, g, b);
            }
            else
            {
                result = Color.FromName(c);
            }

            return result;

        }

        public SVGStyle(string name, string style)
        {
            Name = name;
            OutlineColorPresent = false;
            OutlineWidthPresent = false;
            FillColorPresent = false;

            style = style.Trim(new char[] { '{', '}' });
            string[] stylePairs = style.Split(new char[] { ':', ';' });

            if ((stylePairs.Count() & 1) != 0)
            {
                throw new ArgumentException("Failed to parse style");
            }

            for (int index = 0; index < stylePairs.Count(); index += 2)
            {
                switch (stylePairs[index])
                {
                    case "stroke":
                        OutlineColor = ParseColor(stylePairs[index + 1]);
                        OutlineColorPresent = true;
                        break;
                    case "stroke-width":
                        OutlineWidth = double.Parse(stylePairs[index + 1]);
                        OutlineWidthPresent = true;
                        break;
                    case "fill":
                        FillColor = ParseColor(stylePairs[index + 1]);
                        FillColorPresent = true;
                        break;
                    default: break;
                }
            }
        }

    }

    /// <summary>
    /// An SVG Document. Read from file and build in-memory representation.
    /// </summary>
    public class SVGDocument
    {
        private List<ISVGElement> shapes = new List<ISVGElement>();

        public static SVGDocument LoadFromFile(string path)
        {
            DateTime start = DateTime.UtcNow;

            // All this nonsense is to prevent a 10 second delay when reading
            // the first SVG file. It gets hung up trying to resolve the DTD?
            MemoryStream ms = new MemoryStream();
            {
                StreamReader sr = new StreamReader(path);
                StreamWriter sw = new StreamWriter(ms);

                while (sr.EndOfStream == false)
                {
                    string line = sr.ReadLine();
                    if (line.StartsWith("<!DOCTYPE") == false)
                    {
                        sw.WriteLine(line);
                    }
                }
                sw.Flush();
                ms.Seek(0, SeekOrigin.Begin);
            }

            // Here begins the reading of the SVG file
            XmlTextReader reader = new XmlTextReader(ms);
            SVGDocument doc = new SVGDocument();
            Dictionary<string, SVGStyle> styleDictionary = new Dictionary<string, SVGStyle>();

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "style")
                    {
                        // Inline style
                        string styleData = reader.ReadElementContentAsString();
                        StringReader styleReader = new StringReader(styleData);
                        string line;

                        while ((line = styleReader.ReadLine()) != null)
                        {
                            string[] splitLine;

                            line = line.Trim();
                            splitLine = line.Split(new char[] { ' ', '\t' });

                            string name = splitLine[0];
                            if (name.StartsWith("."))
                            {
                                name = name.Substring(1);
                            }
                            if (splitLine.Count() == 2)
                            {
                                styleDictionary.Add(name, new SVGStyle(name, splitLine[1]));
                            }

                        };
                    }
                    else if (reader.Name == "rect")
                    {
                        if (reader.GetAttribute("class") != null)
                        {
                            doc.AddShape(new SVGRect(reader, styleDictionary));
                        }
                    }
                    else if (reader.Name == "circle")
                    {
                        doc.AddShape(new SVGCircle(reader, styleDictionary));
                    }
                    else if (reader.Name == "polygon")
                    {
                        doc.AddShape(new SVGPolygon(reader, styleDictionary));
                    }
                    else if (reader.Name == "polyline")
                    {
                        doc.AddShape(new SVGPolygon(reader, styleDictionary));
                    }
                    else if (reader.Name == "path")
                    {
                        doc.AddShape(new SVGPath(reader, styleDictionary));
                    }
                    else if (reader.Name == "image")
                    {
                        doc.AddShape(new SVGImage(reader, styleDictionary, path));
                    }
                }
            }

            TimeSpan duration = DateTime.UtcNow - start;
            System.Console.WriteLine("### Load took {0}s", ((double)duration.TotalMilliseconds / 1000.0));

            return doc;
        }

        public void AddShape(ISVGElement shape)
        {
            shapes.Add(shape);
        }

        public IEnumerable<IEnumerable<PointF>> GetContours()
        {
            // Enumerate each shape in the document
            foreach (ISVGElement shape in shapes)
            {
                foreach (var contour in shape.GetContours())
                {
                    yield return contour;
                }
            }
        }

        public void Render(Graphics gc, bool rasterOnly)
        {
            foreach (ISVGElement shape in shapes)
            {
                if (shape is SVGImage)
                {
                    // Polymorphism? What's that?
                    SVGImage img = shape as SVGImage;

                    gc.DrawImage(img.bits, img.DestBounds);
                }

                if (shape.OutlineWidth < .01 && shape.FillColor.A == 0 && rasterOnly)
                {
                    continue;
                }

                GraphicsPath p = shape.GetPath();
                if (shape.FillColor.A > 0)
                {
                    Brush b = new SolidBrush(shape.FillColor);
                    gc.FillPath(b, p);
                    b.Dispose();
                }
                if (shape.OutlineWidth > 0 && shape.OutlineColor.A > 0)
                {
                    Pen pen = new Pen(shape.OutlineColor, (float)shape.OutlineWidth);
                    gc.DrawPath(pen, p);
                    pen.Dispose();
                }
            }
        }

        string GCodeHeader =
            @"(paperpixels SVG to GCode v0.1)
            N30 G17 (active plane)
            N35 G40 (turn compensation off)
            N40 G20 (inch mode) 
            N45 G90 (Absolute mode, current coordinates)
            N50 G61 (Exact stop mode for raster scanning)";

        string GCodeFooter =
            @"M5 (Laser Off)
            E1P0 (Program End)
            G0 X0 Y0 Z1 (Home and really turn off laser)
            M30 (End)";

        /// <summary>
        /// This emits GCode suitable for driving a laser cutter to vector/raster
        /// the document. There are numerous assumptions made here so that it
        /// works exactly with my cheap Chinese laser cutter, my controller board
        /// and my Mach3 config. You may need to fiddle with things here to
        /// get the axes directions correct, etc.
        /// </summary>
        /// <param name="path">Output path for GCode file</param>
        /// <param name="raster">If true a raster path is emitted</param>
        /// <param name="rasterDpi">DPI resolution for raster</param>
        /// <param name="rasterFeedRate">IPS feed for raster scan</param>
        /// <param name="vector">If true a vector cut is emitted</param>
        /// <param name="vectorDpi">DPI resolution for vector cut</param>
        /// <param name="vectorFeedRate">IPS feed for vector cut</param>
        /// <param name="vectorCV">Use constant velocity mode? Smooths out 
        /// discontinuities that occur at polygon vertices/corners using
        /// lookahead. Mach3 does all this, this simply emits a GCode to
        /// turn this on. This, plus sufficient lookahead configured in
        /// Mach3 made my laser perform much smoother.</param>
        /// <param name="progressDelegate">Callback for progress notification</param>
        public void EmitGCode(string path, bool raster, int rasterDpi, int rasterFeedRate, bool vector, int vectorDpi, int vectorFeedRate, bool vectorCV, Action<double> progressDelegate)
        {
            // Open up file for writing
            StreamWriter gcode = new StreamWriter(path);

            // Emit header
            gcode.WriteLine(GCodeHeader);

            // BUGBUG: Should read size from SVG file header. But we usually are doing 11x11
            //         This always assumes an 11" x 11" document. 
            double docWidth = 11.0;
            double docHeight = 11.0;
            double bandHeight = 0.5;
            double totalProgress = docHeight / bandHeight + 1.0;
            double progress = 0;

            // First pass is raster engraving
            if (raster)
            {
                // Create a bitmap image used for banding
                Bitmap band = new Bitmap(
                    (int)(docWidth * rasterDpi),    // Band Width in px
                    (int)(bandHeight * rasterDpi),  // Band Height in px
                    PixelFormat.Format32bppArgb);
                Graphics gc = Graphics.FromImage(band);

                // Now render each band of the image
                for (double bandTop = 0.0; bandTop <= docHeight - bandHeight; bandTop += bandHeight)
                {
                    // Call progress method once per band
                    if (progressDelegate != null)
                    {
                        progressDelegate(progress / totalProgress);
                    }

                    // Set up the GC transform to render this band
                    gc.ResetTransform();
                    gc.FillRectangle(Brushes.White, 0, 0, 99999, 99999);
                    gc.ScaleTransform(-rasterDpi, -rasterDpi);
                    gc.TranslateTransform((float)-docWidth, (float)-bandTop);

                    // Erase whatever was there before
                    gc.FillRectangle(Brushes.White, -50, -50, 50, 50);

                    // Render just the raster shapes into the band
                    this.Render(gc, true);

                    // Now scan the band and emit gcode. We access the bitmap data 
                    // directly for higher performance. The use of unsafe pointer
                    // access here sped up perf significantly over GetPixel() which
                    // is obvious but they don't teach PhD's this so I mention it here.
                    unsafe
                    {
                        BitmapData lockedBits = band.LockBits(
                            Rectangle.FromLTRB(0, 0, band.Width, band.Height),
                            ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                        bool laserOn = false;
                        int onStart = 0;
                        for (int y = 0; y < band.Height; ++y)
                        {
                            if (laserOn)
                            {
                                throw new ApplicationException("Expected laser off");
                            }

                            // Get the bits for this scanline using something I call a "pointer"
                            byte* pScanline = (byte*)((int)lockedBits.Scan0 + ((band.Height - 1) - y) * lockedBits.Stride);
                            for (int x = 0; x < band.Width; ++x)
                            {
                                int b = *pScanline++;
                                int g = *pScanline++;
                                int r = *pScanline++;
                                int a = *pScanline++;
                                int luma = r + g + b;

                                if (luma < 400)
                                {
                                    if (!laserOn)
                                    {
                                        // Found an "on" edge
                                        onStart = x;
                                        laserOn = true;
                                    }
                                }
                                else
                                {
                                    if (laserOn)
                                    {
                                        // Found an "off" edge
                                        double fx = (double)onStart / (double)rasterDpi;
                                        double fy = ((double)y / (double)rasterDpi) + bandTop;
                                        fy = docHeight - fy + bandHeight;
                                        fx = docWidth - fx;
                                        gcode.WriteLine(string.Format("G1 X{0:0.0000} Y{1:0.0000} F{2}", fx, fy, rasterFeedRate));
                                        fx = (double)x / (double)rasterDpi;
                                        fx = docWidth - fx;
                                        gcode.WriteLine("G1 Z0 (Laser On)");
                                        gcode.WriteLine(string.Format("X{0:0.0000}", fx));
                                        gcode.WriteLine("Z0.002 (Laser Off)");

                                        laserOn = false;
                                    }
                                }
                            }

                            // If we get here and laser is still on then we
                            // turn it off at the edge here.
                            if (laserOn && false)
                            {
                                double fx = (double)onStart / (double)rasterDpi;
                                double fy = ((double)y / (double)rasterDpi) + bandTop;
                                gcode.WriteLine(string.Format("G1 X{0:0.0000} Y{1:0.0000}", fx, fy));
                                fx = (double)band.Width / (double)rasterDpi;
                                fx = docWidth - fx;
                                gcode.WriteLine("G1 Z0 (Laser On)");
                                gcode.WriteLine(string.Format("X{0:0.0000} F{1:0.0000}", fx, rasterFeedRate));
                                gcode.WriteLine("Z0.002 (Laser Off)");

                                laserOn = false;
                            }
                        }

                        // Unlock band bits
                        band.UnlockBits(lockedBits);

                        // Increment progress
                        progress += 1.0;
                    }
                }
            }

            // Pause inbetween for the operator to adjust power
            // You need to create a M995 custom macro in Mach3 to
            // stick up a dialog that says "Adjust Power for Vector"
            gcode.WriteLine("(=================================================================================)");
            gcode.WriteLine("(Pause for operator power adjustment)");
            gcode.WriteLine("(Depends on macro M995 set up to prompt operator)");
            gcode.WriteLine("(=================================================================================)");
            gcode.WriteLine("M995");

            // Second pass is vector cuts
            double contourIncrement = 1.0 / GetVectorContours(100).Count();
            if (vectorCV)
            {
                gcode.WriteLine("G64 (Constant velocity mode for vector cuts)");
            }
            foreach (var contour in GetVectorContours(vectorDpi))
            {
                int contourFeed;

                if (vectorFeedRate > 0)
                {
                    contourFeed = vectorFeedRate;
                }
                else
                {
                    // This is trying to be overly cute and is probably 
                    // not useful. It maps colors to different speeds.
                    contourFeed = (int)((1.0 - contour.Brightness) * 1000);
                    if (contour.Color.Name == "Blue")
                    {
                        contourFeed = 50;
                    }
                    else if (contour.Color.Name == "Aqua")
                    {
                        contourFeed = 40;
                    }
                    else if (contour.Color.Name == "Lime")
                    {
                        contourFeed = 30;
                    }
                    else if (contour.Color.Name == "Yellow")
                    {
                        contourFeed = 20;
                    }
                    else if (contour.Color.Name == "Red")
                    {
                        contourFeed = 10;
                    }
                }

                bool first = true;
                foreach (var point in contour.Points)
                {
                    // Transform point to laser coordinate system
                    double laserX = point.X;
                    double laserY = 11.0 - point.Y;

                    if (first)
                    {
                        // Rapid to the start of the contour
                        gcode.WriteLine(string.Format("G0 X{0:0.0000} Y{1:0.0000}", laserX, laserY));
                        gcode.WriteLine(string.Format("G1 Z-0.002 F{0} (Turn on laser. Set feed for this contour)", contourFeed));
                        first = false;
                    }
                    else
                    {
                        // Next point in contour
                        gcode.WriteLine(string.Format("X{0:0.0000} Y{1:0.0000}", laserX, laserY));
                    }
                }
                gcode.WriteLine("Z0 (Turn off laser)");

                progress += contourIncrement;
                if (progressDelegate != null)
                {
                    progressDelegate(progress / totalProgress);
                }
            }

            // Shut down
            gcode.WriteLine(GCodeFooter);
            gcode.Flush();
            gcode.Close();
        }

        /// <summary>
        /// Get the points for all contours in all shapes.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PointF> GetPoints()
        {
            // Enumerate each shape in the document
            foreach (ISVGElement shape in shapes)
            {
                foreach (var contour in shape.GetContours())
                {
                    foreach (PointF point in contour)
                    {
                        yield return point;
                    }
                }
            }
        }

        public int GetShapeCenter(ISVGElement shape)
        {
            double centerX = 99999.0;
            double centerY = 99999.0;

            foreach (var contour in shape.GetContours())
            {
                foreach (PointF point in contour)
                {
                    if (point.X < centerX)
                    {
                        centerX = point.X;
                    }
                    if (point.Y < centerY)
                    {
                        centerY = point.Y;
                    }
                }
            }

            centerX = centerX * 10;
            centerY = centerY * 10;
            int x = (int)centerX / 1;
            int y = (int)centerY / 1;

            return y * 10000 + x;
        }


        public double Distance(PointF a, PointF b)
        {
            double xd = Math.Abs(a.X - b.X);
            double yd = Math.Abs(a.Y - b.Y);

            xd = xd * xd;
            yd = yd * yd;

            return Math.Sqrt(xd + yd);
        }

        /// <summary>
        /// Given a specific DPI resolution, returns vectors to approximate
        /// the shapes in the document at that resolution. This allows us to
        /// thin out the tons of polygon edges for curved segments.
        /// </summary>
        /// <param name="dpi"></param>
        /// <returns></returns>
        public IEnumerable<VectorContour> GetVectorContours(double dpi)
        {
            double threshhold = 1.0 / dpi;
            int total = 0;
            int thin = 0;


            foreach (ISVGElement shape in shapes)
            {
                if ((shape.OutlineWidth >= .01 && shape.OutlineColor.A == 255) || shape.FillColor.A == 255)
                {
                    continue;
                }

                foreach (var contour in shape.GetContours())
                {
                    List<PointF> thinnedContour = new List<PointF>();
                    PointF lastPoint = contour.First();
                    bool first = true;

                    foreach (PointF point in contour)
                    {
                        ++total;

                        if (first)
                        {
                            thinnedContour.Add(new PointF(point.X, point.Y));
                            lastPoint = point;
                            first = false;
                        }
                        else
                        {
                            if (Distance(point, lastPoint) > threshhold)
                            {
                                ++thin;
                                thinnedContour.Add(point);
                                lastPoint = point;
                            }
                        }
                    }

                    yield return new VectorContour()
                    {
                        Brightness = shape.OutlineColor.GetBrightness(),
                        Color = shape.OutlineColor,
                        Points = thinnedContour
                    };
                }
            }

            System.Console.WriteLine("Thinned contour ({0}/{1}) = {2}%", thin, total, (int)((double)thin / (double)total * 100.0));
        }
    }
}

