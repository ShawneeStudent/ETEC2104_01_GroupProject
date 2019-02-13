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
            "create table posts (uid integer, pid integer primary key, forumid integer, parentid integer, content text)",
            "create table comment (uid integer,commentID integer primary key,postID integer,date integer,data text)",
            "create table tag (uid integer, tagID integer primary key)",
            "create table rating (uid integer, pid integer, rating integer)",
            "create table view (uid integer, pid integer, date integer, ipAddress text)",

            "insert into users (uid,email,picture,name,lastlogin) values (0,'postmaster@example.com','keyboardcat.jpg','admin',20190213)",
            "insert into posts (uid,pid,forumid,parentid,content) values (0,0,1,NULL,'First post')",
            "insert into comment(uid,commentID,postID,date,data) values (0,1,0,20190213,'Hi Jim')"
            //"insert into ...
        };

            SQLiteCommand cmd;

            foreach (string c in s)
            {
                cmd = new SQLiteCommand(c, conn);
                cmd.ExecuteNonQuery();
            }


            cmd = new SQLiteCommand("select content from posts where uid = 100;", conn);
            SQLiteDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Console.WriteLine("SELECT RESULT: " + rdr["content"]);
            }

            Console.WriteLine("--------------------");
            cmd = new SQLiteCommand("select name,uid from users where name = $name;", conn);
            cmd.Parameters.AddWithValue("$name", "alice");
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Console.WriteLine("SELECT RESULT 2: " + rdr["name"] + " " + rdr["uid"]);
            }
            Console.WriteLine("--------------------");

            //assume n is some string
            cmd = new SQLiteCommand("select users.name as BLAH, count(posts.pid) as BOOM from users " +
            "left outer join posts on users.uid=posts.uid group by users.name " +
            "order by count(posts.pid) desc;", conn);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Console.WriteLine("OUTER JOIN RESULT: " + rdr["BLAH"] + " " + rdr["BOOM"]);
            }

            conn.Close();
        }
    }
}
