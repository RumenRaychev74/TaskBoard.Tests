using System.Text.Json.Serialization;

namespace TaskBoardAPITests
{
    public class TaskBoard
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string msg { get; set; }
        public string errMsg { get; set; }
        public Board board { get; set; }
        public DateTime dateCreated { get; set; }
        public DateTime dateModified { get; set; }
    }
    public class Board
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}