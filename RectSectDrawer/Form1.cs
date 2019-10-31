using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Codetesting
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();


            dgv1.AllowUserToAddRows = false;
            dgv1.RowTemplate.Height = 16;


            comboBox1.Items.Add("Rektangulært");
            comboBox1.Items.Add("Polygon");
            comboBox1.Items.Add("Diverse");


        }


        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            dgv1.Rows.Clear();
            dgv1.Refresh();
            dgv1.ColumnCount = 1;

            tvType = comboBox1.Text;
            switch (tvType)
            {
                case "Rektangulært":
                    dgv1.RowCount = 2;

                    dgv1.Rows[0].HeaderCell.Value = "A1";
                    dgv1.Rows[1].HeaderCell.Value = "A2";
                    break;
                case "Polygon":
                    dgv1.RowCount = 6;
                    for (int i = 0; i < dgv1.RowCount; i++)
                    {
                        dgv1.Rows[i].HeaderCell.Value = "A" + (i + 1).ToString();

                    }

                    break;

                case "Diverse":
                    dgv1.RowCount = 3;

                    dgv1.Rows[0].HeaderCell.Value = "A1";
                    dgv1.Rows[1].HeaderCell.Value = "A2";
                    dgv1.Rows[2].HeaderCell.Value = "A3";
                    break;
            }
        }



        public string tvType;

        
        //public string TvType
        //{
        //    get { return comboBox1.Text; }
        //    // set { tvType = value; }
        //}





        private void dgv1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
            for (int i = 0; i < dgv1.RowCount; i++)
            {

                if (e.RowIndex == i)
                {

                    this.Picture1.Paint += Picture1_Paint;
                    Picture1.Invalidate();

                }


            }

        }


        private void Picture1_Paint(object sender, PaintEventArgs e)
        {

            Draw(tvType ,Picture1, e);

        }

        private void Text1_TextChanged(object sender, EventArgs e)
        {
            //this.Picture1.Paint += Picture1_Paint;
            Picture1.Refresh();
            //Picture1.Invalidate();
        }

        public void Draw(string sectionType, Panel panel, PaintEventArgs e)
        {
           

            var containerState = e.Graphics.BeginContainer();
            Graphics graphics = e.Graphics;
            
            graphics.TranslateTransform(panel.Width / 2, panel.Height / 2);
            Pen penDraw = new Pen(Color.Black, 1f);
            Pen penDim = new Pen(Color.Red, 1f);
            AdjustableArrowCap customArrowCap = new AdjustableArrowCap(5, 5);
            List<string> tvSnittDimList = new List<string>();

            float scale = 0.5f;
            

            float maxWidth = panel.Width - 200;
            float maxHeight = panel.Height - 200;
            float scaleWidth = 1;
            float scaleHeight = 1;


            
            

            switch (sectionType)
            {
                case "Rektangulært":
                    int recWidth = Convert.ToInt32(dgv1[0, 0].Value);
                    int recHeight = Convert.ToInt32(dgv1[0, 1].Value);

                    if (recWidth > maxWidth)
                    {
                        scaleWidth = maxWidth/recWidth;
                    }

                    if (recHeight > maxHeight)
                    {
                        scaleHeight = maxHeight / recHeight;
                    }


                    scale = Math.Min(scaleWidth, scaleHeight);
                    graphics.ScaleTransform(scale, scale);


                    string strRecWidth = recWidth.ToString();
                    string strRecHeight = recHeight.ToString();


                    StringFormat strFormat = new StringFormat();
                    strFormat.LineAlignment = StringAlignment.Center;
                    strFormat.Alignment = StringAlignment.Center;



                    //Make rectangel in center of panel

                    graphics.DrawRectangle(penDraw, new Rectangle(-recWidth/2, -recHeight/2, recWidth, recHeight));

                    //Make dimension lines
                    penDim.CustomStartCap = customArrowCap;
                    penDim.CustomEndCap = customArrowCap;
                    Font dimFont = new Font("Arial", 12 / scale, FontStyle.Regular);

                    graphics.DrawLine(penDim, -recWidth / 2, recHeight / 2 + 20 / scale, recWidth / 2, recHeight / 2 + 20 / scale);
                    graphics.DrawLine(penDim, recWidth / 2 + 20 / scale, recHeight / 2, recWidth / 2 + 20 / scale, -recHeight / 2);
                    graphics.DrawString(strRecWidth, dimFont, Brushes.Red, 0, recHeight / 2 + 40 / scale, strFormat);
                    graphics.DrawString(strRecHeight, dimFont, Brushes.Red, recWidth / 2 + 50 / scale, 0, strFormat);
                    

                    break;

                case "Polygon":

                    List<PointF> pointList = new List<PointF>();

                    PointF[] pointF = new PointF[] { };
                    //PointF pointF;
                    int j = 100;
                    int k = 50;


                    for (int i = 0; i < dgv1.RowCount; i++)
                    {
                        pointList.Add(new PointF(j, k));

                        //pointF[i].X = j; //new Point { X = 0, Y = 0 };
                        //pointF[i].Y = k;

                        j += 50;
                        k += Convert.ToInt32(Math.Pow(3.5, i));

                    }

                    pointF = pointList.ToArray();

                    graphics.DrawPolygon(penDraw, pointF);
                    //graphics.TranslateTransform(200, 0);
                    graphics.DrawClosedCurve(penDraw, pointF);

                    //graphics.TranslateTransform(-200, 0);

                    break;

                case "Diverse":
                    ////FLip the Y-axis
                    //graphics.ScaleTransform(1.0f, -1.0f);

                    ////Transefer the drawing graphics accordingly
                    //float height = panel.Height;
                    //graphics.TranslateTransform(0.0f, -height);

                    //Point[] points =
                    //{
                    //     new Point(10,  10),
                    //     new Point(10, 100),
                    //     new Point(200,  50),
                    //     new Point(250, 300)

                    //};

                    //AdjustableArrowCap customArrowCap = new AdjustableArrowCap(6, 6);

                    //pen.CustomEndCap = customArrowCap;
                    //Point p1 = new Point(50, 50);
                    //Point p2 = new Point(400, 200);


                    //graphics.DrawLines(pen, points);
                    //graphics.DrawLine(pen, p1, p2);
                    //graphics.DrawString("1000", new Font("Verdana", 12), Brushes.Black, 100, 50);

                    //graphics.EndContainer(containerstate);
                    


                    //String theString = "45 Degree Rotated Text";
                    //SizeF sz = graphics.VisibleClipBounds.Size;
                    ////Offset the coordinate system so that point (0, 0) is at the center of the desired area.
                    //graphics.TranslateTransform(sz.Width / 2, sz.Height / 2);
                    ////Rotate the Graphics object.
                    //graphics.RotateTransform(45);
                    //sz = graphics.MeasureString(theString, this.Font);
                    ////Offset the Drawstring method so that the center of the string matches the center.
                    //graphics.DrawString(theString, this.Font, Brushes.Black, -(sz.Width / 2), -(sz.Height / 2));
                    //graphics.DrawString("1000", new Font("Verdana", 12), Brushes.Black, 100, 50);
                    ////Reset the graphics object Transformations.
                    //graphics.ResetTransform();


                    break;
            }
            graphics.EndContainer(containerState);
            penDraw.Dispose();
            penDim.Dispose();



        }

        public void drawRotatedText(Panel panel, int x, int y, float angle, string text, Font font, Brush brush, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TranslateTransform(x, y); // Set rotation point
            g.RotateTransform(angle); // Rotate text
            g.TranslateTransform(-x, -y); // Reset translate transform
            SizeF size = g.MeasureString(text, font); // Get size of rotated text (bounding box)
            g.DrawString(text, font, brush, new PointF(x - size.Width / 2.0f, y - size.Height / 2.0f)); // Draw string centered in x, y
            g.ResetTransform(); // Only needed if you reuse the Graphics object for multiple calls to DrawString
            g.Dispose();
        }

 
    }
}
