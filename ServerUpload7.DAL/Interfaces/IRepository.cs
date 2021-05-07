using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerUpload7.DAL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public IEnumerable<T> GetAll(Func<T, Boolean> predicate);
        public T Get(int id);
        public T Find(Func<T, Boolean> predicate);
        public T Create(T item, byte [] FileBytes, string path);
        public T Update(T item);
        public void Delete(int id);

        public void Save();
    }
}
