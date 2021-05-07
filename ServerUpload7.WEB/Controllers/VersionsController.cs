using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using ServerUpload7.WEB.Resources;
using ServerUpload7.BLL.Interfaces;
using Version = ServerUpload7.DAL.Entities.Version;
using ServerUpload7.DAL.Entities;
using System.IO;
using AutoMapper;
using System.Security.Cryptography;

namespace ServerUpload7.WEB.Controllers
{
    [Route("version")]
    public class VersionsController : Controller
    {
        
        private readonly IVersionsService _versionsService;
        public IMapper _mapper;
        public IWebHostEnvironment _appEnvironment;

        public VersionsController(IVersionsService materialService, IWebHostEnvironment appEnvironment, IMapper mapper)
        {
            this._mapper = mapper;
            this._versionsService = materialService;
            this._appEnvironment = appEnvironment;
        }
        
        [HttpPost]
        [Route("")]
        public  VersionView Version(IFormFile uploadedFile, string Name, string Category)
        {
            
                var MemStream = new MemoryStream();
                uploadedFile.CopyTo(MemStream);
                var FileBytes = MemStream.ToArray();
                var hash = MD5.Create().ComputeHash(FileBytes);
                string StrHash = Convert.ToBase64String(hash);

                string path = _versionsService.GetPath(Category, Name, _mapper, StrHash, uploadedFile.FileName);
                if (path == null)
                    return null;

                var version = _mapper.Map<VersionView>(_versionsService.CreateVersion(FileBytes, Name, Category, uploadedFile.Length, _mapper, path, StrHash, uploadedFile.FileName));

            MemStream.Dispose();
            return version;
        }
        
        [HttpGet]
        [Route("")]
        public FileResult Version(string Name, string Category, int Num)
        {
            var Result = _versionsService.DownloadVers(Num, Name, Category, _mapper);
            if (Result == null)
                return null;
            return PhysicalFile($"C:/Users/My/source/repos/ServerUpload7/ServerUpload7.WEB/Files/{Result}", System.Net.Mime.MediaTypeNames.Application.Octet, $"{Result.Split("/").Last()}");
        }
    }
}
