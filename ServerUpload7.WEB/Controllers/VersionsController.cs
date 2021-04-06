using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using ServerUpload7.WEB.Resources;
using ServerUpload7.DAL.Services;
using Version = ServerUpload7.DAL.Entities.Version;
using ServerUpload7.DAL.Entities;
using System.IO;

namespace ServerUpload7.WEB.Controllers
{
    public class VersionsController : Controller
    {
        
        private readonly IVersionsService _versionsService;
        public IWebHostEnvironment _appEnvironment;
        public VersionsController(IVersionsService materialService, IWebHostEnvironment appEnvironment)
        {
              this._versionsService = materialService;
            this._appEnvironment = appEnvironment;
        }
        
        [HttpPost("CreateVersion")]
        public  Version CreateVersion(IFormFile uploadedFile, string Name, string Category)
        {
            Version version;
            string path = _versionsService.GetPath(Category, Name);
            if (path == null)
                return null;
            
            using (var stream = new FileStream( path, FileMode.Create))
            {
                uploadedFile.CopyTo(stream);
                version = _versionsService.CreateVersion(Name, Category, _appEnvironment.WebRootPath, uploadedFile.Length);
            }
            return version;
        }
        
        [HttpPost("DownloadVers")]
        public FileResult DownloadVers(string Name, string Category, int Num)
        {
            var Result = _versionsService.DownloadVers(Num, Name, Category);
            if (Result == null)
                return null;
            return PhysicalFile($"C:/Users/My/source/repos/ServerUpload7/ServerUpload7.WEB/Files/{Result}", System.Net.Mime.MediaTypeNames.Application.Octet, $"{Name.Split("/").Last()}");

        }
    }
}
