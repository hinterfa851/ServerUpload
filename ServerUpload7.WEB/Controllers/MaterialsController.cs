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

// to del 
using ServerUpload7.DAL.Entities;



namespace ServerUpload7.WEB.Controllers
{
    public class MaterialsController : Controller
    {

        private readonly IMaterialsService _materialsService;
  //      private readonly IMapper _mapper;
        public IWebHostEnvironment _appEnvironment;

        public MaterialsController(IMaterialsService materialService, IWebHostEnvironment appEnvironment)
        {
    //        this._mapper = mapper;
            this._materialsService = materialService;
            this._appEnvironment = appEnvironment;
        }

        [HttpPost("Crmat")]
        public  Material CreateMaterial(IFormFile uploadedFile, string category)
        {   
            // potential validator
            // if (validation() != ok) return (fail())
            
            string path = _materialsService.GetPath(category, uploadedFile.FileName, 1);
            if (path == null)
            {
                Console.WriteLine("null returned");
                return null;
            }
            var Result = new Material();
            using (var stream = new FileStream(path, FileMode.Create))
            {
                uploadedFile.CopyTo(stream);
                Result = _materialsService.CreateMaterial(stream, category, uploadedFile.FileName, _appEnvironment.WebRootPath, uploadedFile.Length);
            }
            return Result;
        }
        
        [HttpPost("DownloadActual")]
        // make it async???
        public  FileResult DownloadActualVersion(string Name, string Category)
        {
            
            var Result =  _materialsService.DownloadActualVersion(Name, Category);
            if (Result == null)
                return null;
            return PhysicalFile($"C:/Users/My/source/repos/ServerUpload7/ServerUpload7.WEB/Files/{Result}", System.Net.Mime.MediaTypeNames.Application.Octet, $"{Name.Split("/").Last()}");
        }

        [HttpGet("GetMaterialInfo")]
        public ActionResult<MaterialResource> GetMaterialInfo(string Name, string Category)
        {
            var Result =  _materialsService.GetMaterialInfo(Name, Category);
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
        public ActionResult<List<MaterialResource>> FilterMat(string Category)
        {
            var Result =  _materialsService.FilterMat(Category);
            if (Result == null)
                return null;
            return Ok(Result);
        }
    }
}
