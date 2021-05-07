using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using ServerUpload7.WEB.Resources;
using ServerUpload7.BLL.Services;
using System.IO;
using ServerUpload7.BLL.Interfaces;
using AutoMapper;
using ServerUpload7.DAL.Entities;
using System.Security.Cryptography;


namespace ServerUpload7.WEB.Controllers
{
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
        public  MaterialView Material(IFormFile uploadedFile, string category)
        {          
            var MemStream = new MemoryStream();
            uploadedFile.CopyTo(MemStream);
            var FileBytes = MemStream.ToArray();
            var hash = MD5.Create().ComputeHash(FileBytes);
            string StrHash = Convert.ToBase64String(hash);

            string path = _materialsService.GetPath(category, uploadedFile.FileName, 1, _mapper, StrHash);
            if (path == null)
               return null;

            var Result = _mapper.Map<MaterialView>(_materialsService.CreateMaterial(FileBytes, category, uploadedFile.FileName, uploadedFile.Length, _mapper, path, StrHash));

            MemStream.Dispose();
            return Result;
        }
            
        
        [HttpGet]
        [Route("actual-version")]
        public  FileResult ActualVersion(string Name, string Category)
        {
            
            var Result =  _materialsService.DownloadActualVersion(Name, Category, _mapper);
            if (Result == null)
                return null;
            return PhysicalFile($"C:/Users/My/source/repos/ServerUpload7/ServerUpload7.WEB/Files/{Result}", System.Net.Mime.MediaTypeNames.Application.Octet, $"{Name.Split("/").Last()}");
        }

        [HttpGet]
        [Route("info")]
        public ActionResult<MaterialView> GetMaterialInfo(string Name, string Category)
        {
            var Result = _mapper.Map<MaterialView>(_materialsService.GetMaterialInfo(Name, Category, _mapper));
            if (Result == null)    
                return null;
            return Ok(Result);
        }

        [HttpPut]
        [Route("new-category")]
        public ActionResult NewCategory(string Name, string OldCategory, string NewCategory)
        {
            var Result = _materialsService.ChangeCategory(Name, OldCategory, NewCategory, _mapper);
            if (Result == 0)
                return null;
            return Ok();
        }

        [HttpGet]
        [Route("info/category")]
        public ActionResult<List<MaterialView>> Category(string Category)
        {
            var Result = _mapper.Map<IEnumerable<MaterialView>>(_materialsService.FilterMat(Category, _mapper));
            if (Result == null)
                return null;
            return Ok(Result);
        }
    }
}
