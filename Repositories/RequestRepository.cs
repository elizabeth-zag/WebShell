using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShell.Models;

namespace WebShell.Repositories
{
    public class RequestRepository
    {
        private RequestContext db;
        public RequestRepository(RequestContext context)
        {
            this.db = context;
        }
        public void Create(Request request)
        {
            db.Requests.Add(request);
        }
        public void Save()
        {
            db.SaveChanges();
        }
        public IEnumerable<Request> GetAll()
        {
            return db.Requests;
        }

    }
}
