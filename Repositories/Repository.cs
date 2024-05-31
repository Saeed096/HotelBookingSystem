using HotelBookingSystem.hotelContext;
using HotelBookingSystem.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSystem.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly Context Context;

        public Repository(Context _context)
        {
            Context = _context;
        }

        public void Delete(T item)
        {
            Context.Remove(item);
        }

        public List<T> GetAll(string include = null)
        {
            if (include == null)
            {
                return Context.Set<T>().ToList();
            }
            return Context.Set<T>().Include(include).ToList();
        }

        public T GetById(int? Id)
        {
            return Context.Set<T>().Find(Id);
        }


        public List<T> Get(Func<T, bool> where, string? include = null)
        {
            if(include == null)
            {
                return Context.Set<T>().Where(where).ToList();
            }
            return Context.Set<T>().Include(include).Where(where).ToList();
        }

        public List<T> GetRange(Func<T, bool> where, int take , string? include = null) 
        {
            if (include == null)
            {
                return Context.Set<T>().Where(where).Take(take).ToList();
            }
            return Context.Set<T>().Include(include).Where(where).Take(take).ToList();
        }

        public void Insert(T item)
        {
            Context.Add(item);
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public void Update(T item)
        {
            Context.Update(item);
        }
    }
}
