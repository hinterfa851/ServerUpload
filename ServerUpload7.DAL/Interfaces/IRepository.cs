using System;
using System.Collections.Generic;

namespace ServerUpload7.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public IEnumerable<T> GetAll(Func<T, bool> predicate);
        public T Get(int id);
        public T Find(Func<T, bool> predicate);
        public T Create(T item, byte [] FileBytes, string path);
        public T Update(T item);
        public void Delete(int id);

        public void Save();
    }
}
