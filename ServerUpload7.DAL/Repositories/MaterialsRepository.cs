﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServerUpload7.DAL.EF;
using ServerUpload7.DAL.Entities;
using ServerUpload7.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace ServerUpload7.DAL.Repositories
{
    public class MaterialsRepository : IRepository<Material>
    {
        private ApplicationContext db;

        public MaterialsRepository(ApplicationContext context)
        {
            this.db = context;
        }

        public IEnumerable<Material> GetAll(Func<Material, Boolean> predicate)
        {
            return db.Materials.Include(o => o.Versions).Where(predicate);
        }

        public Material Get(int id)
        {
            return db.Materials.Find(id);
        }

        public Material Create(Material material)
        {
            db.Materials.Add(material);
            db.SaveChanges();
            return (material);
        }
     
        public Material Find(Func<Material, Boolean> predicate)
        {
            return db.Materials.Include(o => o.Versions).FirstOrDefault(predicate);
        }

        public Material Update(Material material)
        {
            var dbEntity = db.Materials.FirstOrDefault(m => m.Id == material.Id);
            dbEntity.Name = material.Name;
            dbEntity.Category = material.Category;
            dbEntity.Versions = material.Versions;
            db.SaveChanges();
            return dbEntity;
        }
        public async void Delete(int id)
        {
            var dbEntity = await db.Materials.FindAsync(id);
            if (dbEntity != null)
                db.Materials.Remove(dbEntity);
        }

        public void Save()
        {
            db.SaveChangesAsync();
        }
    }
}
