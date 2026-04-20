using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Hastane_Otomasyonu.Models;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyApiProject.Models;

namespace Hastane_Otomasyonu.Business
{

    public class AppointmentManagement
    {

        private readonly ILogger<AppointmentManagement> _logger;
        private readonly HastaneContext _context;
        public AppointmentManagement(ILogger<AppointmentManagement> logger, HastaneContext context)
        {
            _logger = logger;
            _context = context;
        }




        public List<TimeOnly> times = new List<TimeOnly>
        {
            TimeOnly.Parse("09:00:00.0000000"), TimeOnly.Parse("10:00:00.0000000"), TimeOnly.Parse("11:00:00.0000000"),
            TimeOnly.Parse("12:00:00.0000000"), TimeOnly.Parse("13:00:00.0000000"), TimeOnly.Parse("14:00:00.0000000"),
            TimeOnly.Parse("15:00:00.0000000"), TimeOnly.Parse("16:00:00.0000000"), TimeOnly.Parse("17:00:00.0000000")
        };



        // Bugünden itibaren gelecek 5 iş gününü hesapla
        public List<DateOnly> GetNextWorkingDays(int count = 5)
        {
            var result = new List<DateOnly>();
            var date = DateOnly.FromDateTime(DateTime.Today);
            while (result.Count < count)
            {
                date = date.AddDays(1);
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                    result.Add(date);
            }
            return result;
        }

        // Mesai saatlerini yaz
        public void MesaiEkle(int doktorId)
        {
            var dates = GetNextWorkingDays(5);

            foreach (var item in dates)
            {
                foreach (var item2 in times)
                {
                    _logger.LogInformation($"Tarih ve saat: {item} {item2}");

                    // Slot yoksa önce oluştur
                    var Appointment = _context.Appointments.FirstOrDefault(x => x.SlotDate == item && x.StartTime == item2);
                    if (Appointment == null)
                    {
                        Appointment = new Appointment
                        {
                            SlotDate = item,
                            StartTime = item2,
                            IsAvailable = true
                        };
                        _context.Appointments.Add(Appointment);
                        _context.SaveChanges(); // Id alabilmek için kaydet
                        _logger.LogInformation($"Yeni randevu slotu oluşturuldu: {item} {item2}");
                    }

                    // Doktora bağla (zaten bağlı değilse)
                    bool zatenBagli = _context.AppointmentToDoktors
                        .Any(x => x.DoktorFk == doktorId && x.AppointmentFk == Appointment.Id);

                    if (!zatenBagli)
                    {
                        _context.AppointmentToDoktors.Add(new AppointmentToDoktor
                        {
                            DoktorFk = doktorId,
                            AppointmentFk = Appointment.Id
                        });
                        _logger.LogInformation($"Mesai saati eklendi: {item} {item2}");
                    }
                }
            }
            _context.SaveChanges();
        }

        // Mesai saatlerini sil
        public void MesaiSil(int doktorId, DateOnly date, TimeOnly time)
        {
            var Appointment = _context.Appointments.FirstOrDefault(x => x.SlotDate == date && x.StartTime == time);

            _context.AppointmentToDoktors.Remove(_context.AppointmentToDoktors.FirstOrDefault(x => x.DoktorFk == doktorId && x.AppointmentFk == Appointment.Id));
            _context.SaveChanges();
        }
    }

}