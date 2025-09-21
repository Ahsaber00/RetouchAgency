using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class EventRepository:GenericRepository<Event>,IEventRepository
    {
        public EventRepository(ApplicationContext context):base(context)
        {
            
        }
    }
}
