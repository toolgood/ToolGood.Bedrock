using OpenCvSharp;
using System.Numerics;

namespace ToolGood.BlindWatermark
{
    /// <summary>
    /// C# 图片盲水印
    /// 使用时要添加系统环境dll文件
    /// 感谢 Ran-snow
    /// https://github.com/Ran-snow/BlindWaterMark.NET
    /// </summary>
    public static class ImageUtil
    {
        public static byte[] AddTextWatermark(byte[] srcImage, string watermark)
        {
            var file = Path.GetTempFileName();
            File.WriteAllBytes(file, srcImage);

            Mat outImg = addImageWatermarkWithText(file, watermark);
            List<ImageEncodingParam> imageEncodingParams = new List<ImageEncodingParam>();
            imageEncodingParams.Add(new ImageEncodingParam(ImwriteFlags.JpegQuality, 95));

            Cv2.ImWrite(file, outImg, imageEncodingParams.ToArray());
            var bs = File.ReadAllBytes(file);
            if (File.Exists(file)) { File.Delete(file); }
            return bs;
        }

        public static byte[] AddTextWatermark(string srcImagePath, string watermark)
        {
            Mat outImg = addImageWatermarkWithText(srcImagePath, watermark);
            List<ImageEncodingParam> imageEncodingParams = new List<ImageEncodingParam>();
            imageEncodingParams.Add(new ImageEncodingParam(ImwriteFlags.JpegQuality, 95));

            var file = Path.GetTempFileName();
            Cv2.ImWrite(file, outImg, imageEncodingParams.ToArray());
            var bs = File.ReadAllBytes(file);
            if (File.Exists(file)) { File.Delete(file); }
            return bs;
        }

        public static void AddTextWatermark(string srcImagePath, string watermark, string outPath)
        {
            Mat outImg = addImageWatermarkWithText(srcImagePath, watermark);
            List<ImageEncodingParam> imageEncodingParams = new List<ImageEncodingParam>();
            imageEncodingParams.Add(new ImageEncodingParam(ImwriteFlags.JpegQuality, 95));
            Cv2.ImWrite(outPath, outImg, imageEncodingParams.ToArray());
        }

        public static void GetWatermarkFromImage(string srcImagePath, string outPath)
        {
            Mat testimage = Cv2.ImRead(srcImagePath);
            Mat watermarkImg = getImageWatermarkWithText(testimage);
            Cv2.ImWrite(outPath, watermarkImg);
        }
        public static void GetWatermarkFromImage(byte[] srcImage, string outPath)
        {
            var file = Path.GetTempFileName();
            File.WriteAllBytes(file, srcImage);

            Mat testimage = Cv2.ImRead(file);
            Mat watermarkImg = getImageWatermarkWithText(testimage);
            Cv2.ImWrite(outPath, watermarkImg);
            if (File.Exists(file)) { File.Delete(file); }
        }
        public static byte[] GetWatermarkFromImage(byte[] srcImage)
        {
            var file = Path.GetTempFileName();
            File.WriteAllBytes(file, srcImage);

            Mat testimage = Cv2.ImRead(file);
            Mat watermarkImg = getImageWatermarkWithText(testimage);
            Cv2.ImWrite(file, watermarkImg);

            var bs = File.ReadAllBytes(file);
            if (File.Exists(file)) { File.Delete(file); }
            return bs;
        }



        private static Mat addImageWatermarkWithText(string imagePath, string watermarkText)
        {
            List<Mat> planes = new List<Mat>();
            List<Mat> allPlanes = new List<Mat>();

            Mat complexImage = Cv2.ImRead(imagePath);
            Mat padded = splitSrc(complexImage, allPlanes);
            padded.ConvertTo(padded, MatType.CV_32F);
            planes.Add(padded);
            planes.Add(Mat.Zeros(padded.Size(), MatType.CV_32F));
            Cv2.Merge(planes.ToArray(), complexImage);
            // dft
            Cv2.Dft(complexImage, complexImage);
            // 添加文本水印
            Scalar scalar = new Scalar(0, 0, 0);
            Point point = new Point(40, 40);
            Cv2.PutText(complexImage, watermarkText, point, HersheyFonts.HersheyDuplex, 1D, scalar);
            Cv2.Flip(complexImage, complexImage, FlipMode.XY);
            Cv2.PutText(complexImage, watermarkText, point, HersheyFonts.HersheyDuplex, 1D, scalar);
            Cv2.Flip(complexImage, complexImage, FlipMode.XY);

            return antitransformImage(complexImage, allPlanes);
        }

        private static Mat splitSrc(Mat mat, List<Mat> allPlanes)
        {
            mat = optimizeImageDim(mat);
            Cv2.Split(mat, out Mat[] mv);
            allPlanes.AddRange(mv);

            Mat padded = new Mat();
            if (allPlanes.Count() > 1) {
                for (int i = 0; i < allPlanes.Count(); i++) {
                    if (i == 0) {
                        padded = allPlanes[i];
                        break;
                    }
                }
            } else {
                padded = mat;
            }

            return padded;
        }

        private static Mat optimizeImageDim(Mat image)
        {
            Mat padded = new Mat();
            int addPixelRows = Cv2.GetOptimalDFTSize(image.Rows);
            int addPixelCols = Cv2.GetOptimalDFTSize(image.Cols);
            Cv2.CopyMakeBorder(image, padded, 0, addPixelRows - image.Rows, 0, addPixelCols - image.Cols, BorderTypes.Constant, Scalar.All(0));

            return padded;
        }

        private static Mat antitransformImage(Mat complexImage, List<Mat> allPlanes)
        {
            Mat invDFT = new Mat();
            Cv2.Idft(complexImage, invDFT, DftFlags.Scale | DftFlags.RealOutput, 0);
            Mat restoredImage = new Mat();
            invDFT.ConvertTo(restoredImage, MatType.CV_8U);
            if (allPlanes.Count == 0) {
                allPlanes.Add(restoredImage);
            } else {
                allPlanes[0] = restoredImage;
            }
            Mat lastImage = new Mat(new Size(complexImage.Width, complexImage.Height), MatType.CV_8U);
            Cv2.Merge(allPlanes.ToArray(), lastImage);
            return lastImage;
        }

        private static Mat getImageWatermarkWithText(Mat image)
        {
            List<Mat> allPlanes = new List<Mat>();
            List<Mat> planes = new List<Mat>();
            Mat complexImage = new Mat();
            Mat padded = splitSrc(image, allPlanes);
            padded.ConvertTo(padded, MatType.CV_32F);
            planes.Add(padded);
            planes.Add(Mat.Zeros(padded.Size(), MatType.CV_32F));
            Cv2.Merge(planes.ToArray(), complexImage);
            // dft
            Cv2.Dft(complexImage, complexImage);
            Mat magnitude = createOptimizedMagnitude(complexImage);
            planes.Clear();
            return magnitude;
        }

        private static Mat createOptimizedMagnitude(Mat complexImage)
        {
            List<Mat> newPlanes = new List<Mat>();
            Mat mag = new Mat();
            Cv2.Split(complexImage, out Mat[] mv);
            newPlanes.AddRange(mv);
            Cv2.Magnitude(newPlanes[0], newPlanes[1], mag);
            Cv2.Add(Mat.Ones(mag.Size(), MatType.CV_32F), mag, mag);
            Cv2.Log(mag, mag);
            shiftDFT(mag);
            mag.ConvertTo(mag, MatType.CV_8UC1);
            Cv2.Normalize(mag, mag, 0, 255, NormTypes.MinMax, MatType.CV_8UC1);
            return mag;
        }

        private static void shiftDFT(Mat image)
        {
            image = image.SubMat(new Rect(0, 0, image.Cols & -2, image.Rows & -2));
            int cx = image.Cols / 2;
            int cy = image.Rows / 2;

            Mat q0 = new Mat(image, new Rect(0, 0, cx, cy));
            Mat q1 = new Mat(image, new Rect(cx, 0, cx, cy));
            Mat q2 = new Mat(image, new Rect(0, cy, cx, cy));
            Mat q3 = new Mat(image, new Rect(cx, cy, cx, cy));
            Mat tmp = new Mat();
            q0.CopyTo(tmp);
            q3.CopyTo(q0);
            tmp.CopyTo(q3);
            q1.CopyTo(tmp);
            q2.CopyTo(q1);
            tmp.CopyTo(q2);
        }

    }
}