using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelBookingAPI.Models;
using HotelBookingAPI.Data;
using System.Security.Cryptography.X509Certificates;

namespace HotelBookingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelBookingController : ControllerBase
    {
        private readonly ApiContext _context;
        public HotelBookingController(ApiContext context)//dependincy injection
        {
               _context = context;
        }
        //Create/Edit
        [HttpPost]
        public JsonResult CreateEdit(HotelBooking booking)
        {
            //eğer rezervasyon yoksa rezervasyon ekle
            if (booking.Id == 0)
            {
                _context.Bookings.Add(booking);
            }else//rezervasyon yapılmamışsa
            {
                var bookingInDb = _context.Bookings.Find(booking.Id);//rezervasyon id sini bul

                if (bookingInDb == null)//rezervasyon id si bulunmadıysa bulunamadı uyarısı dön
                {
                    return new JsonResult(NotFound());  
                }
                bookingInDb = booking;//rezervasyon id si varsa id yi güncelle
            }

            _context.SaveChanges(); //yapılan değişiklikleri kaydet
            return new JsonResult(Ok(booking)); //yeni kaydı dönder
        }

        [HttpGet]
        public JsonResult Get(int id)//kullanıcı id leri al
        {
            var result = _context.Bookings.Find(id);
            if (result == null)//eğer id bulunamazsa hata verecek
            {
                return new JsonResult(NotFound());
            }
            return new JsonResult(Ok(result));//id yi döndürüyor
        }

        [HttpDelete]
        public JsonResult Delete(int id)
        {
            var result = _context.Bookings.Find(id);
            if(result == null)
            {
                return new JsonResult(NotFound());
            }
            _context.Bookings.Remove(result);
            _context.SaveChanges();

            return new JsonResult(NoContent());

        }

        [HttpGet("/GetAll")]
        public JsonResult GetAll() {
            var result =_context.Bookings.ToList();
            return new JsonResult(Ok(result));  
        }



    }
 

}
