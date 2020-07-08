using KobberLan.Code;
using MonoTorrent.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Type = System.Type;

namespace KobberLan.Gui
{
    //-------------------------------------------------------------
    [TypeDescriptionProvider(typeof(AbstractUserControl))]
    public abstract class SuggestedGame : UserControl
    //-------------------------------------------------------------
    {
        protected DTO_Suggestion dto_suggestion;
        protected List<IPAddress> Likes;

        //-------------------------------------------------------------
        public DTO_Suggestion GetSuggestion()
        //-------------------------------------------------------------
        {
            return dto_suggestion;
        }

        //-------------------------------------------------------------
        public abstract string GetTitle();
        //-------------------------------------------------------------

        //-------------------------------------------------------------
        public string GetKey()
        //-------------------------------------------------------------
        {
            if(dto_suggestion == null || string.IsNullOrEmpty(dto_suggestion.key))
            {
                Log.Get().Write("No key assigned to SuggestedGame", Log.LogType.Warning);
                return "";
            }

            return dto_suggestion.key;
        }

        //-------------------------------------------------------------
        public abstract void UpdateLike(DTO_Like like);
        //-------------------------------------------------------------

        //-------------------------------------------------------------
        public abstract void UpdateTorrent(DTO_Torrent torrent);
        //-------------------------------------------------------------

        //-------------------------------------------------------------
        public abstract void UpdateProgressBar(TorrentState type, int progress);

        //-------------------------------------------------------------
        public abstract void UpdateStats(DTO_TorrentStatus torrentStatus);
        //-------------------------------------------------------------

        //-------------------------------------------------------------
        public void ExecuteFile(string filename,string directory,string arguments = "")
        //-------------------------------------------------------------
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = filename;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.WorkingDirectory = directory;
                p.Start();
            }
            catch(Exception ex)
            {
                Log.Get().Write("Failed to start game: " + directory + "\\" + filename + " exception: " + ex, Log.LogType.Error);
                MessageBox.Show(this, "Failed to start game: " + filename + " exception: " + ex,"Error");
            }
        }

        //-------------------------------------------------------------
        public Font FontAdjusted(Graphics graphics, Size size, Font font, string str)
        //-------------------------------------------------------------
        {
            SizeF stringSize = graphics.MeasureString(str, font);
            float wRatio = size.Width / stringSize.Width;
            float hRatio = size.Height / stringSize.Height;
            float ratio = Math.Min(hRatio, wRatio);
            float result = font.Size * ratio;
            if (result > 22) result = 22;
            if (result < 7) result = 7;
            Font fontObject = new Font("Arial", result, FontStyle.Bold);
            return fontObject;
        }

        //-------------------------------------------------------------
        public bool NearlyEqual(double a, double b, double epsilon)
        //-------------------------------------------------------------
        {
            const double MinNormal = 2.2250738585072014E-308d;
            double absA = Math.Abs(a);
            double absB = Math.Abs(b);
            double diff = Math.Abs(a - b);

            if (a.Equals(b))
            { // shortcut, handles infinities
                return true;
            }
            else if (a == 0 || b == 0 || absA + absB < MinNormal)
            {
                // a or b is zero or both are extremely close to it
                // relative error is less meaningful here
                return diff < (epsilon * MinNormal);
            }
            else
            { // use relative error
                return diff / (absA + absB) < epsilon;
            }
        }

    }

    //-------------------------------------------------------------
    class CustomToolTip : ToolTip
    //-------------------------------------------------------------
    {
        private TextureBrush textureBrush;
        private DTO_Suggestion dto_suggestion;
        private SuggestedGame suggestedGameForm;
        private LinearGradientBrush brushMaxPlayers = new LinearGradientBrush(
               new Point(20,  0), //Point where color blends from      //Width 40, height: 20
               new Point(20, 40),//Point where color blends from
               Color.FromArgb(222, 247, 5, 5),  // #F70505
               Color.FromArgb(222, 143, 8, 8)); // #8F0808

        private LinearGradientBrush brushDescription = new LinearGradientBrush(
               new Point(20, 0), //Point where color blends from      //Width 40, height: 20
               new Point(20, 40),//Point where color blends from
               Color.FromArgb(222, 50, 50, 50), 
               Color.FromArgb(222,  0,  0,  0));

        //-------------------------------------------------------------
        public CustomToolTip(DTO_Suggestion suggestion, SuggestedGame form)
        //-------------------------------------------------------------
        {
            dto_suggestion = suggestion;
            suggestedGameForm = form;

            this.OwnerDraw = true;
            this.Popup += new PopupEventHandler(this.OnPopup);
            this.Draw += new DrawToolTipEventHandler(this.OnDraw);
        }

        //-------------------------------------------------------------
        public void InitImage(Image background)
        //-------------------------------------------------------------
        {
            if (background == null)
                return;

            textureBrush = new TextureBrush(ResizeImage(background, 512, 512));
        }

        //-------------------------------------------------------------
        private void OnPopup(object sender, PopupEventArgs e) // use this event to set the size of the tool tip
        //-------------------------------------------------------------
        {
            e.ToolTipSize = new Size(512, 512);
        }

        //-------------------------------------------------------------
        private void OnDraw(object sender, DrawToolTipEventArgs eventArgs)
        //-------------------------------------------------------------
        {
            Graphics graphics = eventArgs.Graphics;

            //-------------------------------------------------------------
            //Draw background image
            //-------------------------------------------------------------
            if (textureBrush != null)graphics.FillRectangle(textureBrush, eventArgs.Bounds);

            //-------------------------------------------------------------
            //Draw title
            //-------------------------------------------------------------
            Font drawFont = new Font("Arial", 28,FontStyle.Underline | FontStyle.Bold);
            SolidBrush drawBrush = new SolidBrush(Color.White);
            SizeF size = graphics.MeasureString(dto_suggestion.title.ToString(), drawFont);
            float width = ((float)suggestedGameForm.ClientRectangle.Width);
            graphics.DrawString(dto_suggestion.title, drawFont, drawBrush, (512 - size.Width) / 2, 0);

            //-------------------------------------------------------------
            //Draw version 
            //-------------------------------------------------------------
            if(!String.IsNullOrEmpty(dto_suggestion.version))
            {
                drawFont = new Font("Arial", 16, FontStyle.Italic);
                size = graphics.MeasureString(dto_suggestion.title.ToString(), drawFont);
                graphics.DrawString(dto_suggestion.version, drawFont, drawBrush, (512 - size.Width) / 2, 42);
            }

            //-------------------------------------------------------------
            //Draw Max players 
            //-------------------------------------------------------------
            if (dto_suggestion.maxPlayers > 0)
            {
                //Calcuate text size
                drawFont = new Font("Arial", 16, FontStyle.Italic);
                size = graphics.MeasureString(dto_suggestion.maxPlayers.ToString(), drawFont);

                //Draw banner icon background
                graphics.FillRectangle(brushMaxPlayers, (512 - size.Width - 6 - 3), 512 - 25, size.Width + 6, 24);

                //Draw number in right corner
                graphics.DrawString(dto_suggestion.maxPlayers.ToString(), drawFont, drawBrush, (512 - size.Width) - 6, 512 - 25);
            }

            //-------------------------------------------------------------
            //Draw comment
            //-------------------------------------------------------------
            if (!String.IsNullOrEmpty(dto_suggestion.description))
            {
                //Calcuate text size
                drawFont = new Font("Arial", 16, FontStyle.Regular);
                size = graphics.MeasureString(dto_suggestion.description, drawFont);

                //Draw banner background
                graphics.FillRectangle(brushDescription, 2, 512 - 25, size.Width + 6, 24);

                //Draw description
                graphics.DrawString(dto_suggestion.description, drawFont, drawBrush, 4, 512 - 25);
            }

        }

        //-------------------------------------------------------------
        private Bitmap ResizeImage(Image image, int width, int height)
        //-------------------------------------------------------------
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

    }

    //-------------------------------------------------------------
    public class AbstractUserControl : TypeDescriptionProvider
    //-------------------------------------------------------------
    {
        //-------------------------------------------------------------
        public AbstractUserControl() : base(TypeDescriptor.GetProvider(typeof(UserControl)))
        //-------------------------------------------------------------
        {
        }

        //-------------------------------------------------------------
        public override Type GetReflectionType(Type objectType, object instance)
        //-------------------------------------------------------------
        {
            return typeof(UserControl);
        }

        //-------------------------------------------------------------
        public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
        //-------------------------------------------------------------
        {
            objectType = typeof(UserControl);
            return base.CreateInstance(provider, objectType, argTypes, args);
        }
    }

}
