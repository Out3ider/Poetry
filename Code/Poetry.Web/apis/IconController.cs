using System;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

using Sail.Common;
using Sail.Web;

namespace Poetry.Web
{
    public class IconController : BaseUploadController
    {
        private const int OrientationId = 0x0112;
        public enum ExifOrientations : byte
        {
            Unknown = 0,
            TopLeft = 1,
            TopRight = 2,
            BottomRight = 3,
            BottomLeft = 4,
            LeftTop = 5,
            RightTop = 6,
            RightBottom = 7,
            LeftBottom = 8,
        }

        public static ExifOrientations ImageOrientation(Image img)
        {
            int orientationIndex = Array.IndexOf(img.PropertyIdList, OrientationId);
            if (orientationIndex < 0) return ExifOrientations.Unknown;
            return (ExifOrientations)img.GetPropertyItem(OrientationId).Value[0];
        }

        /// <summary>
        /// 根据exif信息旋转图片
        /// </summary>
        /// <param name="fileName">图片物理路径</param>
        /// <returns></returns>
        public static void RotateFlip(string fileName)
        {
            Image img = Image.FromFile(fileName);

            var rotate = ImageOrientation(img);


            {

                EncoderValue? eRotate = null;
                EncoderValue? eFlip = null;
                switch ((int)rotate)
                {
                    case 2://水平镜像
                        eFlip = EncoderValue.TransformFlipHorizontal;
                        break;
                    case 3://180度
                        eRotate = EncoderValue.TransformRotate180;
                        break;
                    case 4://垂直镜像
                        eFlip = EncoderValue.TransformFlipVertical;
                        break;
                    case 5://水平镜像，270
                        eFlip = EncoderValue.TransformFlipHorizontal;
                        eRotate = EncoderValue.TransformRotate90;
                        break;
                    case 6://270
                        eRotate = EncoderValue.TransformRotate90;
                        break;
                    case 7://水平镜像,90
                        eFlip = EncoderValue.TransformFlipHorizontal;
                        eRotate = EncoderValue.TransformRotate270;
                        break;
                    case 8://90
                        eRotate = EncoderValue.TransformRotate270;
                        break;
                    default:
                        return;
                }

                var encParms = new EncoderParameters(1);
                var enc = Encoder.Transformation;
                var codecs = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo ici = null;
                foreach (ImageCodecInfo codec in codecs)
                {
                    if (codec.MimeType == "image/jpeg")
                        ici = codec;
                }
                string tmpFile = fileName.Insert(fileName.LastIndexOf("."), ".tmp");
                try
                {
                    PropertyItem pi = img.GetPropertyItem(0x112);
                    pi.Value[0] = 1;
                    img.SetPropertyItem(pi);
                }
                catch { }
                if (eRotate != null)
                {
                    EncoderParameter ep = new EncoderParameter(enc, (long)eRotate);
                    encParms.Param[0] = ep;
                    img.Save(tmpFile, ici, encParms);
                }
                if (eFlip != null)
                {
                    EncoderParameter ep = new EncoderParameter(enc, (long)eFlip);
                    encParms.Param[0] = ep;
                    img.Save(tmpFile, ici, encParms);
                }
                img.Dispose();
                if (File.Exists(tmpFile))
                {
                    File.Copy(tmpFile, fileName, true);
                    File.Delete(tmpFile);
                }
            }
        }


        protected virtual string UploadPre => $"~/uploads/{DateTime.Now.ToString("yyyyMM")}/Pre/";

        protected virtual string UploadMediumPre => $"~/uploads/{DateTime.Now.ToString("yyyyMM")}/MediumPre/";

        protected virtual string UploadBigPre => $"~/uploads/{DateTime.Now.ToString("yyyyMM")}/BigPre/";

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="file"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override dynamic Uploaded(MultipartFileData file, HttpRequest request)
        {
            var name = Guid.NewGuid().ToString("N");
            var filex = new FileInfo(file.LocalFileName);

            var oldName = file.Headers.ContentDisposition.FileName.Replace("\"", "");
            var ext = Path.GetExtension(oldName);
            var uploadName = Path.GetFileNameWithoutExtension(oldName);

            var filename = "{0}{1}".FormatWith(name, ext);
            filex.Rename(filename);

            var srcFile = HostingEnvironment.MapPath(UploadRoot + filename);




            HostingEnvironment.MapPath(UploadPre).CreateDir();
            HostingEnvironment.MapPath(UploadMediumPre).CreateDir();
            HostingEnvironment.MapPath(UploadBigPre).CreateDir();


            ImageExtension.MakeThumbnail(srcFile, HostingEnvironment.MapPath(UploadPre + filename), 220, 0,
                ImageExtension.ThumbnailMode.Width);

            ImageExtension.MakeThumbnail(srcFile, HostingEnvironment.MapPath(UploadMediumPre + filename), 440, 0,
                ImageExtension.ThumbnailMode.Width); 

            ImageExtension.MakeThumbnail(srcFile, HostingEnvironment.MapPath(UploadBigPre + filename), 660, 0,
                ImageExtension.ThumbnailMode.Width);



            var filedb = new
            {
                FilePath = VirtualPathUtility.ToAbsolute(UploadRoot + filename) 
            };
            return filedb;
        } 
    }
}