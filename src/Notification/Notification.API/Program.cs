namespace Notification.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            //// Register Fluent Email Services
            //var emailConfig = new EmailConfiguration();
            //configuration.GetSection(nameof(EmailConfiguration)).Bind(emailConfig);

            //builder.Services.AddFluentEmail(defaultFromEmail: emailConfig.Email)
            //    .AddRazorRenderer()
            //    .AddMailKitSender(new SmtpClientOptions()
            //    {
            //        Server = emailConfig.Host,
            //        Port = emailConfig.Port,
            //        //User = emailConfig.Email,
            //        //Password = emailConfig.Password,
            //        //RequiresAuthentication = true,
            //        PreferredEncoding = "utf-8",
            //        UsePickupDirectory = true,
            //        MailPickupDirectory = @"C:\Users\mgayle\email",
            //        UseSsl = emailConfig.EnableSsl
            //    });

            //// Register Email Notification Service
            //builder.Services.AddScoped<IEmailNotification, EmailNotificationService>();

            app.Run();
        }
    }
}
