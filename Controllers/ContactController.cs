using Crito.Models;
using Crito.Services;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace Crito.Controllers
{
    public class ContactController : SurfaceController
    {
        public ContactController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
        }

        [HttpPost]
        public async Task<IActionResult> Index(ContactForm contactForm)
        {
            if(!ModelState.IsValid)
                return CurrentUmbracoPage();

            using var mail = new MailService("no-reply@crito.com", "smtp.crito.com", 567, "contact@crito.com", "BytMig123!");
            //to sender
            await mail.SendAsync(contactForm.Email, "Your request was recieved", "Hi your request was received and we will be in contact with you as soon as possible.").ConfigureAwait(false);

            //to us
            await mail.SendAsync("contact@crito.com", $"{contactForm.Name} sent you a contact request.", contactForm.Message).ConfigureAwait(false);

            return LocalRedirect(contactForm.RedirectUrl ?? "/");
        }
    }
}
