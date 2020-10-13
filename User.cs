using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFNotifier.DAL;

namespace WPFNotifier
{
    public class User
    {
        public int userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }
        public bool smsOptIn { get; set; }
        public string email { get; set; }

        public static List<User> GetUsers()
        {
            using (UserContext db = new UserContext())
            {
                var users = db.Users.ToList();
                return users;
            }
        }

        //Unused currently - future feature.
        public static void CreateUser(User user)
        {
            try
            {
                using (UserContext db = new UserContext())
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Unused currently - future feature.
        public static void UpdateUsers(User user)
        {
            using (UserContext db = new UserContext())
            {
                var existingUser = db.Users.First<User>();
                existingUser = user;
                db.SaveChanges();
            }
        }

        //Unused currently - future feature.
        public static void DeleteUsers(User user)
        {
            using (UserContext db = new UserContext())
            {
                var existingUser = db.Users.First<User>();
                db.Users.Remove(existingUser);
                db.SaveChanges();
            }
        }
    }
}
