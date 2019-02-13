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
            "create table users (uid integer primary key, name text, email text, lastlogin integer);",
            "create table forums (creator integer, title text, id integer primary key)",
            "create table posts (uid integer, id integer primary key, forumid integer, parentid integer, content text)",
            "insert into users (uid,name,email,lastlogin) values (0,'admin','postmaster@example.com',20140304)",
            "insert into users (uid,name,email,lastlogin) values (100,'alice','postmaster@example.com',20140201)",
            "insert into users (uid,name,email,lastlogin) values (105,'bob','bob@bob.bob',20100504)",
            "insert into users (uid,name,email,lastlogin) values (108,'carol',NULL,20000101)",
            "insert into users (uid,name,email,lastlogin) values (109,'dave','dave@dave.example.net',20130315)",
            "insert into forums (creator,title,id) values (0,'gaming',1)",
            "insert into forums (creator,title,id) values (0,'offtopic',2)",
            "insert into forums (creator,title,id) values (0,'help',3)",
            "insert into forums (creator,title,id) values (0,'sales',4)",
            "insert into posts (uid,id,forumid,parentid,content) values (0,0,1,NULL,'First post')",
            "insert into posts (uid,id,forumid,parentid,content) values (100,1,2,NULL,'I like snails')",
            "insert into posts (uid,id,forumid,parentid,content) values (105,2,3,NULL,'How do you use this?')",
            "insert into posts (uid,id,forumid,parentid,content) values (108,3,2,1,'But not escargot!')",
            "insert into posts (uid,id,forumid,parentid,content) values (100,4,2,3,'That''s gross!')",
            "insert into posts (uid,id,forumid,parentid,content) values (100,5,4,NULL,'For sale: Used rag')",
            "insert into posts (uid,id,forumid,parentid,content) values (108,6,4,5,'I''ll take it!')",
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
