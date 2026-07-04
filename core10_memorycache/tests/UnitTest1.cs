// tests/UnitTest1.cs
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using Xunit;
using core10_memorycache.Models;
using core10_memorycache.Controllers;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.TestHost;

namespace tests
{
    public class ContactsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly IMemoryCache _cache;
        private readonly WebApplicationFactory<Program> _factory;


        public ContactsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();            
            _cache = factory.Services.GetRequiredService<IMemoryCache>();
            _factory = factory;
        }

        [Fact]
        public async Task AddContact_WhenEmailDoesNotExist_ShouldReturnSuccessAndSetCache()
        {
            // Assign data
            var newContact = new ContactModel
            {
                Firstname = "Juan",
                Lastname = "Dela Cruz",
                Email = "juan@example.com",
                Mobile = "09171234567"
            };

            // Send the POST request to the API
            var response = await _client.PostAsJsonAsync("/createcontact", newContact);

            // Assert - Verify HTTP Status
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Assert - Verify the email cache key was created on the server
            var cacheKey = $"email_{newContact.Email.ToLowerInvariant()}";
            bool isCached = _cache.TryGetValue(cacheKey, out ContactModel? cachedContact);
            
            Assert.True(isCached);
            Assert.NotNull(cachedContact);
            Assert.Equal("Juan", cachedContact!.Firstname);
        }

        [Fact]
        public async Task AddContact_WhenEmailAlreadyExists_ShouldReturnConflict()
        {
            // Assign Data
            var existingContact = new ContactModel
            {
                Id = 1,
                Firstname = "Maria",
                Lastname = "Clara",
                Email = "maria@example.com",
                Mobile = "09179876543"
            };
            
            string emailCacheKey = $"email_{existingContact.Email.ToLowerInvariant()}";
            _cache.Set(emailCacheKey, existingContact);

            var payloadWithExistingEmail = new ContactModel
            {
                Firstname = "Maria",
                Lastname = "Clara",
                Email = "maria@example.com",
                Mobile = "09179876543"
            };

            // Send the POST request to the API
            var response = await _client.PostAsJsonAsync("/createcontact", payloadWithExistingEmail);

            // Assert - Verify Conflict Response
            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("Email Address is already exists.", responseContent);
        }

        [Fact]
        public async Task DeleteContactByEmail_WhenContactExists_ShouldReturnOk()
        {
            var testEmail = "maria@example.com";
            
            // Assign Data            
            var newContact = new ContactModel { Email = testEmail, Firstname = "Maria", Lastname = "Clara" };
            
            // Send the POST request to the API
            var postResponse = await _client.PostAsJsonAsync("/createcontact", newContact);
            postResponse.EnsureSuccessStatusCode();

            // Send the DELETE request to your endpoint
            var response = await _client.DeleteAsync($"/deletecontact/{testEmail}");

            // ASSERT: Validate the exact status code
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        [Fact]
        public async Task DeleteContactByEmail_WhenContactDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Delete a random email that isn't in the cache
            var response = await client.DeleteAsync("/deletecontact/unknown@example.com");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("not found in cache", responseContent);
        }



        [Fact]
        public async Task GetContacts_WhenCacheIsPopulated_ShouldReturnAllContacts()
        {
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                });
            }).CreateClient();

            var cache = _factory.Services.GetRequiredService<IMemoryCache>();
            
            var contacts = new List<ContactModel>
            {
                new() { 
                    Firstname = "Maria",
                    Lastname = "Clara",
                    Email = "maria@example.com",
                    Mobile = "09179876543"                
                } 
            };
            
            cache.Set("ContactsCacheKey", contacts); 

            var response = await client.GetAsync("/getallcontacts");

            response.EnsureSuccessStatusCode(); 
            
            var responseData = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseData);
        }
    



    }
}
