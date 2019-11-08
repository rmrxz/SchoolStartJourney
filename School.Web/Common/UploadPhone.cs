using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace School.Web.Common
{
    public class UploadPhone
    {
        private IHostingEnvironment _hostingEnv;
        public UploadPhone(IHostingEnvironment hostingEnv)
        {
            this._hostingEnv = hostingEnv;
        }


        /// <summary>
        /// 长传单个图片
        /// </summary>
        /// <param name="file"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public string PhoneNewUpload(IFormFile file, string path)
        {
            string phonePath = "";
            long size = 0;
            var fileName = ContentDispositionHeaderValue
                .Parse(file.ContentDisposition)
                .FileName
                .Trim('"')
                .Substring(file.FileName.LastIndexOf("\\") + 1);
            if (!(fileName.EndsWith(".jpg") || fileName.EndsWith(".gif")|| fileName.EndsWith(".png") || fileName.EndsWith(".JPG") || fileName.EndsWith(".GIF") || fileName.EndsWith(".PNG")))
            {
                throw new Exception("图片上传格式不对");
            }
            phonePath = fileName = $@"\uploadFiles\{path}\{Guid.NewGuid() + fileName}";
            fileName = _hostingEnv.WebRootPath + fileName;
            size += file.Length;
            using (FileStream fs = System.IO.File.Create(fileName))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
            return phonePath;
        }

        /// <summary>
        /// 上传图片集合
        /// </summary>
        /// <param name="files"></param>
        ///  /// <param name="path"></param>
        public void SavaFormFiles(List<IFormFile> files, string path)
        {
            long size = 0;
            foreach (var file in files)
            {
                var fileName = ContentDispositionHeaderValue
                    .Parse(file.ContentDisposition)
                    .FileName
                    .Trim('"')
                    .Substring(file.FileName.LastIndexOf("\\" + 1));
                fileName = Guid.NewGuid() + fileName;
                fileName = _hostingEnv.WebRootPath + $@"\uploadFiles\{path}\{fileName}";
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(fileName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
        }
    }
}
