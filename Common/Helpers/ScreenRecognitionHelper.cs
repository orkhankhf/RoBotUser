using Common.Resources;
using NLog;
using OpenCvSharp;
using System.Drawing;
using NLogLogger = NLog.ILogger;

namespace Common.Helpers
{
    public static class ScreenRecognitionHelper
    {
        private static readonly NLogLogger Logger = LogManager.GetCurrentClassLogger();
        public static bool IsWhatsappNumberAvailable()
        {
            // Step 1: Get the solution/project directory by navigating up from the bin folder
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Directory.GetParent(baseDirectory)?.FullName;

            // Assuming your image is in a folder called "Images" inside your project
            string imagePath = Path.Combine(projectDirectory, "Images", "noContactsFound.png");

            if (!File.Exists(imagePath))
            {
                Logger.Error(string.Format(LogMessagesRes.ImageFileNotFound, imagePath));
                throw new Exception();
            }

            // Step 2: Capture the current screen
            Mat screenMat = CaptureScreen();

            // Step 3: Load the target image you want to find
            Mat targetImage = Cv2.ImRead(imagePath, ImreadModes.Color);

            if (targetImage.Empty())
            {
                Logger.Error(string.Format(LogMessagesRes.FailedToLoadImage, imagePath));
                throw new Exception();
            }

            // Step 4: Convert both images to grayscale to ensure matching
            Mat screenGray = new Mat();
            Mat targetGray = new Mat();
            Cv2.CvtColor(screenMat, screenGray, ColorConversionCodes.BGR2GRAY); // Convert screen to grayscale
            Cv2.CvtColor(targetImage, targetGray, ColorConversionCodes.BGR2GRAY); // Convert target image to grayscale

            // Step 5: Perform template matching
            Mat result = new Mat();
            Cv2.MatchTemplate(screenGray, targetGray, result, TemplateMatchModes.CCoeffNormed);

            // Step 6: Normalize and find the location of the best match
            Cv2.MinMaxLoc(result, out _, out double maxVal, out _, out OpenCvSharp.Point maxLoc);

            // Cleanup
            screenMat.Dispose();
            targetImage.Dispose();
            result.Dispose();
            screenGray.Dispose();
            targetGray.Dispose();

            if (maxVal >= 0.7) // You can adjust the threshold
                return false;
            else
                return true;
        }

        private static Mat CaptureScreen()
        {
            // Capture the screen using OpenCV's Mat
            int screenWidth = 1920; // Adjust this to your screen resolution
            int screenHeight = 1080; // Adjust this to your screen resolution

            Bitmap screenBitmap = new Bitmap(screenWidth, screenHeight);
            using (Graphics g = Graphics.FromImage(screenBitmap))
            {
                g.CopyFromScreen(0, 0, 0, 0, screenBitmap.Size);
            }

            // Convert the Bitmap to Mat
            Mat screenMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(screenBitmap);
            screenBitmap.Dispose();
            return screenMat;
        }
    }
}
