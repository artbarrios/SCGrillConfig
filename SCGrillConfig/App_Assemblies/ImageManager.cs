using @SCGrillConfig.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace SCGrillConfig
{
    public class ImageManager
    {
        public static string GetImagesRelativePath()
        {
            // returns the relative location of image storage for this application
            string imagesRelativePath = @"/Content/images/";
            return imagesRelativePath;
        } // GetImagesRelativePath

        public static string GetImagesHardPath()
        {
            // returns the disk location of image storage for this application
            return HttpContext.Current.Server.MapPath(GetImagesRelativePath());
        } // GetImagesPath

        public static void DeleteAllImageFilesFromDisk()
        {
            // deletes all files of type .jpg, .png, .bmp in the images storage location disk path
            string imageHardPath = GetImagesHardPath();
            string extension = "";
            if (Directory.Exists(imageHardPath))
            {
                // path is good so process each file there
                foreach (String filename in Directory.GetFiles(imageHardPath))
                {
                    extension = Path.GetExtension(filename).ToUpper();
                    // delete the files that are image files
                    if (extension == ".JPG" || extension == ".PNG" || extension == ".BMP") { File.Delete(filename); }
                }
            } // if (Directory.Exists(hardPath))
        } // DeleteAllImageFiles

        public static void WriteAllImageFilesToDisk()
        {
            // for each application object that has a Byte() type writes a file to disk for it
            // we assume the file type is jpg
            string imageHardPath = GetImagesHardPath();
            @SCGrillConfigContext db = new @SCGrillConfigContext();

            // process all objects with image properties
        } // WriteAllImageFileToDisk

    } // public class ImageManager
}
