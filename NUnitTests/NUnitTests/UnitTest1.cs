using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace NUnitTests
{
	[TestFixture]
	public class ApiTests
	{
		private HttpClient client;
		private const string APIURL = "https://api.restful-api.dev/objects";
		private string lastAddedObjectId;

		[OneTimeSetUp]
		public void Setup()
		{
			client = new HttpClient();
		}

		[Test]
		public async Task GetObjects_ReturnsSuccess()
		{
			var response = await client.GetAsync(APIURL);
			response.EnsureSuccessStatusCode();

			Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
		}


		[Test]
		public async Task GetObjectById_ReturnsSuccess()
		{
			int objectId = 1; // Replace with a valid object ID
			var response = await client.GetAsync($"{APIURL}/{objectId}");
			response.EnsureSuccessStatusCode();

			Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
		}

		[Test]
		public async Task AddObject_ReturnsSuccess()
		{
			var newObjectData = JsonConvert.SerializeObject(new
			{
				id = "99", 
				name = "Apple MacBook Pro 17", 
				data = new
				{
					year = 2020,
					price = 1999.99,
					CPU_model = "Intel Core i10", 
					Hard_disk_size = "2 TB"
				}
			});

			var content = new StringContent(newObjectData, System.Text.Encoding.UTF8, "application/json");

			var response = await client.PostAsync(APIURL, content);
			response.EnsureSuccessStatusCode();
			Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));

			var responseObject = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());
			lastAddedObjectId = responseObject.id;
		}

		[Test]
		public async Task UpdateObject_ReturnsSuccess()
		{
			var updatedObjectData = JsonConvert.SerializeObject(new
			{
				id = lastAddedObjectId,
				name = "Apple MacBook Pro 16 Updated",
				data = new
				{
					year = 2020,
					price = 1949.99
				}
			});

			var content = new StringContent(updatedObjectData, System.Text.Encoding.UTF8, "application/json");

			var response = await client.PutAsync($"{APIURL}/{lastAddedObjectId}", content);
			response.EnsureSuccessStatusCode();

			Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
		}

		[Test]
		public async Task DeleteObject_ReturnsSuccess()
		{
			var response = await client.DeleteAsync($"{APIURL}/{lastAddedObjectId}");
			response.EnsureSuccessStatusCode();

			Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
		}
	}
}