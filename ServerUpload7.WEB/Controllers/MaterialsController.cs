using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using ServerUpload7.BLL.Interfaces;
using AutoMapper;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using ServerUpload7.DAL.Entities;
using ServerUpload7.Web.Dto;


namespace ServerUpload7.WEB.Controllers
{
    public enum Categories : byte
    {
        App,
        Presentation,
        Other
    }

    [Route("materials")]
    public class MaterialsController : Controller
    {

        private readonly IMaterialsService _materialsService;
        private readonly IConfiguration _configuration;
        public IMapper _mapper;
        public IWebHostEnvironment _appEnvironment;
        
        public MaterialsController(IMaterialsService materialService, IWebHostEnvironment appEnvironment, IMapper mapper, IConfiguration configuration)
        {
            this._mapper = mapper;
            this._materialsService = materialService;
            this._appEnvironment = appEnvironment;
            this._configuration = configuration;
        }

        
        [HttpPost]
        [Route("")]
        public  MaterialDto Material(IFormFile uploadedFile, Categories category)
        {
            var MemStream = new MemoryStream();
            uploadedFile.CopyTo(MemStream);
            var FileBytes = MemStream.ToArray();
            var hash = MD5.Create().ComputeHash(FileBytes);
            string StrHash = Convert.ToBase64String(hash);

            string path = _materialsService.GetPath((int) category, uploadedFile.FileName, 1, _mapper, StrHash);
            if (path == null)
               return null;

            var Result = _mapper.Map<MaterialDto>(_materialsService.CreateMaterial(FileBytes, (int) category, uploadedFile.FileName, uploadedFile.Length, _mapper, path, StrHash));
            
            MemStream.Dispose();
            return Result;
        }
            
        
        [HttpGet]
        [Route("actual-version")]
        public  FileResult ActualVersion(string name, Categories category)
        {
            
            var Result =  _materialsService.DownloadActualVersion(name, (int) category, _mapper);
            if (Result == null)
                return null;
            
           // return PhysicalFile($"C:/Users/My/source/repos/ServerUpload7/ServerUpload7.WEB/Files/{Result}", System.Net.Mime.MediaTypeNames.Application.Octet, $"{name.Split("/").Last()}");
            return PhysicalFile(_configuration["path"] + Result, System.Net.Mime.MediaTypeNames.Application.Octet, $"{name.Split("/").Last()}");

        }

        [HttpGet]
        [Route("info")]
        public ActionResult<MaterialDto> GetMaterialInfo(string name, Categories category)
        {
            var Result = _mapper.Map<MaterialDto>(_materialsService.GetMaterialInfo(name, (int) category, _mapper));
            if (Result == null)    
                return null;
            return Ok(Result);
        }

        [HttpPut]
        [Route("new-category")]
        public ActionResult NewCategory(string name, Categories oldCategory, Categories newCategory)
        {
            var Result = _materialsService.ChangeCategory(name, (int) oldCategory, (int) newCategory, _mapper);
            if (Result == 0)
                return null;
            return Ok();
        }

        [HttpGet]
        [Route("info/category")]
        public ActionResult<List<MaterialDto>> Category(Categories category)
        {
            var Result = _mapper.Map<IEnumerable<MaterialDto>>(_materialsService.FilterMat((int) category, _mapper));
            if (Result == null)
                return null;
            return Ok(Result);
        }
    }
}
