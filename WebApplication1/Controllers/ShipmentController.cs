using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipmentController : ControllerBase
    {
        private readonly IShipmentRepository shipmentRepository;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public ShipmentController(IShipmentRepository shipmentRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            this.shipmentRepository = shipmentRepository;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        [HttpGet]
        [Authorize]
        public IActionResult GetAll()
        {
            List<Shipment> shipments = shipmentRepository.GetAll();
            List<ShipmentDTO> shipmentDTOs = new List<ShipmentDTO>();
            foreach (Shipment shipment in shipments)
            {
                ShipmentDTO dTO = new ShipmentDTO
                {
                    Id = shipment.Id,
                    Date = shipment.Date,
                    Address = shipment.Address,
                    City = shipment.City,
                    State = shipment.State,
                    Country = shipment.Country,
                    Zip_Code = shipment.Zip_Code
                };
                shipmentDTOs.Add(dTO);
            }
            return Ok(shipmentDTOs);
        }
        [HttpGet("{id:int}")]
        [Authorize]
        public IActionResult GetById(int id)
        {
            Shipment shipment = shipmentRepository.GetById(id);
            if (shipment != null)
            {
                ShipmentDTO shipmentDTO = mapper.Map<ShipmentDTO>(shipment);
                return Ok(shipmentDTO);
            }
            return NotFound();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddShipment(ShipmentDTO shipmentDTO)
        {
            if (ModelState.IsValid)
            {

                var currentUser = await userManager.GetUserAsync(User);
                //Shipment shipment = new Shipment();
                //{
                //    State = shipmentDTO.State,
                //    Address = shipmentDTO.Address,
                //    Zip_Code = shipmentDTO.Zip_Code,
                //    City = shipmentDTO.City,
                //    Country = shipmentDTO.Country,
                //    Date = shipmentDTO.Date,
                //    Customer_Id = currentUser.Id
                //};

                Shipment shipment = mapper.Map<Shipment>(shipmentDTO);
                shipment.Customer_Id = currentUser.Id;
                shipmentRepository.Insert(shipment);
                shipmentRepository.Save();
                return Ok("Addded Successfully");
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Edit(int id, ShipmentDTO shipmentDTO)
        {
            var currentUser = await userManager.GetUserAsync(User);
            Shipment existingShipment = shipmentRepository.GetById(id);

            if (existingShipment != null)
            {
                // Map the properties from the DTO to the existing entity
                mapper.Map(shipmentDTO, existingShipment);
                existingShipment.Id = id;
                existingShipment.Customer_Id = currentUser.Id;
                shipmentRepository.Update(existingShipment);
                shipmentRepository.Save();
                return Ok("Updated");
            }
            return NotFound();
        }
        [HttpDelete]
        [Authorize]
        public IActionResult Remove(int id)
        {
            Shipment shipment = shipmentRepository.GetById(id);
            if (shipment != null)
            {
                shipmentRepository.Delete(id);
                shipmentRepository.Save();
                return Ok("Deleted susseccfully");
            }
            return NotFound("Invalid");

        }



    }
}
