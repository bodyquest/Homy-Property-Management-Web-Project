namespace RPM.Web.Api
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using RPM.Data;
    using RPM.Data.Models;
    using RPM.Services.Common;
    using RPM.Web.Api.Models;
    using Stripe;
    using Stripe.Checkout;

    using static RPM.Common.GlobalConstants;

    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> userManager;
        private readonly IPaymentCommonService paymentService;

        public CitiesController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            IPaymentCommonService paymentService)
        {
            _context = context;
            this.userManager = userManager;
            this.paymentService = paymentService;
        }

        // GET: api/Cities
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<AllCitiesViewModel>>> GetCities()
        {
            var model = await this._context.Cities.Select(x => new AllCitiesViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Country = x.Country.Name,
            })
            .ToListAsync();

            return model;
        }

        [HttpPost]
        [Route("createsession")]
        public async Task<Session> CreateSession([FromBody]string id)
        {
            //using (var reader = new StreamReader(this.Request.Body, Encoding.UTF8))
            //{
            //    id = await reader.ReadToEndAsync();
            //}

            var userId = this.userManager.GetUserId(this.User);
            var payment = await this.paymentService.GetPaymentDetailsAsync(id, userId);

            var successStringUrl = "https://localhost:44319/Checkout/success?session_id={CHECKOUT_SESSION_ID}";
            var cancelStringUrl = "https://localhost:44319/Checkout/cancel?";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },

                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Quantity = 1,
                        Amount = (long)payment.Amount * 100,
                        Currency = CurrencyUSD,

                        Description = $"Payment Id: {payment.Id} for rental at {payment.Address}",

                        Name = $"Rent Payment for {DateTime.UtcNow.Month} | {DateTime.UtcNow.Year} for rental at {payment.Address}",
                    },
                },

                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    ApplicationFeeAmount = (long)((payment.Amount * 0.01m) * 100),
                    CaptureMethod = "manual",

                    TransferData = new SessionPaymentIntentTransferDataOptions
                    {
                        Destination = payment.ToStripeAccountId,
                    },
                },

                SuccessUrl = successStringUrl,
                CancelUrl = cancelStringUrl,
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return session;
        }

        // GET: api/Cities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<City>> GetCity(int id)
        {
            var city = await _context.Cities.FindAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            return city;
        }

        // PUT: api/Cities/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [Route("edit")]
        public async Task<IActionResult> PutCity(int id, City city)
        {
            if (id != city.Id)
            {
                return BadRequest();
            }

            _context.Entry(city).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cities
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<City>> PostCity(City city)
        {
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCity", new { id = city.Id }, city);
        }

        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        [Route("delete")]
        public async Task<ActionResult<City>> DeleteCity(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return city;
        }

        private bool CityExists(int id)
        {
            return _context.Cities.Any(e => e.Id == id);
        }
    }
}
