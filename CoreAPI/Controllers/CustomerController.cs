using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Microsoft.AspNetCore.Mvc;

namespace CoreAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            Randomizer.Seed = new Random(123456);
            var fakeOrders = new Faker<Order>()
                .RuleFor(o => o.Id, Guid.NewGuid)
                .RuleFor(o => o.Date, f => f.Date.Past(3))
                .RuleFor(o => o.OrderValue, f => f.Finance.Amount(0, 10000))
                .RuleFor(o => o.Shipped, f => f.Random.Bool(0.9f));
            var fakeCustomer = new Faker<Customer>()
                .RuleFor(c => c.Id, Guid.NewGuid())
                .RuleFor(c => c.Name, f => f.Company.CompanyName())
                .RuleFor(c => c.Address, f => f.Address.FullAddress())
                .RuleFor(c => c.City, f => f.Address.City())
                .RuleFor(c => c.Country, f => f.Address.Country())
                .RuleFor(c => c.ZipCode, f => f.Address.ZipCode())
                .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.ContactName, (f, c) => f.Name.FullName())
                .RuleFor(c => c.Orders, f => fakeOrders.Generate(f.Random.Number(10)).ToList());
            return fakeCustomer.Generate(5);
        }
    }
    
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ContactName { get; set; }
        public IEnumerable<Order> Orders { get; set; }
    }
    
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public Decimal OrderValue { get; set; }
        public bool Shipped { get; set; }
    }
}