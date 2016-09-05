using System;
using System.Windows.Forms;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.DesktopEdition;


namespace  MapScales
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            winformsMap1.MapUnit = GeographyUnit.Meter;
            winformsMap1.CurrentExtent = new RectangleShape(-14103004,6452457,-7162636,2497428); 
            winformsMap1.BackgroundOverlay.BackgroundBrush = new GeoSolidBrush(GeoColor.FromArgb(255, 198, 255, 255));

            //Displays the World Map Kit as a background.
            ThinkGeo.MapSuite.DesktopEdition.WorldMapKitWmsDesktopOverlay worldMapKitDesktopOverlay = new ThinkGeo.MapSuite.DesktopEdition.WorldMapKitWmsDesktopOverlay();
            worldMapKitDesktopOverlay.Projection = WorldMapKitProjection.SphericalMercator;
            winformsMap1.Overlays.Add(worldMapKitDesktopOverlay);

            //Sets the ZoomLevelSnapping to none to allow having the map at the exact scale desired.
            winformsMap1.ZoomLevelSnapping = ZoomLevelSnappingMode.None;
            //Does not allow zooming in at a scale larger than 1:1000
            winformsMap1.MinimumScale = 1000;
           
            winformsMap1.Refresh();
        }

        //Updates the scroll bar and textbox after the map changed scale.
        private void winformsMap1_CurrentScaleChanged(object sender, CurrentScaleChangedWinformsMapEventArgs e)
        {
            txtBoxScale.Text = String.Format("{0:0,0}", e.CurrentScale);
            if (e.CurrentScale < vScrollBarScale.Maximum)
            { vScrollBarScale.Value = (int)e.CurrentScale; }
            else
            { vScrollBarScale.Value = vScrollBarScale.Maximum; }

        }

        //Sets the map to the scale according to the scroll bar.
        private void vScrollBarScale_MouseCaptureChanged(object sender, EventArgs e)
        {
            winformsMap1.CurrentScale = vScrollBarScale.Value;
            winformsMap1.Refresh();
        }

        //Updates the textbox of the scale value when scrolling the scroll bar.
        private void vScrollBarScale_Scroll(object sender, ScrollEventArgs e)
        {
            txtBoxScale.Text = String.Format("{0:0,0}", e.NewValue);
        }

        //Allows to set the map to the exact desired scale in the text box
        private void txtBoxScale_KeyPress(object sender, KeyPressEventArgs e)
        {
            char keyPress = e.KeyChar;
            //Updates the scale of the map when pressing enter on the text box.
            if (keyPress == 13)
            {
                txtBoxScale.Text = String.Format("{0:0,0}", txtBoxScale.Text);

                int parsedScale = ParseStringToInt(txtBoxScale.Text);

                if (parsedScale < vScrollBarScale.Maximum)
                { vScrollBarScale.Value = parsedScale; }
                else
                { vScrollBarScale.Value = vScrollBarScale.Maximum; }

                winformsMap1.CurrentScale = parsedScale;
                winformsMap1.Refresh();
            }
        }

        //Parses the string entered in the text box to get only the numeric value for the scale.
        private int ParseStringToInt(string String)
        {
            string totalString = "";
            int i;
            int resultInt =  0;
           
            for (i = 0; i <= String.Length - 1; i += 1)
            {
                string subString = String.Substring(i, 1);
                switch (subString)
                {
                    case "0": case "1": case "2": case "3": case "4": case "5": case "6": case "7": case "8":case "9":
                    totalString = totalString + subString; break;
                }
            }
            resultInt = System.Convert.ToInt32(totalString);
            return resultInt;
        }

        private void winformsMap1_MouseMove(object sender, MouseEventArgs e)
        {
            //Displays the X and Y in screen coordinates.
            statusStrip1.Items["toolStripStatusLabelScreen"].Text = "X:" + e.X + " Y:" + e.Y;

            //Gets the PointShape in world coordinates from screen coordinates.
            PointShape pointShape = ExtentHelper.ToWorldCoordinate(winformsMap1.CurrentExtent, new ScreenPointF(e.X, e.Y), winformsMap1.Width, winformsMap1.Height);

            //Displays world coordinates.
            statusStrip1.Items["toolStripStatusLabelWorld"].Text = "(world) X:" + Math.Round(pointShape.X, 4) + " Y:" + Math.Round(pointShape.Y, 4);
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
