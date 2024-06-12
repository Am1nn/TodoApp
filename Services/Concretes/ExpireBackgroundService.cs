
using Microsoft.EntityFrameworkCore;
using TodoApp.Contexts;

namespace TodoApp.Services.Concretes
{
    public class ExpireBackgroundService : BackgroundService
    {
        private IDbContextFactory<AppDbContext> _context;

        public ExpireBackgroundService(IDbContextFactory<AppDbContext> context)
        {
            _context = context;
        }

        private Timer _timer;

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(CheckExpiredItems, null, TimeSpan.Zero, TimeSpan.FromSeconds(3));
            Console.WriteLine("Checking...");
            return Task.CompletedTask;
        }

        public void CheckExpiredItems(object? obj)
        {
            var Context = _context as AppDbContext;
            foreach (var item in Context.TodoItems.ToList())
            {
                if (item.ExpireDateTime - DateTime.Now <= TimeSpan.FromDays(1))
                {


                    foreach (var item1 in Context.Users.ToList())
                    {
                        if (item1.Id == item.UserId)
                        {
                            SmtpService.SendMail(item1.Email, "Expired", "Your todo has been expired!!!!!!");
                        }
                    }

                    
                }
            }
        }
    }
}
