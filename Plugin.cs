using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using QuickLook.Common.Plugin;

namespace QuickLook.Plugin.GraphvizDotViewer
{
    public class Plugin : IViewer
    {
        public int Priority => 0;

        public void Init()
        {
        }

        public bool CanHandle(string path)
        {
            return !Directory.Exists(path) && path.ToLower().EndsWith(".dot");
        }

        public void Prepare(string path, ContextObject context)
        {
            context.PreferredSize = new Size { Width = 600, Height = 400 };
        }

        public void View(string path, ContextObject context)
        {
            byte[] imageBytes = null;
            string errorMessage = null;
            try
            {
                imageBytes = GraphvizWrapper.RenderImage(path, "dot", "png");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            BitmapImage image = null;
            if (imageBytes != null)
            {
                image = new BitmapImage();
                image.BeginInit();
                using (var ms = new MemoryStream(imageBytes))
                {
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = ms;
                    image.EndInit();
                }
            }

            context.Title = Path.GetFileName(path);
            context.ViewerContent = new ImagePanel
            {
                ImageControl = { Source = image },
                ErrorLabel = { Content = errorMessage }
            };
            context.IsBusy = false;
        }

        public void Cleanup()
        {
        }
    }
}