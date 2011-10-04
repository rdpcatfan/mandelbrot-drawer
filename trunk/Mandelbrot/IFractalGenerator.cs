using System.Drawing;

namespace Mandelbrot
{
    interface IFractalGenerator
    {
        /// <summary>
        /// Generate an image of a fractal.
        /// </summary>
        /// <param name="info">Information for the generation of the image.</param>
        /// <returns>The image generated</returns>
        Image generate(ImageInfo info);
    }
}
