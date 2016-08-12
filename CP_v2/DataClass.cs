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

        public void AddPayment(payment pa)
        {
            _context.payments.Add(pa);
            _context.SaveChanges();
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

        public List<PaymentModel> GetAllPayemnts(string userName)
        {
            return _context.payments.Where(x => string.IsNullOrEmpty(userName) || x.ap_user.username.ToLower() == userName.ToLower())
                .Select(x=> new PaymentModel
                {
                    username = x.ap_user.username,
                    amount = x.amount,
                    payment_type = x.payment_type,
                    received_date = x.received_date
                }).ToList();

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

        public bool CheckCarRegisterInCurrentMonth(string veh_no)
        {
            return _context.monthly_reg.Any(x => x.month_name == DateTime.Now.Month.ToString() && x.vehicle_no.ToLower().Equals(veh_no));
        }

        public parked_car GetParkedCarById(Guid id)
        {
            return _context.parked_car.Where(x => x.id == id).FirstOrDefault();
        }

        public void Update()
        {
            _context.SaveChanges();
        }

        public parked_car GetParkedCarByTokenNo(string token_no)
        {
            long result = 0;
            bool token = long.TryParse(token_no,out result);
            parked_car car = null;
            if(token)
                car = _context.parked_car.Where(x=> x.recript_no == result).FirstOrDefault();
            if(car == null)
                car = _context.parked_car.Where(x => x.car_no.Contains(token_no)).FirstOrDefault();
            return car;
        }
    }
}