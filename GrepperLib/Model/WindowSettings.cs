
// Thanks http://www.codeproject.com/Articles/50761/Save-and-Restore-WPF-Window-Size-Position-and-or-S

namespace GrepperLib.Model
{
    public class WindowSettings
    {
        public double Width;
        public double Height;
        public double Top;
        public double Left;

        public void SizeToFit(double screenWidth, double screenHeight)
        {
            if (Height > screenHeight)
            {
                Height = screenHeight;
            }

            if (Width > screenWidth)
            {
                Width = screenWidth;
            }
        }

        public void MoveIntoView(double screenWidth, double screenHeight)
        {
            if (Top + Height / 2 > screenHeight)
            {
                Top = screenHeight - Height;
            }

            if (Left + Width / 2 > screenWidth)
            {
                Left = screenWidth - Width;
            }

            if (Top < 0)
            {
                Top = 0;
            }

            if (Left < 0)
            {
                Left = 0;
            }
        }
    }
}
