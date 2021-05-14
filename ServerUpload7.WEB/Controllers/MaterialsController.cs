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
        public IMapper _mapper;
        public IWebHostEnvironment _appEnvironment;
        
        public MaterialsController(IMaterialsService materialService, IWebHostEnvironment appEnvironment, IMapper mapper)
        {
            this._mapper = mapper;
            this._materialsService = materialService;
            this._appEnvironment = appEnvironment;
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

            string path = _materialsService.GetPath("App", uploadedFile.FileName, 1, _mapper, StrHash);
            if (path == null)
               return null;

            var Result = _mapper.Map<MaterialDto>(_materialsService.CreateMaterial(FileBytes, "App", uploadedFile.FileName, uploadedFile.Length, _mapper, path, StrHash));

            MemStream.Dispose();
            return Result;
        }
            
        
        [HttpGet]
        [Route("actual-version")]
        public  FileResult ActualVersion(string name, Categories category)
        {
            
            var Result =  _materialsService.DownloadActualVersion(name, category, _mapper);
            if (Result == null)
                return null;
            return PhysicalFile($"C:/Users/My/source/repos/ServerUpload7/ServerUpload7.WEB/Files/{Result}", System.Net.Mime.MediaTypeNames.Application.Octet, $"{name.Split("/").Last()}");
        }

        [HttpGet]
        [Route("info")]
        public ActionResult<MaterialDto> GetMaterialInfo(string name, Categories category)
        {
            var Result = _mapper.Map<MaterialDto>(_materialsService.GetMaterialInfo(name, category, _mapper));
            if (Result == null)    
                return null;
            return Ok(Result);
        }

        [HttpPut]
        [Route("new-category")]
        public ActionResult NewCategory(string name, Categories oldCategory, Categories newCategory)
        {
            var Result = _materialsService.ChangeCategory(name, oldCategory, newCategory, _mapper);
            if (Result == 0)
                return null;
            return Ok();
        }

        [HttpGet]
        [Route("info/category")]
        public ActionResult<List<MaterialDto>> Category(string category)
        {
            var Result = _mapper.Map<IEnumerable<MaterialDto>>(_materialsService.FilterMat(category, _mapper));
            if (Result == null)
                return null;
            return Ok(Result);
        }
    }
}
