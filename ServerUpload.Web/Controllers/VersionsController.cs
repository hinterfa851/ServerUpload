using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ServerUpload.Web.Dto;
using ServerUpload.BLL.Enums;
using ServerUpload.BLL.Interfaces;

namespace ServerUpload.Web.Controllers
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
            var memStream = new MemoryStream();
            uploadedFile.CopyTo(memStream);
            var fileBytes = memStream.ToArray();
            string strHash  = _versionsService.GetHash(fileBytes);
            string path = _versionsService.GetPath(category, name, strHash, uploadedFile.FileName);
            var version = _mapper.Map<VersionDto>(_versionsService.CreateVersion(fileBytes, name, category, uploadedFile.Length, path, strHash, uploadedFile.FileName));
            memStream.Dispose();
            return version;
        }
        
        [HttpGet]
        [Route("")]
        public FileResult Version(string name, Categories category, int num)
        {
            var result = _versionsService.DownloadVersion(num, name, category);
            return PhysicalFile(_configuration["FilePath"] + result, System.Net.Mime.MediaTypeNames.Application.Octet, $"{result.Split("/").Last()}");
        }
    }
}
