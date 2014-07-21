using ObjectModel;
using System.Collections.Generic;

namespace Data
{
    public interface IRepository<T> where T : Article
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        int Post(T entity);
        bool Put(T entity);
        void Delete(int id);
        IEnumerable<T> Search(string keyword);
    }
}
