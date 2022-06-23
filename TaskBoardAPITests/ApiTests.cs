using RestSharp;
using System.Net;
using System.Text.Json;

namespace TaskBoardAPITests
{
    public class ApiTests
    {
        private const string Url = "https://taskboard.rumenraychev74.repl.co/api/tasks";
        private RestClient client;
        private RestRequest? request;
        
        
        [SetUp]
        public void Setup()
        {
            client = new RestClient(Url);
        }

        [Test]
        public void Test_GetAllTasks_FirstTasksName()
        {
            // Arrange
            request = new RestRequest(Url);

            // Act
            var response = client.Execute(request);
            var tasks = JsonSerializer.Deserialize<List<TaskBoard>>(response.Content);
            var boards = JsonSerializer.Deserialize<List<Board>>(response.Content);

            // Assert
            Assert.IsNotNull(response.Content);
            Assert.IsTrue(tasks.Count > 0);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            foreach (var board in boards)
            {
                var boardName = board.name;
                if (boardName == "Done")
                {
                    Assert.AreEqual("Project skeleton", tasks.First().title);
                    break;
                }
            }
        }
        [Test]
        public void Test_SearchHomeTask_CheckResult()
        {
            // Arrange
            request = new RestRequest(Url + "/search/home");

            // Act
            var response = client.Execute(request);
            var tasks = JsonSerializer.Deserialize<List<TaskBoard>>(response.Content);
           

            // Assert
            Assert.IsNotNull(response.Content);
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("Home page", tasks.First().title);

        }
        [Test]
        public void Test_SearchTask_EmptyResults()
        {
            // Arrange
            this.request = new RestRequest(Url + "/search/{keyword}");
            request.AddUrlSegment("keyword", "missin462378462378");

            // Act
            var response = this.client.Execute(request, Method.Get);
            var contacts = JsonSerializer.Deserialize<List<TaskBoard>>(response.Content);


            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(contacts.Count, Is.EqualTo(0));
        }
        [Test]
        public void Test_CreateTask_InvalidData()
        {
            //Arenge
            this.request = new RestRequest(Url);
            var body = new
            {
                discription = "Invalid data",
                board = "Done"
            };
            request.AddJsonBody(body);

            //Act
            var response = this.client.Execute(request, Method.Post);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("{\"errMsg\":\"Title cannot be empty!\"}"));

        }
        [Test]
        public void Test_CreateContact_ValidDataValidateResult()
        {
            //Arenge
            this.request = new RestRequest(Url);
            var body = new
            {
                title = "TestRR",
                description = "Valid data",
                board = "Done"
            };
            request.AddJsonBody(body);

            //Act
            var response = this.client.Execute(request, Method.Post);

            //Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var allContacts = this.client.Execute(request, Method.Get);
            var contacts = JsonSerializer.Deserialize<List<TaskBoard>>(allContacts.Content);

            var lastContact = contacts[contacts.Count - 1];
            Assert.That(lastContact.title, Is.EqualTo(body.title));
            Assert.That(lastContact.description, Is.EqualTo(body.description));

        }
    }
}