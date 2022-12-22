namespace Imagination.Server.App.Types;

public class ImageSize
{
    public int Width { get; private set; }
    public int Height { get; private set; }

    public ImageSize(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public bool IsPortrait { get { return Width > Height; } }

    public ImageSize FitIntoSquare(int dimension)
    {
        float ratio = IsPortrait ? Width / dimension : Height / dimension;


        return IsPortrait ? new ImageSize(dimension, (int)Math.Floor(Height / ratio)) : new ImageSize((int)Math.Floor(Width / ratio), dimension);
    }
}
