using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CP_v2.DB;
using CP_v2.Models;

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

        public ParkedTableModel GetParkedCars(int currentPage, string veh_no, string token_no)
        {
            ParkedTableModel pt = new ParkedTableModel();
            pt.TotalPages = 7;// _context.parked_car.Include("ap_user").OrderByDescending(x=>x.parkin_time).Count();
            int recordsPerPage = 10;
            long tokenNoLong = long.Parse(string.IsNullOrEmpty(token_no)? "0" : token_no);
            pt.Cars = _context.parked_car.Where(x=> (string.IsNullOrEmpty(veh_no) || x.car_no.ToLower().Contains(veh_no.ToLower())) &&(string.IsNullOrEmpty(token_no) || x.recript_no.Equals(tokenNoLong))).
                OrderByDescending(x => x.parkin_time).Select(x =>
                new ParkedCars
                {
                    Id = x.id,
                    checkinby = x.ap_user == null ? "" : x.ap_user.username,
                    checkinDate = x.parkin_time,
                    checkoutDate = x.parkout_time,
                   tokenNo = x.recript_no,
                   vehicle_NO = x.car_no

                }
            ).Skip(recordsPerPage * currentPage).Take(recordsPerPage).ToList();
            pt.CurrentPage = currentPage + 1;
            return pt;

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