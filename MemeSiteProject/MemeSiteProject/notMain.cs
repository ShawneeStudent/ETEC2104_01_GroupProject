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
            "create table posts (uid integer, id integer primary key, forumid integer, parentid integer, content text)",
            "create table comment (uid integer primary key, postID integer, userID integer)",
            "create table tag (uid integer primaryKey, content text)",
            "create table rating (userID integer, postID integer, rating integer)",
            "create table view (userID integer, postID integer, date integer, ipAddress text)",

            "insert into users (uid,name,email,lastlogin) values (0,'admin','postmaster@example.com',20140304)",
            "insert into forums (creator,title,id) values (0,'sales',4)",
            "insert into posts (uid,id,forumid,parentid,content) values (0,0,1,NULL,'First post')",
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
            cmd = new SQLiteCommand("select users.name as BLAH, count(posts.id) as BOOM from users " +
            "left outer join posts on users.uid=posts.uid group by users.name " +
            "order by count(posts.id) desc;", conn);
            rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Console.WriteLine("OUTER JOIN RESULT: " + rdr["BLAH"] + " " + rdr["BOOM"]);
            }

            conn.Close();
        }
    }
}
