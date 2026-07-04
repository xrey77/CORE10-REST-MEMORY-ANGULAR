// Controllers/ContactController.cs
using core10_memorycache.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;

namespace core10_memorycache.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ContactsController: ControllerBase
    {
        private readonly IMemoryCache cache;
        private const string CounterKey = "App_Request_Counter";

        public int GetNextContactId()
        {
            var currentCount = cache.GetOrCreate(CounterKey, entry => 
            {
                entry.SlidingExpiration = TimeSpan.FromHours(1);
                return 0; // Initialize counter to 0
            });

            // Atomically increment the integer in the cache by 1
            var incrementedCount = Interlocked.Increment(ref currentCount);
            cache.Set(CounterKey, incrementedCount);

            return incrementedCount;
        }

        public static string GetContactCacheKey(string contactname) => $"contact_{contactname.ToLowerInvariant()}";

        public ContactsController(IMemoryCache _cache)
        {
            cache = _cache;
        }


        [HttpPost("/createcontact")]
        public IActionResult AddContact(ContactModel model)
        {
            string emailCacheKey = $"email_{model.Email.ToLowerInvariant()}"; 

            bool emailExists = cache.TryGetValue(emailCacheKey, out _);

            if (emailExists)
            {
                return Conflict(new { message = "Email Address is already exists."});
            }


            int newId = GetNextContactId();
            var newContact = new ContactModel
            {
                Id = newId,
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                Email = model.Email,
                Mobile = model.Mobile
            };

            // Cache the email to block duplicate submissions
            var cacheEntryOpt = new MemoryCacheEntryOptions 
            { 
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) 
            }; 

            // 5. Store in Memory Cache
            cache.Set(emailCacheKey, newContact, cacheEntryOpt); 

            return Ok(new {
                id = newId,
                message = "New contact has been added successfully."});            
        }        

        [HttpDelete("/deletecontact/{email}")]
        public IActionResult DeleteContactByEmail(string email)
        {
            string emailCacheKey = $"email_{email.ToLowerInvariant()}";

            if (cache.TryGetValue(emailCacheKey, out _))
            {
                cache.Remove(emailCacheKey);
                return Ok(new { message = $"Contact associated with email '{email}' has been removed from the cache." });
            }

            return NotFound(new { message = $"Contact with email '{email}' not found in cache." });
        }


        [HttpGet("/getallcontacts")]
        public IActionResult GetDiagnosticsCache()
        {
            if (cache is MemoryCache memCache)
            {
                var allContacts = memCache.Keys
                    .Where(key => key.ToString()?.StartsWith("email_") == true)
                    .Select(key => cache.Get<ContactModel>(key))
                    .Where(c => c != null)
                    .Cast<ContactModel>()                    
                    .OrderBy(c => c.Id) 
                    .ToList();

                return Ok(allContacts);
            }

            return BadRequest("Underlying cache provider does not support direct key enumeration.");
        }


    }
}