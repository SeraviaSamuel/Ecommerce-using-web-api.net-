using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public PaymentController(IPaymentRepository paymentRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this.paymentRepository = paymentRepository;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Payment> payments = paymentRepository.GetAll();
            List<PaymentDTO> paymentDTOs = new List<PaymentDTO>();
            foreach (Payment payment in payments)
            {
                PaymentDTO dTO = new PaymentDTO
                {
                    Id = payment.Id,
                    Date = payment.Date,
                    Method = payment.Method,
                    Amount = payment.Amount
                };
                paymentDTOs.Add(dTO);
            }
            return Ok(paymentDTOs);
        }
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            Payment payment = paymentRepository.GetById(id);
            if (payment != null)
            {
                PaymentDTO paymentDTO = mapper.Map<PaymentDTO>(payment);
                return Ok(paymentDTO);
            }
            return NotFound("Not Found");
        }
        [HttpPost]
        public async Task<IActionResult> AddPayment(PaymentDTO paymentDTO)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await userManager.GetUserAsync(User);
                Payment payment = mapper.Map<Payment>(paymentDTO);
                payment.Customer_Id = currentUser.Id;
                paymentRepository.Insert(payment);
                paymentRepository.Save();
                return Ok("Added Susseccfully");
            }
            return BadRequest("Not Inserted");
        }
        [HttpPut]
        public async Task<IActionResult> Edit(int id, PaymentDTO paymentDTO)
        {
            var currentUser = await userManager.GetUserAsync(User);
            Payment payment = paymentRepository.GetById(id);
            if (payment != null)
            {
                mapper.Map(paymentDTO, payment);
                payment.Id = id;
                payment.Customer_Id = currentUser.Id;
                paymentRepository.Update(payment);
                paymentRepository.Save();
                return Ok("Updated Susseccfully");
            }
            return NotFound("Not Found");
        }
        [HttpDelete]
        public IActionResult Remove(int id)
        {
            Payment payment = paymentRepository.GetById(id);
            if (payment != null)
            {
                paymentRepository.Delete(id);
                paymentRepository.Save();
                return Ok("Deleted Successfully");
            }
            return NotFound("Not Found");
        }
    }
}
