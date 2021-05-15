using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using ServerUpload7.BLL.Interfaces;
using System.IO;
using AutoMapper;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;

using ServerUpload7.BLL.BusinessModels;
using ServerUpload7.Web.Dto;

namespace ServerUpload7.WEB.Controllers
{
    [Route("version")]
    public class VersionsController : Controller
    {
        
        private readonly IVersionsService _versionsService;
        public readonly IConfiguration _configuration;
        public IMapper _mapper;
        public IWebHostEnvironment _appEnvironment;

        public VersionsController(IVersionsService materialService, IWebHostEnvironment appEnvironment, IMapper mapper, IConfiguration configuration)
        {
            this._mapper = mapper;
            this._versionsService = materialService;
            this._appEnvironment = appEnvironment;
            this._configuration = configuration;
        }
        
        [HttpPost]
        [Route("")]
        public  VersionDto Version(IFormFile uploadedFile, string name, Categories category)
        {
            var MemStream = new MemoryStream();
            uploadedFile.CopyTo(MemStream);
            var FileBytes = MemStream.ToArray();
            string StrHash  = _versionsService.GetHash(FileBytes);
        //    var hash = MD5.Create().ComputeHash(FileBytes);
         //   string StrHash = Convert.ToBase64String(hash);
            
            string path = _versionsService.GetPath((int) category, name, _mapper, StrHash, uploadedFile.FileName);
            if (path == null)
                return null;

            var version = _mapper.Map<VersionDto>(_versionsService.CreateVersion(FileBytes, name, (int) category, uploadedFile.Length, _mapper, path, StrHash, uploadedFile.FileName));

            MemStream.Dispose();
            return version;
        }
        
        [HttpGet]
        [Route("")]
        public FileResult Version(string name, Categories category, int num)
        {
            var Result = _versionsService.DownloadVersion(num, name, (int) category, _mapper);
            if (Result == null)
                return null; 
//            return PhysicalFile($"C:/Users/My/source/repos/ServerUpload7/ServerUpload7.WEB/Files/{Result}", System.Net.Mime.MediaTypeNames.Application.Octet, $"{Result.Split("/").Last()}");
            return PhysicalFile(_configuration["path"], System.Net.Mime.MediaTypeNames.Application.Octet, $"{Result.Split("/").Last()}");

        }
    }
}
