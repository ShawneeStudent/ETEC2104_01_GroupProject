using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;

namespace MemeSiteProject
{
    class notMain
    {
        public static void Main(string[] args)
        {
            string sqlfile = "someFilename.xyz";
            System.IO.File.Delete(sqlfile);
            SQLiteConnection conn = new SQLiteConnection("Data Source=" + sqlfile + ";New=False");
            conn.Open();
            string[] s = {
            "create table users (uid integer primary key, email text, picture text, name text, lastlogin integer);",
            "create table posts (uid integer, postID integer primary key, forumid integer, parentid integer, content text)",
            "create table comment (uid integer, commentID integer primary key, postID integer, date integer, data text)",
            "create table tag (uid integer, tagID integer primary key)",
            "create table rating (uid integer, postID integer, rating integer)",
            "create table view (uid integer, postID integer, date integer, ipAddress text)",

            "insert into users (uid,email,picture,name,lastlogin) values (0,'postmaster@example.com','keyboardcat.jpg','admin',20190213)",
            "insert into users (uid,email,picture,name,lastlogin) values (1337,'myEmail@example.com','profilepic.png','Walter',20190213)",
            "insert into users (uid,email,picture,name,lastlogin) values (1,'school@example.com','something.png','Tyler',20190213)",
            "insert into posts (uid, postID,forumid,parentID,content) values (1337,1,1,NULL,'Hello Peeps')",
            "insert into posts (uid, postID,forumid,parentID,content) values (1337,2,1,NULL,'Shaggy at 0.1% of his power level')",
            "insert into posts (uid,postID,forumid,parentid,content) values (0,0,1,NULL,'First post')",
            "insert into comment (uid,commentID,postID,date,data) values (0,1,0,20190213,'Hi Jim')",
            "insert into comment (uid,commentID,postID,date,data) values  (1,2,0,20190216,'Hello World')",
            "insert into tag (uid,tagID) values (0, 13)",
            "insert into rating (uid,postID,rating) values (0,0,5)",
            "insert into rating (uid,postID,rating) values (1337,1,10)",
            "insert into rating (uid,postID,rating) values (1337,2,100)",
            "insert into view (uid,postID,date,ipAddress) values (0,0,20190214,'192.1337.7357')"
        };

            SQLiteCommand cmd;

            foreach (string c in s)
            {
                cmd = new SQLiteCommand(c, conn);
                cmd.ExecuteNonQuery();
            }

          
            cmd = new SQLiteCommand("select content from posts where uid = 0;", conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Console.WriteLine("SELECT RESULT: " + rdr["content"]);
            }

            Console.WriteLine("--------------------");
            cmd = new SQLiteCommand("select name,uid from users where name = $name;", conn);
            cmd.Parameters.AddWithValue("$name", "Walter");
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Console.WriteLine("SELECT RESULT 2: " + rdr["name"] + " id:" + rdr["uid"]);
            }
            Console.WriteLine("--------------------");

            //assume n is some string
            cmd = new SQLiteCommand("select users.name as BLAH, count(posts.postID) as BOOM from users " +
            "left outer join posts on users.uid=posts.uid group by users.name " +
            "order by count(posts.postID) desc;", conn);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Console.WriteLine("OUTER JOIN RESULT(User): " + rdr["BLAH"] + " | amount of posts: " + rdr["BOOM"]);
            }
            Console.WriteLine("--------------------");

            cmd = new SQLiteCommand("select posts.postID as pos, rating.rating as rate from posts " +
                "left outer join rating on posts.postID=rating.postID group by posts.postID " +
                "order by rating.rating desc;", conn);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Console.WriteLine("OUTER JOIN RESULT(Rating): " + rdr["pos"] + " | recieved a rating of: " + rdr["rate"]);
            }

            conn.Close();
        }
    }
}
