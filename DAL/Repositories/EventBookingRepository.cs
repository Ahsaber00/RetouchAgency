using DAL.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class EventBookingRepository:GenericRepository<EventBooking>,IEventBookingRepository
    {
        public EventBookingRepository(ApplicationContext context):base(context)
        {
            
        }
    }
}
