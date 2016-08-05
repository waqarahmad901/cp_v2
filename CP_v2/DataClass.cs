using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CP_v2.DB;

namespace CP_v2
{
    public class DataClass
    {
        car_parkingEntities _context = new car_parkingEntities();
        public ap_user VerifyUserLogin(string userName, string password)
        {
            return _context.ap_user.Where(x => x.username == userName && x.password == password).FirstOrDefault();

        } 

        public ap_user GetUserByUserName(string userName)
        {
            return _context.ap_user.Where(x => x.username == userName).FirstOrDefault();

        }
        public List<dur_amount> GetDurationAmount()
        {
            return _context.dur_amount.Where(x => x.veh_type.name.Equals("Car")).ToList();
        }

        public List<parked_car> GetParkedCars()
        {
            return _context.parked_car.OrderByDescending(x=>x.parkin_time).Take(20).ToList();
        }

        public bool AddNewParkedCar(parked_car car)
        {
             _context.parked_car.Add(car);
            return _context.SaveChanges() > 0;
        }

        public long GetLastReciptNumber()
        {
            var pc = _context.parked_car.OrderByDescending(x => x.parkin_time).FirstOrDefault();
            if (pc == null)
                return 0;
            return pc.recript_no;
        }
    }
}