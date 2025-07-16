using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ViolationManagement.Models;

namespace ViolationManagement.DAL
{
    public class UserDAL
    {
        private readonly ViolationManagementContext _context;
        public UserDAL(ViolationManagementContext context)
        {
            _context = context;
        }
        public List<string> GetDistinctRole()
        {
            return _context.Users
                           .Where(u => !string.IsNullOrEmpty(u.Role))
                           .Select(u => u.Role)
                           .Distinct()
                           .OrderBy(r => r)
                           .ToList();
        }
        public List<User> SearchUsers(string keyword, string role, int pageIndex, int pageSize, out int totalCount)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(u => u.FullName.Contains(keyword));

            if (!string.IsNullOrEmpty(role))
                query = query.Where(u => u.Role == role);

            totalCount = query.Count();

            return query.OrderBy(u => u.UserId)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        }
        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == id);
        }
        public bool UpdateUserInfo(User user)
        {
            var existing = _context.Users.FirstOrDefault(u => u.UserId == user.UserId);
            if (existing != null)
            {
                existing.FullName = user.FullName;
                existing.Email = user.Email;
                existing.Phone = user.Phone;
                existing.Address = user.Address;
                existing.Role = user.Role;
                existing.Gender = user.Gender;

                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
