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
using ServerUpload7.DAL.Services;
using AutoMapper;
using ServerUpload7.DAL.Entities;



namespace ServerUpload7.WEB.Controllers
{
    public class MaterialsController : Controller
    {

        private readonly IMaterialsService _materialsService;
        private IMapper _mapper;
        public IWebHostEnvironment _appEnvironment;
        
        public MaterialsController(IMaterialsService materialService, IWebHostEnvironment appEnvironment, IMapper mappe)
        {
            this._mapper = mappe;
            this._materialsService = materialService;
            this._appEnvironment = appEnvironment;
        }


        [HttpPost("Crmat")]
        public  MaterialView CreateMaterial(IFormFile uploadedFile, string category)
        {
            string path = _materialsService.GetPath(category, uploadedFile.FileName, 1);
            if (path == null)
            {
                Console.WriteLine("null returned");
                return null;
            }
            Console.WriteLine("null returned");
            var Result = new MaterialView();
            using (var stream = new FileStream(path, FileMode.Create))
            {
                uploadedFile.CopyTo(stream);
                Result = _mapper.Map<MaterialView>(_materialsService.CreateMaterial(stream, category, uploadedFile.FileName, _appEnvironment.WebRootPath, uploadedFile.Length));
            }
            return Result;
        }
        
        [HttpPost("DownloadActual")]
        public  FileResult DownloadActualVersion(string Name, string Category)
        {
            
            var Result =  _materialsService.DownloadActualVersion(Name, Category);
            if (Result == null)
                return null;
            return PhysicalFile($"C:/Users/My/source/repos/ServerUpload7/ServerUpload7.WEB/Files/{Result}", System.Net.Mime.MediaTypeNames.Application.Octet, $"{Name.Split("/").Last()}");
        }

        [HttpGet("GetMaterialInfo")]
        public ActionResult<MaterialView> GetMaterialInfo(string Name, string Category)
        {
            var Result = _mapper.Map<MaterialView>(_materialsService.GetMaterialInfo(Name, Category));
            if (Result == null)    
                return null;
            return Ok(Result);
        }

        [HttpGet("ChangeCategory")]
        public ActionResult ChangeCategory(string Name, string OldCategory, string NewCategory)
        {
            var Result = _materialsService.ChangeCategory(Name, OldCategory, NewCategory);
            if (Result == 0)
                return null;
            return Ok();
        }

        [HttpGet("FilterMat")]
        public ActionResult<List<MaterialView>> FilterMat(string Category)
        {
            var Result = _mapper.Map<IEnumerable<MaterialView>>(_materialsService.FilterMat(Category));
            if (Result == null)
                return null;
            return Ok(Result);
        }
    }
}
