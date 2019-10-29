namespace Ensco.Irma.Oap.Common.Models
{
    public class GridButton
    {
        public GridButton(GridButtonTypes type, bool display = true, string imageBaseUrl = "~/Images", int width = 16, int height = 16)
        {
            Type = type;
            Display = display;
            ImageBaseUrl = imageBaseUrl;
            Width = width;
            Height = height;
        }

        public GridButtonTypes Type { get; }

        public bool Display { get; }

        public string ImageBaseUrl { get; }

        public int Width { get; }

        public int Height { get; }
    }
}