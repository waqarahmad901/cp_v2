using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using CP_v2.DB;
using CP_v2.Models;

namespace CP_v2
{
    public class DataClass
    {
        car_parkingEntities1 _context = new car_parkingEntities1();
        public ap_user VerifyUserLogin(string userName, string password)
        {
            return _context.ap_user.Where(x => x.username == userName && x.password == password).FirstOrDefault();

        }

        public List<DailyCashModel> GetUserReport(string fromdate, string to)
        {
            try
            {
                DateTime fromDate = new DateTime();
                if (!string.IsNullOrEmpty(fromdate))
                    fromDate = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime shif1From = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 8, 0, 0);
                DateTime shift1To = shif1From.AddHours(12);

                DateTime shif2From = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day, 20, 0, 0);
                DateTime shift2To = shif2From.AddHours(12);

                List<DailyCashModel> listModel = new List<DailyCashModel>();
                DailyCashModel model = new DailyCashModel();

                var resultsIn = (from a in _context.parked_car
                                 where a.parkout_time >= shif1From && a.parkout_time <= shift1To
                                 group a by a.out_by
                               ).ToList();

                var finalIn = (from r in resultsIn
                               select new DailyCahTable
                               {
                                   totalAmount = r.Sum(x => x.charged_amount),
                                   user = _context.ap_user.Where(x => x.id == r.Key.Value).FirstOrDefault() == null ? "" : _context.ap_user.Where(x => x.id == r.Key.Value).FirstOrDefault().username,
                                   totalParkedIn = 0,
                                   totalParkedOut = r.Where(a => !a.is_monthly.HasValue || a.is_monthly == (bool?)false).Count(),
                                   totalParkedOutMonthly = r.Where(a => a.is_monthly.HasValue && a.is_monthly == (bool?)true).Count(),
                               }).ToList();


                var resultsOut = (from a in _context.parked_car
                                  where a.parkin_time >= shif1From && a.parkin_time <= shift1To
                                  group a by a.created_by
                   ).ToList();

                var finalOut = (from r in resultsOut
                                select new DailyCahTable
                                {
                                    totalAmount = 0,
                                    user = _context.ap_user.Where(x => x.id == r.Key.Value).FirstOrDefault() == null ? "" : _context.ap_user.Where(x => x.id == r.Key.Value).FirstOrDefault().username,
                                    totalParkedIn = r.Where(a => !a.is_monthly.HasValue || a.is_monthly == (bool?)false).Count(),
                                    totalParkedInMonthly = r.Where(a => a.is_monthly.HasValue && a.is_monthly == (bool?)true).Count(),
                                    totalParkedOut = 0
                                }).ToList();

                model.shiftsTable.AddRange(finalIn.Union(finalOut));

                model.FromTime = shif1From.ToString("dd-MM-yyyy hh:mm:ss tt");
                model.ToTime = shift1To.ToString("dd-MM-yyyy hh:mm:ss tt");
                listModel.Add(model);

                model = new DailyCashModel();

                resultsIn = (from a in _context.parked_car
                             where a.parkout_time >= shif2From && a.parkout_time <= shift2To
                             group a by a.out_by
                            ).ToList();

                finalIn = (from r in resultsIn
                           select new DailyCahTable
                           {
                               totalAmount = r.Sum(x => x.charged_amount),
                               user = _context.ap_user.Where(x => x.id == r.Key.Value).FirstOrDefault() == null ? "" : _context.ap_user.Where(x => x.id == r.Key.Value).FirstOrDefault().username,
                               totalParkedIn = 0,
                               totalParkedOut = r.Where(a => !a.is_monthly.HasValue || a.is_monthly == (bool?)false).Count(),
                               totalParkedOutMonthly = r.Where(a => a.is_monthly.HasValue && a.is_monthly == (bool?)true).Count(),
                           }).ToList();


                resultsOut = (from a in _context.parked_car
                              where a.parkin_time >= shif2From && a.parkin_time <= shift2To
                              group a by a.created_by
                  ).ToList();

                finalOut = (from r in resultsOut
                            select new DailyCahTable
                            {
                                totalAmount = 0,
                                user = _context.ap_user.Where(x => x.id == r.Key.Value).FirstOrDefault() == null ? "" : _context.ap_user.Where(x => x.id == r.Key.Value).FirstOrDefault().username,
                                totalParkedIn = r.Where(a => !a.is_monthly.HasValue || a.is_monthly == (bool?)false).Count(),
                                totalParkedInMonthly = r.Where(a => a.is_monthly.HasValue && a.is_monthly == (bool?)true).Count(),
                                totalParkedOut = 0
                            }).ToList();

                model.shiftsTable.AddRange(finalIn.Union(finalOut));

                model.FromTime = shif2From.ToString("dd-MM-yyyy hh:mm:ss tt");
                model.ToTime = shift2To.ToString("dd-MM-yyyy hh:mm:ss tt");
                listModel.Add(model);

                return listModel;
            }
            catch (Exception ex)
            {
                File.AppendAllText("C:/cp.txt", ex.Message + ex.StackTrace);
            }
            return null;
        }

        public int DeleteSubcriptionbyId(string id)
        {
            Guid guid = Guid.Parse(id);
            var regis = _context.monthly_reg.FirstOrDefault(x => x.id == guid);
            _context.monthly_reg.Remove(regis);
            return _context.SaveChanges();
        }

        public SubscriptionModel GetSubcriptionbyId(string id)
        {
            Guid guid = Guid.Parse(id);
            return _context.monthly_reg.Where(x => x.id == guid)
                .Select(x => new SubscriptionModel
                { amount = x.amount, cninc = x.cnic, carno = x.vehicle_no, mobileno = x.contact_no, month = x.month_name, ownername = x.ownername })
                .FirstOrDefault();
        }

        public monthly_reg GetRegistrationbyId(string id)
        {
            Guid guid = Guid.Parse(id);
            return _context.monthly_reg.Where(x => x.id == guid).FirstOrDefault();
        }

        public int CheckoutCars(string ids, Guid userId)
        {
            List<Guid> splittedGuids = ids.Substring(1).Split(',').Select(s => Guid.Parse(s)).ToList();
            var carList = _context.parked_car.Where(x => splittedGuids.Contains(x.id)).ToList();

            foreach (var item in carList)
            {
                DateTime durationTime = new DateTime((DateTime.Now - item.parkin_time.Value).Ticks);
                item.parked_duration = durationTime.ToString("dd") + " Days " + durationTime.ToString("HH") + " Hours " + durationTime.ToString("mm") + " Minutes ";
            }
            carList.ForEach(x => x.paid_amount = 0);
            carList.ForEach(x => x.charged_amount = 0);
            carList.ForEach(x => x.parkout_time = DateTime.Now);
            carList.ForEach(x => x.out_by = userId);
            return _context.SaveChanges();
        }

        public object GetAllCars(string from, string to)
        {
            throw new NotImplementedException();
        }

        public List<ap_user> GetAllUsers()
        {
            return _context.ap_user.ToList();

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

        public void AddRegistration(monthly_reg reg)
        {
            _context.monthly_reg.Add(reg);
            _context.SaveChanges();
        }

        public SubscriptionTableModel GetAllSubscriptions(string carno, string name, string month, int page)
        {
            int recordPerPage = 100;
            SubscriptionTableModel model = new SubscriptionTableModel();
            model.Subscriptions = _context.monthly_reg
                .Where(x => (string.IsNullOrEmpty(carno) || x.vehicle_no.ToLower() == carno.ToLower())
                && (string.IsNullOrEmpty(name) || x.ownername.ToLower() == name.ToLower())
                && (string.IsNullOrEmpty(month) || x.month_name.ToLower() == month.ToLower()))
              .Select(x => new SubscriptionModel
              {
                  id = x.id,
                  carno = x.vehicle_no,
                  cninc = x.cnic,
                  mobileno = x.contact_no,
                  ownername = x.ownername,
                  amount = x.amount,
                  month = x.month_name
              })

              .ToList();
            model.TotalPages = (model.Subscriptions.Count / recordPerPage) + 1;
            model.Subscriptions = model.Subscriptions.OrderByDescending(x => x.month)
              .Skip(recordPerPage * (page - 1)).Take(recordPerPage).ToList();
            model.CurrentPage = page;

            return model;
        }

        public List<dur_amount> GetDurationAmount()
        {
            return _context.dur_amount.Where(x => x.veh_type.name.Equals("Car")).ToList();
        }

        public ParkedTableModel GetParkedCars(int currentPage, string veh_no, string token_no, int recordsPerPage, string parked, string from, string to, string userid)
        {
            ParkedTableModel pt = new ParkedTableModel();
            Guid userGuid = Guid.Empty;
            if (!string.IsNullOrEmpty(userid))
                userGuid = Guid.Parse(userid);
            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();
            if (!string.IsNullOrEmpty(from))
                fromDate = DateTime.ParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(to))
                toDate = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1);
            var allUsers = _context.ap_user.ToList();
            long tokenNoLong = long.Parse(string.IsNullOrEmpty(token_no) ? "0" : token_no);
            var cars = _context.parked_car.Where(x => (string.IsNullOrEmpty(veh_no) || x.car_no.ToLower().Contains(veh_no.ToLower()))
                    && (parked == "all" || x.parkout_time == null)
                    && (string.IsNullOrEmpty(from) || x.parkout_time.Value >= fromDate)
                    && (string.IsNullOrEmpty(to) || x.parkout_time.Value < toDate)
                    && (string.IsNullOrEmpty(token_no) || x.recript_no.Equals(tokenNoLong))
                    && (Guid.Empty == userGuid || x.out_by.Value == userGuid)).
                        OrderByDescending(x => x.parkin_time).Select(x =>
                        new ParkedCars
                        {
                            Id = x.id,
                            checkinby = x.ap_user == null ? "" : x.ap_user.username,
                            checkinDate = x.parkin_time,
                            checkoutDate = x.parkout_time,
                            tokenNo = x.recript_no,
                            vehicle_NO = x.car_no,
                            Duration = x.parked_duration,
                            Amount = x.charged_amount,
                            monthly = x.is_monthly,
                            night = x.is_nightly,
                            out_by = x.out_by,
                            // checkOutBy = _context.ap_user.Where(a => a.id == x.out_by).FirstOrDefault().username

                        }
            );



            pt.Cars = cars.Skip(recordsPerPage * currentPage).Take(recordsPerPage).ToList();
            pt.Cars.ForEach(x => x.checkOutBy = allUsers.Where(y => y.id == x.out_by).FirstOrDefault() == null ? "" : allUsers.Where(y => y.id == x.out_by).FirstOrDefault().username);
            pt.TotalPages = recordsPerPage == 100 ? (cars.Count() / recordsPerPage) + 1 : 7;
            pt.CurrentPage = currentPage + 1;
            return pt;

        }

        public PaymentTableModel GetAllPayemnts(string userName, int currentPage)
        {
            PaymentTableModel model = new PaymentTableModel();
            model.Payments = _context.payments.Where(x => string.IsNullOrEmpty(userName) || x.ap_user.username.ToLower() == userName.ToLower())
                .Select(x => new PaymentModel
                {
                    username = x.ap_user.username,
                    amount = x.amount,
                    payment_type = x.payment_type,
                    received_date = x.received_date
                }).ToList();
            model.CurrentPage = currentPage;
            model.TotalPages = 7;
            return model;

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
            DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 05);
            var car = _context.monthly_reg.Where(x => x.vehicle_no.ToLower().Equals(veh_no)).FirstOrDefault();
            if (car == null)
                return false;
            DateTime carMonth = new DateTime(DateTime.Now.Year, int.Parse(car.month_name), DateTime.Now.Day);
            return int.Parse(car.month_name) == DateTime.Now.Month || carMonth.AddMonths(1) <= endDate;
        }

        public parked_car GetParkedCarById(Guid id)
        {
            return _context.parked_car.Where(x => x.id == id).FirstOrDefault();
        }

        public List<dur_amount> GetRateListByType(Guid id)
        {
            return _context.dur_amount.Where(x => x.veh_type_id == id).ToList();
        }

        public void Update()
        {
            _context.SaveChanges();
        }

        public int TotalParkedInCars()
        {
            return _context.parked_car.Where(x => x.parkout_time == null).Count();
        }

        public parked_car GetParkedCarByTokenNo(string token_no)
        {
            long result = 0;
            bool token = long.TryParse(token_no, out result);
            parked_car car = _context.parked_car.Where(x => x.recript_no == result || x.car_no.Contains(token_no)).OrderByDescending(x => x.date_created).FirstOrDefault();
            return car;

        }
    }
}